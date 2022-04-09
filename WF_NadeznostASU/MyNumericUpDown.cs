using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WF_NadeznostASU
{
    internal class MyNumericUpDown : NumericUpDown
    {
        public int index = 0;
        public MyNumericUpDown()
        {
            Dock = DockStyle.Fill;
            Maximum = 1000;
            Minimum = 1;
            //Width = 45;
        }
    }
}
