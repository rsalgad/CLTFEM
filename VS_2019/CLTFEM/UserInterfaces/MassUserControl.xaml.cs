using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using CLTFEM.Classes.Helpers;
using CLTFEM.Classes.Structural;
using CLTFEM.Windows;

namespace CLTFEM.UserInterfaces
{
    /// <summary>
    /// Interaction logic for MassUserControl.xaml
    /// </summary>
    public partial class MassUserControl : UserControl
    {
        public MassUserControl()
        {
            InitializeComponent();
            masses_ListBox.ItemsSource = MainWindow.massList;
        }

        private void Add_Mass_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int nodeID = 0, count;

                if (rd_nodeID.IsChecked == true)
                {
                    nodeID = Int32.Parse(_nodeID.Text);
                }
                if (rd_nodeCoord.IsChecked == true)
                {
                    List<Node> nodeList = new List<Node>(MainWindow.nodeList);
                    double x, y, z;
                    x = Double.Parse(_nodeX.Text);
                    y = Double.Parse(_nodeY.Text);
                    z = Double.Parse(_nodeZ.Text);
                    nodeID = Node.FindNodeByCoordinates(x, y, z, nodeList).ID;
                }

                count = MainWindow.loadList.Count;
                Mass mass = new Mass(count + 1, nodeID);

                if (_massX.Text != "0")
                {
                    mass.SetMx(Double.Parse(_massX.Text));
                }
                if (_massY.Text != "0")
                {
                    mass.SetMy(Double.Parse(_massY.Text));
                }
                if (_massZ.Text != "0")
                {
                    mass.SetMz(Double.Parse(_massZ.Text));
                }

                MainWindow.massList.Add(mass);

                DrawingHelper.DrawMasses(new List<Mass>(MainWindow.massList), false, MainWindow.dispList, 1);
            }
            catch
            {
                MessageBox.Show("An error has occurred and the mass could not be defined. Please check your inputs and try again.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Delete_Mass_Click(object sender, RoutedEventArgs e)
        {
            int index = masses_ListBox.SelectedIndex;
            int originalCount = MainWindow.massList.Count;
            if (index != -1)
            {
                MainWindow.massList.RemoveAt(index);
                if ((index + 1) != originalCount)
                {
                    //that means that the item that was removed was not the last item of the original list was removed
                    //so we need to re-organize the list
                    Management.ReorganizeMassList();
                    masses_ListBox.Items.Refresh();
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
