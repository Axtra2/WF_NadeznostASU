namespace WF_NadeznostASU
{
    internal class Task1
    {
        public static decimal CalcPc(double l, double t)
        {
            return (decimal)(Math.Exp(-l * t));
        }

        public static decimal CalcTc(double l)
        {
            return (decimal)(1 / l);
        }
    }
}
