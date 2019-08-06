using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CLTFEM.Classes.Structural
{
    class SpringGeneralModel : Material
    {
        public int _ID;
        public double _iniStiff, _dMax, _fMax, _degStiff, _fRes, _dUlt, _unlStiff, _fUnl, _conStiff, _relStiff;

        public SpringGeneralModel()
        {

        }

        public SpringGeneralModel(int ID, double iniStiff, double dMax, double fMax, double degStiff, double fRes, double dUlt, double unlStiff, double fUnl, double conStiff, double relStiff)
        {
            _ID = ID;
            _iniStiff = iniStiff;
            _dMax = dMax;
            _fMax = fMax;
            _degStiff = degStiff;
            _fRes = fRes;
            _dUlt = dUlt;
            _unlStiff = unlStiff;
            _fUnl = fUnl;
            _conStiff = conStiff;
            _relStiff = relStiff;
        }

        public override string Type
        {
            get
            {
                return "Spring-General";
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
            return String.Format("{0}: Initial Stiffness = {1:F}, Peak Force = {2:F}, Peak Displacement = {3:F}, Degrading Stiffness = {4:F}, Residual Force = {5:F}, Ultimate Displacement = {6:F}, Unload Stiffness = {7:F}, Unload Force = {8:F}, Connection Stiffness = {9:F}, Reload Stiffness = {10:F}", _ID, _iniStiff, _fMax, _dMax, _degStiff, _fRes, _dUlt, _unlStiff, _fUnl, _conStiff, _relStiff);
        }
    }
}
