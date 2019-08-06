using System;

namespace CLTFEM.Classes.Mathematics
{
    public class myPoint
    {
        private double _X;
        private double _Y;
        private double _Z;

        public myPoint()
        {

        }

        public myPoint(double x, double y, double z)
        {
            _X = x;
            _Y = y;
            _Z = z;
        }

        public override string ToString()
        {
            return String.Format("({0:F},{1:F},{2:F})", _X, _Y, _Z);
        }

        public static myPoint operator +(myPoint p1, myPoint p2)
        {
            return new myPoint(p1.x + p2.x, p1.y + p2.y, p1.z + p2.z);
        }

        public static myPoint operator -(myPoint p1, myPoint p2)
        {
            return new myPoint(p1.x - p2.x, p1.y - p2.y, p1.z - p2.z);
        }

        public double x
        {
            get
            {
                return _X;
            }
            set
            {
                _X = value;
            }
        }

        public double y
        {
            get
            {
                return _Y;
            }
            set
            {
                _Y = value;
            }
        }

        public double z
        {
            get
            {
                return _Z;
            }
            set
            {
                _Z = value;
            }
        }
    }
}
