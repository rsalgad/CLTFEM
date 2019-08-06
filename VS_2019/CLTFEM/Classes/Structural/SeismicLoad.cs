using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CLTFEM.Classes.Structural
{
    public class SeismicLoad
    {
        private List<char> _directions = new List<char>();
        private List<List<double>> _records = new List<List<double>>();
        private List<double> _scale = new List<double>();
        private double _deltaT;

        public SeismicLoad()
        {

        }
        
        public List<double> Scales
        {
            get
            {
                return _scale;
            }
        }

        public double DeltaT
        {
            get
            {
                return _deltaT;
            }
            set
            {
                _deltaT = value;
            }
        }

        public List<List<double>> Records
        {
            get
            {
                return _records;
            }
        }

        public List<char> Directions
        {
            get
            {
                return _directions;
            }
        }

        public void SetRecordX(List<double> list, double scale)
        {
            _records.Add(list);
            _directions.Add('x');
            _scale.Add(scale);
        }

        public void SetRecordY(List<double> list, double scale)
        {
            _records.Add(list);
            _directions.Add('y');
            _scale.Add(scale);
        }

        public void SetRecordZ(List<double> list, double scale)
        {
            _records.Add(list);
            _directions.Add('z');
            _scale.Add(scale);
        }

        public static SeismicLoad CreateSeismicLoadFromFiles(double deltaT, string[] listOfX, string[] listOfY, string[] listOfZ, int skipX, int skipY, int skipZ, double scaleX, double scaleY, double scaleZ)
        {
            string[] terms;

            SeismicLoad sLoad = new SeismicLoad();
            sLoad.DeltaT = deltaT;

            List<double> valuesX = new List<double>();
            List<double> valuesY = new List<double>();
            List<double> valuesZ = new List<double>();

            for (var i = skipX; i < listOfX.Length; i++)
            {
                terms = listOfX[i].Split(' ');
                for (int j = 0; j < terms.Length; j++)
                {
                    if (terms[j] != "")
                    {
                        valuesX.Add(double.Parse(terms[j]));
                    }
                }
            }

            sLoad.SetRecordX(valuesX, scaleX);

            if (listOfY != null)
            {
                for (var i = skipY; i < listOfY.Length; i++)
                {
                    terms = listOfY[i].Split(' ');
                    for (int j = 0; j < terms.Length; j++)
                    {
                        if (terms[j] != "")
                        {
                            valuesY.Add(double.Parse(terms[j]));
                        }
                    }
                }
                sLoad.SetRecordY(valuesY, scaleY);
            }


            if (listOfZ != null)
            {
                for (var i = skipZ; i < listOfZ.Length; i++)
                {
                    terms = listOfZ[i].Split(' ');
                    for (int j = 0; j < terms.Length; j++)
                    {
                        if (terms[j] != "")
                        {
                            valuesZ.Add(double.Parse(terms[j]));
                        }
                    }
                }
                sLoad.SetRecordZ(valuesZ, scaleZ);
            }

            return sLoad;
        }

    }
}
