using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media.Media3D;

namespace CLTFEM.Classes.Structural
{
    public class ShellElement8N
    {

        private int _ID;
        private Node[] _nodeList = new Node[8];
        private Node[] _dispList = new Node[8];
        private double _thickness;
        private int _layers;
        private ElasticMaterial _material;

        public ShellElement8N()
        {

        }

        public ShellElement8N(int ID, Node n1, Node n2, Node n3, Node n4, Node n5, Node n6, Node n7, Node n8, double thickness, int layers, ElasticMaterial material)
        {
            _ID = ID;
            _nodeList[0] = n1;
            _nodeList[1] = n2;
            _nodeList[2] = n3;
            _nodeList[3] = n4;
            _nodeList[4] = n5;
            _nodeList[5] = n6;
            _nodeList[6] = n7;
            _nodeList[7] = n8;
            _thickness = thickness;
            _layers = layers;
            _material = material;
        }

        public override string ToString()
        {
            return String.Format("{0}: Thickness = {1:F}, Layers = {2:F}, Material = {3:F}, Nodes = ({4}, {5}, {6}, {7}, {8}, {9}, {10}, {11})", _ID, _thickness, _layers, _material.ID, _nodeList[0].ID, _nodeList[1].ID, _nodeList[2].ID, _nodeList[3].ID, _nodeList[4].ID, _nodeList[5].ID, _nodeList[6].ID, _nodeList[7].ID);
        }

        public double width
        {
            get
            {
                Vector3D v = new Vector3D(_nodeList[1].Point.X - _nodeList[0].Point.X, _nodeList[1].Point.Y - _nodeList[0].Point.Y, _nodeList[1].Point.Z - _nodeList[0].Point.Z);
                return v.Length;
            }
        }

        public double height
        {
            get
            {
                Vector3D v = new Vector3D(_nodeList[3].Point.X - _nodeList[0].Point.X, _nodeList[3].Point.Y - _nodeList[0].Point.Y, _nodeList[3].Point.Z - _nodeList[0].Point.Z);
                return v.Length;
            }
        }

        public int layers
        {
            get
            {
                return _layers;
            }
        }


        public Vector3D widthDirection
        {
            get
            {
                Vector3D dir = new Vector3D(_nodeList[1].Point.X - _nodeList[0].Point.X, _nodeList[1].Point.Y - _nodeList[0].Point.Y, _nodeList[1].Point.Z - _nodeList[0].Point.Z);
                dir.Normalize();
                return dir;
            }
        }

        public Vector3D heightDirection
        {
            get
            {
                Vector3D dir = new Vector3D(_nodeList[3].Point.X - _nodeList[0].Point.X, _nodeList[3].Point.Y - _nodeList[0].Point.Y, _nodeList[3].Point.Z - _nodeList[0].Point.Z);
                dir.Normalize();
                return dir;
            }
        }

        public Vector3D getV3
        {
            get
            {
                Vector3D vector1 = new Vector3D(_nodeList[1].Point.X - _nodeList[0].Point.X, _nodeList[1].Point.Y - _nodeList[0].Point.Y, _nodeList[1].Point.Z - _nodeList[0].Point.Z);
                Vector3D vector2 = new Vector3D(_nodeList[3].Point.X - _nodeList[0].Point.X, _nodeList[3].Point.Y - _nodeList[0].Point.Y, _nodeList[3].Point.Z - _nodeList[0].Point.Z);
                Vector3D V3 = Vector3D.CrossProduct(vector1, vector2);
                V3.Normalize();
                return V3;
            }
        }

        public void FindDisplacementsForElement(List<Node> listDisp)
        {
            for (int i = 0; i < 8; i++)
            {
                _dispList[i] = listDisp[_nodeList[i].ID - 1]; //this will look into the displaced list for the node ID - 1 (e.g, node 1 is index 0), and retrieve its disp info.
            }
        }

        public Node[] Displacements
        {
            get
            {
                return _dispList;
            }
            set
            {
                _dispList = value;
            }
        }

        public static double ShapeFunctionN(int i, double e, double n)
        {

            switch (i)
            {
                case 1:
                    return (1.0 - e) * (1.0 - n) * (-e - n - 1.0) / 4.0;
                case 2:
                    return (1.0 + e) * (1.0 - n) * (e - n - 1.0) / 4.0;
                case 3:
                    return (1.0 + e) * (1.0 + n) * (e + n - 1.0) / 4.0;
                case 4:
                    return (1.0 - e) * (1.0 + n) * (-e + n - 1.0) / 4.0;
                case 5:
                    return (1.0 + e) * (1.0 - e) * (1.0 - n) / 2.0;
                case 6:
                    return (1.0 + e) * (1.0 + n) * (1.0 - n) / 2.0;
                case 7:
                    return (1.0 + e) * (1.0 - e) * (1.0 + n) / 2.0;
                case 8:
                    return (1.0 - e) * (1.0 + n) * (1.0 - n) / 2.0;
                default:
                    //i needs to be between 1 and 8
                    return 0; //If this is executed, i is either greater than 8 or less than 1. This is wrong!
            }
        }

        /*
        public Vector3D Direction
        {
            get
            {
                return new Vector3D(_n2.Point.X - _n1.Point.X, _n2.Point.Y - _n1.Point.Y, _n2.Point.Z - _n1.Point.Z);
            }
        }
        */

        public Node[] nodeList
        {
            get
            {
                return _nodeList;
            }
        }

        public int ID
        {
            get
            {
                return _ID;
            }
        }

        public double thickness
        {
            get
            {
                return _thickness;
            }
        }

        public ElasticMaterial material
        {
            get
            {
                return _material;
            }
        }
    }
}
