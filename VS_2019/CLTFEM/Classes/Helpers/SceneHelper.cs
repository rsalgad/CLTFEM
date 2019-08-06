using System;
using System.Windows.Media.Media3D;
using System.Windows.Media;


namespace CLTFEM.Classes.Helpers
{
    class SceneHelper
    {
        /// <summary>
        /// Sets up the lighting
        /// </summary>
        public static void setupLighting(Model3DGroup my3DGroup)
        {
            AmbientLight myAmbLight = new AmbientLight();
            myAmbLight.Color = Colors.White;
            my3DGroup.Children.Add(myAmbLight);

        }

        /// <summary>
        /// Configures the perspective camera
        /// </summary>
        /// <param name="myPCamera">Reference to which camera to configure</param>
        /// <param name="position">Position of the camera</param>
        /// <param name="lookDir">Looking direction of the camera</param>
        /// <param name="FOV">Field of view</param>
        public static void setupPerspCamera(ref PerspectiveCamera myPCamera, Point3D position, Vector3D lookDir, double FOV)
        {
            // Defines the camera used to view the 3D object. In order to view the 3D object,
            // the camera must be positioned and pointed such that the object is within view 
            // of the camera.
            myPCamera = new PerspectiveCamera();

            // Specify where in the 3D scene the camera is.
            myPCamera.Position = position;

            // Specify the direction that the camera is pointing.
            myPCamera.LookDirection = lookDir;

            myPCamera.UpDirection = new Vector3D(0, 0, 1);

            // Define camera's horizontal field of view in degrees.
            myPCamera.FieldOfView = FOV;
        }

        /// <summary>
        /// Configures the orthotropic camera
        /// </summary>
        /// <param name="myPCamera">Reference to which camera to configure</param>
        /// <param name="position">Position of the camera</param>
        /// <param name="lookDir">Looking direction of the camera</param>
        public static void setupOrthoCamera(ref OrthographicCamera myPCamera, Point3D position, Vector3D lookDir)
        {
            // Defines the camera used to view the 3D object. In order to view the 3D object,
            // the camera must be positioned and pointed such that the object is within view 
            // of the camera.
            myPCamera = new OrthographicCamera();

            myPCamera.UpDirection = new Vector3D(0, 0, 1);

            // Specify where in the 3D scene the camera is.
            myPCamera.Position = position;

            // Specify the direction that the camera is pointing.
            myPCamera.LookDirection = lookDir;
        }

        public static double camRadius(double myCameraX, double myCameraZ)
        {
            return Math.Sqrt(Math.Pow(myCameraX, 2) + Math.Pow(myCameraZ, 2));
        }

        /// <summary>
        /// Sets up the transformation groups responsible to manage the camera rotations
        /// </summary>
        public static Transform3DGroup camTransformGroup(AxisAngleRotation3D camRotX, AxisAngleRotation3D camRotY, AxisAngleRotation3D camRotZ)
        {
            Transform3DGroup transfGroup = new Transform3DGroup();
            RotateTransform3D rotCamX = new RotateTransform3D(camRotX);
            RotateTransform3D rotCamY = new RotateTransform3D(camRotY);
            RotateTransform3D rotCamZ = new RotateTransform3D(camRotZ);
            transfGroup.Children.Add(rotCamX);
            transfGroup.Children.Add(rotCamY);
            transfGroup.Children.Add(rotCamZ);
            return transfGroup;
        }

    }
}