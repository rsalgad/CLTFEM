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
    /// Interaction logic for LoadUserControl.xaml
    /// </summary>
    public partial class LoadUserControl : UserControl
    {
        public LoadUserControl()
        {
            InitializeComponent();
            loads_ListBox.ItemsSource = MainWindow.loadList;
        }

        private void Add_Load_Click(object sender, RoutedEventArgs e)
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
                Load load = new Load(count + 1, nodeID);

                if (_loadX.Text != "0")
                {
                    load.SetFx(Double.Parse(_loadX.Text));
                }
                if (_loadY.Text != "0")
                {
                    load.SetFy(Double.Parse(_loadY.Text));
                }
                if (_loadZ.Text != "0")
                {
                    load.SetFz(Double.Parse(_loadZ.Text));
                }
                if (_momentX.Text != "0")
                {
                    load.SetMx(Double.Parse(_momentX.Text));
                }
                if (_momentY.Text != "0")
                {
                    load.SetMy(Double.Parse(_momentY.Text));
                }
                if (_momentZ.Text != "0")
                {
                    load.SetMz(Double.Parse(_momentZ.Text));
                }

                if (rd_fixed.IsChecked == false)
                {
                    load.Status = "increment";
                }

                MainWindow.loadList.Add(load);

                DrawingHelper.DrawLoads(new List<Load>(MainWindow.loadList), false, MainWindow.dispList, Configuration.zoomParam);
                DrawingHelper.DrawMasses(new List<Mass>(MainWindow.massList), false, MainWindow.dispList, Configuration.zoomParam);
            } catch
            {
                MessageBox.Show("An error has occurred and the load case could not be defined. Please check your inputs and try again.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Delete_Load_Click(object sender, RoutedEventArgs e)
        {
            int index = loads_ListBox.SelectedIndex;
            int originalCount = MainWindow.loadList.Count;
            if (index != -1)
            {
                MainWindow.loadList.RemoveAt(index);
                if ((index + 1) != originalCount)
                {
                    //that means that the item that was removed was not the last item of the original list was removed
                    //so we need to re-organize the list
                    Management.ReorganizeLoadList();
                    loads_ListBox.Items.Refresh();
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
