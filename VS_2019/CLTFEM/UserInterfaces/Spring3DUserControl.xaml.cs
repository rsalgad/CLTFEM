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
    /// Interaction logic for Spring3DUserControl.xaml
    /// </summary>
    public partial class Spring3DUserControl : UserControl
    {
        public Spring3DUserControl()
        {
            InitializeComponent();
            springs_ListBox.ItemsSource = MainWindow.springList;
        }

        private void Add_Spring_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int nodeID = Int32.Parse(_nodeID.Text);
                int matIDX = Int32.Parse(_xMat.Text);
                int matIDY = Int32.Parse(_yMat.Text);
                int matIDZ = Int32.Parse(_zMat.Text);
                char vecX, vecY;

                int nodeCount = MainWindow.nodeList.Count;
                Node n2 = MainWindow.nodeList[nodeID - 1];
                Node n1 = new Node(nodeCount + 1, n2.Point); //create a second node at the same position
                MainWindow.nodeList.Add(n1);

                int[] matList = { matIDX, matIDY, matIDZ };

                if (rd_xDirX.IsChecked == true)
                {
                    vecX = 'x';
                }
                else if (rd_xDirY.IsChecked == true)
                {
                    vecX = 'y';
                }
                else
                {
                    vecX = 'z';
                }

                if (rd_yDirX.IsChecked == true)
                {
                    vecY = 'x';
                }
                else if (rd_yDirY.IsChecked == true)
                {
                    vecY = 'y';
                }
                else
                {
                    vecY = 'z';
                }

                int springCount = MainWindow.springList.Count;

                Spring3D spring = new Spring3D(springCount + 1, n1, n2, matList, vecX, vecY);
                MainWindow.springList.Add(spring);

                Support s = new Support(MainWindow.supportList.Count + 1, n1.ID);
                s.Set_rX(0); //Springs do not have translation DOFs so they are restricted by default.
                s.Set_rY(0);
                s.Set_rZ(0);
                if (chk_xDirX.IsChecked == true)
                {
                    s.Set_tX(0);
                }
                if (chk_xDirY.IsChecked == true)
                {
                    s.Set_tY(0);
                }
                if (chk_xDirZ.IsChecked == true)
                {
                    s.Set_tZ(0);
                }
                MainWindow.supportList.Add(s);
                DrawingHelper.DrawBoundaryConditions(new List<Support>(MainWindow.supportList), false, MainWindow.dispList, 1);
            }
            catch
            {
                MessageBox.Show("An error has occurred and the spring elements could not be created. Please check your inputs and try again.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Delete_Spring_Click(object sender, RoutedEventArgs e)
        {
            int index = springs_ListBox.SelectedIndex;
            int originalCount = MainWindow.loadList.Count;
            if (index != -1)
            {
                MainWindow.springList.RemoveAt(index);
                if ((index + 1) != originalCount)
                {
                    //that means that the item that was removed was not the last item of the original list was removed
                    //so we need to re-organize the list
                    Management.ReorganizeSpring3DList();
                    springs_ListBox.Items.Refresh();
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
