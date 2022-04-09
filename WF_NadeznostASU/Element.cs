using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WF_NadeznostASU
{
    internal class Element
    {
        public string name = "";
        public double value = 0.0;
        public int id = 0;
        public int qty = 0;

        public static List<Element> Parse(string fileName, double m = 1e-6)
        {
            var elements = new List<Element>();
            var sr = new StreamReader(fileName);
            int i = 0;
            foreach (var s in sr.ReadToEnd().Split('\n'))
            {
                var a = s.Split(';');
                var e = new Element
                {
                    name = a[0],
                    value = m * Double.Parse(a[a.Length - 1]),
                    id = i,
                    qty = 0
                };
                elements.Add(e);
            }
            return elements;
        }
    }
}
