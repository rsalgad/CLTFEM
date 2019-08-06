using CLTFEM.Classes.Mathematics;
using System;
using System.Collections.Generic;

namespace CLTFEM.Classes.Structural
{
    public class Spring3D
    {
        private int _ID;
        private int[] _matList;
        Node _n1;
        Node _n2;
        char _vecX, _vecY;
        public Node[] _dispList = new Node[2];

        public Spring3D()
        {

        }

        public Spring3D(int ID, Node n1, Node n2, int[] matList, char vecX, char vecY)
        {
            _ID = ID;
            _n1 = n1;
            _n2 = n2;
            _matList = matList;
            _vecX = vecX;
            _vecY = vecY;
        }

        public override string ToString()
        {
            return String.Format("{0}: Node 1 ID = {1}, Node 2 ID = {2}, Material X-Dir = {3}, Material Y-Dir = {4}, Material Z-Dir = {5})", _ID, _n1.ID, _n2.ID, _matList[0], _matList[1], _matList[2]);
        }

        public void FindDisplacementsForElement(List<Node> listDisp)
        {
            int[] id = { _n1.ID, _n2.ID };
            for (int i = 0; i < 2; i++)
            {
                _dispList[i] = listDisp[id[i] - 1]; //this will look into the displaced list for the node ID - 1 (e.g, node 1 is index 0), and retrieve its disp info.
            }
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

        public Node N1
        {
            get
            {
                return _n1;
            }
        }

        public Node N2
        {
            get
            {
                return _n2;
            }
        }


        public int[] MaterialList
        {
            get
            {
                return _matList;
            }
        }


        public char VectorX
        {
            get
            {
                return _vecX;
            }
        }

        public char VectorY
        {
            get
            {
                return _vecY;
            }
        }
    }
}
