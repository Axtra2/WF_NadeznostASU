using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WF_NadeznostASU
{
    internal class Task3
    {
        static uint Product(uint start, uint end, uint res = 1)
        {
            res *= start;
            if (start >= end) return res;
            return Product(start + 1, end, res);
        }

        static uint C(uint n, uint k)
        {
            if (k == 0) return 1;
            return Product(n - k + 1, n) / Product(1, k);
        }

        static bool NextState(ref uint[] state, in uint[] layers)
        {
            for (int i = 0; i < state.Length; i++)
            {
                if (state[i]++ < layers[i]) return true;
                state[i] = 1;
            }
            return false;
        }

        public static void GenData(uint n1, uint n2, out double[] lambdas, out uint[] layers)
        {
            if (n1 < 1 || n1 > 10) throw new ArgumentException();
            if (n2 < 1 || n2 > 10) throw new ArgumentException();
            lambdas = new double[]
            {
                n1 * 1e-4,
                n2 * 1e-4,
                1e-3 / n1,
                1e-3 / n2
            };
            if (n1 % 2 == 0)
            {
                if (n2 % 2 == 0) layers = new uint[] { 3, 1, 1, 1 };
                else             layers = new uint[] { 3, 2, 1, 1 };
            }
            else
            {
                if (n2 % 2 == 0) layers = new uint[] { 2, 3, 2, 0 };
                else             layers = new uint[] { 3, 3, 1, 0 };
            }
        }

        public static double CalcT(in double[] lambdas, in uint[] layers)
        {
            int l = layers.Length;
            if (lambdas.Length != l) throw new ArgumentException();
            if (l < 1) throw new ArgumentException();
            double sum = 0.0;
            uint[] state = new uint[l];
            Array.Fill<uint>(state, 1);
            do
            {
                long numerator = 1;
                for (int i = 0; i < l; i++)
                {
                    numerator *= C(layers[i], state[i]);
                    if (state[i] % 2 == 0) numerator *= -1;
                }
                double denominator = 0.0;
                for (int i = 0; i < l; i++)
                {
                    denominator += state[i] * lambdas[i];
                }
                sum += numerator / denominator;
            } while (NextState(ref state, layers));
            return sum;
        }
    }
}
