using CLTFEM.Classes.Helpers;
using System;
using System.Collections.Generic;

namespace CLTFEM.Classes.Structural
{
    public class Mass
    {
        private int _ID, _nodeID;
        //the _load array works like this: Index 0 -> Fx, 1 -> Fy, 2 -> Fz
        private List<PairValue> _mass;

        public Mass(int ID, int nodeID)
        {
            _ID = ID;
            _nodeID = nodeID;
            _mass = new List<PairValue>();
        }

        public override string ToString()
        {
            string s = "";
            for (int i = 0; i < _mass.Count; i++)
            {
                s += String.Format(" ({0}, {1:F})", _mass[i].ID, _mass[i].GetVal);
            }
            return String.Format("{0}: Node = {1}", _ID, _nodeID) + s;
        }

        public int ID
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

        public int NodeID
        {
            get
            {
                return _nodeID;
            }
        }

        public List<PairValue> GetMassList
        {
            get
            {
                return _mass;
            }
        }

        public List<PairValue> SetMassList
        {
            set
            {
                _mass = value;
            }
        }

        public void SetMx(double val)
        {
            _mass.Add(new PairValue(1, val));
        }

        public void SetMy(double val)
        {
            _mass.Add(new PairValue(2, val));
        }

        public void SetMz(double val)
        {
            _mass.Add(new PairValue(3, val));
        }

        static void SortByNodeID(ref List<Mass> mass)
        {
            List<Mass> mass1 = new List<Mass>(); //to hold the sorted list
            int maxNodeID = 1;
            int count = 0;
            int iter = 0;
            while (true)
            {
                if (mass[iter]._nodeID == maxNodeID)
                {
                    mass1.Add(mass[iter]);
                    maxNodeID++;
                    count++;
                }
                if (count == mass.Count)
                {
                    break;
                }
                iter++;
                if (iter >= mass.Count)
                {
                    iter = 0;
                }
            }
            mass = mass1;
        }
    }
}
