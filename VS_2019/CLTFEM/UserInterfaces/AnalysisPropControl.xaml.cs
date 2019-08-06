using CLTFEM.Windows;
using System.Windows;
using System.Windows.Controls;
using CLTFEM.Classes.Analysis;


namespace CLTFEM.UserInterfaces
{
    /// <summary>
    /// Interaction logic for AnalysisPropControl.xaml
    /// </summary>
    public partial class AnalysisPropControl : UserControl
    {
        public AnalysisPropControl()
        {
            InitializeComponent();
            analysis_ListBox.ItemsSource = MainWindow.analysis;
        }

        private void Rd_Elastic_Checked(object sender, RoutedEventArgs e)
        {
            if (grpbox_Elastic != null)
            {
                grpbox_Elastic.IsEnabled = true;
                CheckRadioButtons(rd_Elastic);
            }
        }

        private void Rd_Elastic_Unchecked(object sender, RoutedEventArgs e)
        {
            grpbox_Elastic.IsEnabled = false;
        }

        private void Rd_Pushover_Checked(object sender, RoutedEventArgs e)
        {
            if (grpbox_Pushover != null)
            {
                grpbox_Pushover.IsEnabled = true;
                CheckRadioButtons(rd_Pushover);
            }
        }

        private void Rd_Pushover_Unchecked(object sender, RoutedEventArgs e)
        {
            grpbox_Pushover.IsEnabled = false;
        }

        private void Rd_Cyclic_Checked(object sender, RoutedEventArgs e)
        {
            if (grpbox_Cyclic != null)
            {
                grpbox_Cyclic.IsEnabled = true;
                CheckRadioButtons(rd_Cyclic);
            }
        }

        private void Rd_Cyclic_Unchecked(object sender, RoutedEventArgs e)
        {
            grpbox_Cyclic.IsEnabled = false;
        }

        private void Rd_Dynamic_Checked(object sender, RoutedEventArgs e)
        {
            if (grpbox_Dynamic != null)
            {
                grpbox_Dynamic.IsEnabled = true;
                CheckRadioButtons(rd_Dynamic);
            }
        }

        private void Rd_Dynamic_Unchecked(object sender, RoutedEventArgs e)
        {
            grpbox_Dynamic.IsEnabled = false;
        }

        private void CheckRadioButtons(RadioButton rd)
        {
            if (rd_Elastic != null && rd_Pushover != null && rd_Cyclic != null && rd_Dynamic != null)
            {
                if (rd_Elastic.Name != rd.Name)
                {
                    rd_Elastic.IsChecked = false;
                }
                if (rd_Pushover.Name != rd.Name)
                {
                    rd_Pushover.IsChecked = false;
                }
                if (rd_Cyclic.Name != rd.Name)
                {
                    rd_Cyclic.IsChecked = false;
                }
                if (rd_Dynamic.Name != rd.Name)
                {
                    rd_Dynamic.IsChecked = false;
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

        private void UseAnalysis_Click(object sender, RoutedEventArgs e)
        {
            if (analysis_ListBox.Items.Count == 0)
            {
                try
                {
                    if (rd_Elastic.IsChecked == true)
                    {
                        ElasticAnalysis analysis = new ElasticAnalysis();
                        analysis.Steps = int.Parse(txt_ElasticLoadSteps.Text);
                        MainWindow.analysis.Add(analysis);
                    }
                    else if (rd_Pushover.IsChecked == true)
                    {
                        PushoverAnalysis push = new PushoverAnalysis();
                        push.Steps = int.Parse(txt_LoadSteps.Text);
                        push.Iters = int.Parse(txt_Iterations.Text);
                        MainWindow.analysis.Add(push);
                    }
                    else if (rd_Cyclic.IsChecked == true)
                    {
                        CyclicAnalysis cyclic = new CyclicAnalysis();
                        cyclic.Steps = int.Parse(txt_CyclicLoadSteps.Text);
                        cyclic.Iters = int.Parse(txt_CyclicIterations.Text);
                        cyclic.InitialPeak = int.Parse(txt_CyclicIniPeak.Text);
                        cyclic.PeakIncrement = int.Parse(txt_CyclicPeakInc.Text);
                        cyclic.CyclesPerPeak = int.Parse(txt_CyclicCyclesPeak.Text);
                        cyclic.StepsPerPeak = int.Parse(txt_CyclicStepPeak.Text);
                        if (rd_posCyclic.IsChecked == true)
                        {
                            cyclic.Type = 'p';
                        }
                        else
                        {
                            cyclic.Type = 'r';
                        }
                        MainWindow.analysis.Add(cyclic);
                    }
                    else
                    {
                        DynamicAnalysis dyn = new DynamicAnalysis();
                        dyn.DeltaT = double.Parse(txt_SeismicTimeIncr.Text);
                        dyn.AdditionalTime = double.Parse(txt_SeismicAddTime.Text);
                        dyn.Iters = int.Parse(txt_SeismicIterations.Text);
                        dyn.IntegrationMethod = comb_intMethod.SelectedItem.ToString().Trim();
                        if (rd_seismic.IsChecked == true)
                        {
                            dyn.Type = 's';
                        }
                        else
                        {
                            dyn.Type = 'i';
                        }
                        MainWindow.analysis.Add(dyn);
                    }
                }
                catch
                {
                    MessageBox.Show("An error has ocurred and the analysis could not be defined. Please check your inputs and try again.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            } else
            {
                MessageBox.Show("Only one analysis type is allowed. Plase remove the current analysis and define a new one.", "Warning", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }

        private void RemoveAnalysis_Click(object sender, RoutedEventArgs e)
        {
            int index = analysis_ListBox.SelectedIndex;
            if (analysis_ListBox.Items.Count != 0)
            {
                if (index != -1)
                {
                    MainWindow.analysis.RemoveAt(index);
                    analysis_ListBox.Items.Refresh();
                }
                else
                {
                    MessageBox.Show("Please select a previously defined analysis.", "Attention", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }
    }
}
