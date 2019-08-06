using System;
using CLTFEM.Classes.Structural;
using CLTFEM.Classes.Mathematics;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using System.Windows.Controls;
using System.Windows.Documents;
using CLTFEM.Windows;
using System.Collections.Generic;
using System.Windows;

namespace CLTFEM.Classes.Helpers
{
    class DrawingHelper
    {

        /// <summary>
        /// Create a 3D Line in the 3D Space
        /// </summary>
        /// <param name="length">The length of the line</param>
        /// <param name="thickness">The thickness of the line</param>
        /// <param name="iniPoint">The initial position of the line</param>
        /// <param name="dirVec">The direction of the line from the initial position</param>
        /// <param name="color">The color of the line</param>
        /// <returns></returns>
        public static Model3DGroup Draw3DLine(double length, double thickness, Point3D iniPoint, Vector3D dirVec, Color color)
        {
            // Declare scene objects.
            Model3DGroup myModel3DGroup = new Model3DGroup();
            GeometryModel3D myGeometryModel = new GeometryModel3D();

            // The geometry specifes the shape of the 3D plane. In this sample, a flat sheet 
            // is created.
            MeshGeometry3D myMeshGeometry3D = new MeshGeometry3D();

            // Create a collection of vertex positions for the MeshGeometry3D. 
            Point3DCollection myPositionCollection = new Point3DCollection();
            Vector3D myVector3D = new Vector3D(0, 0, 0); //initial position of the Line

            //This block creates a rectangle in the XY plane facing positive Z
            myPositionCollection.Add(new Point3D(myVector3D.X, myVector3D.Y - thickness / 2, myVector3D.Z + thickness / 2)); //0
            myPositionCollection.Add(new Point3D(myVector3D.X + length, myVector3D.Y - thickness / 2, myVector3D.Z + thickness / 2)); //1
            myPositionCollection.Add(new Point3D(myVector3D.X + length, myVector3D.Y + thickness / 2, myVector3D.Z + thickness / 2)); //2
            myPositionCollection.Add(new Point3D(myVector3D.X, myVector3D.Y + thickness / 2, myVector3D.Z + thickness / 2)); //3

            //This block creates a rectangle in the XZ plane facing positive Y
            myPositionCollection.Add(new Point3D(myVector3D.X + length, myVector3D.Y + thickness / 2, myVector3D.Z - thickness / 2)); //6
            myPositionCollection.Add(new Point3D(myVector3D.X, myVector3D.Y + thickness / 2, myVector3D.Z - thickness / 2)); //7

            //This block creates a rectangle in the XZ plane facing negative Y
            myPositionCollection.Add(new Point3D(myVector3D.X + length, myVector3D.Y - thickness / 2, myVector3D.Z - thickness / 2)); //10
            myPositionCollection.Add(new Point3D(myVector3D.X, myVector3D.Y - thickness / 2, myVector3D.Z - thickness / 2)); //11

            myMeshGeometry3D.Positions = myPositionCollection;

            // Create a collection of triangle indices for the MeshGeometry3D.
            Int32Collection myTriangleIndicesCollection = new Int32Collection();
            myTriangleIndicesCollection.Add(0);
            myTriangleIndicesCollection.Add(1);
            myTriangleIndicesCollection.Add(3);
            myTriangleIndicesCollection.Add(3);
            myTriangleIndicesCollection.Add(1);
            myTriangleIndicesCollection.Add(2);

            myTriangleIndicesCollection.Add(3);
            myTriangleIndicesCollection.Add(2);
            myTriangleIndicesCollection.Add(5);
            myTriangleIndicesCollection.Add(5);
            myTriangleIndicesCollection.Add(2);
            myTriangleIndicesCollection.Add(4);

            myTriangleIndicesCollection.Add(1);
            myTriangleIndicesCollection.Add(0);
            myTriangleIndicesCollection.Add(7);
            myTriangleIndicesCollection.Add(7);
            myTriangleIndicesCollection.Add(6);
            myTriangleIndicesCollection.Add(1);

            myTriangleIndicesCollection.Add(6);
            myTriangleIndicesCollection.Add(7);
            myTriangleIndicesCollection.Add(5);
            myTriangleIndicesCollection.Add(5);
            myTriangleIndicesCollection.Add(4);
            myTriangleIndicesCollection.Add(6);

            myTriangleIndicesCollection.Add(3);
            myTriangleIndicesCollection.Add(5);
            myTriangleIndicesCollection.Add(0);
            myTriangleIndicesCollection.Add(0);
            myTriangleIndicesCollection.Add(5);
            myTriangleIndicesCollection.Add(7);

            myTriangleIndicesCollection.Add(4);
            myTriangleIndicesCollection.Add(2);
            myTriangleIndicesCollection.Add(1);
            myTriangleIndicesCollection.Add(1);
            myTriangleIndicesCollection.Add(6);
            myTriangleIndicesCollection.Add(4);

            myMeshGeometry3D.TriangleIndices = myTriangleIndicesCollection;

            // Apply the mesh to the geometry model.
            myGeometryModel.Geometry = myMeshGeometry3D;

            // The material specifies the material applied to the 3D object. In this sample a  
            // linear gradient covers the surface of the 3D object.

            // Create a horizontal linear gradient with four stops.   
            SolidColorBrush myColorBrush = new SolidColorBrush();
            myColorBrush.Color = color;
            myColorBrush.Opacity = 1;

            // Define material and apply to the mesh geometries.
            DiffuseMaterial myMaterial = new DiffuseMaterial(myColorBrush);
            myGeometryModel.Material = myMaterial;

            //Apply the rotation
            Transform3DGroup myTransfGroup = new Transform3DGroup();
            myTransfGroup.Children.Add(applyRotation(dirVec));
            myTransfGroup.Children.Add(applyTranslation(iniPoint));

            myGeometryModel.Transform = myTransfGroup;

            // Add the geometry model to the model group.
            myModel3DGroup.Children.Add(myGeometryModel);
            return myModel3DGroup;

        }

        /// <summary>
        /// Creates the 3D arrow used to represent load arrows
        /// </summary>
        /// <param name="length">Length of the arrow</param>
        /// <param name="thickness">thickness of the arrow</param>
        /// <param name="iniPoint">Initial point of the arrow. This is the node point where the load is applied</param>
        /// <param name="dirVec">Direction of the arrow</param>
        /// <param name="color">Color of the arrow</param>
        /// <returns></returns>
        public static Model3DGroup Draw3DArrow(double length, double thickness, double tipLength, double tipWidth, Point3D iniPoint, Vector3D dirVec, Color color)
        {
            // Declare scene objects.
            Model3DGroup myModel3DGroup = new Model3DGroup();
            GeometryModel3D myGeometryModel = new GeometryModel3D();

            // The geometry specifes the shape of the 3D plane. In this sample, a flat sheet 
            // is created.
            MeshGeometry3D myMeshGeometry3D = new MeshGeometry3D();

            // Create a collection of vertex positions for the MeshGeometry3D. 
            Point3DCollection myPositionCollection = new Point3DCollection();
            Vector3D myVector3D = new Vector3D(-length - tipLength, 0, 0); //initial position of the Line

            { //this block creates the arrow body

                //This block creates a rectangle in the XY plane facing positive Z
                myPositionCollection.Add(new Point3D(myVector3D.X, myVector3D.Y - thickness / 2, myVector3D.Z + thickness / 2)); //0
                myPositionCollection.Add(new Point3D(myVector3D.X + length, myVector3D.Y - thickness / 2, myVector3D.Z + thickness / 2)); //1
                myPositionCollection.Add(new Point3D(myVector3D.X + length, myVector3D.Y + thickness / 2, myVector3D.Z + thickness / 2)); //2
                myPositionCollection.Add(new Point3D(myVector3D.X, myVector3D.Y + thickness / 2, myVector3D.Z + thickness / 2)); //3

                //This block creates a rectangle in the XZ plane facing positive Y
                myPositionCollection.Add(new Point3D(myVector3D.X + length, myVector3D.Y + thickness / 2, myVector3D.Z - thickness / 2)); //4
                myPositionCollection.Add(new Point3D(myVector3D.X, myVector3D.Y + thickness / 2, myVector3D.Z - thickness / 2)); //5

                //This block creates a rectangle in the XZ plane facing negative Y
                myPositionCollection.Add(new Point3D(myVector3D.X + length, myVector3D.Y - thickness / 2, myVector3D.Z - thickness / 2)); //6
                myPositionCollection.Add(new Point3D(myVector3D.X, myVector3D.Y - thickness / 2, myVector3D.Z - thickness / 2)); //7
            }

            {//this block creates the arrow head
                myPositionCollection.Add(new Point3D(myVector3D.X + length + tipLength, myVector3D.Y, myVector3D.Z)); //24
                myPositionCollection.Add(new Point3D(myVector3D.X + length, myVector3D.Y + tipWidth/2, myVector3D.Z + tipWidth / 2)); //25
                myPositionCollection.Add(new Point3D(myVector3D.X + length, myVector3D.Y - tipWidth / 2, myVector3D.Z + tipWidth / 2)); //26
                myPositionCollection.Add(new Point3D(myVector3D.X + length, myVector3D.Y - tipWidth / 2, myVector3D.Z - tipWidth / 2)); //27
                myPositionCollection.Add(new Point3D(myVector3D.X + length, myVector3D.Y + tipWidth / 2, myVector3D.Z - tipWidth / 2)); //28
                myPositionCollection.Add(new Point3D(myVector3D.X + length, myVector3D.Y, myVector3D.Z)); //29
            }

            myMeshGeometry3D.Positions = myPositionCollection;


            Int32Collection myTriangleIndicesCollection = new Int32Collection();
            // Create a collection of triangle indices for the body of the arrow.
            {
                myTriangleIndicesCollection.Add(0);
                myTriangleIndicesCollection.Add(1);
                myTriangleIndicesCollection.Add(3);
                myTriangleIndicesCollection.Add(3);
                myTriangleIndicesCollection.Add(1);
                myTriangleIndicesCollection.Add(2);

                myTriangleIndicesCollection.Add(3);
                myTriangleIndicesCollection.Add(2);
                myTriangleIndicesCollection.Add(5);
                myTriangleIndicesCollection.Add(5);
                myTriangleIndicesCollection.Add(2);
                myTriangleIndicesCollection.Add(4);

                myTriangleIndicesCollection.Add(1);
                myTriangleIndicesCollection.Add(0);
                myTriangleIndicesCollection.Add(7);
                myTriangleIndicesCollection.Add(7);
                myTriangleIndicesCollection.Add(6);
                myTriangleIndicesCollection.Add(1);

                myTriangleIndicesCollection.Add(6);
                myTriangleIndicesCollection.Add(7);
                myTriangleIndicesCollection.Add(5);
                myTriangleIndicesCollection.Add(5);
                myTriangleIndicesCollection.Add(4);
                myTriangleIndicesCollection.Add(6);

                myTriangleIndicesCollection.Add(3);
                myTriangleIndicesCollection.Add(5);
                myTriangleIndicesCollection.Add(0);
                myTriangleIndicesCollection.Add(0);
                myTriangleIndicesCollection.Add(5);
                myTriangleIndicesCollection.Add(7);

                myTriangleIndicesCollection.Add(4);
                myTriangleIndicesCollection.Add(2);
                myTriangleIndicesCollection.Add(1);
                myTriangleIndicesCollection.Add(1);
                myTriangleIndicesCollection.Add(6);
                myTriangleIndicesCollection.Add(4);
            }

            // Create a collection of triangle indices for the tip of the arrow.
            {
                myTriangleIndicesCollection.Add(9);
                myTriangleIndicesCollection.Add(10);
                myTriangleIndicesCollection.Add(8);

                myTriangleIndicesCollection.Add(10);
                myTriangleIndicesCollection.Add(11);
                myTriangleIndicesCollection.Add(8);

                myTriangleIndicesCollection.Add(11);
                myTriangleIndicesCollection.Add(12);
                myTriangleIndicesCollection.Add(8);

                myTriangleIndicesCollection.Add(12);
                myTriangleIndicesCollection.Add(9);
                myTriangleIndicesCollection.Add(8);

                myTriangleIndicesCollection.Add(10);
                myTriangleIndicesCollection.Add(9);
                myTriangleIndicesCollection.Add(13);

                myTriangleIndicesCollection.Add(11);
                myTriangleIndicesCollection.Add(10);
                myTriangleIndicesCollection.Add(13);

                myTriangleIndicesCollection.Add(12);
                myTriangleIndicesCollection.Add(11);
                myTriangleIndicesCollection.Add(13);

                myTriangleIndicesCollection.Add(9);
                myTriangleIndicesCollection.Add(12);
                myTriangleIndicesCollection.Add(13);
            }
            myMeshGeometry3D.TriangleIndices = myTriangleIndicesCollection;

            // Apply the mesh to the geometry model.
            myGeometryModel.Geometry = myMeshGeometry3D;

            // The material specifies the material applied to the 3D object. In this sample a  
            // linear gradient covers the surface of the 3D object.

            // Create a horizontal linear gradient with four stops.   
            SolidColorBrush myColorBrush = new SolidColorBrush();
            myColorBrush.Color = color;
            myColorBrush.Opacity = 1;

            // Define material and apply to the mesh geometries.
            DiffuseMaterial myMaterial = new DiffuseMaterial(myColorBrush);
            myGeometryModel.Material = myMaterial;

            //Apply the rotation
            Transform3DGroup myTransfGroup = new Transform3DGroup();
            myTransfGroup.Children.Add(applyRotation(dirVec));
            myTransfGroup.Children.Add(applyTranslation(iniPoint));

            myGeometryModel.Transform = myTransfGroup;

            // Add the geometry model to the model group.
            myModel3DGroup.Children.Add(myGeometryModel);
            return myModel3DGroup;

        }

        /// <summary>
        /// Creates the 3D objects that represents the traslational supports
        /// </summary>
        /// <param name="length">The length of the object</param>
        /// <param name="width">The width of the object</param>
        /// <param name="iniPoint">The initial point of the object</param>
        /// <param name="dirVec">The direction vector of the object. Which DOF it restrains.</param>
        /// <param name="color">The color of the object</param>
        /// <returns></returns>
        public static Model3DGroup Draw3DTranslSupport(double length, double width, Point3D iniPoint, Vector3D dirVec, Color color)
        {
            // Declare scene objects.
            Model3DGroup myModel3DGroup = new Model3DGroup();
            GeometryModel3D myGeometryModel = new GeometryModel3D();

            // The geometry specifes the shape of the 3D plane. In this sample, a flat sheet 
            // is created.
            MeshGeometry3D myMeshGeometry3D = new MeshGeometry3D();

            // Create a collection of vertex positions for the MeshGeometry3D. 
            Point3DCollection myPositionCollection = new Point3DCollection();
            Vector3D myVector3D = new Vector3D(-length, 0, 0); //initial position of the Line

            myPositionCollection.Add(new Point3D(myVector3D.X + length, myVector3D.Y, myVector3D.Z)); //0
            myPositionCollection.Add(new Point3D(myVector3D.X, myVector3D.Y + width / 2, myVector3D.Z + width / 2)); //1
            myPositionCollection.Add(new Point3D(myVector3D.X, myVector3D.Y - width / 2, myVector3D.Z + width / 2)); //2
            myPositionCollection.Add(new Point3D(myVector3D.X, myVector3D.Y - width / 2, myVector3D.Z - width / 2)); //3
            myPositionCollection.Add(new Point3D(myVector3D.X, myVector3D.Y + width / 2, myVector3D.Z - width / 2)); //4
            myPositionCollection.Add(new Point3D(myVector3D.X, myVector3D.Y, myVector3D.Z)); //5
            myMeshGeometry3D.Positions = myPositionCollection;

            Int32Collection myTriangleIndicesCollection = new Int32Collection();
            myTriangleIndicesCollection.Add(1);
            myTriangleIndicesCollection.Add(2);
            myTriangleIndicesCollection.Add(0);

            myTriangleIndicesCollection.Add(2);
            myTriangleIndicesCollection.Add(3);
            myTriangleIndicesCollection.Add(0);

            myTriangleIndicesCollection.Add(3);
            myTriangleIndicesCollection.Add(4);
            myTriangleIndicesCollection.Add(0);

            myTriangleIndicesCollection.Add(4);
            myTriangleIndicesCollection.Add(1);
            myTriangleIndicesCollection.Add(0);

            myTriangleIndicesCollection.Add(2);
            myTriangleIndicesCollection.Add(1);
            myTriangleIndicesCollection.Add(5);

            myTriangleIndicesCollection.Add(3);
            myTriangleIndicesCollection.Add(2);
            myTriangleIndicesCollection.Add(5);

            myTriangleIndicesCollection.Add(4);
            myTriangleIndicesCollection.Add(3);
            myTriangleIndicesCollection.Add(5);

            myTriangleIndicesCollection.Add(1);
            myTriangleIndicesCollection.Add(4);
            myTriangleIndicesCollection.Add(5);

            myMeshGeometry3D.TriangleIndices = myTriangleIndicesCollection;

            // Apply the mesh to the geometry model.
            myGeometryModel.Geometry = myMeshGeometry3D;

            // The material specifies the material applied to the 3D object. In this sample a  
            // linear gradient covers the surface of the 3D object.

            // Create a horizontal linear gradient with four stops.   
            SolidColorBrush myColorBrush = new SolidColorBrush();
            myColorBrush.Color = color;
            myColorBrush.Opacity = 1;

            // Define material and apply to the mesh geometries.
            DiffuseMaterial myMaterial = new DiffuseMaterial(myColorBrush);
            myGeometryModel.Material = myMaterial;

            //Apply the rotation
            Transform3DGroup myTransfGroup = new Transform3DGroup();
            myTransfGroup.Children.Add(applyRotation(dirVec));
            myTransfGroup.Children.Add(applyTranslation(iniPoint));

            myGeometryModel.Transform = myTransfGroup;

            // Add the geometry model to the model group.
            myModel3DGroup.Children.Add(myGeometryModel);
            return myModel3DGroup;
        }

        /// <summary>
        /// Creates the 3D objects that represent the rotational supports
        /// </summary>
        /// <param name="length">The length of the object</param>
        /// <param name="width">The width of the object</param>
        /// <param name="iniPoint">The initial point of the object</param>
        /// <param name="dirVec">The direction vector of the object. Which DOF it restrains</param>
        /// <param name="color">The color of the object</param>
        /// <returns></returns>
        public static Model3DGroup Draw3DRotSupport(double length, double width, Point3D iniPoint, Vector3D dirVec, Color color)
        {
            // Declare scene objects.
            Model3DGroup myModel3DGroup = new Model3DGroup();
            GeometryModel3D myGeometryModel = new GeometryModel3D();

            // The geometry specifes the shape of the 3D plane. In this sample, a flat sheet 
            // is created.
            MeshGeometry3D myMeshGeometry3D = new MeshGeometry3D();

            // Create a collection of vertex positions for the MeshGeometry3D. 
            Point3DCollection myPositionCollection = new Point3DCollection();
            Vector3D myVector3D = new Vector3D(-length, 0, 0); //initial position of the Line

            myPositionCollection.Add(new Point3D(myVector3D.X + length/2, myVector3D.Y, myVector3D.Z)); //0
            myPositionCollection.Add(new Point3D(myVector3D.X, myVector3D.Y + width / 2, myVector3D.Z + width / 2)); //1
            myPositionCollection.Add(new Point3D(myVector3D.X, myVector3D.Y - width / 2, myVector3D.Z + width / 2)); //2
            myPositionCollection.Add(new Point3D(myVector3D.X, myVector3D.Y - width / 2, myVector3D.Z - width / 2)); //3
            myPositionCollection.Add(new Point3D(myVector3D.X, myVector3D.Y + width / 2, myVector3D.Z - width / 2)); //4
            myPositionCollection.Add(new Point3D(myVector3D.X, myVector3D.Y, myVector3D.Z)); //5

            myPositionCollection.Add(new Point3D(myVector3D.X + length, myVector3D.Y, myVector3D.Z)); //6
            myPositionCollection.Add(new Point3D(myVector3D.X + length/2, myVector3D.Y + width / 2, myVector3D.Z + width / 2)); //7
            myPositionCollection.Add(new Point3D(myVector3D.X + length/2, myVector3D.Y - width / 2, myVector3D.Z + width / 2)); //8
            myPositionCollection.Add(new Point3D(myVector3D.X + length/2, myVector3D.Y - width / 2, myVector3D.Z - width / 2)); //9
            myPositionCollection.Add(new Point3D(myVector3D.X + length/2, myVector3D.Y + width / 2, myVector3D.Z - width / 2)); //10

            myMeshGeometry3D.Positions = myPositionCollection;

            Int32Collection myTriangleIndicesCollection = new Int32Collection();
            //back pyramid
            myTriangleIndicesCollection.Add(1);
            myTriangleIndicesCollection.Add(2);
            myTriangleIndicesCollection.Add(0);

            myTriangleIndicesCollection.Add(2);
            myTriangleIndicesCollection.Add(3);
            myTriangleIndicesCollection.Add(0);

            myTriangleIndicesCollection.Add(3);
            myTriangleIndicesCollection.Add(4);
            myTriangleIndicesCollection.Add(0);

            myTriangleIndicesCollection.Add(4);
            myTriangleIndicesCollection.Add(1);
            myTriangleIndicesCollection.Add(0);

            myTriangleIndicesCollection.Add(2);
            myTriangleIndicesCollection.Add(1);
            myTriangleIndicesCollection.Add(5);

            myTriangleIndicesCollection.Add(3);
            myTriangleIndicesCollection.Add(2);
            myTriangleIndicesCollection.Add(5);

            myTriangleIndicesCollection.Add(4);
            myTriangleIndicesCollection.Add(3);
            myTriangleIndicesCollection.Add(5);

            myTriangleIndicesCollection.Add(1);
            myTriangleIndicesCollection.Add(4);
            myTriangleIndicesCollection.Add(5);

            //Front pyramid
            myTriangleIndicesCollection.Add(7);
            myTriangleIndicesCollection.Add(8);
            myTriangleIndicesCollection.Add(6);

            myTriangleIndicesCollection.Add(8);
            myTriangleIndicesCollection.Add(9);
            myTriangleIndicesCollection.Add(6);

            myTriangleIndicesCollection.Add(9);
            myTriangleIndicesCollection.Add(10);
            myTriangleIndicesCollection.Add(6);

            myTriangleIndicesCollection.Add(10);
            myTriangleIndicesCollection.Add(7);
            myTriangleIndicesCollection.Add(6);

            myTriangleIndicesCollection.Add(8);
            myTriangleIndicesCollection.Add(7);
            myTriangleIndicesCollection.Add(0);

            myTriangleIndicesCollection.Add(9);
            myTriangleIndicesCollection.Add(8);
            myTriangleIndicesCollection.Add(0);

            myTriangleIndicesCollection.Add(10);
            myTriangleIndicesCollection.Add(9);
            myTriangleIndicesCollection.Add(0);

            myTriangleIndicesCollection.Add(7);
            myTriangleIndicesCollection.Add(10);
            myTriangleIndicesCollection.Add(0);

            myMeshGeometry3D.TriangleIndices = myTriangleIndicesCollection;

            // Apply the mesh to the geometry model.
            myGeometryModel.Geometry = myMeshGeometry3D;

            // The material specifies the material applied to the 3D object. In this sample a  
            // linear gradient covers the surface of the 3D object.

            // Create a horizontal linear gradient with four stops.   
            SolidColorBrush myColorBrush = new SolidColorBrush();
            myColorBrush.Color = color;
            myColorBrush.Opacity = 1;

            // Define material and apply to the mesh geometries.
            DiffuseMaterial myMaterial = new DiffuseMaterial(myColorBrush);
            myGeometryModel.Material = myMaterial;

            //Apply the rotation
            Transform3DGroup myTransfGroup = new Transform3DGroup();
            myTransfGroup.Children.Add(applyRotation(dirVec));
            myTransfGroup.Children.Add(applyTranslation(iniPoint));

            myGeometryModel.Transform = myTransfGroup;

            // Add the geometry model to the model group.
            myModel3DGroup.Children.Add(myGeometryModel);
            return myModel3DGroup;
        }
        
        /// <summary>
        /// Creates a 3D cube
        /// </summary>
        /// <param name="thickness">The thickness (width or height) of the cube</param>
        /// <param name="point">The point in space where the cube will be drawn</param>
        /// <param name="color">The color of the cube</param>
        /// <returns></returns>
        public static Model3DGroup Draw3DCube(double thickness, Point3D point, Color color)
        {
            // Declare scene objects.
            Model3DGroup myModel3DGroup = new Model3DGroup();
            GeometryModel3D myGeometryModel = new GeometryModel3D();

            // The geometry specifes the shape of the 3D plane. In this sample, a flat sheet 
            // is created.
            MeshGeometry3D myMeshGeometry3D = new MeshGeometry3D();

            // Create a collection of vertex positions for the MeshGeometry3D. 
            Point3DCollection myPositionCollection = new Point3DCollection();
            Vector3D myVector3D = new Vector3D(point.X, point.Y, point.Z); //initial position of the Line

            //This block creates a rectangle in the XY plane facing positive Z
            myPositionCollection.Add(new Point3D(myVector3D.X - thickness / 2, myVector3D.Y - thickness / 2, myVector3D.Z + thickness / 2)); //0
            myPositionCollection.Add(new Point3D(myVector3D.X + thickness / 2, myVector3D.Y - thickness / 2, myVector3D.Z + thickness / 2)); //1
            myPositionCollection.Add(new Point3D(myVector3D.X + thickness / 2, myVector3D.Y + thickness / 2, myVector3D.Z + thickness / 2)); //2
            myPositionCollection.Add(new Point3D(myVector3D.X - thickness / 2, myVector3D.Y + thickness / 2, myVector3D.Z + thickness / 2)); //3

            //This block creates a rectangle in the XZ plane facing positive Y
            myPositionCollection.Add(new Point3D(myVector3D.X + thickness / 2, myVector3D.Y + thickness / 2, myVector3D.Z - thickness / 2)); //4
            myPositionCollection.Add(new Point3D(myVector3D.X - thickness / 2, myVector3D.Y + thickness / 2, myVector3D.Z - thickness / 2)); //5

            //This block creates a rectangle in the XZ plane facing negative Y
            myPositionCollection.Add(new Point3D(myVector3D.X + thickness / 2, myVector3D.Y - thickness / 2, myVector3D.Z - thickness / 2)); //6
            myPositionCollection.Add(new Point3D(myVector3D.X - thickness / 2, myVector3D.Y - thickness / 2, myVector3D.Z - thickness / 2)); //7

            myMeshGeometry3D.Positions = myPositionCollection;

            // Create a collection of triangle indices for the MeshGeometry3D.
            Int32Collection myTriangleIndicesCollection = new Int32Collection();
            myTriangleIndicesCollection.Add(0);
            myTriangleIndicesCollection.Add(1);
            myTriangleIndicesCollection.Add(3);
            myTriangleIndicesCollection.Add(3);
            myTriangleIndicesCollection.Add(1);
            myTriangleIndicesCollection.Add(2);

            myTriangleIndicesCollection.Add(3);
            myTriangleIndicesCollection.Add(2);
            myTriangleIndicesCollection.Add(5);
            myTriangleIndicesCollection.Add(5);
            myTriangleIndicesCollection.Add(2);
            myTriangleIndicesCollection.Add(4);

            myTriangleIndicesCollection.Add(1);
            myTriangleIndicesCollection.Add(0);
            myTriangleIndicesCollection.Add(7);
            myTriangleIndicesCollection.Add(7);
            myTriangleIndicesCollection.Add(6);
            myTriangleIndicesCollection.Add(1);

            myTriangleIndicesCollection.Add(6);
            myTriangleIndicesCollection.Add(7);
            myTriangleIndicesCollection.Add(5);
            myTriangleIndicesCollection.Add(5);
            myTriangleIndicesCollection.Add(4);
            myTriangleIndicesCollection.Add(6);

            myTriangleIndicesCollection.Add(3);
            myTriangleIndicesCollection.Add(5);
            myTriangleIndicesCollection.Add(0);
            myTriangleIndicesCollection.Add(0);
            myTriangleIndicesCollection.Add(5);
            myTriangleIndicesCollection.Add(7);

            myTriangleIndicesCollection.Add(4);
            myTriangleIndicesCollection.Add(2);
            myTriangleIndicesCollection.Add(1);
            myTriangleIndicesCollection.Add(1);
            myTriangleIndicesCollection.Add(6);
            myTriangleIndicesCollection.Add(4);

            myMeshGeometry3D.TriangleIndices = myTriangleIndicesCollection;

            // Apply the mesh to the geometry model.
            myGeometryModel.Geometry = myMeshGeometry3D;

            // The material specifies the material applied to the 3D object. In this sample a  
            // linear gradient covers the surface of the 3D object.

            // Create a horizontal linear gradient with four stops.   
            SolidColorBrush myColorBrush = new SolidColorBrush();
            myColorBrush.Color = color;
            myColorBrush.Opacity = 1;

            // Define material and apply to the mesh geometries.
            DiffuseMaterial myMaterial = new DiffuseMaterial(myColorBrush);
            myGeometryModel.Material = myMaterial;

            //Apply translation
            //Transform3DGroup myTransfGroup = new Transform3DGroup();
            //myTransfGroup.Children.Add(applyTranslation(point));

            //myGeometryModel.Transform = myTransfGroup;
            //myGeometryModel.Freeze();

            // Add the geometry model to the model group.
            myModel3DGroup.Children.Add(myGeometryModel);

            return myModel3DGroup;

        }

        /// <summary>
        /// Draw the Shell Element Panel with 8 nodes using the data from the specified element and the color
        /// </summary>
        /// <param name="ele">The element that will be drawn</param>
        /// <param name="color">The color of the element</param>
        /// <param name="deformed">True if the element is to be drawn in its deformed shape or false if it is to be drawn in its original shape</param>
        /// <returns></returns>
        public static Model3DGroup Draw3DPanel8N(ShellElement ele, Color color, bool deformed)
        {
            // Get data from element
            Node[] _nodeList;
            Node[] _dispList = null;
            if (deformed) //if defomed shape use the list of deformed nodes
            {
                ele.FindDisplacementsForElement(MainWindow.dispList);
                _dispList = ele.Displacements;
            }

            _nodeList = ele.nodeList;
            Node iniNode = _nodeList[0];
            Vector3D iniVec = new Vector3D(iniNode.Point.X, iniNode.Point.Y, iniNode.Point.Z);
            double eleWidth = ele.width;
            double eleHeight = ele.height;
            double eleThick = ele.thickness;
            Vector3D widthDir = ele.widthDirection;
            Vector3D heightDir = ele.heightDirection;
            Vector3D eleV3 = ele.getV3; //assuming that all node's V3 are the same (i.e., elements aew flat)

            // Define important parameters
            int numPoints = 3;
            double pointWidth = eleWidth / numPoints;
            double pointHeight = eleHeight / numPoints;
            Node[] natCoord = new Node[numPoints + 1]; //eta, neta, c
            int count = 0;

            // Declare scene objects.
            Model3DGroup myModel3DGroup = new Model3DGroup();
            GeometryModel3D myGeometryModel = new GeometryModel3D();

            // The geometry specifes the shape of the 3D plane.
            MeshGeometry3D myMeshGeometry3D = new MeshGeometry3D();

            // Create a collection of vertex positions for the MeshGeometry3D. 
            Point3DCollection myPositionCollection = new Point3DCollection();
            Vector3D myVector3D = new Vector3D(0, 0, 0); //initial position of the Line

            //This block creates a rectangle in the XY plane facing POSITIVE Z
            if (deformed) //if I want to show the deformed shaped
            {
                for (int i = 0; i < (numPoints + 1); i++)
                { // i is horizontal direction. The "numPoints + 1" is so that the last point is again one of the original points of the element
                    for (int j = 0; j < (numPoints + 1); j++) // j is vertical direction
                    {
                        double horAmount = i * pointWidth;
                        double verAmount = j * pointHeight;
                        double eta = -1 + horAmount * 2 / eleWidth;
                        double neta = -1 + verAmount * 2 / eleHeight;
                        double c = 0;
                        //natCoord[count] = new Node(count, new Point3D(-1 + horAmount * 2 / eleWidth, -1 + verAmount * 2 / eleHeight, 0));
                        double x = 0, y = 0, z = 0;
                        for (int k = 0; k < 8; k++)
                        {
                            double shapeResult = ShellElement.ShapeFunctionN(k + 1, eta, neta);
                            x += shapeResult * (_nodeList[k].Point.X + _dispList[k].Point.X) + shapeResult * (eleThick / 2) * c * eleV3.X;
                            y += shapeResult * (_nodeList[k].Point.Y + _dispList[k].Point.Y) + shapeResult * (eleThick / 2) * c * eleV3.Y;
                            z += shapeResult * (_nodeList[k].Point.Z + _dispList[k].Point.Z) + shapeResult * (eleThick / 2) * c * eleV3.Z;
                        }
                        myPositionCollection.Add(new Point3D(x, y, z));
                        //the next lines calculates the points needed for the shape functions

                        count++;
                    }
                }
            }
            else //if I just want to show the undeformed shape
            {
                for (int i = 0; i < (numPoints + 1); i++)
                { // i is horizontal direction. The "numPoints + 1" is so that the last point is again one of the original points of the element
                    for (int j = 0; j < (numPoints + 1); j++) // j is vertical direction
                    {
                        double horAmount = i * pointWidth;
                        double verAmount = j * pointHeight;
                        //this only works for underformed, for deformed we need to calcualte the position based on the shape functions
                        Vector3D vec = iniVec + verAmount * heightDir + horAmount * widthDir;
                        myPositionCollection.Add(new Point3D(vec.X, vec.Y, vec.Z));
                    }
                }
            }
            

            myMeshGeometry3D.Positions = myPositionCollection;

            // Create a collection of triangle indices for the MeshGeometry3D.
            Int32Collection myTriangleIndicesCollection = new Int32Collection();
            for (int i = 0; i < numPoints; i++)
            {
                for (int j = 0; j < numPoints; j++)
                {
                    //first half of a rectangle
                    myTriangleIndicesCollection.Add(j + i * (numPoints + 1));
                    myTriangleIndicesCollection.Add(numPoints + 1 + j + i * (numPoints + 1));
                    myTriangleIndicesCollection.Add(numPoints + 2 + j + i * (numPoints + 1));

                    //second half of a rectangle
                    myTriangleIndicesCollection.Add(j + i * (numPoints + 1));
                    myTriangleIndicesCollection.Add(numPoints + 2 + j + i * (numPoints + 1));
                    myTriangleIndicesCollection.Add(1 + j + i * (numPoints + 1));
                }
            }

            myMeshGeometry3D.TriangleIndices = myTriangleIndicesCollection;

            // Apply the mesh to the geometry model.
            myGeometryModel.Geometry = myMeshGeometry3D;

            // Apply the color to the object with opacity 100%
            SolidColorBrush myColorBrush = new SolidColorBrush();
            myColorBrush.Color = color;
            myColorBrush.Opacity = 1;

            // Define material and apply to the mesh geometries.
            DiffuseMaterial myMaterial = new DiffuseMaterial(myColorBrush);
            myGeometryModel.Material = myMaterial;
            myGeometryModel.BackMaterial = myMaterial;

            //Apply translation
            Transform3DGroup myTransfGroup = new Transform3DGroup();

            //myTransfGroup.Children.Add(applyRotation(ele.widthDirection));
            //myTransfGroup.Children.Add(applyTranslation(iniNode.Point));

            myGeometryModel.Transform = myTransfGroup;
            myGeometryModel.Freeze();
            // Add the geometry model to the model group.
            myModel3DGroup.Children.Add(myGeometryModel);
            return myModel3DGroup;

        }

        /// <summary>
        /// Draw the Shell Element Panel with 9 nodes using the data from the specified element and the color
        /// </summary>
        /// <param name="ele">The element that will be drawn</param>
        /// <param name="color">The color of the element</param>
        /// <param name="deformed">True if the element is to be drawn in its deformed shape or false if it is to be drawn in its original shape</param>
        /// <returns></returns>
        public static Model3DGroup Draw3DSprings(Spring3D ele, Color color, bool deformed, List<Node> dispList)
        {
            // Get data from element
            //Node[] _nodeList;
            Node[] _dispList = null;
            if (deformed) //if deformed shape use the list of deformed nodes
            {
                ele.FindDisplacementsForElement(dispList);
                _dispList = ele._dispList;
            }
 
            Vector3D vec = new Vector3D(ele.N2.Point.X + _dispList[1].Point.X - (ele.N1.Point.X + _dispList[0].Point.X), ele.N2.Point.Y + _dispList[1].Point.Y - (ele.N1.Point.Y + _dispList[0].Point.Y), ele.N2.Point.Z + _dispList[1].Point.Z - (ele.N1.Point.Z + _dispList[0].Point.Z));
            Point3D newPoint = new Point3D(ele.N1.Point.X + _dispList[0].Point.X, ele.N1.Point.Y + _dispList[0].Point.Y, ele.N1.Point.Z + _dispList[0].Point.Z);
            return Draw3DLine(vec.Length, 5, newPoint, vec, color);
        }

        /// <summary>
        /// Creates the 3D Panel with 9 nodes
        /// </summary>
        /// <param name="ele">The ShellElement object</param>
        /// <param name="color">The color of the shell element</param>
        /// <param name="deformed">True = draws the element in the deformed shape; False = draws the element in the undeformed shape</param>
        /// <param name="dispList">The displacement list to be used if deformed = true. Can be set to 'null' if deformed = false</param>
        /// <returns></returns>
        public static Model3DGroup Draw3DPanel(ShellElement ele, Color color, bool deformed, List<Node> dispList, double zoomParam)
        {
            // Get data from element
            Node[] _nodeList;
            Node[] _dispList = null;
            if (deformed) //if defomed shape use the list of deformed nodes
            {
                ele.FindDisplacementsForElement(dispList);
                _dispList = ele.Displacements;
            }

            _nodeList = ele.nodeList;
            Node iniNode = _nodeList[0];
            Vector3D iniVec = new Vector3D(iniNode.Point.X, iniNode.Point.Y, iniNode.Point.Z);
            double eleWidth = ele.width;
            double eleHeight = ele.height;
            double eleThick = ele.thickness;
            Vector3D widthDir = ele.widthDirection;
            Vector3D heightDir = ele.heightDirection;
            Vector3D eleV3 = ele.getV3; //assuming that all node's V3 are the same (i.e., elements aew flat)

            // Define important parameters
            int numPoints = 3;
            double pointWidth = eleWidth / numPoints;
            double pointHeight = eleHeight / numPoints;
            Node[] natCoord = new Node[numPoints + 1]; //eta, neta, c
            int count = 0;

            // Declare scene objects.
            Model3DGroup myModel3DGroup = new Model3DGroup();
            GeometryModel3D myGeometryModel = new GeometryModel3D();

            // The geometry specifes the shape of the 3D plane.
            MeshGeometry3D myMeshGeometry3D = new MeshGeometry3D();

            // Create a collection of vertex positions for the MeshGeometry3D. 
            Point3DCollection myPositionCollection = new Point3DCollection();

            // Create a collection of triangle indices for the MeshGeometry3D.
            Int32Collection myTriangleIndicesCollection = new Int32Collection();


            List<Point3D> listOfPointsX = new List<Point3D>();
            List<Point3D> listOfPointsY = new List<Point3D>();
            //This block creates a rectangle in the XY plane facing POSITIVE Z
            if (deformed) //if I want to show the deformed shaped
            {
                for (int i = 0; i < (numPoints + 1); i++)
                { // i is horizontal direction. The "numPoints + 1" is so that the last point is again one of the original points of the element
                    for (int j = 0; j < (numPoints + 1); j++) // j is vertical direction
                    {
                        double horAmount = i * pointWidth;
                        double verAmount = j * pointHeight;
                        double eta = -1 + horAmount * 2 / eleWidth;
                        double neta = -1 + verAmount * 2 / eleHeight;
                        double c = 0;
                        //natCoord[count] = new Node(count, new Point3D(-1 + horAmount * 2 / eleWidth, -1 + verAmount * 2 / eleHeight, 0));
                        double x = 0, y = 0, z = 0;
                        for (int k = 0; k < 8; k++) //for each node
                        {
                            //the next lines calculates the points needed for the shape functions
                            double shapeResult = ShellElement.ShapeFunctionN(k + 1, eta, neta);
                            x += shapeResult * (_nodeList[k].Point.X + _dispList[k].Point.X) + shapeResult * (eleThick / 2) * c * eleV3.X;
                            y += shapeResult * (_nodeList[k].Point.Y + _dispList[k].Point.Y) + shapeResult * (eleThick / 2) * c * eleV3.Y;
                            z += shapeResult * (_nodeList[k].Point.Z + _dispList[k].Point.Z) + shapeResult * (eleThick / 2) * c * eleV3.Z;
                            //x += shapeResult * (_dispList[k].Point.X) + shapeResult * (eleThick / 2) * c * eleV3.X;
                            //y += shapeResult * (_dispList[k].Point.Y) + shapeResult * (eleThick / 2) * c * eleV3.Y;
                            //z += shapeResult * (_dispList[k].Point.Z) + shapeResult * (eleThick / 2) * c * eleV3.Z;
                        }
                        myPositionCollection.Add(new Point3D(x, y, z));
                        if (i % numPoints == 0)
                        {
                            listOfPointsY.Add(new Point3D(x, y, z));
                        }
                        if (j % numPoints == 0)
                        {
                            listOfPointsX.Add(new Point3D(x, y, z));
                        }
                        count++;
                    }
                }

                double thick = 1.5;
                int incr1 = 0, incr2 = 0;
                for (int i = 0; i < numPoints; i++)  //i is horizontal
                {
                    Vector3D dir = Point3D.Subtract(listOfPointsX[i + 2 + incr1], listOfPointsX[i + incr1]);
                    myModel3DGroup.Children.Add(Draw3DLine(dir.Length, thick* zoomParam, listOfPointsX[i + incr1], dir, Colors.Black));
                    incr1 += 1;
                }
                for (int i = 0; i < numPoints; i++)
                {
                    Vector3D dir = Point3D.Subtract(listOfPointsX[i + 3 + incr2], listOfPointsX[i + 1 + incr2]);
                    myModel3DGroup.Children.Add(Draw3DLine(dir.Length, thick * zoomParam, listOfPointsX[i + 1 + incr2], dir, Colors.Black));
                    incr2 += 1;
                }

                for (int j = 0; j < numPoints; j++)  //j is vertical
                {
                    Vector3D dir = Point3D.Subtract(listOfPointsY[j + 1], listOfPointsY[j]);
                    myModel3DGroup.Children.Add(Draw3DLine(dir.Length, thick * zoomParam, listOfPointsY[j], dir, Colors.Black));
                }
                for (int j = 0; j < numPoints; j++)
                {
                    Vector3D dir = Point3D.Subtract(listOfPointsY[j + numPoints + 2], listOfPointsY[j + numPoints + 1]);
                    myModel3DGroup.Children.Add(Draw3DLine(dir.Length, thick * zoomParam, listOfPointsY[j + numPoints + 1], dir, Colors.Black));
                }

                myMeshGeometry3D.Positions = myPositionCollection;

                for (int i = 0; i < numPoints; i++)
                {
                    for (int j = 0; j < numPoints; j++)
                    {
                        //first half of a rectangle
                        myTriangleIndicesCollection.Add(j + i * (numPoints + 1));
                        myTriangleIndicesCollection.Add(numPoints + 1 + j + i * (numPoints + 1));
                        myTriangleIndicesCollection.Add(numPoints + 2 + j + i * (numPoints + 1));

                        //second half of a rectangle
                        myTriangleIndicesCollection.Add(j + i * (numPoints + 1));
                        myTriangleIndicesCollection.Add(numPoints + 2 + j + i * (numPoints + 1));
                        myTriangleIndicesCollection.Add(1 + j + i * (numPoints + 1));
                    }
                }

                myMeshGeometry3D.TriangleIndices = myTriangleIndicesCollection;

            }
            else //if I just want to show the undeformed shape
            {
                for (int i = 0; i < 2; i++)
                { // i is horizontal direction. The "numPoints + 1" is so that the last point is again one of the original points of the element
                    for (int j = 0; j < 2; j++) // j is vertical direction
                    {
                        double horAmount = i * eleWidth;
                        double verAmount = j * eleHeight;
                        //this only works for underformed, for deformed we need to calcualte the position based on the shape functions
                        Vector3D vec = iniVec + verAmount * heightDir + horAmount * widthDir;
                        myPositionCollection.Add(new Point3D(vec.X, vec.Y, vec.Z));
                    }
                }

                myMeshGeometry3D.Positions = myPositionCollection;

                //first half of a rectangle
                myTriangleIndicesCollection.Add(0);
                myTriangleIndicesCollection.Add(2);
                myTriangleIndicesCollection.Add(1);

                //second half of a rectangle
                myTriangleIndicesCollection.Add(2);
                myTriangleIndicesCollection.Add(3);
                myTriangleIndicesCollection.Add(1);

                myMeshGeometry3D.TriangleIndices = myTriangleIndicesCollection;

                double thick = 1.5;
                myModel3DGroup.Children.Add(Draw3DLine(eleWidth, thick * zoomParam, new Point3D(iniVec.X, iniVec.Y, iniVec.Z), new Vector3D(_nodeList[1].Point.X - _nodeList[0].Point.X, _nodeList[1].Point.Y - _nodeList[0].Point.Y, _nodeList[1].Point.Z - _nodeList[0].Point.Z), Colors.Black));
                myModel3DGroup.Children.Add(Draw3DLine(eleHeight, thick * zoomParam, _nodeList[1].Point, new Vector3D(_nodeList[2].Point.X - _nodeList[1].Point.X, _nodeList[2].Point.Y - _nodeList[1].Point.Y, _nodeList[2].Point.Z - _nodeList[1].Point.Z), Colors.Black));
                myModel3DGroup.Children.Add(Draw3DLine(eleWidth, thick * zoomParam, _nodeList[2].Point, new Vector3D(_nodeList[3].Point.X - _nodeList[2].Point.X, _nodeList[3].Point.Y - _nodeList[2].Point.Y, _nodeList[3].Point.Z - _nodeList[2].Point.Z), Colors.Black));
                myModel3DGroup.Children.Add(Draw3DLine(eleHeight, thick * zoomParam, _nodeList[3].Point, new Vector3D(_nodeList[0].Point.X - _nodeList[3].Point.X, _nodeList[0].Point.Y - _nodeList[3].Point.Y, _nodeList[0].Point.Z - _nodeList[3].Point.Z), Colors.Black));
            }

            // Apply the mesh to the geometry model.
            myGeometryModel.Geometry = myMeshGeometry3D;

            // Apply the color to the object with opacity 100%
            SolidColorBrush myColorBrush = new SolidColorBrush();
            myColorBrush.Color = color;
            myColorBrush.Opacity = 1;

            // Define material and apply to the mesh geometries.
            DiffuseMaterial myMaterial = new DiffuseMaterial(myColorBrush);
            myGeometryModel.Material = myMaterial;
            myGeometryModel.BackMaterial = myMaterial;

            //Apply translation
            Transform3DGroup myTransfGroup = new Transform3DGroup();

            //myTransfGroup.Children.Add(applyRotation(ele.widthDirection));
            //myTransfGroup.Children.Add(applyTranslation(iniNode.Point));

            myGeometryModel.Transform = myTransfGroup;
            //myGeometryModel.Freeze();
            // Add the geometry model to the model group.
            myModel3DGroup.Children.Add(myGeometryModel);


            return myModel3DGroup;

        }

        /// <summary>
        /// Rotate Shell Element Panels based on their width and height directions
        /// </summary>
        /// <param name="axis">The axis to apply the rotation</param>
        /// <param name="angle">the angles, IN DEGREES, to rotate</param>
        /// <returns></returns>
        public static Transform3DGroup RotatePanels(char axis, double angle)
        {
            // Apply a transform to the object. In this sample, a rotation transform is applied,  
            // rendering the 3D object rotated.

            Transform3DGroup myTransfGroup = new Transform3DGroup();

            RotateTransform3D myRotateTransform3D = new RotateTransform3D();
            AxisAngleRotation3D myAxisAngleRotation3d = new AxisAngleRotation3D();
            //Vector vec = new Vector(new myPoint(dirVec.X, dirVec.Y, dirVec.Z));
            if (axis == 'x')
            {
                myAxisAngleRotation3d.Axis = new Vector3D(1, 0, 0); //rotating over the x
            } else if (axis == 'y')
            {
                myAxisAngleRotation3d.Axis = new Vector3D(0, 1, 0); //rotating over the y
            } else //axis == 'z'
            {
                myAxisAngleRotation3d.Axis = new Vector3D(0, 0, 1); //rotating over the z
            }

            myAxisAngleRotation3d.Angle = angle;
            myRotateTransform3D.Rotation = myAxisAngleRotation3d;
            myTransfGroup.Children.Add(myRotateTransform3D);

            return myTransfGroup;
        }

        /// <summary>
        /// Apply rotation to a 3D object
        /// </summary>
        /// <param name="dirVec">The vector 'axis' to apply the rotation</param>
        /// <returns></returns>
        public static Transform3DGroup applyRotation(Vector3D dirVec)
        {
            // Apply a transform to the object. In this sample, a rotation transform is applied,  
            // rendering the 3D object rotated.

            Transform3DGroup myTransfGroup = new Transform3DGroup();

            RotateTransform3D myRotateTransform3DY = new RotateTransform3D();
            AxisAngleRotation3D myAxisAngleRotation3dY = new AxisAngleRotation3D();
            RotateTransform3D myRotateTransform3DZ = new RotateTransform3D();
            AxisAngleRotation3D myAxisAngleRotation3dZ = new AxisAngleRotation3D();
            CLTFEM.Classes.Mathematics.Vector vec = new CLTFEM.Classes.Mathematics.Vector(new myPoint(dirVec.X, dirVec.Y, dirVec.Z));
            myAxisAngleRotation3dY.Axis = new Vector3D(0, 0, 1); //rotating over the z axis will result in a rotation in the x-y plane
            double angX;
            double angY;
            double angRotZ;
            double cos;
            if (Math.Acos(vec.CosZ) == 0 | Math.Acos(vec.CosZ) % Math.PI / 2 == 0)
            {
                angY = 1;
                cos = 0;
            }
            else
            {
                cos = Math.Acos(vec.CosXYx);
                angY = dirVec.X;
            }

            if (Math.Acos(vec.CosZ) <= Math.PI / 2)
            {
                angRotZ = -(Math.PI / 2 - Math.Acos(vec.CosZ));
                angX = -dirVec.Y;
            }
            else
            {
                angRotZ = -(Math.Acos(vec.CosZ) - Math.PI / 2);
                angX = dirVec.Y;
                angY = -angY;
            }

            if (Math.Acos(vec.CosY) > Math.PI / 2)
            {
                cos = Math.PI + (Math.PI - cos);
            }

            myAxisAngleRotation3dY.Angle = cos * 180 / Math.PI;
            myRotateTransform3DY.Rotation = myAxisAngleRotation3dY;
            myTransfGroup.Children.Add(myRotateTransform3DY);

            myAxisAngleRotation3dZ.Axis = new Vector3D(angX, angY, 0);
            myAxisAngleRotation3dZ.Angle = angRotZ * 180 / Math.PI;
            myRotateTransform3DZ.Rotation = myAxisAngleRotation3dZ;
            myTransfGroup.Children.Add(myRotateTransform3DZ);

            return myTransfGroup;
        }

        /// <summary>
        /// Apply translation to a 3D object
        /// </summary>
        /// <param name="iniPoint">The point comprised of the offsets to which the original location will be translated</param>
        /// <returns></returns>
        public static Transform3DGroup applyTranslation(Point3D iniPoint)
        {
            Transform3DGroup myTransfGroup = new Transform3DGroup();
            TranslateTransform3D myTranslateTransform3D = new TranslateTransform3D();
            myTranslateTransform3D.OffsetX = iniPoint.X;
            myTranslateTransform3D.OffsetY = iniPoint.Y;
            myTranslateTransform3D.OffsetZ = iniPoint.Z;
            myTransfGroup.Children.Add(myTranslateTransform3D);

            return myTransfGroup;
        }

        /// <summary>
        /// Creates a ModelVisual3D containing a text label.
        /// </summary>
        /// <param name="text">The string</param>
        /// <param name="textColor">The color of the text.</param>
        /// <param name="bDoubleSided">Visible from both sides?</param>
        /// <param name="height">Height of the characters</param>
        /// <param name="center">The center of the label</param>
        /// <param name="over">Horizontal direction of the label</param>
        /// <param name="up">Vertical direction of the label</param>
        /// <returns>Suitable for adding to your Viewport3D</returns>
        public static ModelVisual3D CreateTextLabel3D(string text, Brush textColor, bool bDoubleSided, double height, Point3D center, Vector3D over, Vector3D up)
        {
            // First we need a textblock containing the text of our label
            TextBlock tb = new TextBlock(new Run(text));
            tb.Foreground = textColor;
            tb.FontFamily = new FontFamily("Arial");

            // Now use that TextBlock as the brush for a material
            DiffuseMaterial mat = new DiffuseMaterial();
            mat.Brush = new VisualBrush(tb);

            // We just assume the characters are square
            double width = text.Length * height;

            // Since the parameter coming in was the center of the label,
            // we need to find the four corners
            // p0 is the lower left corner
            // p1 is the upper left
            // p2 is the lower right
            // p3 is the upper right
            Point3D p0 = center - width / 2 * over - height / 2 * up;
            Point3D p1 = p0 + up * 1 * height;
            Point3D p2 = p0 + over * width;
            Point3D p3 = p0 + up * 1 * height + over * width;

            // Now build the geometry for the sign.  It's just a
            // rectangle made of two triangles, on each side.

            MeshGeometry3D mg = new MeshGeometry3D();
            mg.Positions = new Point3DCollection();
            mg.Positions.Add(p0);    // 0
            mg.Positions.Add(p1);    // 1
            mg.Positions.Add(p2);    // 2
            mg.Positions.Add(p3);    // 3

            if (bDoubleSided)
            {
                mg.Positions.Add(p0);    // 4
                mg.Positions.Add(p1);    // 5
                mg.Positions.Add(p2);    // 6
                mg.Positions.Add(p3);    // 7
            }

            mg.TriangleIndices.Add(0);
            mg.TriangleIndices.Add(3);
            mg.TriangleIndices.Add(1);
            mg.TriangleIndices.Add(0);
            mg.TriangleIndices.Add(2);
            mg.TriangleIndices.Add(3);

            if (bDoubleSided)
            {
                mg.TriangleIndices.Add(4);
                mg.TriangleIndices.Add(5);
                mg.TriangleIndices.Add(7);
                mg.TriangleIndices.Add(4);
                mg.TriangleIndices.Add(7);
                mg.TriangleIndices.Add(6);
            }

            // These texture coordinates basically stretch the
            // TextBox brush to cover the full side of the label.

            
            mg.TextureCoordinates.Add(new System.Windows.Point(0, 1));
            mg.TextureCoordinates.Add(new System.Windows.Point(0, 0));
            mg.TextureCoordinates.Add(new System.Windows.Point(1, 1));
            mg.TextureCoordinates.Add(new System.Windows.Point(1, 0));

            if (bDoubleSided)
            {
                mg.TextureCoordinates.Add(new System.Windows.Point(1, 1));
                mg.TextureCoordinates.Add(new System.Windows.Point(1, 0));
                mg.TextureCoordinates.Add(new System.Windows.Point(0, 1));
                mg.TextureCoordinates.Add(new System.Windows.Point(0, 0));
            }
            
            // And that's all.  Return the result.

            ModelVisual3D mv3d = new ModelVisual3D();
            mv3d.Content = new GeometryModel3D(mg, mat); ;
            return mv3d;
        }

        /// <summary>
        /// Calculate the length between two values 
        /// </summary>
        /// <param name="a">First value</param>
        /// <param name="b">Second value</param>
        /// <returns></returns>
        public static double Length(double a, double b)
        {
            return Math.Sqrt(Math.Pow(a, 2) + Math.Pow(b, 2));
        }

        /// <summary>
        /// Clears all the 3D objects currently being draw, except for the axes
        /// </summary>
        /// <param name="myModel3DGroup">The Model3DGroup that holds all the objects in view</param>
        public static void ClearScreen(ref Model3DGroup myModel3DGroup)
        {
            int count = myModel3DGroup.Children.Count;
            int i = 4; //starts with 4 to a
            while (i < count)
            {
                myModel3DGroup.Children.RemoveAt(i); //index 0 is the light, index 1 - 3 are the axes.
                count--;
            }
        }

        /// <summary>
        /// Draws all the boundary conditions at once
        /// </summary>
        /// <param name="supList">The list of supports</param>
        /// <param name="deformed">True = draw the supports on the deformed shape; false = draw the supports on the undeformed shape</param>
        /// <param name="dispList">The displacement list if deformed = true. Can be set to 'null' if deformed = false</param>
        public static void DrawBoundaryConditions(List<Support> supList, bool deformed, List<Node> dispList, double zoomParam)
        {
            foreach (Support s in supList)
            {
                int count = s.GetSupportList.Count;
                int nodeID = s.NodeID;
                double x, y, z;
                if (deformed)
                {
                    x = MainWindow.nodeList[nodeID - 1].Point.X + dispList[nodeID - 1].Point.X;
                    y = MainWindow.nodeList[nodeID - 1].Point.Y + dispList[nodeID - 1].Point.Y;
                    z = MainWindow.nodeList[nodeID - 1].Point.Z + dispList[nodeID - 1].Point.Z;
                }
                else
                {
                    x = MainWindow.nodeList[nodeID - 1].Point.X;
                    y = MainWindow.nodeList[nodeID - 1].Point.Y;
                    z = MainWindow.nodeList[nodeID - 1].Point.Z;
                }

                for (int i = 0; i < count; i++)
                {
                    switch (s.GetSupportList[i].ID)
                    {
                        case 1:
                            if (s.GetSupportList[i].GetVal != 0)
                            {//if a displacement laod instead of a boundary
                                int sign;
                                if (s.GetSupportList[i].GetVal >= 0)
                                {
                                    sign = 1;
                                }
                                else
                                {
                                    sign = -1;
                                }
                                MainWindow.myModel3DGroup.Children.Add(Draw3DArrow(Configuration.loadArrowLength * zoomParam, Configuration.loadArrowThick * zoomParam, Configuration.loadArrowTipLength * zoomParam, Configuration.loadArrowTipWidth * zoomParam, new Point3D(x, y, z), new Vector3D(sign * 1, 0, 0), Configuration.loadArrowColor));
                            } else
                            {
                                MainWindow.myModel3DGroup.Children.Add(Draw3DTranslSupport(Configuration.translSupLength * zoomParam, Configuration.translSupWidth * zoomParam, new Point3D(x, y, z), new Vector3D(1, 0, 0), Configuration.translSupColor));
                            }
                            break;
                        case 2:
                            if (s.GetSupportList[i].GetVal != 0)
                            {//if a displacement load instead of a boundary
                                int sign;
                                if (s.GetSupportList[i].GetVal >= 0)
                                {
                                    sign = 1;
                                }
                                else
                                {
                                    sign = -1;
                                }
                                MainWindow.myModel3DGroup.Children.Add(Draw3DArrow(Configuration.loadArrowLength * zoomParam, Configuration.loadArrowThick * zoomParam, Configuration.loadArrowTipLength * zoomParam, Configuration.loadArrowTipWidth * zoomParam, new Point3D(x, y, z), new Vector3D(0, sign * 1, 0), Configuration.loadArrowColor));
                            }
                            else
                            {
                                MainWindow.myModel3DGroup.Children.Add(Draw3DTranslSupport(Configuration.translSupLength * zoomParam, Configuration.translSupWidth * zoomParam, new Point3D(x, y, z), new Vector3D(0, 1, 0), Configuration.translSupColor));
                            }
                            break;
                        case 3:
                            if (s.GetSupportList[i].GetVal != 0)
                            {//if a displacement laod instead of a boundary
                                int sign;
                                if (s.GetSupportList[i].GetVal >= 0)
                                {
                                    sign = 1;
                                }
                                else
                                {
                                    sign = -1;
                                }
                                MainWindow.myModel3DGroup.Children.Add(Draw3DArrow(Configuration.loadArrowLength * zoomParam, Configuration.loadArrowThick * zoomParam, Configuration.loadArrowTipLength * zoomParam, Configuration.loadArrowTipWidth * zoomParam, new Point3D(x, y, z), new Vector3D(0, 0, sign * 10), Configuration.loadArrowColor));
                            }
                            else
                            {
                                MainWindow.myModel3DGroup.Children.Add(Draw3DTranslSupport(Configuration.translSupLength * zoomParam, Configuration.translSupWidth * zoomParam, new Point3D(x, y, z), new Vector3D(0, 0, 1), Configuration.translSupColor));
                            }
                            break;
                        case 4:
                            MainWindow.myModel3DGroup.Children.Add(Draw3DRotSupport(Configuration.rotSupLength * zoomParam, Configuration.rotSupWidth * zoomParam, new Point3D(x, y, z), new Vector3D(1, 0, 0), Configuration.rotSupColor));
                            break;
                        case 5:
                            MainWindow.myModel3DGroup.Children.Add(Draw3DRotSupport(Configuration.rotSupLength * zoomParam, Configuration.rotSupWidth * zoomParam, new Point3D(x, y, z), new Vector3D(0, 1, 0), Configuration.rotSupColor));
                            break;
                        default:
                            MainWindow.myModel3DGroup.Children.Add(Draw3DRotSupport(Configuration.rotSupLength * zoomParam, Configuration.rotSupWidth * zoomParam, new Point3D(x, y, z), new Vector3D(0, 0, 1), Configuration.rotSupColor));
                            break;
                    }
                }
            }
        }

        /// <summary>
        /// Draws all the loads at once
        /// </summary>
        /// <param name="loadList">The list of loads</param>
        /// <param name="deformed">True = draw the loads on the deformed shape; false = draw the loads on the undeformed shape</param>
        /// <param name="dispList">The displacement list if deformed = true. Can be set to 'null' if deformed = false</param>
        public static void DrawLoads(List<Load> loadList, bool deformed, List<Node> dispList, double zoomParam)
        {
            foreach (Load l in loadList)
            {
                int count = l.GetLoadList.Count;
                int nodeID = l.NodeID;
                double x, y, z;
                if (deformed)
                {
                    x = MainWindow.nodeList[nodeID - 1].Point.X + dispList[nodeID - 1].Point.X;
                    y = MainWindow.nodeList[nodeID - 1].Point.Y + dispList[nodeID - 1].Point.Y;
                    z = MainWindow.nodeList[nodeID - 1].Point.Z + dispList[nodeID - 1].Point.Z;
                }
                else
                {
                    x = MainWindow.nodeList[nodeID - 1].Point.X;
                    y = MainWindow.nodeList[nodeID - 1].Point.Y;
                    z = MainWindow.nodeList[nodeID - 1].Point.Z;
                }

                for (int i = 0; i < count; i++)
                {
                    int sign;
                    
                    if (l.GetLoadList[i].GetVal >= 0)
                    {
                        sign = 1;
                    }
                    else
                    {
                        sign = -1;
                    }
                    switch (l.GetLoadList[i].ID)
                    {
                        case 1:
                            MainWindow.myModel3DGroup.Children.Add(DrawingHelper.Draw3DArrow(Configuration.loadArrowLength * zoomParam, Configuration.loadArrowThick * zoomParam, Configuration.loadArrowTipLength * zoomParam, Configuration.loadArrowTipWidth * zoomParam, new Point3D(x, y, z), new Vector3D(sign * 1, 0, 0), Configuration.loadArrowColor));
                            break;
                        case 2:
                            MainWindow.myModel3DGroup.Children.Add(DrawingHelper.Draw3DArrow(Configuration.loadArrowLength * zoomParam, Configuration.loadArrowThick * zoomParam, Configuration.loadArrowTipLength * zoomParam, Configuration.loadArrowTipWidth * zoomParam, new Point3D(x, y, z), new Vector3D(0, sign * 1, 0), Configuration.loadArrowColor));
                            break;
                        case 3:
                            MainWindow.myModel3DGroup.Children.Add(DrawingHelper.Draw3DArrow(Configuration.loadArrowLength * zoomParam, Configuration.loadArrowThick * zoomParam, Configuration.loadArrowTipLength * zoomParam, Configuration.loadArrowTipWidth * zoomParam, new Point3D(x, y, z), new Vector3D(0, 0, sign * 1), Configuration.loadArrowColor));
                            break;
                        case 4:
                            //Needs implementation
                            break;
                        case 5:
                            //Needs implementation
                            break;
                        default:
                            //Needs implementation
                            break;
                    }
                }
            }
        }

        /// <summary>
        /// Draws all the masses at once
        /// </summary>
        /// <param name="massList">The list of masses</param>
        /// <param name="deformed">True = draw the loads on the deformed shape; false = draw the loads on the undeformed shape</param>
        /// <param name="dispList">The displacement list if deformed = true. Can be set to 'null' if deformed = false</param>
        public static void DrawMasses(List<Mass> massList, bool deformed, List<Node> dispList, double zoomParam)
        {
            foreach (Mass m in massList)
            {
                int count = m.GetMassList.Count;
                int nodeID = m.NodeID;
                double x, y, z;
                if (deformed)
                {
                    x = MainWindow.nodeList[nodeID - 1].Point.X + dispList[nodeID - 1].Point.X;
                    y = MainWindow.nodeList[nodeID - 1].Point.Y + dispList[nodeID - 1].Point.Y;
                    z = MainWindow.nodeList[nodeID - 1].Point.Z + dispList[nodeID - 1].Point.Z;
                }
                else
                {
                    x = MainWindow.nodeList[nodeID - 1].Point.X;
                    y = MainWindow.nodeList[nodeID - 1].Point.Y;
                    z = MainWindow.nodeList[nodeID - 1].Point.Z;
                }

                for (int i = 0; i < count; i++)
                {
                    int sign;

                    if (m.GetMassList[i].GetVal >= 0)
                    {
                        sign = 1;
                    }
                    else
                    {
                        sign = -1;
                    }
                    switch (m.GetMassList[i].ID)
                    {
                        case 1:
                            MainWindow.myModel3DGroup.Children.Add(DrawingHelper.Draw3DArrow(Configuration.massArrowLength * zoomParam, Configuration.massArrowThick * zoomParam, Configuration.massArrowTipLength * zoomParam, Configuration.massArrowTipWidth * zoomParam, new Point3D(x, y, z), new Vector3D(sign * 1, 0, 0), Configuration.massArrowColor));
                            break;
                        case 2:
                            MainWindow.myModel3DGroup.Children.Add(DrawingHelper.Draw3DArrow(Configuration.massArrowLength * zoomParam, Configuration.massArrowThick * zoomParam, Configuration.massArrowTipLength * zoomParam, Configuration.massArrowTipWidth * zoomParam, new Point3D(x, y, z), new Vector3D(0, sign * 1, 0), Configuration.massArrowColor));
                            break;
                        case 3:
                            MainWindow.myModel3DGroup.Children.Add(DrawingHelper.Draw3DArrow(Configuration.massArrowLength * zoomParam, Configuration.massArrowThick * zoomParam, Configuration.massArrowTipLength * zoomParam, Configuration.massArrowTipWidth * zoomParam, new Point3D(x, y, z), new Vector3D(0, 0, sign * 1), Configuration.massArrowColor));
                            break;
                        default:
                            //Needs implementation
                            break;
                    }
                }
            }
        }

        /// <summary>
        /// Creates the global X Y Z Axes at the startup of the program
        /// </summary>
        /// <param name="myModel3DGroup">The Model3DGroup used to store the Axes 3D objects</param>
        public static void createAxes(ref Model3DGroup myModel3DGroup)
        {
            var origin = new Point3D(0, 0, 0);
            var axisX = new Vector3D(1, 0, 0);
            var axisY = new Vector3D(0, 1, 0);
            var axisZ = new Vector3D(0, 0, 1);

            myModel3DGroup.Children.Add(DrawingHelper.Draw3DLine(Configuration.axisLength, Configuration.axisWidth, origin, axisX, Configuration.xAxisColor));
            myModel3DGroup.Children.Add(DrawingHelper.Draw3DLine(Configuration.axisLength, Configuration.axisWidth, origin, axisY, Configuration.yAxisColor));
            myModel3DGroup.Children.Add(DrawingHelper.Draw3DLine(Configuration.axisLength, Configuration.axisWidth, origin, axisZ, Configuration.zAxisColor));
        }

        /// <summary>
        /// Function that draws all the structural elements at once. Used when Open a structure file.
        /// </summary>
        public static void DrawStructure(double zoomParam)
        {
            for (int i = 0; i < MainWindow.nodeList.Count; i++)
            {
                MainWindow.myModel3DGroup.Children.Add(DrawingHelper.Draw3DCube(Configuration.nodeSize * zoomParam, MainWindow.nodeList[i].Point, Configuration.nodeColor)); //draws the element on the viewModel3D in the mainWindow
            }

            for (int i = 0; i < MainWindow.shellList.Count; i++)
            {
                MainWindow.myModel3DGroup.Children.Add(DrawingHelper.Draw3DPanel(MainWindow.shellList[i], Configuration.shellEleColor, false, null, zoomParam));
            }

            DrawBoundaryConditions(new List<Support>(MainWindow.supportList), false, null, zoomParam);

            DrawLoads(new List<Load>(MainWindow.loadList), false, null, zoomParam);
            DrawMasses(new List<Mass>(MainWindow.massList), false, null, zoomParam);
        }

        public static List<Node> GetDispListScaled(List<Node> dispList, double scale)
        {
            List<Node> dispListScaled = new List<Node>();
            for (int i = 0; i < dispList.Count; i++)
            {
                double x = dispList[i].Point.X * scale;
                double y = dispList[i].Point.Y * scale;
                double z = dispList[i].Point.Z * scale;
                dispListScaled.Add(new Node(dispList[i].ID, new Point3D(x, y, z)));
            }
            return dispListScaled;
        }
    }
}

