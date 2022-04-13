namespace WF_NadeznostASU
{
    partial class Task3DataGen
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.groupBox13 = new System.Windows.Forms.GroupBox();
            this.nudT3GenN1 = new System.Windows.Forms.NumericUpDown();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.nudT3GenN2 = new System.Windows.Forms.NumericUpDown();
            this.bOk = new System.Windows.Forms.Button();
            this.groupBox13.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudT3GenN1)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudT3GenN2)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox13
            // 
            this.groupBox13.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox13.Controls.Add(this.nudT3GenN1);
            this.groupBox13.Location = new System.Drawing.Point(12, 12);
            this.groupBox13.Name = "groupBox13";
            this.groupBox13.Size = new System.Drawing.Size(290, 60);
            this.groupBox13.TabIndex = 0;
            this.groupBox13.TabStop = false;
            this.groupBox13.Text = "n_1 (последняя цифра шифра)";
            // 
            // nudT3GenN1
            // 
            this.nudT3GenN1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.nudT3GenN1.Location = new System.Drawing.Point(3, 19);
            this.nudT3GenN1.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.nudT3GenN1.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudT3GenN1.Name = "nudT3GenN1";
            this.nudT3GenN1.Size = new System.Drawing.Size(284, 23);
            this.nudT3GenN1.TabIndex = 0;
            this.nudT3GenN1.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.nudT3GenN2);
            this.groupBox1.Location = new System.Drawing.Point(12, 78);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(290, 60);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "n_2 (предпоследняя цифра шифра)";
            // 
            // nudT3GenN2
            // 
            this.nudT3GenN2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.nudT3GenN2.Location = new System.Drawing.Point(3, 19);
            this.nudT3GenN2.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.nudT3GenN2.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudT3GenN2.Name = "nudT3GenN2";
            this.nudT3GenN2.Size = new System.Drawing.Size(284, 23);
            this.nudT3GenN2.TabIndex = 0;
            this.nudT3GenN2.Tag = "";
            this.nudT3GenN2.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // bOk
            // 
            this.bOk.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.bOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.bOk.Location = new System.Drawing.Point(12, 149);
            this.bOk.Name = "bOk";
            this.bOk.Size = new System.Drawing.Size(290, 29);
            this.bOk.TabIndex = 2;
            this.bOk.Text = "OK";
            this.bOk.UseVisualStyleBackColor = true;
            this.bOk.Click += new System.EventHandler(this.bOk_Click);
            // 
            // Task3DataGen
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.ClientSize = new System.Drawing.Size(314, 190);
            this.Controls.Add(this.bOk);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBox13);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Task3DataGen";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Сгенерировать значения";
            this.groupBox13.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.nudT3GenN1)).EndInit();
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.nudT3GenN2)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private Button bOk;
        private GroupBox groupBox13;
        public NumericUpDown nudT3GenN1;
        private GroupBox groupBox1;
        public NumericUpDown nudT3GenN2;
    }
}