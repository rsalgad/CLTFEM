using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CLTFEM.Classes.Mathematics
{
    public class MatrixMath
    {
        private double[,] _matrix;

        /// <summary>
        /// Default constructor
        /// </summary>
        public MatrixMath()
        {

        }

        /// <summary>
        /// Constructs the matrix based on line by line input from the user
        /// </summary>
        /// <param name="rows">Rows of the matrix</param>
        public MatrixMath(double[,] matrix)
        {
            _matrix = matrix;
        }

        public static MatrixMath operator +(MatrixMath m1, MatrixMath m2)
        {
            double[,] addedMatrix = new double[m1.nRow, m1.nCol];
            for (int i = 0; i < m1.nRow; i++)
            {
                for (int j = 0; j < m1.nCol; j++)
                {
                    addedMatrix[i, j] = m1.matrix[i, j] + m2.matrix[i, j];
                }
            }
            return new MatrixMath(addedMatrix);
        }

        public static MatrixMath operator -(MatrixMath m1, MatrixMath m2)
        {
            double[,] subMatrix = new double[m1.nRow, m1.nCol];
            for (int i = 0; i < m1.nRow; i++)
            {
                for (int j = 0; j < m1.nCol; j++)
                {
                    subMatrix[i, j] = m1.matrix[i, j] - m2.matrix[i, j];
                }
            }
            return new MatrixMath(subMatrix);
        }

        public static MatrixMath operator *(MatrixMath m1, MatrixMath m2)
        {
            double[,] multipliedMatrix = new double[m1.nRow, m2.nCol];
            double multiplication = 0;
            for (int i = 0; i < m1.nRow; i++)
            {
                for (int k = 0; k < m2.nCol; k++)
                {
                    for (int j = 0; j < m1.nCol; j++)
                    {
                        multiplication += m1.matrix[i, j] * m2.matrix[j, k];
                    }
                    multipliedMatrix[i, k] = multiplication;
                    multiplication = 0;
                }
            }
            return new MatrixMath(multipliedMatrix);
        }

        public override string ToString()
        {
            string row = "";

            for (int i = 0; i < nRow; i++)
            {
                for (int j = 0; j < nCol; j++)
                {
                    if (j == 0)
                    {
                        row += String.Format("[{0} ", _matrix[i, j]);
                    }
                    else if (j == (_matrix.GetLength(1) - 1))
                    {
                        row += String.Format("{0}]\n", _matrix[i, j]);
                    }
                    else
                    {
                        row += String.Format("{0} ", _matrix[i, j]);
                    }
                }
            }
            return row;
        }

        public static MatrixMath I(int dim)
        {
            double[,] m = new double[dim, dim];
            for (int i = 0; i < dim; i++)
            {
                for (int j = 0; j < dim; j++)
                {
                    if (i == j)
                    {
                        m[i, j] = 1;
                    }
                    else
                    {
                        m[i, j] = 0;
                    }
                }
            }
            return new MatrixMath(m);
        }

        public static MatrixMath Transpose(MatrixMath m)
        {
            double[,] newMatrix = new double[m.matrix.GetLength(0), m.matrix.GetLength(1)];

            for (int i = 0; i < m.matrix.GetLength(0); i++)
            {
                for (int j = 0; j < m.matrix.GetLength(1); j++)
                {
                    newMatrix[j, i] = m.matrix[i, j];
                }
            }
            return new MatrixMath(newMatrix);
        }

        public static MatrixMath AddMatrixRight(MatrixMath original, MatrixMath addMatrix)
        {

            // Adds a new matrix to the right of an existing 2D Array (Matrix)
            // Have to specify 1D array to be added as the new matrix
            // Can only do fixed column addition

            int oriRow, oriCol, addRow, addCol;

            oriRow = original.nRow; // original number of rows
            oriCol = original.nCol; // original number of columns
            addRow = addMatrix.nRow; // number of rows of the second matrix
            addCol = addMatrix.nCol; // number of columns of the second matrix

            double[,] newMatrix = new double[oriRow, oriCol + addCol];

            for (int i = 0; i < (oriRow); i++)
            {
                for (int j = 0; j < (oriCol + addCol); j++)
                {
                    if (j < oriCol)
                    {
                        newMatrix[i, j] = original.matrix[i, j];
                    }
                    else
                    {
                        newMatrix[i, j] = addMatrix.matrix[i, j - addCol];
                    }


                }

            }

            return new MatrixMath(newMatrix);

        }

        public static MatrixMath ExtractMatrixFromEnd(MatrixMath m, int dim)
        {
            double[,] matrix = new double[m.nRow, m.nCol - dim];
            for (int i = 0; i < m.nRow; i++)
            {

                for (int j = m.nCol - dim; j < m.nCol; j++)
                {

                    matrix[i, j - (m.nCol - dim)] = m.matrix[i, j];

                }
            }
            return new MatrixMath(matrix);
        }

        public static double CalculateDeterminant(MatrixMath m)
        {
            double[,] matrix = m.matrix;

            for (int k = 0; k < matrix.GetLength(0); k++) // This index keeps zeroeing everything related to this row.
            {
                if (k < matrix.GetLength(0) - 1)
                {
                    for (int i = 0; i < matrix.GetLength(0); i++)
                    {
                        if (i == k)
                        {
                        }
                        else if (i > k)
                        {
                            double n1 = matrix[i, k] / matrix[k, k];
                            for (int j = 0; j < matrix.GetLength(1); j++)
                            {
                                matrix[i, j] = matrix[i, j] - n1 * matrix[k, j];
                            }
                        }
                        else
                        {
                            //do nothing if we are analyzing a row that has already been normalized
                        }
                    }
                }
                else // i.e., for the last row just divide the last element by itself to make it 1.
                {
                }
            }

            double determinant = matrix[0, 0];
            for (int i = 1; i < matrix.GetLength(0); i++)
            {
                determinant *= matrix[i, i];
            }

            return determinant;
        }

        public static void RotateVectorIn3D(ref MatrixMath vec, double angleX, double angleY, double angleZ)
        {
            double angleXRad = angleX * Math.PI / 180;
            double angleYRad = angleY * Math.PI / 180;
            double angleZRad = angleZ * Math.PI / 180;

            double cosx = Math.Cos(angleXRad);
            double sinx = Math.Sin(angleXRad);
            double cosy = Math.Cos(angleYRad);
            double siny = Math.Sin(angleYRad);
            double cosz = Math.Cos(angleZRad);
            double sinz = Math.Sin(angleZRad);
            double[,] rotX = new double[3, 3];
            double[,] rotY = new double[3, 3];
            double[,] rotZ = new double[3, 3];
            rotX[0, 0] = 1;
            rotX[1, 1] = cosx;
            rotX[1, 2] = -sinx;
            rotX[2, 1] = sinx;
            rotX[2, 2] = cosx;
            rotY[0, 0] = cosy;
            rotY[0, 2] = siny;
            rotY[1, 1] = 1;
            rotY[2, 0] = -siny;
            rotY[2, 2] = cosy;
            rotZ[0, 0] = cosz;
            rotZ[0, 1] = -sinz;
            rotZ[1, 0] = sinz;
            rotZ[1, 1] = cosz;
            rotZ[2, 2] = 1;

            MatrixMath rotXMat = new MatrixMath(rotX);
            MatrixMath rotYMat = new MatrixMath(rotY);
            MatrixMath rotZMat = new MatrixMath(rotZ);

            vec = rotZMat * rotYMat * rotXMat;
        }

        public double[,] matrix
        {
            get
            {
                return _matrix;
            }
        }

        public int nRow
        {
            get
            {
                return _matrix.GetLength(0);
            }
        }

        public int nCol
        {
            get
            {
                return _matrix.GetLength(1);
            }
        }
    }
}
