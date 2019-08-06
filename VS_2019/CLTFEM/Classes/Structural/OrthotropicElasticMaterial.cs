using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CLTFEM.Classes.Structural
{
    public class OrthotropicElasticMaterial : Material
    {
        private int _ID;
        private double _Ex;
        private double _Ey;
        private double _vxy;
        private double _Gxy;
        private double _Gyz;
        private double _Gxz;

        /// <summary>
        /// Initializes a new simple Elastic Material
        /// </summary>
        /// <param name="ID">ID of the material</param>
        /// <param name="E">Elastic stiffness of the material</param>
        /// <param name="v">Poisson ratio of the material</param>
        public OrthotropicElasticMaterial(int ID, double Ex, double Ey, double vxy, double Gxy, double Gyz, double Gxz)
        {
            _ID = ID;
            _Ex = Ex;
            _Ey = Ey;
            _vxy = vxy;
            _Gxy = Gxy;
            _Gyz = Gyz;
            _Gxz = Gxz;
        }

        public override string ToString()
        {
            return String.Format("{0}: Ex = {1:F}, Ey = {2:F}, Vxy = {3:F}, Gxy = {4:F}, Gyz = {5:F}, Gxz = {6:F}", _ID, _Ex, _Ey, _vxy, _Gxy, _Gyz, _Gxz);
        }

        public override string Type
        {
            get
            {
                return "OrthoElastic";
            }
        }

        public override int ID
        {
            get
            {
                return _ID;
            }
            set
            {
                _ID = value;
            }
        }

        public double Ex
        {
            get
            {
                return _Ex;
            }
        }

        public double Ey
        {
            get
            {
                return _Ey;
            }
        }

        public double Vxy
        {
            get
            {
                return _vxy;
            }
        }

        public double Gxy
        {
            get
            {
                return _Gxy;
            }
        }

        public double Gyz
        {
            get
            {
                return _Gyz;
            }
        }

        public double Gxz
        {
            get
            {
                return _Gxz;
            }
        }

    }
}
