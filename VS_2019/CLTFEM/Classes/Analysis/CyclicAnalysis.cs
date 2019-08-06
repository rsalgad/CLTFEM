using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CLTFEM.Classes.Analysis
{
    class CyclicAnalysis : Analyses
    {
        private int _nStep;
        private int _nIter;
        private double _iniPeak;
        private int _nStepPeak;
        private double _peakInc;
        private int _nCyclesPeak;
        private char _type;



        public CyclicAnalysis()
        {

        }

        public CyclicAnalysis(int nStep, int nIter, double iniPeak, int nStepPeak, double peakInc, int nCyclesPeak, char type)
        {
            _nStep = nStep;
            _nIter = nIter;
            _iniPeak = iniPeak;
            _nStepPeak = nStepPeak;
            _peakInc = peakInc;
            _nCyclesPeak = nCyclesPeak;
            _type = type;
        }

        public int Steps
        {
            get{ return _nStep; }
            set { _nStep = value; }
        }

        public int Iters
        {
            get { return _nIter; }
            set { _nIter = value; }
        }

        public double InitialPeak
        {
            get { return _iniPeak; }
            set { _iniPeak = value; }
        }

        public int StepsPerPeak
        {
            get { return _nStepPeak; }
            set { _nStepPeak = value; }
        }

        public double PeakIncrement
        {
            get { return _peakInc; }
            set { _peakInc = value; }
        }

        public int CyclesPerPeak
        {
            get { return _nCyclesPeak; }
            set { _nCyclesPeak = value; }
        }

        public char Type
        {
            get { return _type; }
            set { _type = value; }
        }

        public override string AnalysisType()
        {
            return "Cyclic";
        }

        public override string ToString()
        {
            return String.Format("{0} Analysis: Load Steps: {1}, Max Iterations: {2}, Initial Peak: {3:F}, Steps per Peak: {4}, Peak Increment: {5:F}, Cycles per Peak: {6}, Type: {7}",
                AnalysisType(), _nStep, _nIter, _iniPeak, _nStepPeak, _peakInc, _nCyclesPeak, _type);
        }
    }
}
