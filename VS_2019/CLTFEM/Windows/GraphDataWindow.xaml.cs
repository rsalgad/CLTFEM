using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace CLTFEM.Windows
{
    /// <summary>
    /// Interaction logic for GraphDataWindow.xaml
    /// </summary>
    public partial class GraphDataWindow : Window
    {
        private Point[] _pointList;

        public GraphDataWindow(ref Point[] pointList)
        {
            _pointList = pointList;
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //txt_data.Width = this.Width;
            //txt_data.Height = this.Height;
            ShowData();
        }

        private void ShowData()
        {
            string text = "";
            for (int i = 0; i < _pointList.Length; i++)
            {
                text += _pointList[i].X.ToString() + "\t" + _pointList[i].Y.ToString() + "\n";
            }
            txt_data.Text = text;
        }
    }
}
