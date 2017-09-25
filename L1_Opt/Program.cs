using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L1_Opt
{
    class Program
    {
        static Boolean CheckForOptimal(double[,] Simplex)
        {
            Boolean check = true;
            for (int i = 0; i < Simplex.GetLength(1); i++)
            { 
                if(Simplex[Simplex.GetLength(0) -1 , i] > 0)
                {
                    check = false;
                    break;
                }
            }
            return check;
        }
        static int FindResolvingColumn(double[,] Simplex)
        {
            int place = 0;
            for (int i = 1; i < Simplex.GetLength(1); i++)
            {
                if (Simplex[Simplex.GetLength(0)-1, i] > 0)
                {
                    place = i;
                    break;
                }
            }
            return place;
        }
        static int FindResolvingRow(int Column, double[,] Simplex)
        {
            double min = 15000; 
            int place = 0;
            for (int i = 0; i < Simplex.GetLength(0) - 1; i++)
            {
                if ((Simplex[i, 0] / Simplex[i, Column] < min) && (Simplex[i, 0] / Simplex[i, Column] >0) && (Simplex[i, 0]!= 0) && (Simplex[i, Column]!=0))
                {
                    min = Simplex[i, 0] / Simplex[i, Column];
                    place = i;
                }
            }
            return place;
        }
        static double[,] RecalculationOfTable(int r, int k, double[,] Simplex)
        {
            double[,] SimplexBuf = new double[4, 4];
            for (int i = 0; i < 4; i++)
                for (int j = 0; j < 4; j++)
                    SimplexBuf[i, j] = Simplex[i, j];
            SimplexBuf[r, k] = 1 / Simplex[r, k];
            for (int i = 0; i < Simplex.GetLength(1); i++)
            {
                if (i != k)
                {
                    SimplexBuf[r, i] = Simplex[r, i] / Simplex[r, k];
                }
            }
            for (int i = 1; i < Simplex.GetLength(0); i++)
            {
                if (i != r)
                {
                    SimplexBuf[i, k] = -Simplex[i, k] / Simplex[r, k];
                }
            }
            for (int i = 0; i < Simplex.GetLength(0); i++)
                for (int j = 0; j < Simplex.GetLength(1); j++)
                {
                    if( (i!= r)&& (j!= k))
                    {
                        SimplexBuf[i, j] = Simplex[i, j] - ((Simplex[i, k] * Simplex[r, j]) / Simplex[r, k]);
                    }
                }
             return SimplexBuf;
        }
        static void OutputData(double[] c, double[,] A, double[] b)
        {
            Console.WriteLine("F = {0}X1+{1}X2+{2}X3 --> max", c[0], c[1], c[2]);
            Console.WriteLine("{0}X1+{1}X2+{2}X3 <= {3} \n{4}X1+{5}X2+{6}X3 <= {7} \n{8}X1+{9}X2+{10}X3 <= {11}", A[0, 0], A[0, 1], A[0, 2], b[0], A[1, 0], A[1, 1], A[1, 2], b[1], A[2, 0], A[2, 1], A[2, 2], b[2]);
        }
        static double[,] MakeSimplexTable(double[] c, double[,] A, double[] b, int amount)
        {
            double[,] SimplexTable = new double[amount + 1, amount + 1];
            for (int i = 0; i < 3; i++)
            {
                SimplexTable[i, 0] = b[i];
            }
            for (int i = 0; i < 3; i++)
                for (int j = 1; j < 4; j++)
                {
                    SimplexTable[i, j] = A[i,j-1];
                }
            for (int i = 1; i < 4; i++)
            {
                SimplexTable[3, i] = c[i-1];
            }
            return SimplexTable;
        }
        static void Main(string[] args)
        {
            // Начальные условия
            int NumberOfVariables = 3;
            double[] c = new double[3] { 2, 8, 3 };
            double[,] A = new double[3, 3] { { 2, 1, 1 }, { 1, 2, 0 }, { 0, 0.5, 1 } };
            double[] b = new double[3] { 4, 6, 2 };
            OutputData(c, A, b);
            double[,] Simplex = MakeSimplexTable(c, A, b, NumberOfVariables);
            Boolean kk = CheckForOptimal(Simplex);
            int counter = 0;
            Console.WriteLine("");
            while (!kk)
            {
                counter++;
                int k = FindResolvingColumn(Simplex);
                int r = FindResolvingRow(k, Simplex);
                Simplex = RecalculationOfTable(r, k, Simplex);
                Console.WriteLine("Операция номер {0}", counter);
                for (int i = 0; i < 4; i++)
                {
                    for (int j = 0; j < 4; j++)
                        Console.Write(" {0} ", Simplex[i, j]);
                    Console.Write("\n");
                }
                Console.WriteLine("");
                kk = CheckForOptimal(Simplex);
            }
            double p = Simplex[Simplex.GetLength(0) - 1, 0];
            Console.WriteLine("Ответ : {0}", -p);
            Console.ReadLine();
        }
    }
}
