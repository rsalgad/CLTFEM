using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CLTFEM.Classes.Structural
{
    class SpringAxialModel : Material
    {
        public int _ID;
        public double _iniStiff, _dMax, _fMax, _degStiff, _fRes, _dUlt, _compStiff, _unlStiff, _fUnl, _conStiff, _relStiff;

        public SpringAxialModel()
        {

        }

        public SpringAxialModel(int ID, double iniStiff, double dMax, double fMax, double degStiff, double fRes, double dUlt, double compStiff, double unlStiff, double fUnl, double conStiff, double relStiff)
        {
            _ID = ID;
            _iniStiff = iniStiff;
            _dMax = dMax;
            _fMax = fMax;
            _degStiff = degStiff;
            _fRes = fRes;
            _dUlt = dUlt;
            _compStiff = compStiff;
            _unlStiff = unlStiff;
            _fUnl = fUnl;
            _conStiff = conStiff;
            _relStiff = relStiff;
        }

        public override string Type
        {
            get
            {
                return "Spring-Axial";
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

        public override string ToString()
        {
            return String.Format("{0}: Initial Stiffness = {1:F}, Peak Force = {2:F}, Peak Displacement = {3:F}, Degrading Stiffness = {4:F}, Residual Force = {5:F}, Ultimate Displacement = {6:F}, Compressive Stiffness = {7:F}, Unload Stiffness = {8:F}, Unload Force = {9:F}, Connection Stiffness = {10:F}, Reload Stiffness = {11:F}", _ID, _iniStiff, _fMax, _dMax, _degStiff, _fRes, _dUlt, _compStiff, _unlStiff, _fUnl, _conStiff, _relStiff);
        }
    }
}
