using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CLTFEM.Classes.Analysis
{
    class ElasticAnalysis : Analyses
    {
        private int _nStep;

        public ElasticAnalysis()
        {

        }

        public ElasticAnalysis(int nStep)
        {
            _nStep = nStep;
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

        public override string AnalysisType()
        {
            return "Elastic";
        }

        public override string ToString()
        {
            return String.Format("{0} Analysis: Load Steps: {1}", AnalysisType(), _nStep);
        }
    }
}
