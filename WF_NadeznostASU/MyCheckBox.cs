using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WF_NadeznostASU
{
    internal class MyCheckBox : CheckBox
    {
        public int index = 0;
        public MyCheckBox()
        {
            Dock = DockStyle.Top;
        }
    }
}
