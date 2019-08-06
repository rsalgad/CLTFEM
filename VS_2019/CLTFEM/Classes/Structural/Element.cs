using System;
using System.Windows.Media.Media3D;

namespace CLTFEM.Classes.Structural
{
    public class Element
    {

        private int _ID;
        private Node _n1;
        private Node _n2;

        public Element()
        {

        }

        public Element(int ID, Node node1, Node node2)
        {
            _ID = ID;
            _n1 = node1;
            _n2 = node2;

        }

        public override string ToString()
        {
            return String.Format("{0}: Node {1} to Node {2}", _ID, _n1.ID, _n2.ID);
        }

        public double Length
        {
            get
            {
                return Math.Sqrt(Math.Pow(_n2.Point.X - _n1.Point.X, 2) + Math.Pow(_n2.Point.Y - _n1.Point.Y, 2) + Math.Pow(_n2.Point.Z - _n1.Point.Z, 2));
            }
        }

        public Vector3D Direction
        {
            get
            {
                return new Vector3D(_n2.Point.X - _n1.Point.X, _n2.Point.Y - _n1.Point.Y, _n2.Point.Z - _n1.Point.Z);
            }
        }

        public Node n1
        {
            get
            {
                return _n1;
            }
        }

        public Node n2
        {
            get
            {
                return _n2;
            }
        }

        public int ID
        {
            get
            {
                return _ID;
            }
        }
    }
}
