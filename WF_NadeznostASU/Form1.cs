using System.Runtime.InteropServices;

namespace WF_NadeznostASU
{
    public partial class Form1 : Form
    {
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = false)]
        static extern IntPtr SendMessage(HandleRef hWnd, Int32 Msg, IntPtr wParam, IntPtr lParam);

        static void EnableRepaint(HandleRef handle, bool enable)
        {
            const int WM_SETREDRAW = 0x000B;
            SendMessage(handle, WM_SETREDRAW, new IntPtr(enable ? 1 : 0), IntPtr.Zero);
        }

        public Form1()
        {
            InitializeComponent();
            AddCheckBoxes();
        }

        private void Form1_ResizeBegin(object sender, EventArgs e)
        {
            SuspendLayout();
        }

        private void Form1_ResizeEnd(object sender, EventArgs e)
        {
            ResumeLayout();
        }


        #region Task 1 Implementaion

        const int LABEL_H = 30;
        const int SPLITTER_DISTANCE = 130;

        readonly List<Element> elements = Element.Parse("Data/everything.txt");
        double lambdaC = 0;

        void AddCheckBoxes()
        {
            int i = 1;
            foreach (var item in elements.Reverse<Element>())
            {
                var chkBx = new MyCheckBox
                {
                    Text = item.name,
                    index = elements.Count - i,
                    TabStop = true,
                    TabIndex = elements.Count - i,
                };
                chkBx.CheckedChanged += OnCheckBoxUpdate;
                pChkBxs.Controls.Add(chkBx);
                ++i;
            }
        }

        void UpdateTask1()
        {
            if (pQty.Controls.Count == 0)
            {
                tbPc.Text = "";
                tbTc.Text = "";
            }
            else
            {
                tbPc.Text = Task1.CalcPc(lambdaC, (double)nudTime.Value).ToString();
                tbTc.Text = Task1.CalcTc(lambdaC).ToString();
            }
        }

        void UpdateTask1Values(int index, int newQty)
        {
            lambdaC += (newQty - elements[index].qty) * elements[index].value;
            elements[index].qty = newQty;
            UpdateTask1();
        }

        void OnQtyUpdate(object sender, EventArgs e)
        {
            var upDown = sender as MyNumericUpDown;
            if (upDown == null) return;
            UpdateTask1Values(upDown.index, (int)upDown.Value);
        }

        void OnTimeUpdate(object sender, EventArgs e)
        {
            UpdateTask1();
        }

        static int BinarySearch(int l, int r, Func<int, bool> check)
        {
            while (true)
            {
                if (l >= r) break;
                int m = (l + r) / 2;
                if (check(m))
                {
                    r = m;
                }
                else
                {
                    l = m + 1;
                }
            }
            return r;
        }

        void OnCheckBoxUpdate(object sender, EventArgs e)
        {
            var gh = new HandleRef(pQty, pQty.Handle);
            EnableRepaint(gh, false);

            var chkBx = sender as MyCheckBox;
            if (chkBx == null) return;
            if (chkBx.Checked)
            {
                var split = new SplitContainer
                {
                    Dock = DockStyle.Top,
                    Height = LABEL_H,
                    SplitterDistance = SPLITTER_DISTANCE,
                    IsSplitterFixed = true,
                    TabStop = false,
                    TabIndex = pQty.Controls.Count,
                };

                var label = new Label
                {
                    Text = elements[chkBx.index].name,
                    Dock = DockStyle.Fill,
                    AutoEllipsis = true,
                };

                split.Panel1.Controls.Add(label);

                var upDown = new MyNumericUpDown
                {
                    index = chkBx.index,
                    TabStop = true,
                };
                upDown.ValueChanged += OnQtyUpdate;

                split.Panel2.Controls.Add(upDown);

                pQty.Controls.Add(split);

                int i = BinarySearch(
                    0, pQty.Controls.Count - 1,
                    i => ((pQty.Controls[i] as SplitContainer).Panel2.Controls[0] as MyNumericUpDown).index <= upDown.index
                );
                pQty.Controls.SetChildIndex(split, i);

                UpdateTask1Values(chkBx.index, 1);
            }
            else
            {
                int i = BinarySearch(
                    0, pQty.Controls.Count - 1,
                    i => ((pQty.Controls[i] as SplitContainer).Panel2.Controls[0] as MyNumericUpDown).index <= chkBx.index
                );
                pQty.Controls.RemoveAt(i);

                UpdateTask1Values(chkBx.index, 0);
            }

            EnableRepaint(gh, true);
            pQty.Invalidate(true);
        }

        void bClear_Click(object sender, EventArgs e)
        {
            foreach (var item in pChkBxs.Controls)
            {
                var t = item as MyCheckBox;
                t.CheckedChanged -= OnCheckBoxUpdate;
                t.Checked = false;
                t.CheckedChanged += OnCheckBoxUpdate;
            }
            foreach (var item in elements)
            {
                item.qty = 0;
            }
            pQty.Controls.Clear();
            //pQty.Invalidate(); // sometimes scrollbar doesn't disappear properly, so forced update might help
            lambdaC = 0;
            UpdateTask1();
        }

        #endregion

        #region Task 2 Implementaion

        void UpdateTask2()
        {
            var N0 = (double)nudN0.Value;
            //var t = (double) nudT.Value; // this value is not used
            var deltaT = (double)nudDeltaT.Value;
            var nt = (double)nudNt.Value;
            var nDeltaT = (double)nudNDeltaT.Value;

            const double EPS = 1e-7;
            if (N0 < EPS || deltaT < EPS || nt + nDeltaT < EPS || nt + nDeltaT > N0)
            {
                tbPt.Text = "";
                tbPtDeltaT.Text = "";
                tbAt.Text = "";
                tbLambdaT.Text = "";
                return;
            }

            tbPt.Text = Task2.CalcPt(N0, nt).ToString();
            tbPtDeltaT.Text = Task2.CalcPtDeltaT(N0, nt, nDeltaT).ToString();
            tbAt.Text = Task2.CalcAt(N0, deltaT, nDeltaT).ToString();
            tbLambdaT.Text = Task2.CalcLambdat(nt, deltaT, nDeltaT).ToString();
        }

        void OnInputUpdate(object sender, EventArgs e)
        {
            UpdateTask2();
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

        #endregion

        #region Task 3 Implementaion

        void GetT3Data(out double[] lambdas, out uint[] layers, out int[] indices)
        {
            var indicesL = new List<int> { 1, 2, 3, 4 };
            var lambdasL = new List<double>
            {
                (double)nudT3L1.Value,
                (double)nudT3L2.Value,
                (double)nudT3L3.Value,
                (double)nudT3L4.Value
            };
            var layersL = new List<uint>
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

        void UpdateTask3()
        {
            double[] lambdas;
            uint[] layers;
            int[] indices;
            GetT3Data(out lambdas, out layers, out indices);
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
            using var bmp = new Bitmap(pbDiagram.Width, pbDiagram.Height);
            using var g = Graphics.FromImage(bmp);
            DrawT3Diagram(g, layers, indices);
            pbDiagram.CreateGraphics().DrawImage(bmp, 0, 0);
        }

        void OnTask3InputUpdate(object sender, EventArgs e)
        {
            UpdateTask3();
        }

        private void bT3Gen_Click(object sender, EventArgs e)
        {
            var gen = new Task3DataGen();
            gen.ShowDialog();
            if (gen.DialogResult == DialogResult.OK)
            {
                double[] lambdas;
                uint[] layers;
                Task3.GenData((uint)gen.nudT3GenN1.Value, (uint)gen.nudT3GenN2.Value, out lambdas, out layers);

                nudT3L1.ValueChanged -= OnTask3InputUpdate;
                nudT3L2.ValueChanged -= OnTask3InputUpdate;
                nudT3L3.ValueChanged -= OnTask3InputUpdate;
                nudT3L4.ValueChanged -= OnTask3InputUpdate;

                nudT3N1.ValueChanged -= OnTask3InputUpdate;
                nudT3N2.ValueChanged -= OnTask3InputUpdate;
                nudT3N3.ValueChanged -= OnTask3InputUpdate;
                nudT3N4.ValueChanged -= OnTask3InputUpdate;


                nudT3L1.Value = (decimal)lambdas[0];
                nudT3L2.Value = (decimal)lambdas[1];
                nudT3L3.Value = (decimal)lambdas[2];
                nudT3L4.Value = (decimal)lambdas[3];

                nudT3N1.Value = layers[0];
                nudT3N2.Value = layers[1];
                nudT3N3.Value = layers[2];
                nudT3N4.Value = layers[3];


                nudT3L1.ValueChanged += OnTask3InputUpdate;
                nudT3L2.ValueChanged += OnTask3InputUpdate;
                nudT3L3.ValueChanged += OnTask3InputUpdate;
                nudT3L4.ValueChanged += OnTask3InputUpdate;

                nudT3N1.ValueChanged += OnTask3InputUpdate;
                nudT3N2.ValueChanged += OnTask3InputUpdate;
                nudT3N3.ValueChanged += OnTask3InputUpdate;
                nudT3N4.ValueChanged += OnTask3InputUpdate;

                UpdateTask3();
            }
        }

        void DrawT3Diagram(Graphics g, in uint[] layers, in int[] indices)
        {
            g.Clear(Color.White);
            if (layers.Length > 0)
            {
                Task3.DrawLayers(g, new Point(pbDiagram.Width / 2, pbDiagram.Height / 2), layers, indices);
            }
        }

        private void pbDiagram_Paint(object sender, PaintEventArgs e)
        {
            uint[] layers;
            int[] indices;
            GetT3Data(out _, out layers, out indices);
            using var bmp = new Bitmap(pbDiagram.Width, pbDiagram.Height);
            using var g = Graphics.FromImage(bmp);
            DrawT3Diagram(g, layers, indices);
            e.Graphics.DrawImage(bmp, 0, 0);
        }

        #endregion
    }
}