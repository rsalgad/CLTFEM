using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CLTFEM.Classes.Analysis
{
    class DynamicAnalysis : Analyses
    {
        private double _deltaT;
        private double _addTime;
        private int _nIter;
        private string _intMethod;
        private char _type;

        public DynamicAnalysis()
        {

        }

        public DynamicAnalysis(double deltaT, double addTime, int nIter, string intMethod, char type)
        {
            _deltaT = deltaT;
            _addTime = addTime;
            _nIter = nIter;
            _intMethod = intMethod;
            _type = type;
        }

        public double DeltaT
        {
            get { return _deltaT; }
            set { _deltaT = value; }
        }

        public double AdditionalTime
        {
            get { return _addTime; }
            set { _addTime = value; }
        }

        public int Iters
        {
            get { return _nIter; }
            set { _nIter = value; }
        }

        public string IntegrationMethod
        {
            get { return _intMethod; }
            set { _intMethod = value; }
        }

        public char Type
        {
            get { return _type; }
            set { _type = value; }
        }

        public override string AnalysisType()
        {
            return "Dynamic";
        }

        public override string ToString()
        {
            return String.Format("{0} Analysis: Delta T: {1:F}, Additional Time: {2:F}, Max Iterations: {3}, Integration Method: {4}, Type: {5}", AnalysisType(), _deltaT, _addTime, _nIter, _intMethod, _type);
        }
    }
}
