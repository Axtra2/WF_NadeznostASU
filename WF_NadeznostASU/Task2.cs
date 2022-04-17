namespace WF_NadeznostASU
{
    [System.Runtime.Versioning.SupportedOSPlatform("windows")]
    internal class Task2
    {
        public static decimal CalcPt(double N0, double nt)
        {
            return (decimal)((N0 - nt) / N0);
        }

        public static decimal CalcPtDeltaT(double N0, double nt, double nDeltaT)
        {
            return (decimal)((N0 - (nt + nDeltaT)) / N0);
        }

        public static decimal CalcAt(double N0, double deltaT, double nDeltaT)
        {
            return (decimal)(nDeltaT / (N0 * deltaT));
        }

        public static decimal CalcLambdat(double nt, double deltaT, double nDeltaT)
        {
            var Nm = (nt + nDeltaT) / 2;
            return (decimal)(nDeltaT / (Nm * deltaT));
        }
    }
}
