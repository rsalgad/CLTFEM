using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using CLTFEM.Classes.Structural;
using CLTFEM.Windows;

namespace CLTFEM.UserInterfaces
{
    /// <summary>
    /// Interaction logic for ImpulseLoadUserControl.xaml
    /// </summary>
    public partial class ImpulseLoadUserControl : UserControl
    {
        List<int> nodeIDs = new List<int>();
        public ImpulseLoadUserControl()
        {
            InitializeComponent();
        }

        private void Button_Add_Click(object sender, RoutedEventArgs e)
        {
            if (MainWindow.impulseLoad.Count == 0)
            {
                if (nodeIDs.Count != 0)
                {
                    double force1, force2, force3, time1, time2, time3;
                    force1 = double.Parse(txt_force1.Text);
                    force2 = double.Parse(txt_force2.Text);
                    force3 = double.Parse(txt_force3.Text);

                    time1 = double.Parse(txt_time1.Text);
                    time2 = double.Parse(txt_time2.Text);
                    time3 = double.Parse(txt_time3.Text);

                    ImpulseLoad impLoad = new ImpulseLoad();
                    impLoad.SetPoint1(force1, time1);
                    impLoad.SetPoint2(force2, time2);
                    impLoad.SetPoint3(force3, time3);

                    MainWindow.impulseLoad.Add(impLoad);
                    UpdateListBox(impLoad);

                    Load.DeleteAllLoadsWithFlag(ref MainWindow.loadList, "increment");
                    for (int i = 0; i < nodeIDs.Count; i++)
                    {
                        Load l = new Load(MainWindow.loadList.Count + 1, nodeIDs[i], "impulse");
                        l.SetFx(1);
                        MainWindow.loadList.Add(l);
                    }
                }
                else
                {
                    MessageBox.Show("Please specify at least one node where the load will be applied.");
                }
            }
            else
            {
                MessageBox.Show("Only one impulse load can be defined per analysis.");
            }
        }

        private void Button_Delete_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.impulseLoad.Clear();
            loads_ListBox.Items.Clear();
            nodeIDs.Clear();
            Load.DeleteAllLoadsWithFlag(ref MainWindow.loadList, "impulse");
        }

        private void Button_AddNode_Click(object sender, RoutedEventArgs e)
        {
            int nodeID = int.Parse(txt_NodeID.Text);
            nodeIDs.Add(nodeID);
        }

        private void UpdateListBox(ImpulseLoad impLoad)
        {
            string s1, s2, s3;
            s1 = string.Format("Point 1: Time = {0:F}, Force = {1:F}", impLoad.Points[0, 0], impLoad.Points[0, 1]);
            loads_ListBox.Items.Add(s1);
            s2 = string.Format("Point 2: Time = {0:F}, Force = {1:F}", impLoad.Points[1, 0], impLoad.Points[1, 1]);
            loads_ListBox.Items.Add(s2);
            s3 = string.Format("Point 3: Time = {0:F}, Force = {1:F}", impLoad.Points[2, 0], impLoad.Points[2, 1]);
            loads_ListBox.Items.Add(s3);
        }
    }
}
