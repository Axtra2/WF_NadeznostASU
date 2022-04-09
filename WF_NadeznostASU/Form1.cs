namespace WF_NadeznostASU
{
    public partial class Form1 : Form
    {
        List<Element> elements = Element.Parse("Data/everything.txt");
        List<int> selected = new List<int>();

        const int labelH = 30;
        const int splitterDistance = 130;
        double lambdaC = 0;

        public Form1()
        {
            InitializeComponent();
            nudTime.ValueChanged += onTimeUpdate;
            addCheckBoxes();
        }

        void addCheckBoxes()
        {
            int i = 0;
            foreach (var item in elements)
            {
                var t = new MyCheckBox { Text = item.name, index = i };
                t.CheckedChanged += onCheckBoxUpdate;
                panel2.Controls.Add(t);
                ++i;
            }
        }
        double calcPc(double l, double t)
        {
            return Math.Exp(-l * t);
        }

        double calcTc(double l)
        {
            return 1 / l;
        }

        void update()
        {
            if (selected.Count == 0)
            {
                tbPc.Text = "";
                tbTc.Text = "";
            }
            else
            {
                tbPc.Text = calcPc(lambdaC, (double)nudTime.Value).ToString();
                tbTc.Text = calcTc(lambdaC).ToString();
            }
        }

        void update(int i, int newQty)
        {
            lambdaC += (newQty - elements[i].qty) * elements[i].value;
            elements[i].qty = newQty;
            update();
        }

        void onQtyUpdate(object sender, EventArgs e)
        {
            var upDown = sender as MyNumericUpDown;
            if (upDown == null) return;
            update(upDown.index, (int)upDown.Value);
        }

        void onTimeUpdate(object sender, EventArgs e)
        {
            update();
        }

        void onCheckBoxUpdate(object sender, EventArgs e)
        {
            var chkBx = sender as MyCheckBox;
            if (chkBx == null) return;
            if (chkBx.Checked)
            {
                selected.Add(chkBx.index);

                var split = new SplitContainer
                {
                    Dock = DockStyle.Top,
                    Height = labelH,
                    SplitterDistance = splitterDistance,
                    IsSplitterFixed = true
                };
                split.Panel1.Controls.Add(new Label{ Text = elements[chkBx.index].name, Dock = DockStyle.Fill });
                var upDown = new MyNumericUpDown { index = chkBx.index };
                upDown.ValueChanged += onQtyUpdate;
                split.Panel2.Controls.Add(upDown);
                pQty.Controls.Add(split);


                update(chkBx.index, 1);
            }
            else
            {
                var i = selected.FindIndex(x => x == chkBx.index);
                selected.RemoveAt(i);
                pQty.Controls.RemoveAt(i);

                update(chkBx.index, 0);
            }
        }

        void bClear_Click(object sender, EventArgs e)
        {
            foreach (var item in panel2.Controls)
            {
                var t = item as MyCheckBox;
                t.CheckedChanged -= onCheckBoxUpdate;
                t.Checked = false;
                t.CheckedChanged += onCheckBoxUpdate;
            }
            foreach (var item in elements)
            {
                item.qty = 0;
            }
            pQty.Controls.Clear();
            //pQty.Invalidate(); // sometimes scrollbar doesn't disappear properly, so forced update might help
            selected.Clear();
            lambdaC = 0;
            update();
        }

        private void Form1_ResizeBegin(object sender, EventArgs e)
        {
            this.SuspendLayout();
        }

        private void Form1_ResizeEnd(object sender, EventArgs e)
        {
            this.ResumeLayout();
        }
    }
}