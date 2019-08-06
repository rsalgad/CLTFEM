using System;

namespace CLTFEM.Classes.Mathematics
{
    public class Vector
    {
        private myPoint _Point;
        private myPoint _iniPoint = new myPoint(0, 0, 0);

        public Vector()
        {

        }

        public Vector(myPoint p)
        {
            _Point = p;
        }

        /// <summary>
        /// This constructor is used to create a vector using the input of two points
        /// </summary>
        /// <param name="iniPoint">First point to calculate the vector</param>
        /// <param name="endPoint">Second point to calculate the vector</param>
        public Vector(myPoint iniPoint, myPoint endPoint)
        {
            _Point = endPoint - iniPoint;
            _iniPoint = iniPoint;
        }

        public override string ToString()
        {
            return String.Format("({0:F},{1:F},{2:F})", _Point.x, _Point.y, _Point.z);
        }

        public myPoint Point
        {
            get
            {
                return _Point;
            }
            set
            {
                _Point = value;
            }
        }

        public double Length
        {
            get
            {
                return Math.Sqrt(Math.Pow(_Point.x, 2) + Math.Pow(_Point.y, 2) + Math.Pow(_Point.z, 2));
            }
        }

        /// <summary>
        /// Gets the length of the projection of the vector in the XY plane
        /// </summary>
        public double LengthXY
        {
            get
            {
                return Math.Sqrt(Math.Pow(_Point.x, 2) + Math.Pow(_Point.y, 2));
            }
        }

        /// <summary>
        /// Add a number of Vectors together
        /// </summary>
        /// <param name="vectorList">Vectors that are going to be added</param>
        /// <returns>Returns a Vector that represents the summation of the input Vectors</returns>
        public static Vector Add(params Vector[] vectorList)
        {
            myPoint P = new myPoint(0, 0, 0);

            foreach (Vector v in vectorList)
            {
                P += v.Point;
            }

            return new Vector(P);
        }

        /// <summary>
        /// Performs the dot product operation between two vectors
        /// </summary>
        /// <param name="v1">First vector in the dot product</param>
        /// <param name="v2">Second vector in the dot product</param>
        /// <returns></returns>
        public static double DotProduct(Vector v1, Vector v2)
        {
            return (v1.Length * v2.Length) * CosAngleBetween(v1, v2);
        }

        /// <summary>
        /// Returns the cosine of the angle between two vectors
        /// </summary>
        /// <param name="v1">First vector to calculate the cosine</param>
        /// <param name="v2">Second vector to calculate the cosine</param>
        /// <returns></returns>
        public static double CosAngleBetween(Vector v1, Vector v2)
        {
            return (v1.Point.x * v2.Point.x + v1.Point.y * v2.Point.y + v1.Point.z * v2.Point.z) / (v1.Length * v2.Length);
        }


        /// <summary>
        /// Get a point on the direction of a specified vector
        /// </summary>
        /// <param name="v">Vector to be used as the direction to obtain the point</param>
        /// <param name="iniPoint">The initial point at which the new point will be assessed from</param>
        /// <param name="dist">The distance from the initial point</param>
        /// <returns>Returns a point at a distance from the initial point following the direction of the input vector</returns>
        public static myPoint GetPointAtVector(Vector v, myPoint iniPoint, double dist)
        {
            myPoint P = new myPoint(iniPoint.x + v.CosX * dist, iniPoint.y + v.CosY * dist, iniPoint.z + v.CosZ * dist);
            return P;
        }

        /// <summary>
        /// Gets a unit vector that is perpendicular from the given vector in the Z-axis
        /// </summary>
        /// <param name="v">The input vector at which the perpedicular direction will be calculated from</param>
        /// <returns>Returns a Vector that is perpendicular to the given vector in the Z direction</returns>
        public static Vector GetOrthoVectorToZ(Vector v)
        {
            double x;
            double y;
            double lengthXY = Math.Sqrt(Math.Pow(v.Point.x, 2) + Math.Pow(v.Point.y, 2));
            double z;
            double zAngle = Math.Acos(v.CosZ);
            Vector perpVec;

            if (zAngle <= Math.PI / 2)
            {
                x = -v.Point.x;
                y = -v.Point.y;
                z = lengthXY / Math.Tan(Math.PI / 2 - zAngle);
            }
            else
            {
                x = v.Point.x;
                y = v.Point.y;
                z = lengthXY / Math.Tan(zAngle - Math.PI / 2);
            }

            perpVec = new Vector(new myPoint(x, y, z));

            return new Vector(new myPoint(x / perpVec.Length, y / perpVec.Length, z / perpVec.Length));
        }

        public double CosX
        {
            get
            {
                return Point.x / Length;
            }
        }

        public double CosY
        {
            get
            {
                return Point.y / Length;
            }
        }

        public double CosZ
        {
            get
            {
                return Point.z / Length;
            }
        }

        public double CosXYx
        {
            get
            {
                return Point.x / LengthXY;
            }
        }

        public double CosXYy
        {
            get
            {
                return Point.y / LengthXY;
            }
        }
    }
}
