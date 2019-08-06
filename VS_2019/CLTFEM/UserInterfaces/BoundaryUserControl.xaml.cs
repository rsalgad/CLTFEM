using CLTFEM.Classes.Helpers;
using CLTFEM.Classes.Structural;
using CLTFEM.Windows;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace CLTFEM.UserInterfaces
{
    /// <summary>
    /// Interaction logic for BoundaryUserControl.xaml
    /// </summary>
    public partial class BoundaryUserControl : UserControl
    {
        public BoundaryUserControl()
        {
            InitializeComponent();
            bound_ListBox.ItemsSource = MainWindow.supportList;
        }

        private void Add_Boundary_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int nodeID = 0, count;
                int nodeNumb = Int32.Parse(_nodeNumb.Text);
                double incX = Double.Parse(_incX.Text);
                double incY = Double.Parse(_incY.Text);
                double incZ = Double.Parse(_incZ.Text);

                for (int i = 0; i < nodeNumb; i++)
                {
                    if (rd_nodeID.IsChecked == true)
                    {
                        nodeID = Int32.Parse(_nodeID.Text);
                    }
                    if (rd_nodeCoord.IsChecked == true)
                    {
                        List<Node> nodeList = new List<Node>(MainWindow.nodeList);
                        double x, y, z;
                        x = Double.Parse(_nodeX.Text) + i * incX;
                        y = Double.Parse(_nodeY.Text) + i * incY;
                        z = Double.Parse(_nodeZ.Text) + i * incZ;
                        nodeID = Node.FindNodeByCoordinates(x, y, z, nodeList).ID;
                    }

                    count = MainWindow.supportList.Count;
                    Support sup = new Support(count + 1, nodeID);

                    if (chk_xTrans.IsChecked == true)
                    {
                        sup.Set_tX(Double.Parse(_tX.Text));
                    }
                    if (chk_yTrans.IsChecked == true)
                    {
                        sup.Set_tY(Double.Parse(_tY.Text));
                    }
                    if (chk_zTrans.IsChecked == true)
                    {
                        sup.Set_tZ(Double.Parse(_tZ.Text));
                    }
                    if (chk_xRot.IsChecked == true)
                    {
                        sup.Set_rX(Double.Parse(_rX.Text));
                    }
                    if (chk_yRot.IsChecked == true)
                    {
                        sup.Set_rY(Double.Parse(_rY.Text));
                    }
                    if (chk_zRot.IsChecked == true)
                    {
                        sup.Set_rZ(Double.Parse(_rZ.Text));
                    }

                    MainWindow.supportList.Add(sup);
                }
                DrawingHelper.DrawBoundaryConditions(new List<Support>(MainWindow.supportList), false, MainWindow.dispList, 1);
                
            }
            catch
            {
                MessageBox.Show("An error has occurred and the boundary conditions could not be defined. Please check your inputs and try again.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Delete_Boundary_Click(object sender, RoutedEventArgs e)
        {
            int index = bound_ListBox.SelectedIndex;
            int originalCount = MainWindow.supportList.Count;
            if (index != -1)
            {
                MainWindow.supportList.RemoveAt(index);
                if ((index + 1) != originalCount)
                {
                    //that means that the item that was removed was not the last item of the original list was removed
                    //so we need to re-organize the list
                    Management.ReorganizeSupportList();
                    bound_ListBox.Items.Refresh();
                }
            }
        }

        private void Rd_nodeID_Checked(object sender, RoutedEventArgs e)
        {
            if (rd_nodeCoord.IsChecked == true)
            {
                rd_nodeCoord.IsChecked = false;
            }
        }

        private void Rd_nodeCoord_Checked(object sender, RoutedEventArgs e)
        {
            if (rd_nodeID.IsChecked == true)
            {
                rd_nodeID.IsChecked = false;
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
