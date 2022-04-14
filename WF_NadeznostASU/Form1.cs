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
                var t = new MyCheckBox {
                    Text = item.name,
                    index = elements.Count - i,
                    TabStop = true,
                    TabIndex = elements.Count - i,
                };
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
                    IsSplitterFixed = true,
                    TabStop = false,
                    TabIndex = elements.Count - chkBx.index - 1,
                };
                split.Panel1.Controls.Add(new Label
                {
                    Text = elements[chkBx.index].name,
                    Dock = DockStyle.Fill,
                    AutoEllipsis = true,
                    
                });
                var upDown = new MyNumericUpDown { 
                    index = chkBx.index, 
                    TabStop = true, 
                };
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

        void getT3Data(out double[] lambdas, out uint[] layers, out int[] indices)
        {
            List<int> indicesL = new List<int> { 1, 2, 3, 4 };
            List<double> lambdasL = new List<double>
            {
                (double)nudT3L1.Value,
                (double)nudT3L2.Value,
                (double)nudT3L3.Value,
                (double)nudT3L4.Value
            };
            List<uint> layersL = new List<uint>
            {
                (uint)nudT3N1.Value,
                (uint)nudT3N2.Value,
                (uint)nudT3N3.Value,
                (uint)nudT3N4.Value
            };
            for (int i = 0; i < layersL.Count; i++)
            {
                if (layersL[i] == 0)
                {
                    layersL.RemoveAt(i);
                    lambdasL.RemoveAt(i);
                    indicesL.RemoveAt(i);
                    i--;
                }
            }
            indices = indicesL.ToArray();
            lambdas = lambdasL.ToArray();
            layers = layersL.ToArray();
        }

        void updateTask3()
        {
            double[] lambdas;
            uint[] layers;
            int[] indices;
            getT3Data(out lambdas, out layers, out indices);
            if (lambdas.Length > 0)
            {
                const double T = 100;
                tbT3Tcp.Text = Task3.CalcT(lambdas, layers).ToString();
                tbT3Pc.Text = Task3.CalcPc(lambdas, layers, T).ToString();

            }
            else
            {
                tbT3Tcp.Text = "";
                tbT3Pc.Text = "";
            }
            var bmp = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            var g = Graphics.FromImage(bmp);
            drawT3Diagram(g, layers, indices);
            pictureBox1.CreateGraphics().DrawImage(bmp, 0, 0);
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
                
                nudT3L1.ValueChanged -= onTask3InputUpdate;
                nudT3L2.ValueChanged -= onTask3InputUpdate;
                nudT3L3.ValueChanged -= onTask3InputUpdate;
                nudT3L4.ValueChanged -= onTask3InputUpdate;

                nudT3N1.ValueChanged -= onTask3InputUpdate;
                nudT3N2.ValueChanged -= onTask3InputUpdate;
                nudT3N3.ValueChanged -= onTask3InputUpdate;
                nudT3N4.ValueChanged -= onTask3InputUpdate;


                nudT3L1.Value = (decimal)lambdas[0];
                nudT3L2.Value = (decimal)lambdas[1];
                nudT3L3.Value = (decimal)lambdas[2];
                nudT3L4.Value = (decimal)lambdas[3];

                nudT3N1.Value = (decimal)layers[0];
                nudT3N2.Value = (decimal)layers[1];
                nudT3N3.Value = (decimal)layers[2];
                nudT3N4.Value = (decimal)layers[3];


                nudT3L1.ValueChanged += onTask3InputUpdate;
                nudT3L2.ValueChanged += onTask3InputUpdate;
                nudT3L3.ValueChanged += onTask3InputUpdate;
                nudT3L4.ValueChanged += onTask3InputUpdate;

                nudT3N1.ValueChanged += onTask3InputUpdate;
                nudT3N2.ValueChanged += onTask3InputUpdate;
                nudT3N3.ValueChanged += onTask3InputUpdate;
                nudT3N4.ValueChanged += onTask3InputUpdate;

                updateTask3();
            }
        }

        void drawT3Diagram(Graphics g, in uint[] layers, in int[] indices)
        {
            g.Clear(Color.White);
            if (layers.Length > 0)
                Task3.DrawLayers(g, new Point(pictureBox1.Width / 2, pictureBox1.Height / 2), layers, indices);
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            double[] _;
            uint[] layers;
            int[] indices;
            getT3Data(out _, out layers, out indices);
            var bmp = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            var g = Graphics.FromImage(bmp);
            drawT3Diagram(g, layers, indices);
            e.Graphics.DrawImage(bmp, 0, 0);
        }
    }
}