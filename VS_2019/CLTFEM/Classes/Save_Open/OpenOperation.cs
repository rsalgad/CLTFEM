using System;
using System.Linq;
using System.Xml;
using System.Windows.Media.Media3D;
using CLTFEM.Classes.Structural;
using CLTFEM.Classes.Helpers;
using CLTFEM.Windows;
using System.IO;
using System.Collections.Generic;
using System.Windows;
using CLTFEM.Classes.Analysis;

namespace CLTFEM.Classes.Save_Open
{
    class OpenOperation
    {
        /// <summary>
        /// Reads the xml file of a saved structure
        /// </summary>
        /// <param name="pathToOpen">The path of the file</param>
        public static void OpenStructure(string pathToOpen)
        {
            //cleans whatever's on the screen when the open command is called
            Management.CleanCurrentStrucure();

            //reads the analysis
            XmlReader analysisRead = XmlReader.Create(pathToOpen);
            while (analysisRead.ReadToFollowing("analysis"))
            {
                string type = analysisRead.GetAttribute("type");
                if (type == "Elastic")
                {
                    int nStep = int.Parse(analysisRead.GetAttribute("load-steps"));
                    MainWindow.analysis.Add(new ElasticAnalysis(nStep));
                }
                else if (type == "Pushover")
                {
                    int nStep = int.Parse(analysisRead.GetAttribute("load-steps"));
                    int nIter = int.Parse(analysisRead.GetAttribute("iterations"));
                    MainWindow.analysis.Add(new PushoverAnalysis(nStep, nIter));
                }
                else if (type == "Cyclic")
                {
                    int nStep = int.Parse(analysisRead.GetAttribute("load-steps"));
                    int nIter = int.Parse(analysisRead.GetAttribute("iterations"));
                    double iniPeak = double.Parse(analysisRead.GetAttribute("initial-peak"));
                    int cyclesPeak = int.Parse(analysisRead.GetAttribute("cycles-per-peak"));
                    double peakInc = double.Parse(analysisRead.GetAttribute("peak-increment"));
                    int stepPeak = int.Parse(analysisRead.GetAttribute("steps-per-peak"));
                    char typeAnalysis = char.Parse(analysisRead.GetAttribute("type"));
                    MainWindow.analysis.Add(new CyclicAnalysis(nStep, nIter, iniPeak, stepPeak, peakInc, cyclesPeak, typeAnalysis));
                }
                else
                {
                    double deltaT = double.Parse(analysisRead.GetAttribute("deltaT"));
                    double addTime = double.Parse(analysisRead.GetAttribute("additional-time"));
                    int nIter = int.Parse(analysisRead.GetAttribute("iterations"));
                    string intMethod = analysisRead.GetAttribute("itegration-method");
                    char typeAnalysis = char.Parse(analysisRead.GetAttribute("type"));
                    MainWindow.analysis.Add(new DynamicAnalysis(deltaT, addTime, nIter, intMethod, typeAnalysis));
                }
            }
            analysisRead.Close();

            //reads the materials
            XmlReader matRead = XmlReader.Create(pathToOpen);
            while (matRead.ReadToFollowing("material"))
            {
                string type = matRead.GetAttribute("type");
                if (type == "Elastic")
                {
                    double elasticity = Double.Parse(matRead.GetAttribute("elasticity"));
                    double poisson = Double.Parse(matRead.GetAttribute("poisson"));
                    MainWindow.materialList.Add(new ElasticMaterial(Int32.Parse(matRead.ReadInnerXml()), elasticity, poisson));
                }
                else if (type == "OrthoElastic")
                {
                    double eX = Double.Parse(matRead.GetAttribute("ex"));
                    double eY = Double.Parse(matRead.GetAttribute("ey"));
                    double vxy = Double.Parse(matRead.GetAttribute("vxy"));
                    double gXY = Double.Parse(matRead.GetAttribute("gxy"));
                    double gYZ = Double.Parse(matRead.GetAttribute("gyz"));
                    double gXZ = Double.Parse(matRead.GetAttribute("gxz"));
                    MainWindow.materialList.Add(new OrthotropicElasticMaterial(Int32.Parse(matRead.ReadInnerXml()), eX, eY, vxy, gXY, gYZ, gXZ));
                }
                else if (type == "Spring-Axial")
                {
                    double iniStiff = double.Parse(matRead.GetAttribute("initialStiffness"));
                    double fMax = double.Parse(matRead.GetAttribute("peakForce"));
                    double dMax = double.Parse(matRead.GetAttribute("peakDisplacement"));
                    double degStiff = double.Parse(matRead.GetAttribute("degradingStiffness"));
                    double fRes = double.Parse(matRead.GetAttribute("residualForce"));
                    double dUlt = double.Parse(matRead.GetAttribute("ultimateDisplacement"));
                    double compStiff = double.Parse(matRead.GetAttribute("compressiveStiffness"));
                    double unlStiff = 0, fUnl = 0, conStiff = 0, relStiff = 0;
                    try
                    {
                        unlStiff = double.Parse(matRead.GetAttribute("unloadStiffness"));
                        fUnl = double.Parse(matRead.GetAttribute("unloadForce"));
                        conStiff = double.Parse(matRead.GetAttribute("connectStiffness"));
                        relStiff = double.Parse(matRead.GetAttribute("reloadStiffness"));
                    } catch
                    {
                        MessageBox.Show("Error reading Axial Spring Material Models.");
                        continue;
                    }
                    int ID = Int32.Parse(matRead.ReadInnerXml());
                    MainWindow.materialList.Add(new SpringAxialModel(ID, iniStiff, dMax, fMax, degStiff, fRes, dUlt, compStiff, unlStiff, fUnl, conStiff, relStiff));
                }
                else
                {
                    double iniStiff = double.Parse(matRead.GetAttribute("initialStiffness"));
                    double fMax = double.Parse(matRead.GetAttribute("peakForce"));
                    double dMax = double.Parse(matRead.GetAttribute("peakDisplacement"));
                    double degStiff = double.Parse(matRead.GetAttribute("degradingStiffness"));
                    double fRes = double.Parse(matRead.GetAttribute("residualForce"));
                    double dUlt = double.Parse(matRead.GetAttribute("ultimateDisplacement"));
                    double unlStiff = 0, fUnl = 0, conStiff = 0, relStiff = 0;
                    try
                    {
                        unlStiff = double.Parse(matRead.GetAttribute("unloadStiffness"));
                        fUnl = double.Parse(matRead.GetAttribute("unloadForce"));
                        conStiff = double.Parse(matRead.GetAttribute("connectStiffness"));
                        relStiff = double.Parse(matRead.GetAttribute("reloadStiffness"));
                    } catch
                    {
                        MessageBox.Show("Error reading General Spring Material Model.");
                        continue;
                    }
                    int ID = Int32.Parse(matRead.ReadInnerXml());
                    MainWindow.materialList.Add(new SpringGeneralModel(ID, iniStiff, dMax, fMax, degStiff, fRes, dUlt, unlStiff, fUnl, conStiff, relStiff));
                }
            }
            matRead.Close();

            //reads the nodes
            XmlReader nodeRead = XmlReader.Create(pathToOpen);
            while (nodeRead.ReadToFollowing("node"))
            {
                double nodeX = Double.Parse(nodeRead.GetAttribute("X"));
                double nodeY = Double.Parse(nodeRead.GetAttribute("Y"));
                double nodeZ = Double.Parse(nodeRead.GetAttribute("Z"));
                MainWindow.nodeList.Add(new Node(Int32.Parse(nodeRead.ReadInnerXml()), new Point3D(nodeX, nodeY, nodeZ)));
            }
            nodeRead.Close();

            //reads the spring elements
            XmlReader SpringRead = XmlReader.Create(pathToOpen);
            SpringRead.ReadToFollowing("spring3D");
            if (SpringRead.ReadToDescendant("element"))
            {
                do
                {
                    int[] matList = new int[3];
                    Node node1 = MainWindow.nodeList.ElementAt(Int32.Parse(SpringRead.GetAttribute("N1")) - 1);
                    Node node2 = MainWindow.nodeList.ElementAt(Int32.Parse(SpringRead.GetAttribute("N2")) - 1);
                    matList[0] = Int32.Parse(SpringRead.GetAttribute("materialX"));
                    matList[1] = Int32.Parse(SpringRead.GetAttribute("materialY"));
                    matList[2] = Int32.Parse(SpringRead.GetAttribute("materialZ"));

                    char vecX = char.Parse(SpringRead.GetAttribute("axial-vec"));
                    char vecY = char.Parse(SpringRead.GetAttribute("shear-vec"));

                    MainWindow.springList.Add(new Spring3D(Int32.Parse(SpringRead.ReadInnerXml()), node1, node2, matList, vecX, vecY));
                } while (SpringRead.ReadToNextSibling("element"));
            }
            SpringRead.Close();

            //reads the shell elements
            XmlReader EleRead = XmlReader.Create(pathToOpen);
            EleRead.ReadToFollowing("shell");
            if (EleRead.ReadToDescendant("element"))
            {
                do
                {
                    double thick = Double.Parse(EleRead.GetAttribute("thickness"));
                    int layers = Int32.Parse(EleRead.GetAttribute("layers"));
                    int matIndex = Int32.Parse(EleRead.GetAttribute("material"));
                    Node n1 = MainWindow.nodeList.ElementAt(Int32.Parse(EleRead.GetAttribute("N1")) - 1);
                    Node n2 = MainWindow.nodeList.ElementAt(Int32.Parse(EleRead.GetAttribute("N2")) - 1);
                    Node n3 = MainWindow.nodeList.ElementAt(Int32.Parse(EleRead.GetAttribute("N3")) - 1);
                    Node n4 = MainWindow.nodeList.ElementAt(Int32.Parse(EleRead.GetAttribute("N4")) - 1);
                    Node n5 = MainWindow.nodeList.ElementAt(Int32.Parse(EleRead.GetAttribute("N5")) - 1);
                    Node n6 = MainWindow.nodeList.ElementAt(Int32.Parse(EleRead.GetAttribute("N6")) - 1);
                    Node n7 = MainWindow.nodeList.ElementAt(Int32.Parse(EleRead.GetAttribute("N7")) - 1);
                    Node n8 = MainWindow.nodeList.ElementAt(Int32.Parse(EleRead.GetAttribute("N8")) - 1);
                    Node n9 = MainWindow.nodeList.ElementAt(Int32.Parse(EleRead.GetAttribute("N9")) - 1);
                    MainWindow.shellList.Add(new ShellElement(Int32.Parse(EleRead.ReadInnerXml()), n1, n2, n3, n4, n5, n6, n7, n8, n9, thick, layers, MainWindow.materialList.ElementAt(matIndex - 1)));
                } while (EleRead.ReadToNextSibling("element"));
            }
            EleRead.Close();

            //reads the loads
            XmlReader LoadRead = XmlReader.Create(pathToOpen);
            while (LoadRead.ReadToFollowing("load"))
            {
                int ID = Int32.Parse(LoadRead.GetAttribute("ID"));
                int nodeID = Int32.Parse(LoadRead.GetAttribute("nodeID"));
                string status = LoadRead.GetAttribute("status");
                int totalDir = Int32.Parse(LoadRead.GetAttribute("values"));
                List<PairValue> pvList = new List<PairValue>();
                for (int i = 0; i < totalDir; i++)
                {
                    LoadRead.ReadToFollowing("value");
                    int dir = Int32.Parse(LoadRead.GetAttribute("direction"));
                    int val = Int32.Parse(LoadRead.ReadInnerXml());
                    pvList.Add(new PairValue(dir, val));
                }
                Load l = new Load(ID, nodeID, status);
                l.SetLoadList = pvList;
                MainWindow.loadList.Add(l);
            }
            LoadRead.Close();

            //reads the loads
            XmlReader MassRead = XmlReader.Create(pathToOpen);
            while (MassRead.ReadToFollowing("mass"))
            {
                int ID = Int32.Parse(MassRead.GetAttribute("ID"));
                int nodeID = Int32.Parse(MassRead.GetAttribute("nodeID"));
                int totalDir = Int32.Parse(MassRead.GetAttribute("values"));
                List<PairValue> pvList = new List<PairValue>();
                for (int i = 0; i < totalDir; i++)
                {
                    MassRead.ReadToFollowing("value");
                    int dir = Int32.Parse(MassRead.GetAttribute("direction"));
                    int val = Int32.Parse(MassRead.ReadInnerXml());
                    pvList.Add(new PairValue(dir, val));
                }
                Mass m = new Mass(ID, nodeID);
                m.SetMassList = pvList;
                MainWindow.massList.Add(m);
            }
            LoadRead.Close();

            //reads the supports
            XmlReader SupRead = XmlReader.Create(pathToOpen);
            while (SupRead.ReadToFollowing("boundary"))
            {
                int ID = Int32.Parse(SupRead.GetAttribute("ID"));
                int nodeID = Int32.Parse(SupRead.GetAttribute("nodeID"));
                int totalDir = Int32.Parse(SupRead.GetAttribute("values"));
                List<PairValue> pvList = new List<PairValue>();
                for (int i = 0; i < totalDir; i++)
                {
                    SupRead.ReadToFollowing("value");
                    int dir = Int32.Parse(SupRead.GetAttribute("direction"));
                    int val = Int32.Parse(SupRead.ReadInnerXml());
                    pvList.Add(new PairValue(dir, val));
                }
                Support s = new Support(ID, nodeID);
                s.SetSupportList = pvList;
                MainWindow.supportList.Add(s);
            }
            SupRead.Close();
            DrawingHelper.DrawStructure(1);
        }

        /// <summary>
        /// Reads a txt file of the results of an analysis
        /// </summary>
        /// <param name="pathToOpen">The path of the file</param>
        public static void OpenDeformedFile(string pathToOpen)
        {
            List<string> linesList = new List<String>();
            int counter = 1;
            string[] terms;

            var lines = File.ReadAllLines(pathToOpen);
            for (var i = 0; i < lines.Length; i ++)
            {
                terms = lines[i].Split(' ');
                MainWindow.dispList.Add(new Node(counter, new Point3D(Double.Parse(terms[0]), Double.Parse(terms[1]), Double.Parse(terms[2]))));
                counter++;
            }
        }

        /// <summary>
        /// Reads a txt file of the results of an analysis
        /// </summary>
        /// <param name="pathToOpen">The path of the file</param>
        public static void OpenForceFile(string pathToOpen)
        {
            //Requires implementation, if wanted
            List<string> linesList = new List<String>();
            int counter = 1;
            string[] terms;

            var lines = File.ReadAllLines(pathToOpen);
            for (var i = 0; i < lines.Length; i++)
            {
                terms = lines[i].Split(' ');
                //MainWindow.dispList.Add(new Node(counter, new Point3D(Double.Parse(terms[0]), Double.Parse(terms[1]), Double.Parse(terms[2]))));
                counter++;
            }
        }

        /// <summary>
        /// Reads a txt file of the results of an analysis
        /// </summary>
        /// <param name="pathToOpen">The path of the file</param>
        public static void OpenSeismicFile(string pathToOpen, ref string[] list)
        {
            list = File.ReadAllLines(pathToOpen);
        }

        /// <summary>
        /// Reads a series of txt files resultant from an analysis
        /// </summary>
        /// <param name="pathsToOpen">The path of the file</param>
        public static void OpenSeriesOfDeformedFile(string[] pathsToOpen)
        {
            MainWindow.numberLoadSteps = pathsToOpen.Length;

            List<string> linesList = new List<String>();
            int counter = 1;
            string[] terms;
            int filecount = 0;
            foreach (String file in pathsToOpen)
            {
                var lines = File.ReadAllLines(file);
                List<Node> listNodes = new List<Node>();
                for (var i = 0; i < lines.Length; i++)
                {
                    terms = lines[i].Split(' ');

                    listNodes.Add(new Node(counter, new Point3D(Double.Parse(terms[0]), Double.Parse(terms[1]), Double.Parse(terms[2]))));
                    counter++;
                }
                MainWindow.seriesDispList.Add(listNodes);
                filecount++;
            }
        }

        /// <summary>
        /// Reads a series of txt files resultant from an analysis
        /// </summary>
        /// <param name="pathsToOpen">The path of the file</param>
        public static void OpenSeriesOfForceFile(string[] pathsToOpen)
        {
            List<string> linesList = new List<String>();
            int counter = 1;
            string[] terms;
            int filecount = 0;
            foreach (String file in pathsToOpen)
            {
                var lines = File.ReadAllLines(file);
                List<Load> listOfLoads = new List<Load>();
                for (var i = 0; i < lines.Length; i++)
                {
                    terms = lines[i].Split(' ');
                    Load l = new Load(counter, counter);
                    l.SetFx(Double.Parse(terms[0]));
                    l.SetFy(Double.Parse(terms[1]));
                    l.SetFz(Double.Parse(terms[2]));

                    listOfLoads.Add(l);
                    counter++;
                }
                MainWindow.seriesLoadList.Add(listOfLoads);
                filecount++;
            }
        }

        /// <summary>
        /// Reads an xml file with all the results of the structure
        /// </summary>
        /// <param name="pathToOpen">The path of the file</param>
        public static void OpenResultsFile(string pathToOpen)
        {
            XmlReader natFreqReader = XmlReader.Create(pathToOpen);
            natFreqReader.ReadToFollowing("frequencies");
            while (natFreqReader.ReadToFollowing("frequency"))
            {
                MainWindow.natFreqs.Add(double.Parse(natFreqReader.GetAttribute("value")));
            }
            natFreqReader.Close();

            XmlReader modeShapeReader = XmlReader.Create(pathToOpen);
            modeShapeReader.ReadToFollowing("modal");

            while (modeShapeReader.ReadToFollowing("mode"))
            {
                List<Node> nodeList = new List<Node>();
                int size = int.Parse(modeShapeReader.GetAttribute("total"));
                for (int i = 0; i < size; i++)
                {
                    modeShapeReader.ReadToFollowing("node");
                    Node n = new Node(i + 1, new Point3D(double.Parse(modeShapeReader.GetAttribute("x")), double.Parse(modeShapeReader.GetAttribute("y")), double.Parse(modeShapeReader.GetAttribute("z"))));
                    nodeList.Add(n);
                }
                MainWindow.modalDispList.Add(nodeList);
            }

            modeShapeReader.Close();

            XmlReader dispRead = XmlReader.Create(pathToOpen);
            while (dispRead.ReadToFollowing("displacements"))
            {
                List<Node> list = new List<Node>();
                int total = Int32.Parse(dispRead.GetAttribute("total"));
                for (int i = 0; i < total; i++)
                {
                    dispRead.ReadToFollowing("displacement");
                    double x = double.Parse(dispRead.GetAttribute("x"));
                    double y = double.Parse(dispRead.GetAttribute("y"));
                    double z = double.Parse(dispRead.GetAttribute("z"));
                    int ID = Int32.Parse(dispRead.ReadInnerXml());
                    list.Add(new Node(ID, new Point3D(x, y, z)));
                }
                MainWindow.seriesDispList.Add(list);
            }
            dispRead.Close();

            XmlReader forceRead = XmlReader.Create(pathToOpen);
            while (forceRead.ReadToFollowing("forces"))
            {
                List<Load> list = new List<Load>();
                int total = Int32.Parse(forceRead.GetAttribute("total"));
                int count = 1;
                for (int i = 0; i < total; i++)
                {
                    forceRead.ReadToFollowing("force");
                    double x = double.Parse(forceRead.GetAttribute("x"));
                    double y = double.Parse(forceRead.GetAttribute("y"));
                    double z = double.Parse(forceRead.GetAttribute("z"));
                    int ID = Int32.Parse(forceRead.ReadInnerXml());
                    Load l = new Load(count, ID);
                    l.SetFx(x);
                    l.SetFy(y);
                    l.SetFz(z);
                    list.Add(l);
                }
                MainWindow.seriesLoadList.Add(list);
            }
            forceRead.Close();
        }


        /// <summary>
        /// Configures the OpenDialog settings
        /// </summary>
        /// <param name="title">Title of the window</param>
        /// <param name="extension">Type of extension to open</param>
        /// <param name="filter">Filter of file type to show on the window</param>
        /// <returns></returns>
        public static Microsoft.Win32.OpenFileDialog SetOpenDialogParameters(string title, string extension, string filter)
        {
            Microsoft.Win32.OpenFileDialog openWindow = new Microsoft.Win32.OpenFileDialog();
            openWindow.DefaultExt = extension;
            //openWindow.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            openWindow.InitialDirectory = Directory.GetCurrentDirectory();
            openWindow.Title = title;
            openWindow.Filter = filter;
            openWindow.Multiselect = true;

            return openWindow;
        }
        
        /// <summary>
        /// Checks if the file the user selected to open is Ok to open
        /// </summary>
        public static void OpenDeformed_FileOk(object sender, System.ComponentModel.CancelEventArgs e)
        {
            string path = ((Microsoft.Win32.OpenFileDialog)sender).FileName;
            OpenOperation.OpenDeformedFile(path);
        }

        public static void OpenResult_FileOk(object sender, System.ComponentModel.CancelEventArgs e)
        {
            string path = ((Microsoft.Win32.OpenFileDialog)sender).FileName;
            OpenOperation.OpenResultsFile(path);
        }

        /// <summary>
        /// Checks if the file the user selected to open is Ok to open
        /// </summary>
        public static void OpenForce_FileOk(object sender, System.ComponentModel.CancelEventArgs e)
        {
            string path = ((Microsoft.Win32.OpenFileDialog)sender).FileName;
            OpenOperation.OpenForceFile(path);
        }

        /// <summary>
        /// Checks if the file the user selected to open is Ok to open
        /// </summary>
        public static void OpenSeismic_FileOk(object sender, System.ComponentModel.CancelEventArgs e, ref string[] stringXFile)
        {
            string path = ((Microsoft.Win32.OpenFileDialog)sender).FileName;
            OpenOperation.OpenSeismicFile(path, ref stringXFile);
        }

        /// <summary>
        /// Checks if the file the user selected to open is Ok to open
        /// </summary>
        public static void OpenSeriesOfDeformed_FileOk(object sender, System.ComponentModel.CancelEventArgs e)
        {
            string[] path = ((Microsoft.Win32.OpenFileDialog)sender).FileNames;
            OpenOperation.OpenSeriesOfDeformedFile(path);
        }


        /// <summary>
        /// Checks if the file the user selected to open is Ok to open
        /// </summary>
        public static void OpenSeriesOfForce_FileOk(object sender, System.ComponentModel.CancelEventArgs e)
        {
            string[] path = ((Microsoft.Win32.OpenFileDialog)sender).FileNames;
            OpenOperation.OpenSeriesOfForceFile(path);
        }

        /// <summary>
        /// Checks if the file the user selected to open is Ok to open
        /// </summary>
        public static void OpenStructure_FileOk(object sender, System.ComponentModel.CancelEventArgs e)
        {
            string path = ((Microsoft.Win32.OpenFileDialog)sender).FileName;

            MainWindow.fileName = Path.GetFileName(path).TrimEnd(new char[] { '.', 'x', 'm', 'l' });
            Application.Current.MainWindow.Title = "CLTFEM - " + MainWindow.fileName.TrimEnd(new char[] { '.', 'x', 'm', 'l' });
            Directory.SetCurrentDirectory(Path.GetDirectoryName(path));

            OpenOperation.OpenStructure(path);
        }

    }
}
