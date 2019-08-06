using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using CLTFEM.Classes.Save_Open;
using CLTFEM.Classes.Structural;
using CLTFEM.Windows;

namespace CLTFEM.UserInterfaces
{
    /// <summary>
    /// Interaction logic for SeismicLoadUserControl.xaml
    /// </summary>
    public partial class SeismicLoadUserControl : UserControl
    {
        string[] stringXFile;
        string[] stringYFile;
        string[] stringZFile;
        bool xDir = true, yDir = false, zDir = false;

        public SeismicLoadUserControl()
        {
            InitializeComponent();
            
        }

        private void Select_XDirSeismic_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog openWindow = OpenOperation.SetOpenDialogParameters("Open", ".*", "All Files (*.*)|*.*");
            openWindow.FileOk += delegate(object s, System.ComponentModel.CancelEventArgs args) { OpenOperation.OpenSeismic_FileOk(s, args, ref stringXFile); };
            openWindow.ShowDialog();
            txt_XDir_FileName.Text = openWindow.FileName;
        }

        private void Select_YDirSeismic_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog openWindow = OpenOperation.SetOpenDialogParameters("Open", ".*", "All Files (*.*)|*.*");
            openWindow.FileOk += delegate (object s, System.ComponentModel.CancelEventArgs args) { OpenOperation.OpenSeismic_FileOk(s, args, ref stringYFile); };
            openWindow.ShowDialog();
            txt_YDir_FileName.Text = openWindow.FileName;
        }

        private void Select_ZDirSeismic_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog openWindow = OpenOperation.SetOpenDialogParameters("Open", ".*", "All Files (*.*)|*.*");
            openWindow.FileOk += delegate (object s, System.ComponentModel.CancelEventArgs args) { OpenOperation.OpenSeismic_FileOk(s, args, ref stringZFile); };
            openWindow.ShowDialog();
            txt_ZDir_FileName.Text = openWindow.FileName;
        }

        private void Chk_XDir_Checked(object sender, RoutedEventArgs e)
        {
            if (chk_XDir != null && gb_XDir != null)
            {
                gb_XDir.IsEnabled = true;
                xDir = true;
            }
        }

        private void Chk_YDir_Checked(object sender, RoutedEventArgs e)
        {
            if (chk_YDir != null && gb_YDir != null)
            {
                gb_YDir.IsEnabled = true;
                yDir = true;
            }
        }

        private void Chk_ZDir_Checked(object sender, RoutedEventArgs e)
        {
            if (chk_ZDir != null && gb_ZDir != null)
            {
                gb_ZDir.IsEnabled = true;
                zDir = true;
            }
        }

        private void Add_SeismicLoad_Click(object sender, RoutedEventArgs e)
        {
            if (MainWindow.seismicLoad.Count == 0)
            {
                try
                {
                    int skipX, skipY, skipZ;
                    double scaleX, scaleY, scaleZ, deltaT;

                    skipX = int.Parse(txt_XDir_LineSkip.Text);
                    skipY = int.Parse(txt_YDir_LineSkip.Text);
                    skipZ = int.Parse(txt_ZDir_LineSkip.Text);

                    scaleX = double.Parse(txt_XDir_Scale.Text);
                    scaleY = double.Parse(txt_YDir_Scale.Text);
                    scaleZ = double.Parse(txt_ZDir_Scale.Text);
                    deltaT = double.Parse(txt_DeltaT.Text);

                    SeismicLoad sLoad = SeismicLoad.CreateSeismicLoadFromFiles(deltaT, stringXFile, stringYFile, stringZFile, skipX, skipY, skipZ, scaleX, scaleY, scaleZ);
                    MainWindow.seismicLoad.Add(sLoad);
                    UpdateListBox(sLoad);

                    Load.DeleteAllLoadsWithFlag(ref MainWindow.loadList, "increment");
                    for (int i = 0; i < MainWindow.nodeList.Count; i++)
                    {
                        Load l = new Load(MainWindow.loadList.Count + 1, MainWindow.nodeList[i].ID, "seismic");
                        if (xDir)
                        {
                            l.SetFx(1);
                        }
                        if (yDir)
                        {
                            l.SetFy(2);
                        }
                        if (zDir)
                        {
                            l.SetFz(3);
                        }
                        MainWindow.loadList.Add(l);
                    }
                }
                catch
                {
                    MessageBox.Show("An error has occurred and the seismic load could not be defined. Please check your inputs and try again.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }

            } else
            {
                MessageBox.Show("Only one seismic load can be defined per analysis. Please delete the current seismic load in order to define a new one.", "Attention", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void Delete_SeismicLoad_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.seismicLoad.Clear();
            loads_ListBox.Items.Clear();
            Load.DeleteAllLoadsWithFlag(ref MainWindow.loadList, "seismic");
        }

        private void UpdateListBox(SeismicLoad sLoad)
        {
            List<double> scales = sLoad.Scales;
            double deltaT = 0;

            for (int i = 0; i < sLoad.Records[0].Count; i++)
            {
                deltaT += sLoad.DeltaT;
                string s = deltaT.ToString() + " ";

                for (int j = 0; j < sLoad.Records.Count; j++)
                {
                    s += (sLoad.Records[j][i] * scales[j]).ToString() + " ";
                }
                loads_ListBox.Items.Add(s);
            }
        }

        private void Chk_ZDir_Unchecked(object sender, RoutedEventArgs e)
        {
            if (chk_ZDir != null && gb_ZDir != null)
            {
                gb_ZDir.IsEnabled = false;
                zDir = false;
            }
        }

        private void Chk_YDir_Unchecked(object sender, RoutedEventArgs e)
        {
            if (chk_YDir != null && gb_YDir != null)
            {
                gb_YDir.IsEnabled = false;
                yDir = false;
            }
        }

        private void Chk_XDir_Unchecked(object sender, RoutedEventArgs e)
        {
            if (chk_XDir != null && gb_XDir != null)
            {
                gb_XDir.IsEnabled = false;
                xDir = false;
            }
        }

        private void Chk_XDir_Scale_Checked(object sender, RoutedEventArgs e)
        {
            if (txt_XDir_Scale != null)
            {
                txt_XDir_Scale.IsEnabled = true;
            }
        }

        private void Chk_XDir_Scale_Unchecked(object sender, RoutedEventArgs e)
        {
            if (txt_XDir_Scale != null)
            {
                txt_XDir_Scale.Text = "1";
                txt_XDir_Scale.IsEnabled = false;
            }
        }

        private void Chk_YDir_Scale_Checked(object sender, RoutedEventArgs e)
        {
            if (txt_YDir_Scale != null)
            {
                txt_YDir_Scale.IsEnabled = true;
            }
        }

        private void Chk_YDir_Scale_Unchecked(object sender, RoutedEventArgs e)
        {
            if (txt_YDir_Scale != null)
            {
                txt_YDir_Scale.IsEnabled = false;
            }
        }

        private void Chk_ZDir_Scale_Checked(object sender, RoutedEventArgs e)
        {
            if (txt_ZDir_Scale != null)
            {
                txt_ZDir_Scale.IsEnabled = true;
            }
        }

        private void Chk_ZDir_Scale_Unchecked(object sender, RoutedEventArgs e)
        {
            if (txt_ZDir_Scale != null)
            {
                txt_ZDir_Scale.IsEnabled = true;
            }
        }
    }
}
