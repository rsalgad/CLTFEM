using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;
using System.Windows.Media;

namespace CLTFEM.Classes.Helpers
{
    class Configuration
    {
        public static double mouseScrollSens = 100; //defines a value to serve as the 'sensitivity' of the middle mouse scroll speed
        public static double mouseScrollLimit = 500;
        public static double mouseDragSens = 1000;
        public static double mouse3DSens = 1000;  //defines the sensitivity of the x-y point on screen to actual 3d model coordinates
        public static Point3D initialCamPos = new Point3D(5000, 0, 0);
        public static Vector3D initialCamLookingDir = new Vector3D(-1, 0, 0);
        public static double perspCamFOV = 60;
        public static double mouseRotSens = 10;
        public static double selectAnimDur = 10;

        public static Color shellEleColor = Colors.Blue;
        public static Color selectedItemColor = Colors.GreenYellow;
        public static Color nodeColor = Colors.Black;
        public static Color spring3DColor = Colors.LightGreen;
        public static double nodeSize = 5;

        public static double translSupLength = 15;
        public static double translSupWidth = 10;
        public static Color translSupColor = Colors.Yellow;
        public static double rotSupLength = 30;
        public static double rotSupWidth = 5;
        public static Color rotSupColor = Colors.HotPink;

        public static double loadArrowLength = 30;
        public static double loadArrowThick = 5;
        public static double loadArrowTipLength = 10;
        public static double loadArrowTipWidth = 15;
        public static Color loadArrowColor = Colors.Red;

        public static double massArrowLength = 30;
        public static double massArrowThick = 5;
        public static double massArrowTipLength = 10;
        public static double massArrowTipWidth = 15;
        public static Color massArrowColor = Colors.Blue;

        public static double axisLength = 50;
        public static double axisWidth = 5;
        public static Color xAxisColor = Colors.Black;
        public static Color yAxisColor = Colors.Red;
        public static Color zAxisColor = Colors.Green;

        public static double minZoomParam = 1;
        public static double zoomParam = 1;
    }
}
