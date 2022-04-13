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
            int i = 1;
            foreach (var item in elements.Reverse<Element>())
            {
                var t = new MyCheckBox { Text = item.name, index = elements.Count - i };
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
                split.Panel1.Controls.Add(new Label
                {
                    Text = elements[chkBx.index].name,
                    Dock = DockStyle.Fill,
                    AutoEllipsis = true
                });
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

        void updateTask2()
        {
            var Nz = (double)nudNz.Value;
            //var t = (double) nudT.Value; // this value is not used but still provided??
            var deltaT = (double)nudDeltaT.Value;
            var nt = (double)nudNt.Value;
            var nDeltaT = (double)nudNDeltaT.Value;

            const double EPS = 1e-7;
            if (Nz < EPS || deltaT < EPS || nt + nDeltaT < EPS || nt + nDeltaT > Nz)
            {
                tbPt.Text = "";
                tbPtDeltaT.Text = "";
                tbAt.Text = "";
                tbLambdaT.Text = "";
                return;
            }

            var Pt = (Nz - nt) / Nz;
            var PtDeltaT = (Nz - (nt + nDeltaT)) / Nz;

            var Nm = (nt + nDeltaT) / 2;
            //var nm = Nz - Nm; // this value is not used as well

            var aT = nDeltaT / (Nz * deltaT);
            var lambdaT = nDeltaT / (Nm * deltaT);

            tbPt.Text = Pt.ToString();
            tbPtDeltaT.Text = PtDeltaT.ToString();
            tbAt.Text = aT.ToString();
            tbLambdaT.Text = lambdaT.ToString();

        }

        void onInputUpdate(object sender, EventArgs e)
        {
            updateTask2();
        }

        void bClear2_Click(object sender, EventArgs e)
        {
            var gbs = tabPage2.Controls.OfType<GroupBox>();
            var nuds = gbs.SelectMany(gb => gb.Controls.OfType<NumericUpDown>());

            foreach (var nud in nuds)
            {
                nud.Value = nud.Minimum;
            }
        }

        void updateTask3()
        {
            List<double> lambdas = new List<double>
            {
                (double)nudT3L1.Value,
                (double)nudT3L2.Value,
                (double)nudT3L3.Value,
                (double)nudT3L4.Value
            };
            List<uint> layers = new List<uint>
            {
                (uint)nudT3N1.Value,
                (uint)nudT3N2.Value,
                (uint)nudT3N3.Value,
                (uint)nudT3N4.Value
            };
            for (int i = 0; i < layers.Count; i++)
            {
                if (layers[i] == 0)
                {
                    layers.RemoveAt(i);
                    lambdas.RemoveAt(i);
                    i--;
                }
            }
            if (lambdas.Count > 0)
            {
                const double T = 100;
                tbT3Tcp.Text = Task3.CalcT(lambdas.ToArray(), layers.ToArray()).ToString();
                tbT3Pc.Text = Task3.CalcPc(lambdas.ToArray(), layers.ToArray(), T).ToString();
            }
            else
            {
                tbT3Tcp.Text = "";
                tbT3Pc.Text = "";
            }
        }

        void onTask3InputUpdate(object sender, EventArgs e)
        {
            updateTask3();
        }

        private void bT3Gen_Click(object sender, EventArgs e)
        {
            var gen = new Task3DataGen();
            gen.ShowDialog();
            if (gen.DialogResult == DialogResult.OK)
            {
                double[] lambdas;
                uint[]   layers;
                Task3.GenData((uint)gen.nudT3GenN1.Value, (uint)gen.nudT3GenN2.Value, out lambdas, out layers);
                nudT3L1.Value = (decimal)lambdas[0];
                nudT3L2.Value = (decimal)lambdas[1];
                nudT3L3.Value = (decimal)lambdas[2];
                nudT3L4.Value = (decimal)lambdas[3];

                nudT3N1.Value = (decimal)layers[0];
                nudT3N2.Value = (decimal)layers[1];
                nudT3N3.Value = (decimal)layers[2];
                nudT3N4.Value = (decimal)layers[3];
            }
        }
    }
}