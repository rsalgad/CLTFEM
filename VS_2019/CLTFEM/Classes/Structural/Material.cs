using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CLTFEM.Classes.Structural
{
    public abstract class Material
    {
        public abstract int ID
        {
            get;
            set;
        }
        public abstract string Type
        {
            get;
        }
        public abstract override string ToString();
    }
}
