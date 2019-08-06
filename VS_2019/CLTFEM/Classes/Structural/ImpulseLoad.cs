using System.Collections.Generic;
using CLTFEM.Classes.Helpers;

namespace CLTFEM.Classes.Structural
{
    public class ImpulseLoad
    {
        private double[,] _points = new double[3, 2];

        public ImpulseLoad()
        {

        }

        public double[,] Points
        {
            get
            {
                return _points;
            }
        }

        public void SetPoint1(double force, double time)
        {
            _points[0, 0] = time;
            _points[0, 1] = force;
        }

        public void SetPoint2(double force, double time)
        {
            _points[1, 0] = time;
            _points[1, 1] = force;
        }

        public void SetPoint3(double force, double time)
        {
            _points[2, 0] = time;
            _points[2, 1] = force;
        }
    }
}
