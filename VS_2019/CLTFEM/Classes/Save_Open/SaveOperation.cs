using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Xml;
using CLTFEM.Classes.Helpers;
using CLTFEM.Classes.Structural;
using CLTFEM.Windows;
using CLTFEM.Classes.Analysis;

namespace CLTFEM.Classes.Save_Open
{
    class SaveOperation
    {
        /// <summary>
        /// Saves the current structure to an .xml file
        /// </summary>
        /// <param name="pathToSave">The path to save the file</param>
        public static void SaveStructure(string pathToSave, List<Material> materialsList, 
                                        List<Node> nodesList, List<ShellElement> shellList, 
                                        List<Spring3D> spring3DList, List<Load> loadsList, 
                                        List<Mass> massesList, List<Support> supportsList, 
                                        List<SeismicLoad> seismicList, List<ImpulseLoad> impulseList,
                                        List<Analyses> analysesList)
        {
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.NewLineChars = "\r\n";

            XmlWriter writer = XmlWriter.Create(pathToSave, settings);

            writer.WriteStartDocument(true);
            writer.WriteStartElement("structure");

            //write the analysis type
            writer.WriteStartElement("analyses");
            int count = 1;
            foreach (Analyses analysis in analysesList)
            {
                writer.WriteStartElement("analysis");
                writer.WriteAttributeString("type", analysis.AnalysisType());
                if (analysis.AnalysisType() == "Elastic")
                {
                    ElasticAnalysis mElas = analysis as ElasticAnalysis;
                    writer.WriteAttributeString("load-steps", mElas.Steps.ToString());
                }
                else if (analysis.AnalysisType() == "Pushover")
                {
                    PushoverAnalysis push = analysis as PushoverAnalysis;
                    writer.WriteAttributeString("load-steps", push.Steps.ToString());
                    writer.WriteAttributeString("iterations", push.Iters.ToString());
                }
                else if (analysis.AnalysisType() == "Cyclic")
                {
                    CyclicAnalysis cyclic = analysis as CyclicAnalysis;
                    writer.WriteAttributeString("load-steps", cyclic.Steps.ToString());
                    writer.WriteAttributeString("iterations", cyclic.Iters.ToString());
                    writer.WriteAttributeString("initial-peak", cyclic.InitialPeak.ToString());
                    writer.WriteAttributeString("cycles-per-peak", cyclic.CyclesPerPeak.ToString());
                    writer.WriteAttributeString("peak-increment", cyclic.PeakIncrement.ToString());
                    writer.WriteAttributeString("steps-per-peak", cyclic.StepsPerPeak.ToString());
                    writer.WriteAttributeString("type", cyclic.Type.ToString());
                }
                else
                {
                    DynamicAnalysis dyn = analysis as DynamicAnalysis;
                    writer.WriteAttributeString("deltaT", dyn.DeltaT.ToString());
                    writer.WriteAttributeString("additional-time", dyn.AdditionalTime.ToString());
                    writer.WriteAttributeString("iterations", dyn.Iters.ToString());
                    writer.WriteAttributeString("itegration-method", dyn.IntegrationMethod.ToString());
                    writer.WriteAttributeString("type", dyn.Type.ToString());
                }
                writer.WriteString(count.ToString());
                count++;
                writer.WriteEndElement();
            }
            writer.WriteEndElement();

            //write the materials
            writer.WriteStartElement("materials");
            writer.WriteAttributeString("total", materialsList.Count.ToString());
            foreach (Material m in materialsList)
            {
                writer.WriteStartElement("material");
                writer.WriteAttributeString("type", m.Type);
                if (m.Type == "Elastic")
                {
                    ElasticMaterial mElas = m as ElasticMaterial;
                    writer.WriteAttributeString("elasticity", mElas.E.ToString());
                    writer.WriteAttributeString("poisson", mElas.V.ToString());
                }
                else if (m.Type == "OrthoElastic")
                {
                    OrthotropicElasticMaterial mElas = m as OrthotropicElasticMaterial;
                    writer.WriteAttributeString("ex", mElas.Ex.ToString());
                    writer.WriteAttributeString("ey", mElas.Ey.ToString());
                    writer.WriteAttributeString("vxy", mElas.Vxy.ToString());
                    writer.WriteAttributeString("gxy", mElas.Gxy.ToString());
                    writer.WriteAttributeString("gyz", mElas.Gyz.ToString());
                    writer.WriteAttributeString("gxz", mElas.Gxz.ToString());
                }
                else if (m.Type == "Spring-Axial")
                {
                    SpringAxialModel springAxial = m as SpringAxialModel;
                    writer.WriteAttributeString("initialStiffness", springAxial._iniStiff.ToString());
                    writer.WriteAttributeString("peakForce", springAxial._fMax.ToString());
                    writer.WriteAttributeString("peakDisplacement", springAxial._dMax.ToString());
                    writer.WriteAttributeString("degradingStiffness", springAxial._degStiff.ToString());
                    writer.WriteAttributeString("residualForce", springAxial._fRes.ToString());
                    writer.WriteAttributeString("ultimateDisplacement", springAxial._dUlt.ToString());
                    writer.WriteAttributeString("compressiveStiffness", springAxial._compStiff.ToString());
                    writer.WriteAttributeString("unloadStiffness", springAxial._unlStiff.ToString());
                    writer.WriteAttributeString("unloadForce", springAxial._fUnl.ToString());
                    writer.WriteAttributeString("connectStiffness", springAxial._conStiff.ToString());
                    writer.WriteAttributeString("reloadStiffness", springAxial._relStiff.ToString());
                } else
                {
                    SpringGeneralModel springGeneral = m as SpringGeneralModel;
                    writer.WriteAttributeString("initialStiffness", springGeneral._iniStiff.ToString());
                    writer.WriteAttributeString("peakForce", springGeneral._fMax.ToString());
                    writer.WriteAttributeString("peakDisplacement", springGeneral._dMax.ToString());
                    writer.WriteAttributeString("degradingStiffness", springGeneral._degStiff.ToString());
                    writer.WriteAttributeString("residualForce", springGeneral._fRes.ToString());
                    writer.WriteAttributeString("ultimateDisplacement", springGeneral._dUlt.ToString());
                    writer.WriteAttributeString("unloadStiffness", springGeneral._unlStiff.ToString());
                    writer.WriteAttributeString("unloadForce", springGeneral._fUnl.ToString());
                    writer.WriteAttributeString("connectStiffness", springGeneral._conStiff.ToString());
                    writer.WriteAttributeString("reloadStiffness", springGeneral._relStiff.ToString());
                }
                writer.WriteString(m.ID.ToString());
                writer.WriteEndElement();
            }
            writer.WriteEndElement();

            //write the loads
            writer.WriteStartElement("loads");
            writer.WriteAttributeString("total", loadsList.Count.ToString());
            foreach (Load l in loadsList)
            {
                writer.WriteStartElement("load");
                writer.WriteAttributeString("ID", l.ID.ToString());
                writer.WriteAttributeString("nodeID", l.NodeID.ToString());
                writer.WriteAttributeString("status", l.Status);
                writer.WriteAttributeString("values", l.GetLoadList.Count.ToString());
                //for each pair of dir-val in the load element
                foreach (PairValue pv in l.GetLoadList) 
                {
                    writer.WriteStartElement("value");
                    writer.WriteAttributeString("direction", pv.ID.ToString());
                    writer.WriteString(pv.GetVal.ToString());
                    writer.WriteEndElement();
                }
                writer.WriteEndElement();
            }
            writer.WriteEndElement();

            //write the masses
            writer.WriteStartElement("masses");
            writer.WriteAttributeString("total", massesList.Count.ToString());
            foreach (Mass m in massesList)
            {
                writer.WriteStartElement("mass");
                writer.WriteAttributeString("ID", m.ID.ToString());
                writer.WriteAttributeString("nodeID", m.NodeID.ToString());
                writer.WriteAttributeString("values", m.GetMassList.Count.ToString());
                //for each pair of dir-val in the mass element
                foreach (PairValue pv in m.GetMassList)
                {
                    writer.WriteStartElement("value");
                    writer.WriteAttributeString("direction", pv.ID.ToString());
                    writer.WriteString(pv.GetVal.ToString());
                    writer.WriteEndElement();
                }
                writer.WriteEndElement();
            }
            writer.WriteEndElement();

            //write the boundaries
            writer.WriteStartElement("boundaries");
            writer.WriteAttributeString("total", supportsList.Count.ToString());
            foreach (Support s in supportsList) // for each support condition applied to a node
            {
                writer.WriteStartElement("boundary");
                writer.WriteAttributeString("ID", s.ID.ToString());
                writer.WriteAttributeString("nodeID", s.NodeID.ToString());
                writer.WriteAttributeString("values", s.GetSupportList.Count.ToString());
                foreach (PairValue pv in s.GetSupportList) // for each pair of dir-val in the support element
                {
                    writer.WriteStartElement("value");
                    writer.WriteAttributeString("direction", pv.ID.ToString());
                    writer.WriteString(pv.GetVal.ToString());
                    writer.WriteEndElement();
                }
                writer.WriteEndElement();
            }
            writer.WriteEndElement();

            //write the nodes
            writer.WriteStartElement("nodes");
            writer.WriteAttributeString("total", nodesList.Count.ToString());
            foreach (Node n in nodesList)
            {
                writer.WriteStartElement("node");
                writer.WriteAttributeString("X", n.Point.X.ToString());
                writer.WriteAttributeString("Y", n.Point.Y.ToString());
                writer.WriteAttributeString("Z", n.Point.Z.ToString());
                writer.WriteString(n.ID.ToString());
                writer.WriteEndElement();
            }
            writer.WriteEndElement();

            //write the spring3D elements
            writer.WriteStartElement("spring3D");
            writer.WriteAttributeString("total", spring3DList.Count.ToString());
            foreach (Spring3D e in spring3DList)
            {
                writer.WriteStartElement("element");
                writer.WriteAttributeString("N1", e.N1.ID.ToString());
                writer.WriteAttributeString("N2", e.N2.ID.ToString());
                writer.WriteAttributeString("materialX", e.MaterialList[0].ToString());
                writer.WriteAttributeString("materialY", e.MaterialList[1].ToString());
                writer.WriteAttributeString("materialZ", e.MaterialList[2].ToString());
                writer.WriteAttributeString("axial-vec", e.VectorX.ToString());
                writer.WriteAttributeString("shear-vec", e.VectorY.ToString());
                writer.WriteString(e.ID.ToString());
                writer.WriteEndElement();
            }
            writer.WriteEndElement();

            //write the shell elements
            writer.WriteStartElement("shell");
            writer.WriteAttributeString("total", shellList.Count.ToString());
            foreach (ShellElement e in shellList)
            {
                writer.WriteStartElement("element");
                writer.WriteAttributeString("thickness", e.thickness.ToString());
                writer.WriteAttributeString("layers", e.layers.ToString());
                writer.WriteAttributeString("material", e.material.ID.ToString());
                writer.WriteAttributeString("N1", e.nodeList[0].ID.ToString());
                writer.WriteAttributeString("N2", e.nodeList[1].ID.ToString());
                writer.WriteAttributeString("N3", e.nodeList[2].ID.ToString());
                writer.WriteAttributeString("N4", e.nodeList[3].ID.ToString());
                writer.WriteAttributeString("N5", e.nodeList[4].ID.ToString());
                writer.WriteAttributeString("N6", e.nodeList[5].ID.ToString());
                writer.WriteAttributeString("N7", e.nodeList[6].ID.ToString());
                writer.WriteAttributeString("N8", e.nodeList[7].ID.ToString());
                writer.WriteAttributeString("N9", e.nodeList[8].ID.ToString());
                writer.WriteString(e.ID.ToString());
                writer.WriteEndElement();
            }
            writer.WriteEndElement();

            if (seismicList.Count != 0)
            {
                //write the seismic load
                writer.WriteStartElement("seismic");
                writer.WriteAttributeString("total", seismicList[0].Records[0].Count.ToString());
                for (int i = 0; i < seismicList[0].Records[0].Count; i++)
                {
                    writer.WriteStartElement("data-point");
                    writer.WriteAttributeString("time", (seismicList[0].DeltaT * (i + 1)).ToString());
                    for (int j = 0; j < seismicList[0].Directions.Count; j++)
                    {
                        if (seismicList[0].Directions[j] == 'x')
                        {
                            writer.WriteAttributeString("x", (seismicList[0].Records[j][i] * seismicList[0].Scales[j]).ToString());
                        }
                        else if (seismicList[0].Directions[j] == 'y')
                        {
                            writer.WriteAttributeString("y", (seismicList[0].Records[j][i] * seismicList[0].Scales[j]).ToString());
                        }
                        else
                        {
                            writer.WriteAttributeString("z", (seismicList[0].Records[j][i] * seismicList[0].Scales[j]).ToString());
                        }
                    }
                    writer.WriteString((i + 1).ToString());
                    writer.WriteEndElement();
                }
                writer.WriteEndElement();
            }

            if (impulseList.Count != 0)
            {
                //write the seismic load
                writer.WriteStartElement("impulse");
                writer.WriteAttributeString("total", impulseList[0].Points.GetLength(0).ToString());
                for (int i = 0; i < impulseList[0].Points.GetLength(0); i++)
                {
                    writer.WriteStartElement("data-point");
                    writer.WriteAttributeString("time", (impulseList[0].Points[i, 0]).ToString());
                    writer.WriteAttributeString("force", (impulseList[0].Points[i, 1]).ToString());
                    writer.WriteString((i + 1).ToString());
                    writer.WriteEndElement();
                }
                writer.WriteEndElement();
            }
            writer.WriteEndDocument();
            writer.Close();
        }

        /// <summary>
        /// Configures the save window
        /// </summary>
        /// <param name="title">Title of the window</param>
        /// <param name="extension">Exension of the file that it will save</param>
        /// <param name="filter">Filter by only files of the same types</param>
        public static Microsoft.Win32.SaveFileDialog SetSaveDialogParameters(string title, string extension, string filter)
        {
            Microsoft.Win32.SaveFileDialog saveWindow = new Microsoft.Win32.SaveFileDialog();
            saveWindow.DefaultExt = extension;
            saveWindow.AddExtension = true;
            //saveWindow.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            saveWindow.InitialDirectory = Directory.GetCurrentDirectory();
            saveWindow.Title = title;
            saveWindow.Filter = filter;
            saveWindow.FileOk += SaveWindow_FileOk;

            return saveWindow;
        }

        /// <summary>
        /// Checks if the file the user is saving is OK to save
        /// </summary>
        private static void SaveWindow_FileOk(object sender, System.ComponentModel.CancelEventArgs e)
        {
            string path = ((Microsoft.Win32.SaveFileDialog)sender).FileName;

            MainWindow.savePath = Path.GetDirectoryName(path);
            MainWindow.fileName = Path.GetFileName(path);

            Application.Current.MainWindow.Title = "CLTFEM - " + MainWindow.fileName.TrimEnd(new char[] { '.', 'x', 'm', 'l' });

            Directory.SetCurrentDirectory(MainWindow.savePath);

            SaveOperation.SaveStructure(path, new List<Material>(MainWindow.materialList),
                new List<Node>(MainWindow.nodeList),
                new List<ShellElement>(MainWindow.shellList),
                new List<Spring3D>(MainWindow.springList),
                new List<Load>(MainWindow.loadList),
                new List<Mass>(MainWindow.massList),
                new List<Support>(MainWindow.supportList),
                new List<SeismicLoad>(MainWindow.seismicLoad),
                new List<ImpulseLoad>(MainWindow.impulseLoad),
                new List<Analyses>(MainWindow.analysis));
        }
        
    }
}



