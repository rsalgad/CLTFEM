using CLTFEM.Classes.Helpers;
using CLTFEM.Classes.Structural;
using CLTFEM.Windows;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Media3D;
using Material = CLTFEM.Classes.Structural.Material;

namespace CLTFEM.UserInterfaces
{
    /// <summary>
    /// Interaction logic for ShellUserControl.xaml
    /// </summary>
    public partial class ShellUserControl : UserControl
    {
        public ShellUserControl()
        {
            InitializeComponent();
            elements_ListBox.ItemsSource = MainWindow.shellList;
        }

        private void Add_Element_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                double _length, _height, _thickness, _sizeX, _sizeY, _xIni, _yIni, _zIni;
                int _mat, _layers, signLength, signHeight;
                _length = Double.Parse(txt_length.Text);
                _height = Double.Parse(txt_height.Text);
                _thickness = Double.Parse(txt_thickness.Text);
                _sizeX = Double.Parse(txt_sizeLength.Text);
                _sizeY = Double.Parse(txt_sizeHeight.Text);
                _mat = Int32.Parse(txt_mat.Text);
                _xIni = Double.Parse(txt_xIni.Text);
                _yIni = Double.Parse(txt_yIni.Text);
                _zIni = Double.Parse(txt_zIni.Text);
                _layers = Int32.Parse(txt_layer.Text);

                Vector3D _iniVec, _lengthVec, _heightVec;
                _iniVec = new Vector3D(_xIni, _yIni, _zIni);

                //gets the sign of the length of the element
                if (_length < 0)
                {
                    signLength = -1;
                }
                else
                {
                    signLength = 1;
                }

                //gets the sign of the height of the element
                if (_height < 0)
                {
                    signHeight = -1;
                }
                else
                {
                    signHeight = 1;
                }

                //sets the length direction and height direction in terms of vectors
                if (rd_xLeng.IsChecked == true)
                {
                    _lengthVec = new Vector3D(1, 0, 0);
                }
                else if (rd_yLeng.IsChecked == true)
                {
                    _lengthVec = new Vector3D(0, 1, 0);
                }
                else
                {
                    _lengthVec = new Vector3D(0, 0, 1);
                }

                if (rd_xHeight.IsChecked == true)
                {
                    _heightVec = new Vector3D(1, 0, 0);
                }
                else if (rd_yHeight.IsChecked == true)
                {
                    _heightVec = new Vector3D(0, 1, 0);
                }
                else
                {
                    _heightVec = new Vector3D(0, 0, 1);
                }

                double eleNumberDir1 = Math.Abs(_length) / _sizeX;
                double eleNumberDir2 = Math.Abs(_height) / _sizeY;

                double nodeNumberDir1 = eleNumberDir1 * 2 + 1;
                double nodeNumberDir2 = eleNumberDir2 * 2 + 1;
                int count = MainWindow.nodeList.Count;

                //Create the nodes
                for (int i = 0; i < nodeNumberDir2; i++) //Iterate over height
                {
                    for (int j = 0; j < nodeNumberDir1; j++) //iterate over length (the order is reversed so that the numbering comes in rows instead of columns
                    {
                        double xPos, yPos, zPos;
                        xPos = _iniVec.X + signLength * j * _lengthVec.X * (_sizeX / 2) + signHeight * i * _heightVec.X * (_sizeY / 2);
                        yPos = _iniVec.Y + signLength * j * _lengthVec.Y * (_sizeX / 2) + signHeight * i * _heightVec.Y * (_sizeY / 2);
                        zPos = _iniVec.Z + signLength * j * _lengthVec.Z * (_sizeX / 2) + signHeight * i * _heightVec.Z * (_sizeY / 2);
                        if (Node.FindNodeByCoordinates(xPos, yPos, zPos, new List<Node>(MainWindow.nodeList)) == null)
                        {
                            Node n = new Node(count + 1, new Point3D(xPos, yPos, zPos));
                            MainWindow.nodeList.Add(n);
                            MainWindow.myModel3DGroup.Children.Add(DrawingHelper.Draw3DCube(Configuration.nodeSize * Configuration.zoomParam, n.Point, Configuration.nodeColor));
                            count++;
                        }
                    }
                    /* this is for the 8-node element
                    if (i % 2 == 0) //divide by 2 to know in which row of the nodes we are, if it is on the row with 3 nodes per element or on the row with 2 nodes per element
                    {
                        for (int j = 0; j < nodeNumberDir1; j++) //iterate over length (the order is reversed so that the numbering comes in rows instead of columns
                        {
                            double xPos, yPos, zPos;
                            xPos = _iniVec.X + j * _lengthVec.X * (_size / 2) + i * _heightVec.X * (_size / 2);
                            yPos = _iniVec.Y + j * _lengthVec.Y * (_size / 2) + i * _heightVec.Y * (_size / 2);
                            zPos = _iniVec.Z + j * _lengthVec.Z * (_size / 2) + i * _heightVec.Z * (_size / 2);
                            Node n = new Node(count + 1, new Point3D(xPos, yPos, zPos));
                            MainWindow.nodeList.Add(n);
                            MainWindow.myModel3DGroup.Children.Add(DrawingHelper.Draw3DCube(nodeSize, n.Point, Colors.Black));
                            count++;
                        }
                    } else
                    {
                        for (int j = 0; j < nodeNumberDir1 - eleNumberDir1; j++) //iterate over the row of nodes that has lower number of nodess on the 8-node element model
                        {
                            double xPos, yPos, zPos;
                            xPos = _iniVec.X + j * _lengthVec.X * (_size) + i * _heightVec.X * (_size / 2);
                            yPos = _iniVec.Y + j * _lengthVec.Y * (_size) + i * _heightVec.Y * (_size / 2);
                            zPos = _iniVec.Z + j * _lengthVec.Z * (_size) + i * _heightVec.Z * (_size / 2);
                            Node n = new Node(count + 1, new Point3D(xPos, yPos, zPos));
                            MainWindow.nodeList.Add(n);
                            MainWindow.myModel3DGroup.Children.Add(DrawingHelper.Draw3DCube(nodeSize, n.Point, Colors.Black));
                            count++;
                        }
                    }
                    */
                }

                List<Node> nodeList = new List<Node>(MainWindow.nodeList);

                List<Material> materialList = new List<Material>(MainWindow.materialList);

                //Create the Elements
                int countEle = MainWindow.shellList.Count;
                for (int i = 0; i < eleNumberDir2; i++) //for each element
                {
                    for (int j = 0; j < eleNumberDir1; j++)
                    {

                        Vector3D pos = _iniVec;
                        Node n1 = Node.FindNodeByCoordinates(pos.X, pos.Y, pos.Z, nodeList);
                        pos += _sizeX * signLength * _lengthVec;
                        Node n2 = Node.FindNodeByCoordinates(pos.X, pos.Y, pos.Z, nodeList);
                        pos += _sizeY * signHeight * _heightVec;
                        Node n3 = Node.FindNodeByCoordinates(pos.X, pos.Y, pos.Z, nodeList);
                        pos -= _sizeX * signLength * _lengthVec;
                        Node n4 = Node.FindNodeByCoordinates(pos.X, pos.Y, pos.Z, nodeList);
                        pos = pos - _sizeY * signHeight * _heightVec + (_sizeX / 2) * signLength * _lengthVec;
                        Node n5 = Node.FindNodeByCoordinates(pos.X, pos.Y, pos.Z, nodeList);
                        pos = pos + (_sizeX / 2) * signLength * _lengthVec + (_sizeY / 2) * signHeight * _heightVec;
                        Node n6 = Node.FindNodeByCoordinates(pos.X, pos.Y, pos.Z, nodeList);
                        pos = pos - (_sizeX / 2) * signLength * _lengthVec + (_sizeY / 2) * signHeight * _heightVec;
                        Node n7 = Node.FindNodeByCoordinates(pos.X, pos.Y, pos.Z, nodeList);
                        pos = pos - (_sizeX / 2) * signLength * _lengthVec - (_sizeY / 2) * signHeight * _heightVec;
                        Node n8 = Node.FindNodeByCoordinates(pos.X, pos.Y, pos.Z, nodeList);
                        pos = pos + (_sizeX / 2) * signLength * _lengthVec;
                        Node n9 = Node.FindNodeByCoordinates(pos.X, pos.Y, pos.Z, nodeList);

                        //adds an element to the element list
                        ShellElement ele = new ShellElement(countEle + 1, n1, n2, n3, n4, n5, n6, n7, n8, n9, _thickness, _layers, materialList[_mat - 1]);
                        countEle++;
                        MainWindow.shellList.Add(ele);

                        //draws the element on the viewModel3D in the mainWindow
                        MainWindow.myModel3DGroup.Children.Add(DrawingHelper.Draw3DPanel(ele, Configuration.shellEleColor, false, MainWindow.dispList, Configuration.zoomParam));

                        _iniVec += _sizeX * signLength * _lengthVec;
                    }
                    _iniVec.X = _xIni;
                    _iniVec.Y = _yIni;
                    _iniVec.Z = _zIni;

                    _iniVec += (i + 1) * _sizeY * signHeight * _heightVec;

                }
            }
            catch
            {
                MessageBox.Show("An error has occurred and the shell elements could not be created. Please check your inputs and try again", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Delete_Element_Click(object sender, RoutedEventArgs e)
        {
            int index = elements_ListBox.SelectedIndex;
            int originalCount = MainWindow.loadList.Count;
            if (index != -1)
            {
                MainWindow.shellList.RemoveAt(index);
                if ((index + 1) != originalCount)
                {
                    //that means that the item that was removed was not the last item of the original list was removed
                    //so we need to re-organize the list
                    Management.ReorganizeShellList();
                    elements_ListBox.Items.Refresh();
                }
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateUILayout();
        }

        public void UpdateUILayout()
        {
            MainWindow mw = (MainWindow)Application.Current.MainWindow;
            Height = mw.viewPortBackground.ActualHeight;
        }
    }
}
