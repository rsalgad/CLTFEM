using CLTFEM.Classes.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;

namespace CLTFEM.Classes.Structural
{
    public class Load
    {
        private int _ID, _nodeID;
        //the _load array works like this: Index 0 -> Fx, 1 -> Fy, 2 -> Fz, 3 -> Mx, 4 -> My, 5 -> Mz
        private List<PairValue> _load;
        private string _status = "constant";

        public Load(int ID, int nodeID)
        {
            _ID = ID;
            _nodeID = nodeID;
            _load = new List<PairValue>();
        }

        public Load(int ID, int nodeID, string status)
        {
            _ID = ID;
            _nodeID = nodeID;
            _load = new List<PairValue>();
            _status = status;
        }

        public override string ToString()
        {
            string s = "";
            for (int i = 0; i < _load.Count; i++)
            {
                s += String.Format(" ({0}, {1:F})", _load[i].ID, _load[i].GetVal);
            }
            return String.Format("{0}: {1} Node = {2}", _ID, _status, _nodeID) + s;
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

        public List<PairValue> GetLoadList
        {
            get
            {
                return _load;
            }
        }

        public List<PairValue> SetLoadList
        {
            set
            {
                _load = value;
            }
        }

        public string Status
        {
            get
            {
                return _status;
            }
            set
            {
                _status = value;
            }
        }

        public void SetFx(double val)
        {
            _load.Add(new PairValue(1,val));
        }

        public void SetFy(double val)
        {
            _load.Add(new PairValue(2, val));
        }

        public void SetFz(double val)
        {
            _load.Add(new PairValue(3, val));
        }

        public void SetMx(double val)
        {
            _load.Add(new PairValue(4, val));
        }

        public void SetMy(double val)
        {
            _load.Add(new PairValue(5, val));
        }

        public void SetMz(double val)
        {
            _load.Add(new PairValue(6, val));
        }

        public static void SortByNodeID(ref List<Load> load)
        {
            List<Load> load1 = new List<Load>(); //to hold the sorted list
            int maxNodeID = 1;
            int count = 0;
            int iter = 0;
            while (true)
            {
                if (load[iter]._nodeID == maxNodeID)
                {
                    load1.Add(load[iter]);
                    maxNodeID++;
                    count++;
                }
                if (count == load.Count)
                {
                    break;
                }
                iter++;
                if (iter >= load.Count)
                {
                    iter = 0;
                }
            }
            load = load1;
        }

        public static bool IsThereFlaggedLoads(ref ObservableCollection<Load> listOfLoads, string flag)
        {
            for (int i = 0; i < listOfLoads.Count; i++)
            {
                if (listOfLoads[i].Status == flag)
                {
                    return true;
                }
            }
            return false;
        }

        public static void DeleteAllLoadsWithFlag(ref ObservableCollection<Load> listOfLoads, string flag)
        {
            if (IsThereFlaggedLoads(ref listOfLoads, flag) == true)
            {
                MessageBox.Show("Existing " + flag + " loads will be deleted.");

                int deleted = 0;
                for (int i = 0; i < listOfLoads.Count; i++)
                {
                    if (listOfLoads[i].Status == flag)
                    {
                        listOfLoads.RemoveAt(i - deleted);
                    }
                }

                Management.ReorganizeLoadList();
            }
        }

    }
}
