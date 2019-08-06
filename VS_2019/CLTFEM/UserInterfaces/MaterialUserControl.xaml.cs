using CLTFEM.Classes.Helpers;
using CLTFEM.Classes.Structural;
using CLTFEM.Windows;
using System;
using System.Windows;
using System.Windows.Controls;

namespace CLTFEM.UserInterfaces
{
    /// <summary>
    /// Interaction logic for MaterialUserControl.xaml
    /// </summary>
    public partial class MaterialUserControl : UserControl
    {
        public MaterialUserControl()
        {
            InitializeComponent();
            materials_ListBox.ItemsSource = MainWindow.materialList;
            nonlinearRegion.IsEnabled = false;
            if (_compStiff != null)
            {
                _compStiff.IsEnabled = false;
            }
        }

        private void chk_Elastic_Unchecked(object sender, RoutedEventArgs e)
        {
            elasticRegion.IsEnabled = false;
            nonlinearRegion.IsEnabled = true;
            chk_Nonlinear.IsChecked = true;
        }

        private void chk_Elastic_Checked(object sender, RoutedEventArgs e)
        {
            if (chk_Nonlinear != null && chk_Elastic != null)
            {
                elasticRegion.IsEnabled = true;
                nonlinearRegion.IsEnabled = false;
                chk_Nonlinear.IsChecked = false;
            }
        }

        private void chk_Nonlinear_Unchecked(object sender, RoutedEventArgs e)
        {
            elasticRegion.IsEnabled = true;
            nonlinearRegion.IsEnabled = false;
            chk_Elastic.IsChecked = true;
        }

        private void chk_Nonlinear_Checked(object sender, RoutedEventArgs e)
        {
            if (chk_Nonlinear != null && chk_Elastic != null)
            {
                elasticRegion.IsEnabled = false;
                nonlinearRegion.IsEnabled = true;
                chk_Elastic.IsChecked = false;
            }
        }

        private void rd_Axial_Checked(object sender, RoutedEventArgs e)
        {
            if (_compStiff != null)
            {
                _compStiff.Text = "100000";
                _compStiff.IsEnabled = false;
            }
        }

        private void rd_General_Checked(object sender, RoutedEventArgs e)
        {
            if (_compStiff != null)
            {
                _compStiff.IsEnabled = true;
            }
        }

        private void Add_Material_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int count = MainWindow.materialList.Count + 1;

                if (chk_Elastic.IsChecked == true)
                {
                    MainWindow.materialList.Add(new OrthotropicElasticMaterial(count, Double.Parse(_EX.Text), Double.Parse(_EY.Text), Double.Parse(_poisson.Text), Double.Parse(_GXY.Text), Double.Parse(_GYZ.Text), Double.Parse(_GXZ.Text)));
                }
                else
                {
                    double iniStiff, fMax, dMax, degStiff, fRes, dUlt, compStiff, unlStiff, fUnl, conStiff, relStiff;
                    iniStiff = double.Parse(_iniStiff.Text);
                    fMax = double.Parse(_fMax.Text);
                    dMax = double.Parse(_dMax.Text);
                    degStiff = double.Parse(_degStiff.Text);
                    fRes = double.Parse(_fRes.Text);
                    dUlt = double.Parse(_dUlt.Text);
                    unlStiff = double.Parse(_unlStiff.Text);
                    fUnl = double.Parse(_fUnl.Text);
                    conStiff = double.Parse(_conStiff.Text);
                    relStiff = double.Parse(_relStiff.Text);
                    if (rd_Axial.IsChecked == true)
                    {
                        compStiff = double.Parse(_compStiff.Text);
                        MainWindow.materialList.Add(new SpringAxialModel(count, iniStiff, dMax, fMax, degStiff, fRes, dUlt, compStiff, unlStiff, fUnl, conStiff, relStiff));
                    }
                    else
                    {
                        MainWindow.materialList.Add(new SpringGeneralModel(count, iniStiff, dMax, fMax, degStiff, fRes, dUlt, unlStiff, fUnl, conStiff, relStiff));
                    }
                }
            }
            catch
            {
                MessageBox.Show("An error has occurred and the material could not be defined. Please check your inputs and try again.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Delete_Material_Click(object sender, RoutedEventArgs e)
        {
            int index = materials_ListBox.SelectedIndex;
            int originalCount = MainWindow.materialList.Count;
            if (index != -1)
            {
                MainWindow.materialList.RemoveAt(index);
                if ((index + 1) != originalCount)
                {
                    //that means that the item that was removed was not the last item of the original list
                    //so we need to re-organize the list
                    Management.ReorganizeMaterialList();
                    materials_ListBox.Items.Refresh();
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

        private void Materials_ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int index = materials_ListBox.SelectedIndex;
            if (index != -1)
            {
                Material mat = MainWindow.materialList[index];
                if (mat is OrthotropicElasticMaterial)
                {
                    OrthotropicElasticMaterial oMat = mat as OrthotropicElasticMaterial;
                    _EX.Text = oMat.Ex.ToString();
                    _EY.Text = oMat.Ey.ToString();
                    _GXY.Text = oMat.Gxy.ToString();
                    _GXZ.Text = oMat.Gxz.ToString();
                    _GYZ.Text = oMat.Gyz.ToString();
                    _poisson.Text = oMat.Vxy.ToString();
                    chk_Elastic.IsChecked = true;

                } else if (mat is SpringAxialModel){
                    SpringAxialModel saMat = mat as SpringAxialModel;
                    _iniStiff.Text = saMat._iniStiff.ToString();
                    _compStiff.Text = saMat._compStiff.ToString();
                    _fMax.Text = saMat._fMax.ToString();
                    _dMax.Text = saMat._dMax.ToString();
                    _degStiff.Text = saMat._degStiff.ToString();
                    _fRes.Text = saMat._fRes.ToString();
                    _dUlt.Text = saMat._dUlt.ToString();
                    _unlStiff.Text = saMat._unlStiff.ToString();
                    _fUnl.Text = saMat._fUnl.ToString();
                    _conStiff.Text = saMat._conStiff.ToString();
                    _relStiff.Text = saMat._relStiff.ToString();
                    chk_Nonlinear.IsChecked = true;
                }
                else //SpringGeneralModel
                {
                    SpringGeneralModel sgMat = mat as SpringGeneralModel;
                    _iniStiff.Text = sgMat._iniStiff.ToString();
                    _fMax.Text = sgMat._fMax.ToString();
                    _dMax.Text = sgMat._dMax.ToString();
                    _degStiff.Text = sgMat._degStiff.ToString();
                    _fRes.Text = sgMat._fRes.ToString();
                    _dUlt.Text = sgMat._dUlt.ToString();
                    _unlStiff.Text = sgMat._unlStiff.ToString();
                    _fUnl.Text = sgMat._fUnl.ToString();
                    _conStiff.Text = sgMat._conStiff.ToString();
                    _relStiff.Text = sgMat._relStiff.ToString();
                    chk_Nonlinear.IsChecked = true;
                }
            }
        }
    }
}
