using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CLTFEM.Classes.Helpers
{
    public class PairValue
    {
        private int _ID;
        private double _val;

        public PairValue(int ID, double val)
        {
            _ID = ID;
            _val = val;
        }

        public int ID
        {
            get
            {
                return _ID;
            }
        }

        public double GetVal
        {
            get
            {
                return _val;
            }
        }
    }
}
