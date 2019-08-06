using CLTFEM.Classes.Helpers;
using System;
using System.Windows;
using System.Windows.Controls;

namespace CLTFEM.Windows
{
    /// <summary>
    /// Interaction logic for GraphWindow.xaml
    /// </summary>
    public partial class GraphWindow : Window
    {
        public double[,] origin = new double[,] { { 50, 50 } };
        public static int loadSteps = 10;
        public static int numberFiles;
        public Point[] pointList;

        public GraphWindow()
        {
            InitializeComponent();
            numberFiles = MainWindow.seriesDispList.Count;
            pointList = new Point[numberFiles + 1];
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            txt_width.Text = GraphCanvas.ActualWidth.ToString();
            txt_height.Text = GraphCanvas.ActualHeight.ToString();

            bck_rectangle.Width = GraphCanvas.ActualWidth;
            bck_rectangle.Height = GraphCanvas.ActualHeight;

            DrawGraph();
            GraphHelper.DrawAxis(GraphCanvas, origin);
        }


        private void Window_LayoutUpdated(object sender, System.EventArgs e)
        {
            txt_width.Text = GraphCanvas.ActualWidth.ToString();
            txt_height.Text = GraphCanvas.ActualHeight.ToString();

            bck_rectangle.Width = GraphCanvas.ActualWidth;
            bck_rectangle.Height = GraphCanvas.ActualHeight;
        }

        private void DrawGraph()
        {
            double totalLoad = 0;
            /*
            int vertNodeID = MainWindow.graphVertNodeID;
            if (MainWindow.vertAxisProp == "totXLoad")
            {
                for (int i = 0; i < MainWindow.seriesLoadList.Count; i++) //for each load
                {
                    for (int j = 0; j < MainWindow.seriesLoadList[i].GetLoadList.Count; j++) //for each direction
                    {
                        if (MainWindow.loadList[i].GetLoadList[j].ID == 1) //if x-dir
                        {
                            totalLoad += MainWindow.loadList[i].GetLoadList[j].GetVal;
                        }
                    }
                }
            } else if (MainWindow.vertAxisProp == "totYLoad")
            {
                for (int i = 0; i < MainWindow.seriesLoadList.Count; i++)
                {
                    for (int j = 0; j < MainWindow.seriesLoadList[i].GetLoadList.Count; j++)
                    {
                        if (MainWindow.loadList[i].GetLoadList[j].ID == 2)
                        {
                            totalLoad += MainWindow.loadList[i].GetLoadList[j].GetVal;
                        }
                    }
                }
            } else
            {
                for (int i = 0; i < MainWindow.seriesLoadList.Count; i++)
                {
                    for (int j = 0; j < MainWindow.seriesLoadList[i].GetLoadList.Count; j++)
                    {
                        if (MainWindow.loadList[i].GetLoadList[j].ID == 3)
                        {
                            totalLoad += MainWindow.loadList[i].GetLoadList[j].GetVal;
                        }
                    }
                }
            }
            */
            int nodeID = MainWindow.graphNodeID;
            int vertNodeID = MainWindow.graphVertNodeID;
            pointList[0] = new Point(0, 0);
            if (MainWindow.horAxisProp == "xTrans")
            {
                if (MainWindow.vertAxisProp == "totXLoad")
                {
                    for (int i = 0; i < numberFiles; i++)
                    {
                        pointList[i + 1] = new Point(MainWindow.seriesDispList[i][nodeID - 1].Point.X, MainWindow.seriesLoadList[i][vertNodeID - 1].GetLoadList[0].GetVal);
                    }
                } else if (MainWindow.vertAxisProp == "totYLoad")
                {
                    for (int i = 0; i < numberFiles; i++)
                    {
                        pointList[i + 1] = new Point(MainWindow.seriesDispList[i][nodeID - 1].Point.X, MainWindow.seriesLoadList[i][vertNodeID - 1].GetLoadList[1].GetVal);
                    }
                } else
                {
                    for (int i = 0; i < numberFiles; i++)
                    {
                        pointList[i + 1] = new Point(MainWindow.seriesDispList[i][nodeID - 1].Point.X, MainWindow.seriesLoadList[i][vertNodeID - 1].GetLoadList[2].GetVal);
                    }
                }
            } else if (MainWindow.horAxisProp == "yTrans"){
                if (MainWindow.vertAxisProp == "totXLoad")
                {
                    for (int i = 0; i < numberFiles; i++)
                    {
                        pointList[i + 1] = new Point(MainWindow.seriesDispList[i][nodeID - 1].Point.Y, MainWindow.seriesLoadList[i][vertNodeID - 1].GetLoadList[0].GetVal);
                    }
                }
                else if (MainWindow.vertAxisProp == "totYLoad")
                {
                    for (int i = 0; i < numberFiles; i++)
                    {
                        pointList[i + 1] = new Point(MainWindow.seriesDispList[i][nodeID - 1].Point.Y, MainWindow.seriesLoadList[i][vertNodeID - 1].GetLoadList[1].GetVal);
                    }
                }
                else
                {
                    for (int i = 0; i < numberFiles; i++)
                    {
                        pointList[i + 1] = new Point(MainWindow.seriesDispList[i][nodeID - 1].Point.Y, MainWindow.seriesLoadList[i][vertNodeID - 1].GetLoadList[2].GetVal);
                    }
                }
            } else
            {
                if (MainWindow.vertAxisProp == "totXLoad")
                {
                    for (int i = 0; i < numberFiles; i++)
                    {
                        pointList[i + 1] = new Point(MainWindow.seriesDispList[i][nodeID - 1].Point.Z, MainWindow.seriesLoadList[i][vertNodeID - 1].GetLoadList[0].GetVal);
                    }
                }
                else if (MainWindow.vertAxisProp == "totYLoad")
                {
                    for (int i = 0; i < numberFiles; i++)
                    {
                        pointList[i + 1] = new Point(MainWindow.seriesDispList[i][nodeID - 1].Point.Z, MainWindow.seriesLoadList[i][vertNodeID - 1].GetLoadList[1].GetVal);
                    }
                }
                else
                {
                    for (int i = 0; i < numberFiles; i++)
                    {
                        pointList[i + 1] = new Point(MainWindow.seriesDispList[i][nodeID - 1].Point.Z, MainWindow.seriesLoadList[i][vertNodeID - 1].GetLoadList[2].GetVal);
                    }
                }
            }

            double maxY = 0, maxX = 0, minY = 0, minX = 0;

            for (int i = 0; i < numberFiles + 1; i++)
            {
                if (pointList[i].X < 0)
                {
                    if (pointList[i].X < minX)
                    {
                        minX = pointList[i].X;
                    }
                } else
                {
                    if (pointList[i].X > maxX)
                    {
                        maxX = pointList[i].X;
                    }
                }

                if (pointList[i].Y < 0)
                {
                    if (pointList[i].Y < minY)
                    {
                        minY = pointList[i].Y;
                    }
                }
                else
                {
                    if (pointList[i].Y > maxY)
                    {
                        maxY = pointList[i].Y;
                    }
                }
            }

            double maxXRatio;
            double totalAvailX = 0.9 * GraphCanvas.ActualWidth;
            double totalAvailY = 0.9 * GraphCanvas.ActualHeight;
            double maxYRatio;
            double originX;
            double originY;

            if (maxX == 0)
            {
                maxXRatio = minX / (maxX - minX);
                maxX = minX;
                originX = 0.05 * GraphCanvas.ActualWidth - maxXRatio * totalAvailX;
            } else
            {
                maxXRatio = maxX / (maxX - minX);
                originX = 0.05 * GraphCanvas.ActualWidth + (1 - maxXRatio) * totalAvailX;
            }

            if (maxY == 0)
            {
                maxYRatio = minY / (maxY - minY);
                maxY = minY;
                originY = 0.05 * GraphCanvas.ActualHeight - maxYRatio * totalAvailY;
            } else
            {
                maxYRatio = maxY / (maxY - minY);
                originY = 0.05 * GraphCanvas.ActualHeight + (1 - maxYRatio) * totalAvailY;
            }

            origin[0, 0] = originX;
            origin[0, 1] = originY;

            for (int i = 0; i < numberFiles; i++)
            {
                Point p1 = new Point(originX + pointList[i].X * (maxXRatio * totalAvailX) / maxX, originY + pointList[i].Y * (maxYRatio * totalAvailY) / maxY);
                Point p2 = new Point(originX + pointList[i + 1].X * (maxXRatio * totalAvailX) / maxX, originY + pointList[i + 1].Y * (maxYRatio * totalAvailY) / maxY);
                GraphHelper.DrawLine(GraphCanvas, p1, p2);
            }
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            CleanCanvas(GraphCanvas);
            DrawGraph();
            GraphHelper.DrawAxis(GraphCanvas, origin);
        }

        private void GraphCanvas_MouseRightButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {

        }

        private void CleanCanvas(Canvas canv)
        {
            for (int i = 5; i <  canv.Children.Count; i++)
            {
                canv.Children.RemoveRange(5, canv.Children.Count - 5);
            }
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            GraphDataWindow window = new GraphDataWindow(ref pointList);
            window.Show();
        }
    }
}
