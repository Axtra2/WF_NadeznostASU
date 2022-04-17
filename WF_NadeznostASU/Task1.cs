namespace WF_NadeznostASU
{
    internal class Task1
    {
        public static double CalcPc(double l, double t)
        {
            return Math.Exp(-l * t);
        }

        public static double CalcTc(double l)
        {
            return 1 / l;
        }
    }
}
