using System.Windows;
using CLTFEM.Classes.Helpers;
using CLTFEM.Classes.Structural;
using System.Collections.ObjectModel;
using System;
using System.Windows.Input;
using System.Windows.Media.Media3D;
using System.Collections.Generic;
using System.Windows.Media;
using CLTFEM.Classes.Save_Open;
using System.Windows.Media.Animation;
using Material = CLTFEM.Classes.Structural.Material;
using CLTFEM.Classes.Analysis;
using CLTFEM.UserInterfaces;
using System.Windows.Controls;
using System.Diagnostics;
using System.IO;

namespace CLTFEM.Windows
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        //initialize camera properties
        private System.Windows.Media.Media3D.PerspectiveCamera myPerspCamera; //initialize the variable for the perspective camera
        private System.Windows.Media.Media3D.OrthographicCamera myOrthoCamera; //initialize the variable for the orthographic camera
        private double camRadius; //initialize the variable to store the radius of the camera and help with turning and zooming the camera
        //initialize the variable for the rotations of the camera
        System.Windows.Media.Media3D.AxisAngleRotation3D camRotX = new System.Windows.Media.Media3D.AxisAngleRotation3D(new System.Windows.Media.Media3D.Vector3D(1, 0, 0), 0);
        System.Windows.Media.Media3D.AxisAngleRotation3D camRotY = new System.Windows.Media.Media3D.AxisAngleRotation3D(new System.Windows.Media.Media3D.Vector3D(0, 1, 0), 0);
        System.Windows.Media.Media3D.AxisAngleRotation3D camRotZ = new System.Windows.Media.Media3D.AxisAngleRotation3D(new System.Windows.Media.Media3D.Vector3D(0, 0, 1), 0);

        public static string savePath; //keeps track of where the file has been saved
        public static string fileName; //keeps track of the name of the file being used

        // initialize the variable for the 3D Graphics model
        public static System.Windows.Media.Media3D.ModelVisual3D myModelVisual3D = new System.Windows.Media.Media3D.ModelVisual3D();
        public static System.Windows.Media.Media3D.Model3DGroup myModel3DGroup = new System.Windows.Media.Media3D.Model3DGroup();

        // sets up the list of important objects
        public static ObservableCollection<Node> nodeList = new ObservableCollection<Node>(); //initialize the variable for the list of nodes in the project
        public static ObservableCollection<ShellElement> shellList = new ObservableCollection<ShellElement>(); //initialize the variable for the list of elements in the project
        public static ObservableCollection<Spring3D> springList = new ObservableCollection<Spring3D>(); //initialize the variable for the list of elements in the project
        public static ObservableCollection<Material> materialList = new ObservableCollection<Material>(); //initialize the variable for the list of materials in the project
        public static ObservableCollection<Load> loadList = new ObservableCollection<Load>(); //initialize the variable for the list of loads in the project
        public static ObservableCollection<Support> supportList = new ObservableCollection<Support>(); //initialize the variable for the list of supports in the project
        public static ObservableCollection<Mass> massList = new ObservableCollection<Mass>(); //initialize the variable for the list of masses in the project
        public static ObservableCollection<SeismicLoad> seismicLoad = new ObservableCollection<SeismicLoad>();
        public static ObservableCollection<ImpulseLoad> impulseLoad = new ObservableCollection<ImpulseLoad>();
        public static ObservableCollection<Analyses> analysis = new ObservableCollection<Analyses>();
        public static List<Node> dispList = new List<Node>(); //to hold the displaced values of nodes read from the file
        public static List<List<Node>> seriesDispList = new List<List<Node>>(); //to hold the displaced values of nodes read from the file
        public static List<List<Load>> seriesLoadList = new List<List<Load>>(); //list of loads resultant in the analyses
        public static List<List<Node>> modalDispList = new List<List<Node>>(); //to hold the displaced values of nodes read from the file
        public static List<double> natFreqs = new List<double>();

        public static int iteration = 1; //keeps track of which iteration is being displaced in the deformed shape view
        public static double scale = 1; //keeps track of the scale of the deformations being displayed

        public static string vertAxisProp = "totForce";
        public static string horAxisProp = "";
        public static int graphNodeID;
        public static int graphVertNodeID;

        public static int numberLoadSteps = 0;

        public MainWindow()
        {
            InitializeComponent();

            //Setup Camera
            SceneHelper.setupPerspCamera(ref myPerspCamera, Configuration.initialCamPos, Configuration.initialCamLookingDir, Configuration.perspCamFOV); //set the perspective camera as the default
            camRadius = SceneHelper.camRadius(myPerspCamera.Position.X, myPerspCamera.Position.Z);
            myPerspCamera.Transform = SceneHelper.camTransformGroup(camRotX, camRotY, camRotZ); //apply the transformations to the camera so it can be changed by the code later on
            //camRotX.Angle += 90;
            myViewPort.Camera = myPerspCamera; // Asign the camera to the viewport
           
            // Define the lights cast in the scene.
            SceneHelper.setupLighting(myModel3DGroup);

            //create the x-y-z axes
            DrawingHelper.createAxes(ref myModel3DGroup);
            myModelVisual3D.Content = myModel3DGroup;
            myViewPort.Children.Add(myModelVisual3D);

            Directory.SetCurrentDirectory(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments));

            //RenderOptions.ProcessRenderMode = System.Windows.Interop.RenderMode.SoftwareOnly;

            //MessageBox.Show((RenderCapability.Tier / 0x10000).ToString());
        }

        //Below methods reflect actions when the user uses the mouse on the 3D screen (rotation, panning, etc)

        Point lastMousePos; // registers the last mouse position when the mouse moves over the screen
        Point firstMousePos; // registers the mouse position when the a mouse button is pressed
        System.Windows.Media.Media3D.Point3D firstCamPos; // registers the camera position when the a mouse button is pressed
        int countRight = 0; // auxiliary variable to 'count' when the first right click has been clicked
        int countMiddle = 0; // auxiliary variable to 'count' when the first middle click has been clicked

        /// <summary>
        /// Defines what happens when the user moves the mouse on top of the 3D screen
        /// </summary>
        private void ViewPortBackground_MouseMove(object sender, MouseEventArgs e)
        {

            //get the mouse position and display it on the screen
            Point mousePos = new Point((e.GetPosition(myViewPort).X - myViewPort.ActualWidth / 2) / Configuration.mouse3DSens, -(e.GetPosition(myViewPort).Y - myViewPort.ActualHeight / 2) / Configuration.mouse3DSens);
            lbl_mouseX.Content = e.GetPosition(myViewPort).X.ToString("F");
            lbl_mouseY.Content = e.GetPosition(myViewPort).Y.ToString("F");

            //lbl_camVecX.Content = camRotX.Angle.ToString();
            //lbl_camVecY.Content = camRotY.Angle.ToString();
            lbl_camVecZ.Content = camRotZ.Angle.ToString();

            // Controls what happens when the right mouse button is pressed while moving the mouse
            if (e.RightButton == MouseButtonState.Pressed)
            {
                countRight++;
                if (countRight == 1)
                {
                    lastMousePos = mousePos;
                    firstMousePos = mousePos;
                }

                //calculates the difference between the last registered mouse point and the actual mouse point to determine if it is going up/down/left/right
                double difX = mousePos.X - lastMousePos.X;
                double difY = mousePos.Y - lastMousePos.Y;

                //calculates the length between the first registered mouse point (when the button was pressed) and the actual mouse point to determine the intensity of the action
                double lengthX = mousePos.X - firstMousePos.X;
                double lengthY = mousePos.Y - firstMousePos.Y;

                //MessageBox.Show(myPerspCamera.UpDirection.ToString());

                if (difX <= 0)
                {
                    camRotZ.Angle += Configuration.mouseRotSens * Math.Abs(lengthX);
                }
                else
                {
                    camRotZ.Angle -= Configuration.mouseRotSens * Math.Abs(lengthX);
                }
                if (difY <= 0)
                {
                    camRotY.Angle -= Configuration.mouseRotSens * Math.Abs(lengthY);
                }
                else
                {
                    camRotY.Angle += Configuration.mouseRotSens * Math.Abs(lengthY);
                }
            }
            else
            {
                //reset the count to 0 when the user releases the button
                countRight = 0;
            }

            // Controls what happens when the middle mouse button is pressed while moving the mouse
            if (e.MiddleButton == MouseButtonState.Pressed)
            {
                countMiddle++;
                if (countMiddle == 1)
                {
                    firstMousePos = mousePos;
                    if (myViewPort.Camera.GetType().ToString() == "System.Windows.Media.Media3D.PerspectiveCamera")
                    {
                        firstCamPos = myPerspCamera.Position;
                    }
                    else
                    {
                        firstCamPos = myOrthoCamera.Position;
                    }

                }

                //calculates the length between the first registered mouse point (when the button was pressed) and the actual mouse point to determine the intensity of the action
                double lengthX = Configuration.mouse3DSens * (mousePos.X - firstMousePos.X);
                double lengthY = Configuration.mouse3DSens * (mousePos.Y - firstMousePos.Y);
                lbl_length.Content = lengthX.ToString("F");

                if (myViewPort.Camera.GetType().ToString() == "System.Windows.Media.Media3D.PerspectiveCamera")
                {
                    myPerspCamera.Position = new Point3D(firstCamPos.X, firstCamPos.Y - lengthX, firstCamPos.Z - lengthY);
                }
                else
                {
                    myOrthoCamera.Position = new Point3D(firstCamPos.X, firstCamPos.Y - lengthX, firstCamPos.Z - lengthY);
                }

            }
            else
            {
                //reset the count to 0 when the user releases the button
                countMiddle = 0;
            }

            //reset the last mouse position to the actual mouse position every time the user moves the mouse
            lastMousePos = mousePos;

        }

        /// <summary>
        /// Defines what happens when the user uses the mouse wheel over the 3D screen
        /// </summary>
        private void ViewPortBackground_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            //creates a point which will serve as the increment to be added to the camera position based on the mouse wheel action
            var point = new Point3D(-e.Delta * Configuration.mouseScrollSens / 120, 0, 0); //120 is the recommended by microsoft

            //Sends the data to the label in the window
            lbl_camPos.Content = "(" + myPerspCamera.Position.X.ToString() + "," + myPerspCamera.Position.Y.ToString() + "," + myPerspCamera.Position.Z.ToString() + ")";

            //increment the camera position depending on the camera type
            if (myViewPort.Camera.GetType().ToString() == "System.Windows.Media.Media3D.PerspectiveCamera")
            {
                if ((myPerspCamera.Position.X + point.X) > Configuration.mouseScrollLimit)
                {
                    var added = new Point3D(myPerspCamera.Position.X + point.X, myPerspCamera.Position.Y + point.Y, myPerspCamera.Position.Z + point.Z);
                    myPerspCamera.Position = added;
                    camRadius = SceneHelper.camRadius(myPerspCamera.Position.X, myPerspCamera.Position.Z);
                }
            }
            else
            {
                if ((myOrthoCamera.Position.X + point.X) > Configuration.mouseScrollLimit)
                {
                    var added = new Point3D(myOrthoCamera.Position.X + point.X, myOrthoCamera.Position.Y + point.Y, myOrthoCamera.Position.Z + point.Z);
                    myOrthoCamera.Width += point.X;
                    myOrthoCamera.Position = added;
                    camRadius = SceneHelper.camRadius(myOrthoCamera.Position.X, myOrthoCamera.Position.Z);
                }
            }

            double newZoomParam = 3 * myPerspCamera.Position.X / Configuration.initialCamPos.X;

            lbl_camVecY.Content = Configuration.zoomParam;
            lbl_camVecX.Content = newZoomParam;

            if (newZoomParam / Configuration.zoomParam > 1.5 || newZoomParam / Configuration.zoomParam < 0.667)
            {
                if (newZoomParam < Configuration.minZoomParam)
                {
                    Configuration.zoomParam = Configuration.minZoomParam;
                }
                else
                {
                    Configuration.zoomParam = newZoomParam;
                }

                DrawingHelper.ClearScreen(ref myModel3DGroup);
                DrawingHelper.DrawStructure(Configuration.zoomParam);
            }
        }

        /// <summary>
        /// Defines what happens when the user click on an 3D object
        /// </summary>
        private void myViewPort_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                Point p = e.GetPosition(myViewPort);
                //MessageBox.Show(p.ToString());
                VisualTreeHelper.HitTest(myViewPort, null, HitTestResult, new PointHitTestParameters(p));
            }
        }

        public bool nodeHit = false;
        /// <summary>
        /// Defines what happens when the user clicks on an object on the Screen
        /// </summary>
        public HitTestResultBehavior HitTestResult(HitTestResult result)
        {
            //MessageBox.Show("You hit something!");
            RayMeshGeometry3DHitTestResult rayHTResult = result as RayMeshGeometry3DHitTestResult;
            //MessageBox.Show(rayHTResult.VisualHit.GetType().ToString());
            if (rayHTResult != null)
            {
                // Did we hit a MeshGeometry3D?
                RayMeshGeometry3DHitTestResult rayMeshResult =
                    rayHTResult as RayMeshGeometry3DHitTestResult;

                GeometryModel3D hitGeometry = rayHTResult.ModelHit as GeometryModel3D;

                double x = 0, y = 0, z = 0;
                int count = rayHTResult.MeshHit.Positions.Count;
                for (int i = 0; i < count; i++)
                {
                    x += rayHTResult.MeshHit.Positions[i].X;
                    y += rayHTResult.MeshHit.Positions[i].Y;
                    z += rayHTResult.MeshHit.Positions[i].Z;
                }

                Point3D centroid = new Point3D(x / count, y / count, z / count);

                int hitIndex = -1;
                DiffuseMaterial materialHit = hitGeometry.Material as DiffuseMaterial;
                SolidColorBrush brushHit = materialHit.Brush as SolidColorBrush;

                if (brushHit.Color == Configuration.shellEleColor)
                {
                    if (nodeHit == false)
                    {
                        for (int i = 0; i < shellList.Count; i++) //for each element in the shell element list
                        {
                            Point3D val = shellList[i].GetCentroidPoint();
                            if (Math.Round(centroid.X, 3) == Math.Round(val.X, 3) && Math.Round(centroid.Y, 3) == Math.Round(val.Y, 3) && Math.Round(centroid.Z, 3) == Math.Round(val.Z, 3)) //the rounding needs to be done due to loss of precision with double variables 
                            {
                                hitIndex = i;
                                break;
                            }
                        }

                        if (hitIndex != -1)
                        {
                            ShellElement shell = shellList[hitIndex];
                            ColorAnimation animSelect = new ColorAnimation(Configuration.selectedItemColor, new Duration(TimeSpan.FromMilliseconds(Configuration.selectAnimDur)));
                            ColorAnimation animUnselect = new ColorAnimation(Configuration.shellEleColor, new Duration(TimeSpan.FromMilliseconds(Configuration.selectAnimDur)));
                            brushHit.BeginAnimation(SolidColorBrush.ColorProperty, animSelect);
                            MessageBox.Show(shell.ToString());
                            brushHit.BeginAnimation(SolidColorBrush.ColorProperty, animUnselect);
                        }
                    }
                    else
                    {
                        //everytime the user click a node, it also hit the element uderneath it. On the second run of this routine, the nodeHit will be true, and therefore, won't activate the shell element hit functions
                        //Thus, this following code will be run instead, reseting the nodeHit variable to false, enabling the user to clikc a shell element and reading its result.
                        nodeHit = false;
                    }
                }
                if (brushHit.Color == Configuration.nodeColor)
                {
                    nodeHit = true;
                    for (int i = 0; i < nodeList.Count; i++) //for each element in the shell element list
                    {
                        Point3D val = nodeList[i].Point;
                        if (Math.Round(centroid.X, 3) == Math.Round(val.X, 3) && Math.Round(centroid.Y, 3) == Math.Round(val.Y, 3) && Math.Round(centroid.Z, 3) == Math.Round(val.Z, 3)) //the rounding needs to be done due to loss of precision with double variables 
                        {
                            hitIndex = i;
                            break;
                        }
                    }

                    if (hitIndex != -1)
                    {
                        Node node = nodeList[hitIndex];
                        ColorAnimation animSelect = new ColorAnimation(Configuration.selectedItemColor, new Duration(TimeSpan.FromMilliseconds(Configuration.selectAnimDur)));
                        ColorAnimation animUnselect = new ColorAnimation(Configuration.nodeColor, new Duration(TimeSpan.FromMilliseconds(Configuration.selectAnimDur)));
                        brushHit.BeginAnimation(SolidColorBrush.ColorProperty, animSelect);
                        MessageBox.Show(node.ToString());
                        brushHit.BeginAnimation(SolidColorBrush.ColorProperty, animUnselect);
                    }

                }

                if (rayMeshResult != null)
                {
                    // Yes we did!
                }
            }

            return HitTestResultBehavior.Continue;
        }

        // Below methods reflect actions when the user clicks on screen buttons

        /// <summary>
        /// Defines what happens when the user clicks the 'exit' button on the top menu
        /// </summary>
        private void Menu_Exit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Defines what happens when the user clicks the 'save' button on the top menu
        /// </summary>
        private void Menu_Save_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.SaveFileDialog saveWindow = SaveOperation.SetSaveDialogParameters("Save", ".xml", "XML File (*.xml)|*.xml");
            saveWindow.ShowDialog();
        }

        /// <summary>
        /// Defines what happens when the user clicks the 'open' button on the top menu
        /// </summary>
        private void Menu_Open_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog openWindow = OpenOperation.SetOpenDialogParameters("Open",".xml","XML File (*.xml)|*.xml");
            openWindow.FileOk += OpenOperation.OpenStructure_FileOk;
            openWindow.ShowDialog();
        }

        private void Button_OpenResult_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog openWindow = OpenOperation.SetOpenDialogParameters("Open Results", ".res", "Results File (*.res)|*.res");
            openWindow.FileOk += OpenOperation.OpenResult_FileOk;
            openWindow.ShowDialog();
        }

        /// <summary>
        /// Defines what happens when the user changes from the orthographics to the perspective camera on the top menu
        /// </summary>
        private void MenuItem_Camera_Pesp_Click(object sender, RoutedEventArgs e)
        {
            if (myViewPort.Camera.GetType().ToString() == "System.Windows.Media.Media3D.OrthographicCamera")
            {
                System.Windows.Media.Media3D.Point3D camPos = myOrthoCamera.Position;
                System.Windows.Media.Media3D.Vector3D camDir = myOrthoCamera.LookDirection;

                myPerspCamera.Position = camPos;
                myPerspCamera.LookDirection = camDir;
                myViewPort.Camera = myPerspCamera;
            }
        }

        /// <summary>
        /// Defines what happens when the user changes from the perspective to the orthographic camera on the top menu
        /// </summary>
        private void MenuItem_Camera_Ortho_Click(object sender, RoutedEventArgs e)
        {
            if (myViewPort.Camera.GetType().ToString() == "System.Windows.Media.Media3D.PerspectiveCamera")
            {
                System.Windows.Media.Media3D.Point3D camPos = myPerspCamera.Position;
                System.Windows.Media.Media3D.Vector3D camDir = myPerspCamera.LookDirection;

                SceneHelper.setupOrthoCamera(ref myOrthoCamera, camPos, camDir);
                myOrthoCamera.Width = myOrthoCamera.Position.X;
                myOrthoCamera.Transform = SceneHelper.camTransformGroup(camRotX, camRotY, camRotZ);
                myViewPort.Camera = myOrthoCamera;
            }
        }

        /// <summary>
        /// Draws the node numbers on the screen
        /// </summary>
        private void Button_NodeNumbers_Click(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < nodeList.Count; i++)
            {
                //not really used
                myModelVisual3D.Children.Add(DrawingHelper.CreateTextLabel3D(nodeList[i].ID.ToString(), Brushes.Black, true, 10, new Point3D(nodeList[i].Point.X - 5, nodeList[i].Point.Y - 10, nodeList[i].Point.Z), new Vector3D(0, 1, 0), new Vector3D(0, 0, 1)));
            }
        }

        /// <summary>
        /// Initiates the process of opening a results file containing deformation results
        /// </summary>
        private void Button_OpenDeformed_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog openWindow = OpenOperation.SetOpenDialogParameters("Open",".txt","TXT File (*.txt)|*.txt");
            openWindow.FileOk += OpenOperation.OpenDeformed_FileOk;
            openWindow.ShowDialog();
        }

        public static bool deformed = false;
        /// <summary>
        /// Shows the deformed shape of the structure
        /// </summary>
        private void Button_ShowDeformed_Click(object sender, RoutedEventArgs e)
        {
            if (deformed == false)
            {
                DrawingHelper.ClearScreen(ref myModel3DGroup);

                List<Node> dispListScaled = DrawingHelper.GetDispListScaled(dispList, scale);

                for (int i = 0; i < shellList.Count; i++)
                {
                    myModel3DGroup.Children.Add(DrawingHelper.Draw3DPanel(shellList[i], Configuration.shellEleColor, true, dispListScaled, Configuration.zoomParam));
                }
                for (int i = 0; i < springList.Count; i++)
                {
                    myModel3DGroup.Children.Add(DrawingHelper.Draw3DSprings(springList[i], Configuration.spring3DColor, true, dispListScaled));
                }
                DrawingHelper.DrawBoundaryConditions(new List<Support>(supportList), true, dispListScaled, Configuration.zoomParam);
                DrawingHelper.DrawLoads(new List<Load>(loadList), true, dispListScaled, Configuration.zoomParam);
                DrawingHelper.DrawMasses(new List<Mass>(massList), true, dispListScaled, Configuration.zoomParam);
            } 
            deformed = !deformed;
        }

        /// <summary>
        /// Defines what happens when the user clicks the 'add materials' button on the side menu
        /// </summary>
        public bool materialButtonClicked = false;
        private void Button_Materials_Click(object sender, RoutedEventArgs e)
        {
            CheckIfOpenedUserControls(materialUserControl);
            if (!materialButtonClicked)
            {
                materialUserControl.Visibility = Visibility.Visible;
                materialButtonClicked = true;
            }
            else
            {
                materialUserControl.Visibility = Visibility.Collapsed;
                materialButtonClicked = false;
            }
            /*
            Material_Creation window = new Material_Creation();
            window.Show();
            */
        }


        /// <summary>
        /// Defines what happens when the user clicks the 'add node' button on the side menu
        /// </summary>
        public bool nodeButtonClicked = false;
        private void Button_Nodes_Click(object sender, RoutedEventArgs e)
        {
            CheckIfOpenedUserControls(nodeUserControl);
            if (!nodeButtonClicked)
            {
                nodeUserControl.Visibility = Visibility.Visible;
                nodeButtonClicked = true;
            }
            else
            {
                nodeUserControl.Visibility = Visibility.Collapsed;
                nodeButtonClicked = false;
            }
            /*
            Node_Creation window = new Node_Creation();
            window.Show();
            */
        }

        /// <summary>
        /// Open the 'Define Loads' windows
        /// </summary>
        public bool loadButtonClicked = false;
        private void Button_DefLoads_Click(object sender, RoutedEventArgs e)
        {
            CheckIfOpenedUserControls(loadUserControl);
            if (!loadButtonClicked)
            {
                loadUserControl.Visibility = Visibility.Visible;
                loadButtonClicked = true;
            }
            else
            {
                loadUserControl.Visibility = Visibility.Collapsed;
                loadButtonClicked = false;
            }

            /*
            Load_Definition window = new Load_Definition();
            window.Show();
            */
        }

        public bool seismicLoadButtonClicked = false;
        private void Button_SeismicLoads_Click(object sender, RoutedEventArgs e)
        {
            CheckIfOpenedUserControls(seismicLoadUserControl);
            if (!seismicLoadButtonClicked)
            {
                seismicLoadUserControl.Visibility = Visibility.Visible;
                seismicLoadButtonClicked = true;
            }
            else
            {
                seismicLoadUserControl.Visibility = Visibility.Collapsed;
                seismicLoadButtonClicked = false;
            }
        }

        public bool impulseLoadButtonClicked = false;
        private void Button_ImpulseLoads_Click(object sender, RoutedEventArgs e)
        {
            CheckIfOpenedUserControls(impulseLoadUserControl);
            if (!impulseLoadButtonClicked)
            {
                impulseLoadUserControl.Visibility = Visibility.Visible;
                impulseLoadButtonClicked = true;
            }
            else
            {
                impulseLoadUserControl.Visibility = Visibility.Collapsed;
                impulseLoadButtonClicked = false;
            }
        }

        public bool massButtonClicked = false;
        private void Button_DefMasses_Click(object sender, RoutedEventArgs e)
        {
            CheckIfOpenedUserControls(massUserControl);
            if (!massButtonClicked)
            {
                massUserControl.Visibility = Visibility.Visible;
                massButtonClicked = true;
            }
            else
            {
                massUserControl.Visibility = Visibility.Collapsed;
                massButtonClicked = false;
            }
        }

        /// <summary>
        /// Open the 'Define Boundary Conditions' window
        /// </summary>
        public bool supButtonClicked = false;
        private void Button_DefBounds_Click(object sender, RoutedEventArgs e)
        {
            CheckIfOpenedUserControls(supportUserControl);
            if (!supButtonClicked)
            {
                supportUserControl.Visibility = Visibility.Visible;
                supButtonClicked = true;
            }
            else
            {
                supportUserControl.Visibility = Visibility.Collapsed;
                supButtonClicked = false;
            }
            
            /*
            Boundary_Definition window = new Boundary_Definition();
            window.Show();
            */
        }

        public bool analysisPropButtonClicked = false;
        private void Button_DefineAnalysis_Click(object sender, RoutedEventArgs e)
        {
            CheckIfOpenedUserControls(analysisPropertiesUserControl);
            if (!analysisPropButtonClicked)
            {
                analysisPropertiesUserControl.Visibility = Visibility.Visible;
                analysisPropButtonClicked = true;
            }
            else
            {
                analysisPropertiesUserControl.Visibility = Visibility.Collapsed;
                analysisPropButtonClicked = false;
            }
        }

        /// <summary>
        /// Initiates the process of clearing all the 3D objects from the scene and deleting all structural information
        /// </summary>
        private void Button_ClearStructure_Click(object sender, RoutedEventArgs e)
        {
            Management.CleanCurrentStrucure();
        }

        /// <summary>
        /// Defines what happens when the user clicks the 'add shells' button on the side menu
        /// </summary>
        public bool shellButtonClicked = false;
        private void Button_Shells_Click(object sender, RoutedEventArgs e)
        {
            CheckIfOpenedUserControls(shellUserControl);
            if (!shellButtonClicked)
            {
                shellUserControl.Visibility = Visibility.Visible;
                shellButtonClicked = true;
            }
            else
            {
                shellUserControl.Visibility = Visibility.Collapsed;
                shellButtonClicked = false;
            }

            /*
            Element_Creation window = new Element_Creation();
            window.Show();
            */
        }

        /// <summary>
        /// Defines what happens when the user clicks the 'add spring' button on the side menu
        /// </summary>
        public bool springButtonClicked = false;
        private void Button_Springs_Click(object sender, RoutedEventArgs e)
        {
            CheckIfOpenedUserControls(spring3DUserControl);
            if (!springButtonClicked)
            {
                spring3DUserControl.Visibility = Visibility.Visible;
                springButtonClicked = true;
            }
            else
            {
                spring3DUserControl.Visibility = Visibility.Collapsed;
                springButtonClicked = false;
            }

            /*
            Spring3D_Creation window = new Spring3D_Creation();
            window.Show();
            */
        }

        public bool graphPropButtonClicked = false;
        private void Button_SelectGraphProp_Click(object sender, RoutedEventArgs e)
        {
            CheckIfOpenedUserControls(graphPropUserControl);
            if (!graphPropButtonClicked)
            {
                graphPropUserControl.Visibility = Visibility.Visible;
                graphPropButtonClicked = true;
            }
            else
            {
                graphPropUserControl.Visibility = Visibility.Collapsed;
                graphPropButtonClicked = false;
            }
        }

        /// <summary>
        /// Opens up a series of loadstages result files
        /// </summary>
        private void Button_LoadStageResult_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog openWindow = OpenOperation.SetOpenDialogParameters("Open", ".txt", "TXT File (*.txt)|*.txt");
            openWindow.FileOk += OpenOperation.OpenSeriesOfDeformed_FileOk;
            openWindow.ShowDialog();
        }


        private void Button_OpenForceDeformed_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog openWindow = OpenOperation.SetOpenDialogParameters("Open", ".txt", "TXT File (*.txt)|*.txt");
            openWindow.FileOk += OpenOperation.OpenDeformed_FileOk;
            openWindow.ShowDialog();
        }

        private void Button_ForceLoadStageResult_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog openWindow = OpenOperation.SetOpenDialogParameters("Open", ".txt", "TXT File (*.txt)|*.txt");
            openWindow.FileOk += OpenOperation.OpenSeriesOfForce_FileOk;
            openWindow.ShowDialog();
        }

        /// <summary>
        /// Initiates the process of showing the results of individual load stages
        /// </summary>
        private void Button_Loadsteps_Click(object sender, RoutedEventArgs e)
        {
            ShowUndeformed();

            DrawingHelper.ClearScreen(ref myModel3DGroup);

            List<Node> dispListScaled = DrawingHelper.GetDispListScaled(seriesDispList[iteration - 1], scale);

            if (deformed == false)
            {
                iterCount.Content = 1;
                for (int i = 0; i < shellList.Count; i++)
                {
                    myModel3DGroup.Children.Add(DrawingHelper.Draw3DPanel(shellList[i], Configuration.shellEleColor, true, dispListScaled, Configuration.zoomParam));
                }
                for (int i = 0; i < springList.Count; i++)
                {
                    myModel3DGroup.Children.Add(DrawingHelper.Draw3DSprings(springList[i], Configuration.spring3DColor, true, dispListScaled));
                }
                DrawingHelper.DrawBoundaryConditions(new List<Support>(supportList), true, dispListScaled, Configuration.zoomParam);
                DrawingHelper.DrawLoads(new List<Load>(loadList), true, dispListScaled, Configuration.zoomParam);
                DrawingHelper.DrawMasses(new List<Mass>(massList), true, dispListScaled, Configuration.zoomParam);
                loadStepViewerPanel.Visibility = Visibility.Visible;
            }
            deformed = !deformed;
        }

        private void Button_ShowUndeformed_Click(object sender, RoutedEventArgs e)
        {
            ShowUndeformed();
        }

        private void ShowUndeformed()
        {
            if (deformed == true || modalShape == true)
            {
                DrawingHelper.ClearScreen(ref myModel3DGroup);
                for (int i = 0; i < nodeList.Count; i++)
                {
                    myModel3DGroup.Children.Add(DrawingHelper.Draw3DCube(Configuration.nodeSize * Configuration.zoomParam, nodeList[i].Point, Configuration.nodeColor));
                }
                for (int i = 0; i < shellList.Count; i++)
                {
                    myModel3DGroup.Children.Add(DrawingHelper.Draw3DPanel(shellList[i], Configuration.shellEleColor, false, null, Configuration.zoomParam));
                }
                DrawingHelper.DrawBoundaryConditions(new List<Support>(supportList), false, null, Configuration.zoomParam);
                DrawingHelper.DrawLoads(new List<Load>(loadList), false, null, Configuration.zoomParam);
                DrawingHelper.DrawMasses(new List<Mass>(massList), false, null, Configuration.zoomParam);
                loadStepViewerPanel.Visibility = Visibility.Collapsed;
                natFreqViewPanel.Visibility = Visibility.Collapsed;
                deformed = false;
                modalShape = false;
            }
        }

        /// <summary>
        /// Shows the next set of results from a load stage collection
        /// </summary>
        private void Button_NextLoadStep_Click(object sender, RoutedEventArgs e)
        {
            DrawingHelper.ClearScreen(ref myModel3DGroup);
            List<Node> dispListScaled;
            int nIter = 0;
            if (deformed)
            {
                nIter = seriesDispList.Count;
            } else if (modalShape)
            {
                nIter = modalDispList.Count;
            }

            if ((int)iterCount.Content == nIter)
            {
                iteration = 1;
            }
            else
            {
                iteration = (int)iterCount.Content + 1;
            }

            iterCount.Content = iteration;
            if (modalShape == true)
            {
                natFreq.Content = natFreqs[iteration - 1].ToString("F");
            }

            if (deformed)
            {
                dispListScaled = DrawingHelper.GetDispListScaled(seriesDispList[iteration - 1], scale);
            }
            else if (modalShape)
            {
                dispListScaled = DrawingHelper.GetDispListScaled(modalDispList[iteration - 1], scale);
            } else
            {
                dispListScaled = null;
            }

            for (int i = 0; i < shellList.Count; i++)
            {
                myModel3DGroup.Children.Add(DrawingHelper.Draw3DPanel(shellList[i], Configuration.shellEleColor, true, dispListScaled, Configuration.zoomParam));
            }
            for (int i = 0; i < springList.Count; i++)
            {
                myModel3DGroup.Children.Add(DrawingHelper.Draw3DSprings(springList[i], Configuration.spring3DColor, true, dispListScaled));
            }
            DrawingHelper.DrawBoundaryConditions(new List<Support>(supportList), true, dispListScaled, Configuration.zoomParam);
            DrawingHelper.DrawLoads(new List<Load>(loadList), true, dispListScaled, Configuration.zoomParam);
            DrawingHelper.DrawMasses(new List<Mass>(massList), true, dispListScaled, Configuration.zoomParam);
        }

        /// <summary>
        /// Shows the previous set of results from a load stage collection
        /// </summary>
        private void Button_PreviousLoadStep_Click(object sender, RoutedEventArgs e)
        {
            DrawingHelper.ClearScreen(ref myModel3DGroup);
            if ((int)iterCount.Content == 1)
            {
                if (deformed)
                {
                    iteration = seriesDispList.Count;
                }
                else if (modalShape)
                {
                    iteration =  modalDispList.Count;
                }
            } else
            {
                iteration = (int)iterCount.Content - 1;
            }

            iterCount.Content = iteration;
            if (modalShape == true)
            {
                natFreq.Content = natFreqs[iteration - 1].ToString("F");
            }

            List<Node> dispListScaled;
            if (deformed)
            {
                dispListScaled = DrawingHelper.GetDispListScaled(seriesDispList[iteration - 1], scale);
            } else if (modalShape)
            {
                dispListScaled = DrawingHelper.GetDispListScaled(modalDispList[iteration - 1], scale);
            } else
            {
                dispListScaled = null;
            }

                for (int i = 0; i < shellList.Count; i++)
            {
                myModel3DGroup.Children.Add(DrawingHelper.Draw3DPanel(shellList[i], Configuration.shellEleColor, true, dispListScaled, Configuration.zoomParam));
            }
            for (int i = 0; i < springList.Count; i++)
            {
                myModel3DGroup.Children.Add(DrawingHelper.Draw3DSprings(springList[i], Configuration.spring3DColor, true, dispListScaled));
            }
            DrawingHelper.DrawBoundaryConditions(new List<Support>(supportList), true, dispListScaled, Configuration.zoomParam);
            DrawingHelper.DrawLoads(new List<Load>(loadList), true, dispListScaled, Configuration.zoomParam);
            DrawingHelper.DrawMasses(new List<Mass>(massList), true, dispListScaled, Configuration.zoomParam);
        }
        

        public void CheckIfOpenedUserControls(UserControl sender)
        {
            foreach (UserControl uc in panelUserControl.Children)
            {
                if (uc.Visibility == Visibility.Visible && uc != sender)
                {
                    if (uc.Name == "nodeUserControl")
                    {
                        nodeButtonClicked = false;
                    } else if (uc.Name == "shellUserControl")
                    {
                        shellButtonClicked = false;
                    } else if (uc.Name == "spring3DUserControl")
                    {
                        springButtonClicked = false;
                    } else if (uc.Name == "loadUserControl")
                    {
                        loadButtonClicked = false;
                    } else if(uc.Name == "supportUserControl")
                    {
                        supButtonClicked = false;
                    } else if (uc.Name == "materialUserControl")
                    {
                        materialButtonClicked = false;
                    } else if(uc.Name == "graphPropUserControl")
                    {
                        graphPropButtonClicked = false;
                    } else if (uc.Name == "massUserControl")
                    {
                        massButtonClicked = false;
                    } else if (uc.Name == "seismicLoadUserControl")
                    {
                        seismicLoadButtonClicked = false;
                    } else if (uc.Name == "impulseLoadUserControl")
                    {
                        impulseLoadButtonClicked = false;
                    }
                    else
                    {

                    }
                    uc.Visibility = Visibility.Collapsed;
                }
            }
        }

        private void mainWindowRibbon_Loaded(object sender, RoutedEventArgs e)
        {
            Grid child = VisualTreeHelper.GetChild((DependencyObject)sender, 0) as Grid;
            if (child != null)
            {
                child.RowDefinitions[0].Height = new GridLength(0); //hides the quicktoolbar of the main window's Ribbon
            }
        }

        private void Button_XYPlane_Click(object sender, RoutedEventArgs e)
        {
            camRotX.Angle = -90;
            camRotY.Angle = -90;
            camRotZ.Angle = 0;
        }

        private void Button_XZPlane_Click(object sender, RoutedEventArgs e)
        {
            camRotX.Angle = 0;
            camRotY.Angle = 0;
            camRotZ.Angle = -90;
        }

        private void Button_YZPlane_Click(object sender, RoutedEventArgs e)
        {
            camRotX.Angle = 0;
            camRotY.Angle = 0;
            camRotZ.Angle = 0;
        }

        private void Button_45Degree_Click(object sender, RoutedEventArgs e)
        {
            camRotX.Angle = 0;
            camRotY.Angle = -45;
            camRotZ.Angle = 45;
        }

        private void Button_XAxisPlus_Click(object sender, RoutedEventArgs e)
        {
            camRotX.Angle += 15;
        }

        private void Button_XAxisMinus_Click(object sender, RoutedEventArgs e)
        {
            camRotX.Angle -= 15;
        }

        private void Button_YAxisPlus_Click(object sender, RoutedEventArgs e)
        {
            camRotY.Angle -= 15;
        }

        private void Button_YAxisMinus_Click(object sender, RoutedEventArgs e)
        {
            camRotY.Angle += 15;
        }

        private void Button_ZAxisPlus_Click(object sender, RoutedEventArgs e)
        {
            camRotZ.Angle -= 15;
        }

        private void Button_ZAxisMinus_Click(object sender, RoutedEventArgs e)
        {
            camRotZ.Angle += 15;
        }

        private void Button_ChangeScale_Click(object sender, RoutedEventArgs e)
        {
            scale = Double.Parse(txt_Scale.Text);
            DrawingHelper.ClearScreen(ref myModel3DGroup);
            List<Node> dispListScaled;
            if (deformed)
            {
                dispListScaled = DrawingHelper.GetDispListScaled(seriesDispList[iteration - 1], scale);
            } else if (modalShape)
            {
                dispListScaled = DrawingHelper.GetDispListScaled(modalDispList[iteration - 1], scale);
            } else
            {
                dispListScaled = null;
            }

            for (int i = 0; i < shellList.Count; i++)
            {
                myModel3DGroup.Children.Add(DrawingHelper.Draw3DPanel(shellList[i], Configuration.shellEleColor, true, dispListScaled, Configuration.zoomParam));
            }
            for (int i = 0; i < springList.Count; i++)
            {
                myModel3DGroup.Children.Add(DrawingHelper.Draw3DSprings(springList[i], Configuration.spring3DColor, true, dispListScaled));
            }
            DrawingHelper.DrawBoundaryConditions(new List<Support>(supportList), true, dispListScaled, Configuration.zoomParam);
            DrawingHelper.DrawLoads(new List<Load>(loadList), true, dispListScaled, Configuration.zoomParam);
            DrawingHelper.DrawMasses(new List<Mass>(massList), true, dispListScaled, Configuration.zoomParam);
        }

        private void LoadStepViewerPanel_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            iterCount.Content = iteration;
            txt_Scale.Text = scale.ToString();
        }

        private void Button_StartAnalysis_Click(object sender, RoutedEventArgs e)
        {
            if (fileName != null)
            {
                // Use ProcessStartInfo class
                ProcessStartInfo startInfo = new ProcessStartInfo();
                //startInfo.CreateNoWindow = true;
                startInfo.CreateNoWindow = false;
                startInfo.UseShellExecute = false;
                startInfo.FileName = Directory.GetCurrentDirectory() + "\\ConsoleFEMTest.exe";
                startInfo.WindowStyle = ProcessWindowStyle.Hidden;
                startInfo.Arguments = fileName;

                try
                {
                    // Start the process with the info we specified.
                    // Call WaitForExit and then the using statement will close.
                    using (Process exeProcess = Process.Start(startInfo))
                    {
                        exeProcess.WaitForExit();
                    }
                    MessageBox.Show("Analysis Completed!");
                }
                catch (Exception error)
                {
                    MessageBox.Show(error.Message);
                }

            } else
            {
                MessageBox.Show("Please save your project before running the analysis.");
            }
        }

        private void Button_ShowGraph_Click(object sender, RoutedEventArgs e)
        {
            GraphWindow window = new GraphWindow();
            window.Show();
        }

        private void MyViewPort_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            foreach (UserControl UI in panelUserControl.Children)
            {
                if (UI.Visibility == Visibility.Visible)
                {
                    if (UI.Name == "nodeUserControl")
                    {
                        NodeUserControl MuC = UI as NodeUserControl;
                        MuC.UpdateUILayout();
                    }
                    else if (UI.Name == "shellUserControl")
                    {
                        ShellUserControl MuC = UI as ShellUserControl;
                        MuC.UpdateUILayout();
                    }
                    else if (UI.Name == "spring3DUserControl")
                    {
                        Spring3DUserControl MuC = UI as Spring3DUserControl;
                        MuC.UpdateUILayout();
                    }
                    else if (UI.Name == "loadUserControl")
                    {
                        LoadUserControl MuC = UI as LoadUserControl;
                        MuC.UpdateUILayout();
                    }
                    else if (UI.Name == "supportUserControl")
                    {
                        BoundaryUserControl MuC = UI as BoundaryUserControl;
                        MuC.UpdateUILayout();
                    }
                    else if (UI.Name == "materialUserControl")
                    {
                        MaterialUserControl MuC = UI as MaterialUserControl;
                        MuC.UpdateUILayout();
                    }
                    else if (UI.Name == "graphPropUserControl")
                    {
                        GraphPropSelect MuC = UI as GraphPropSelect;
                        MuC.UpdateUILayout();
                    }
                    else if (UI.Name == "analysisPropertiesUserControl")
                    {
                        AnalysisPropControl MuC = UI as AnalysisPropControl;
                        MuC.UpdateUILayout();
                    }
                    else
                    {

                    }
                }
            }
        }


        public static bool modalShape = false;
        private void Button_ModeShapes_Click(object sender, RoutedEventArgs e)
        {
            ShowUndeformed();

            DrawingHelper.ClearScreen(ref myModel3DGroup);

            List<Node> dispListScaled = DrawingHelper.GetDispListScaled(modalDispList[iteration - 1], scale);

            if (modalShape == false)
            {
                iterCount.Content = 1;
                natFreq.Content = natFreqs[0].ToString("F");
                for (int i = 0; i < shellList.Count; i++)
                {
                    myModel3DGroup.Children.Add(DrawingHelper.Draw3DPanel(shellList[i], Configuration.shellEleColor, true, dispListScaled, Configuration.zoomParam));
                }
                for (int i = 0; i < springList.Count; i++)
                {
                    myModel3DGroup.Children.Add(DrawingHelper.Draw3DSprings(springList[i], Configuration.spring3DColor, true, dispListScaled));
                }
                DrawingHelper.DrawBoundaryConditions(new List<Support>(supportList), true, dispListScaled, Configuration.zoomParam);
                DrawingHelper.DrawLoads(new List<Load>(loadList), true, dispListScaled, Configuration.zoomParam);
                DrawingHelper.DrawMasses(new List<Mass>(massList), true, dispListScaled, Configuration.zoomParam);
                loadStepViewerPanel.Visibility = Visibility.Visible;
                natFreqViewPanel.Visibility = Visibility.Visible;
            }
            modalShape = !modalShape;
        }
    }
}
