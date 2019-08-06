using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CLTFEM.Classes.Analysis
{
    class PushoverAnalysis : Analyses
    {
        private int _nStep;
        private int _nIter;

        public PushoverAnalysis()
        {

        }

        public PushoverAnalysis(int nStep, int nIter)
        {
            _nStep = nStep;
            _nIter = nIter;
        }

        public int Steps
        {
            get
            {
                return _nStep;
            }
            set
            {
                _nStep = value; 
            }
        }

        public int Iters
        {
            get
            {
                return _nIter;
            }
            set
            {
                _nIter = value;
            }
        }

        public override string AnalysisType()
        {
            return "Pushover";
        }

        public override string ToString()
        {
            return String.Format("{0} Analysis: Load Steps: {1}, Max Iterations: {2}", AnalysisType(), _nStep, _nIter);
        }
    }
}
