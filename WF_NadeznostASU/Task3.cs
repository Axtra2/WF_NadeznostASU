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

        private static double P(double lambda, double t)
        {
            return Math.Exp(-1 * lambda * t);
        }

        private static double Pl(double lambda, double t, double layer)
        {
            return 1 - Math.Pow(1 - P(lambda, t), layer);
        }

        public static double CalcPc(in double[] lambdas, in uint[] layers, double t)
        {
            int l = layers.Length;
            if (lambdas.Length != l) throw new ArgumentException();
            if (l < 1) throw new ArgumentException();

            return lambdas
                .Zip(layers, (lambda, layer) => Pl(lambda, t, layer))
                .Aggregate(1.0, (a, b) => a * b);
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

        const int GS = 10;
        const int Y_OFFSET = GS * 4;
        const int CONNECT_W = GS;
        static Size R_SIZE = new Size(GS*4, GS*2);
        static Pen PEN = new Pen(Color.Black);
        static Font FONT = new Font("Consolas", 10);
        static Brush BRUSH = new SolidBrush(PEN.Color);

        static string subscript(int number)
        {
            var symbols = "₀₁₂₃₄₅₆₇₈₉";
            var charArray = number
                .ToString()
                .Select(ch => symbols[ch - '0']);
            return string.Join("", charArray);
        }

        static void DrawElement(Graphics g, Point p, int index)
        {
            var leftLineEnd = new Point(p.X + CONNECT_W, p.Y);
            g.DrawLine(PEN, p, leftLineEnd);

            var topLeft = new Point(leftLineEnd.X, leftLineEnd.Y - R_SIZE.Height / 2);
            var r = new Rectangle(topLeft, R_SIZE);
            g.DrawRectangle(PEN, r);

            var format = new StringFormat
            {
                LineAlignment = StringAlignment.Center,
                Alignment = StringAlignment.Center
            };
            g.DrawString("λ" + subscript(index), FONT, BRUSH, r, format);


            var rightLineStart = new Point(leftLineEnd.X + R_SIZE.Width, leftLineEnd.Y);
            var rightLineEnd = new Point(rightLineStart.X + CONNECT_W, rightLineStart.Y);
            g.DrawLine(PEN, rightLineStart, rightLineEnd);
        }

        static void DrawElements(Graphics g, Point p, int n, int index)
        {
            int halfH = Y_OFFSET * (n - 1) / 2;
            
            p.Offset(R_SIZE.Width + 2 * CONNECT_W, 0);
            g.DrawLine(PEN, p, new Point(p.X, p.Y - halfH));
            g.DrawLine(PEN, p, new Point(p.X, p.Y + halfH));

            p.Offset(-R_SIZE.Width - 2 * CONNECT_W, 0);
            g.DrawLine(PEN, p, new Point(p.X, p.Y - halfH));
            g.DrawLine(PEN, p, new Point(p.X, p.Y + halfH));

            p.Y -= halfH;
            for (int i = 0; i < n; i++)
            {
                DrawElement(g, p, index);
                p.Y += Y_OFFSET;
            }
        }

        static int calcDiagramWidth(in uint[] layers)
        {
            return layers.Length * (3 * CONNECT_W + R_SIZE.Width) + CONNECT_W;
        }

        public static void DrawLayers(Graphics g, Point center, in uint[] layers, in int[] indices)
        {
            Point p = new Point(center.X - calcDiagramWidth(layers) / 2, center.Y);
            for (int i = 0; i < layers.Length; i++)
            {
                g.DrawLine(PEN, p.X, p.Y, p.X + CONNECT_W, p.Y);
                p.X += CONNECT_W;
                DrawElements(g, p, (int)layers[i], indices[i]);
                p.X += 2 * CONNECT_W + R_SIZE.Width;
            }
            g.DrawLine(PEN, p.X, p.Y, p.X + CONNECT_W, p.Y);
        }
    }
}
