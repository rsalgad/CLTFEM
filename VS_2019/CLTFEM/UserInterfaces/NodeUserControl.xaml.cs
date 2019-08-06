using CLTFEM.Classes.Helpers;
using CLTFEM.Classes.Structural;
using CLTFEM.Windows;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Media3D;

namespace CLTFEM.UserInterfaces
{
    /// <summary>
    /// Interaction logic for Node.xaml
    /// </summary>
    public partial class NodeUserControl : UserControl
    {
        private int count = MainWindow.nodeList.Count; //get the current number of nodes in the nodeList;
        public NodeUserControl()
        {
            InitializeComponent();
            nodes_ListBox.ItemsSource = MainWindow.nodeList;
        }

        private void Add_Node_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                double _xini, _yini, _zini, _xincr, _yincr, _zincr, _xnum, _ynum, _znum;
                _xini = Double.Parse(_nodeX.Text);
                _yini = Double.Parse(_nodeY.Text);
                _zini = Double.Parse(_nodeZ.Text);
                _xincr = Double.Parse(_XIncr.Text);
                _yincr = Double.Parse(_YIncr.Text);
                _zincr = Double.Parse(_ZIncr.Text);
                _xnum = Double.Parse(_XNumb.Text);
                _ynum = Double.Parse(_YNumb.Text);
                _znum = Double.Parse(_ZNumb.Text);

                for (int i = 1; i < _ynum + 1; i++) //xnum and ynum are ivnerted so that the node numbers increase in the x direction first, then in the y direction
                {
                    for (int j = 1; j < _xnum + 1; j++)
                    {
                        // makes a Point3D element from the coordinates input by the user
                        Point3D p = new Point3D(_xini + (j - 1) * _xincr, _yini + (i - 1) * _yincr, _zini);
                        count++; // increases the node count
                        MainWindow.nodeList.Add(new Node(count, p));//adds the created node to the nodeList in the mainWindow   
                        MainWindow.myModel3DGroup.Children.Add(DrawingHelper.Draw3DCube(Configuration.nodeSize * Configuration.zoomParam, p, Configuration.nodeColor)); //draws the node on the viewModel3D in the mainWindow
                    }
                }
            }
            catch
            {
                MessageBox.Show("An error has occurred and the node could not be created. Please check your inputs and try again.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Delete_Node_Click(object sender, RoutedEventArgs e)
        {
            int index = nodes_ListBox.SelectedIndex;
            int originalCount = MainWindow.nodeList.Count;
            if (index != -1)
            {
                MainWindow.nodeList.RemoveAt(index);
                if ((index + 1) != originalCount)
                {
                    //that means that the item that was removed was not the last item of the original list was removed
                    //so we need to re-organize the list
                    Management.ReorganizeNodeList();
                    nodes_ListBox.Items.Refresh();
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
