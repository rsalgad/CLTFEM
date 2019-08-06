using CLTFEM.Classes.Helpers;
using System;
using System.Collections.Generic;

namespace CLTFEM.Classes.Structural
{
    

    public class Support
    {
        private int _ID, _nodeID;
        //the _support array works like this: Index 0 -> Tx, 1 -> Ty, 2 -> Tz, 3 -> Rx, 4 -> Ry, 5 -> Rz
        private List<PairValue> _support;

        public Support(int ID, int nodeID)
        {
            _ID = ID;
            _nodeID = nodeID;
            _support = new List<PairValue>();
        }

        public override string ToString()
        {
            string s = "";
            for (int i = 0; i < _support.Count; i++)
            {
                s += String.Format(" ({0}, {1:F})", _support[i].ID, _support[i].GetVal);
            }
            return String.Format("{0}: Node = {1}", _ID, _nodeID) + s; ;
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

        public List<PairValue> GetSupportList
        {
            get
            {
                return _support;
            }
        }

        public List<PairValue> SetSupportList
        {
            set
            {
                _support = value;
            }
        }

        public void Set_tX(double val)
        {
            _support.Add(new PairValue(1, val));
        }

        public void Set_tY(double val)
        {
            _support.Add(new PairValue(2, val));
        }

        public void Set_tZ(double val)
        {
            _support.Add(new PairValue(3, val));
        }

        public void Set_rX(double val)
        {
            _support.Add(new PairValue(4, val));
        }

        public void Set_rY(double val)
        {
            _support.Add(new PairValue(5, val));
        }

        public void Set_rZ(double val)
        {
            _support.Add(new PairValue(6, val));
        }

        public static void SortByNodeID(ref List<Support> sup)
        {
            List<Support> sup1 = new List<Support>(); //to hold the sorted list
            int maxNodeID = 1;
            int count = 0;
            int iter = 0;
            while (true)
            {
                if (sup[iter]._nodeID == maxNodeID)
                {
                    sup1.Add(sup[iter]);
                    maxNodeID++;
                    count++;
                }
                if (count == sup.Count)
                {
                    break;
                }
                iter++;
                if (iter >= sup.Count)
                {
                    iter = 0;
                }
            }
            sup = sup1;
        }

        public static bool IsDOFFixed(int DOF, List<Support> supList)
        {
            int nDOF = 6;
            for (int i = 0; i < supList.Count; i++)
            {
                List<PairValue> list = supList[i].GetSupportList;
                for (int j = 0; j < list.Count; j++)
                {
                    int supportDOF = (supList[i].NodeID - 1) * nDOF + list[i].ID;
                    if (supportDOF == DOF)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

    }
}
