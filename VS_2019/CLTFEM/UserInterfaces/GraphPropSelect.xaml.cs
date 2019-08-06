using CLTFEM.Windows;
using System.Windows;
using System.Windows.Controls;

namespace CLTFEM.UserInterfaces
{
    /// <summary>
    /// Interaction logic for GraphPropSelect.xaml
    /// </summary>
    public partial class GraphPropSelect : UserControl
    {
        public GraphPropSelect()
        {
            InitializeComponent();
        }

        private void Button_Select_Clicked(object sender, RoutedEventArgs e)
        {
            if (rd_xTrans.IsChecked == true)
            {
                MainWindow.horAxisProp = "xTrans";
            } else if (rd_yTrans.IsChecked == true)
            {
               MainWindow.horAxisProp = "yTrans";
            } else
            {
               MainWindow.horAxisProp = "zTrans";
            }

            if (rd_xLoad.IsChecked == true)
            {
                MainWindow.vertAxisProp = "totXLoad";
            }
            else if (rd_yLoad.IsChecked == true)
            {
                MainWindow.vertAxisProp = "totYLoad";
            }
            else
            {
                MainWindow.vertAxisProp = "totZLoad";
            }

            MainWindow.graphNodeID = int.Parse(txt_nodeID.Text);
            MainWindow.graphVertNodeID = int.Parse(txt_vertNodeID.Text);

            btn_Select.IsEnabled = false;
        }

        private void Txt_nodeID_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (btn_Select != null)
            {
                if (btn_Select.IsEnabled == false)
                {
                    btn_Select.IsEnabled = true;
                }
            }

        }

        private void Rd_xTrans_Checked(object sender, RoutedEventArgs e)
        {
            if (btn_Select != null)
            {
                if (btn_Select.IsEnabled == false)
                {
                    btn_Select.IsEnabled = true;
                }
            }
        }

        private void Rd_yTrans_Checked(object sender, RoutedEventArgs e)
        {
            if (btn_Select != null)
            {
                if (btn_Select.IsEnabled == false)
                {
                    btn_Select.IsEnabled = true;
                }
            }
        }

        private void Rd_zTrans_Checked(object sender, RoutedEventArgs e)
        {
            if (btn_Select != null)
            {
                if (btn_Select.IsEnabled == false)
                {
                    btn_Select.IsEnabled = true;
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
