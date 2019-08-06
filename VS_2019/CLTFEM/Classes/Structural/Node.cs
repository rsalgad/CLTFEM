using System;
using System.Collections.Generic;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace CLTFEM.Classes.Structural
{
    public class Node
    {

        private int _ID;
        private Point3D _p;
        static private DiffuseMaterial _nodeMat;

        public Node()
        {

        }

        public Node(int ID, Point3D point)
        {
            _ID = ID;
            _p = point;
        }

        static public DiffuseMaterial SetNodeMaterial
        {
            get
            {
                return _nodeMat;
            }
            set
            {
                _nodeMat = value;
            }
        }

        public override string ToString()
        {
            return String.Format("{0}: {1:F}, {2:F}, {3:F}", _ID, _p.X, _p.Y, _p.Z);
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

        public Point3D Point
        {
            get
            {
                return _p;
            }
        }

        public static Node FindNodeByCoordinates(double x, double y, double z, List<Node> nodeList)
        {
            //this should be optmized
            for (int i = 0; i < nodeList.Count; i++)
            {
                if (nodeList[i].Point.X == x && nodeList[i].Point.Y == y && nodeList[i].Point.Z == z)
                {
                    return nodeList[i];
                }
            }
            return null; //if not found
        }

        private void SetNodeViewProperties()
        {

        }
    }
}
