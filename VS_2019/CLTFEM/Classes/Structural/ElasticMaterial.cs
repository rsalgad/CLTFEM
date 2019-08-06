using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CLTFEM.Classes.Structural
{
    public class ElasticMaterial : Material
    {

        private int _ID;
        private double _E;
        private double _v;

        /// <summary>
        /// Initializes a new simple Elastic Material
        /// </summary>
        /// <param name="ID">ID of the material</param>
        /// <param name="E">Elastic stiffness of the material</param>
        /// <param name="v">Poisson ratio of the material</param>
        public ElasticMaterial(int ID, double E, double v)
        {
            _ID = ID;
            _E = E;
            _v = v;
        }

        public override string ToString()
        {
            return String.Format("{0}: Elasticity = {1:F}, Poisson = {2:F}", _ID, _E, _v);
        }

        public override string Type
        {
            get
            {
                return "Elastic";
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

        public double E
        {
            get
            {
                return _E;
            }
        }

        public double V
        {
            get
            {
                return _v;
            }
        }
    }
}
