using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace CLTFEM.Classes.Helpers
{
    class GraphHelper
    {
        public GraphHelper()
        {
        }

        public static void DrawAxis(Canvas canv, double[,] origin)
        {
            Line xAxis = new Line();
            xAxis.Stroke = Brushes.Black;
            xAxis.X1 = 0.05 * canv.ActualWidth;
            xAxis.X2 = xAxis.X1 + 0.9 * canv.ActualWidth;
            xAxis.Y1 = origin[0, 1];
            xAxis.Y2 = origin[0, 1];

            Line yAxis = new Line();
            yAxis.Stroke = Brushes.Black;
            yAxis.X1 = origin[0, 0];
            yAxis.X2 = origin[0, 0];
            yAxis.Y1 = 0.05 * canv.ActualHeight;
            yAxis.Y2 = yAxis.Y1 + 0.9 * canv.ActualHeight;

            canv.Children.Add(xAxis);
            canv.Children.Add(yAxis);
        }

        public static void DrawLine(Canvas canv, Point p1, Point p2, double[,] origin)
        {
            Line line = new Line();
            line.Stroke = Brushes.Black;

            if (p1.X < 0)
            {
                line.X1 = origin[0, 0] - p1.X;
            } else
            {
                line.X1 = origin[0, 0] + p1.X;
            }

            if (p2.X < 0)
            {
                line.X2 = origin[0, 0] - p2.X;
            }
            else
            {
                line.X2 = p2.X + origin[0, 0];
            }

            if (p1.Y < 0)
            {
                line.Y1 = canv.ActualHeight - origin[0, 1] + p1.Y;
            } else
            {
                line.Y1 = canv.ActualHeight - origin[0, 1] - p1.Y;
            }

            if (p2.Y < 0)
            {
                line.Y2 = canv.ActualHeight - origin[0, 1] + p2.Y;
            } else
            {
                line.Y2 = canv.ActualHeight - origin[0, 1] - p2.Y;
            }

            canv.Children.Add(line);
        }

        public static void DrawLine(Canvas canv, Point p1, Point p2)
        {
            Line line = new Line();
            line.Stroke = Brushes.Red;

            line.X1 = p1.X;
            line.X2 = p2.X;
            line.Y1 = p1.Y;
            line.Y2 = p2.Y;
            canv.Children.Add(line);
        }

        public static void DrawAPoint(Canvas canv, double xPos, double yPos)
        {
            Ellipse point = new Ellipse();
            point.Stroke = Brushes.Black;
            point.Height = 5;
            point.Width = 5;
            point.Fill = Brushes.Black;
            point.Margin = new System.Windows.Thickness(xPos - point.Width / 2, yPos - point.Height / 2, 0, 0);
            canv.Children.Add(point);
        }

        public static void DeleteAllPoints(Canvas canv)
        {
            for (int i = 0; i < canv.Children.Count; i++)
            {
                if (canv.Children[i].GetType().ToString() == "System.Windows.Shapes.Ellipse")
                {
                    canv.Children.RemoveAt(i);
                }
            }

            /*
            foreach (UIElement element in canv.Children)
            {
                if (element is Ellipse)
                {
                    canv.Children.Remove(element);
                }
            }
            */

        }
    }
}
