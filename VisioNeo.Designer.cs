namespace VisioNeo_App
{
    partial class VisioNeo
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(VisioNeo));
            panel1 = new Panel();
            MaximizeBTN = new PictureBox();
            CloseBTN = new PictureBox();
            MinimizeBTN = new PictureBox();
            pictureBox2 = new PictureBox();
            pictureBox1 = new PictureBox();
            VisualPB = new PictureBox();
            panel2 = new Panel();
            devListTBox = new ListBox();
            CnctBTN = new Button();
            LoadingPB = new PictureBox();
            searchBTN = new MaterialSkin.Controls.MaterialButton();
            panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)MaximizeBTN).BeginInit();
            ((System.ComponentModel.ISupportInitialize)CloseBTN).BeginInit();
            ((System.ComponentModel.ISupportInitialize)MinimizeBTN).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)VisualPB).BeginInit();
            panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)LoadingPB).BeginInit();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            panel1.BackColor = Color.Transparent;
            panel1.Controls.Add(MaximizeBTN);
            panel1.Controls.Add(CloseBTN);
            panel1.Controls.Add(MinimizeBTN);
            panel1.Controls.Add(pictureBox2);
            panel1.Controls.Add(pictureBox1);
            panel1.Location = new Point(-1, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(1649, 74);
            panel1.TabIndex = 0;
            // 
            // MaximizeBTN
            // 
            MaximizeBTN.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            MaximizeBTN.BackColor = Color.Transparent;
            MaximizeBTN.Image = (Image)resources.GetObject("MaximizeBTN.Image");
            MaximizeBTN.Location = new Point(1571, 22);
            MaximizeBTN.Name = "MaximizeBTN";
            MaximizeBTN.Size = new Size(25, 25);
            MaximizeBTN.SizeMode = PictureBoxSizeMode.StretchImage;
            MaximizeBTN.TabIndex = 6;
            MaximizeBTN.TabStop = false;
            MaximizeBTN.Click += MaximizeBTN_Click;
            // 
            // CloseBTN
            // 
            CloseBTN.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            CloseBTN.BackColor = Color.Transparent;
            CloseBTN.Image = (Image)resources.GetObject("CloseBTN.Image");
            CloseBTN.Location = new Point(1613, 22);
            CloseBTN.Name = "CloseBTN";
            CloseBTN.Size = new Size(25, 25);
            CloseBTN.SizeMode = PictureBoxSizeMode.StretchImage;
            CloseBTN.TabIndex = 3;
            CloseBTN.TabStop = false;
            CloseBTN.Click += CloseBTN_Click;
            // 
            // MinimizeBTN
            // 
            MinimizeBTN.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            MinimizeBTN.BackColor = Color.Transparent;
            MinimizeBTN.Image = (Image)resources.GetObject("MinimizeBTN.Image");
            MinimizeBTN.Location = new Point(1528, 22);
            MinimizeBTN.Name = "MinimizeBTN";
            MinimizeBTN.Size = new Size(25, 25);
            MinimizeBTN.SizeMode = PictureBoxSizeMode.StretchImage;
            MinimizeBTN.TabIndex = 5;
            MinimizeBTN.TabStop = false;
            MinimizeBTN.Click += MinimizeBTN_Click;
            // 
            // pictureBox2
            // 
            pictureBox2.BackColor = Color.Transparent;
            pictureBox2.Image = (Image)resources.GetObject("pictureBox2.Image");
            pictureBox2.Location = new Point(95, 22);
            pictureBox2.Name = "pictureBox2";
            pictureBox2.Size = new Size(128, 39);
            pictureBox2.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox2.TabIndex = 2;
            pictureBox2.TabStop = false;
            // 
            // pictureBox1
            // 
            pictureBox1.BackColor = Color.Transparent;
            pictureBox1.Image = (Image)resources.GetObject("pictureBox1.Image");
            pictureBox1.Location = new Point(9, 12);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(80, 51);
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox1.TabIndex = 1;
            pictureBox1.TabStop = false;
            // 
            // VisualPB
            // 
            VisualPB.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            VisualPB.BackColor = Color.Transparent;
            VisualPB.Image = (Image)resources.GetObject("VisualPB.Image");
            VisualPB.Location = new Point(3, 53);
            VisualPB.Name = "VisualPB";
            VisualPB.Size = new Size(1121, 633);
            VisualPB.SizeMode = PictureBoxSizeMode.StretchImage;
            VisualPB.TabIndex = 1;
            VisualPB.TabStop = false;
            VisualPB.Click += VisualPB_Click;
            // 
            // panel2
            // 
            panel2.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            panel2.BackgroundImage = (Image)resources.GetObject("panel2.BackgroundImage");
            panel2.BackgroundImageLayout = ImageLayout.Stretch;
            panel2.Controls.Add(devListTBox);
            panel2.Controls.Add(CnctBTN);
            panel2.Controls.Add(LoadingPB);
            panel2.Controls.Add(searchBTN);
            panel2.Controls.Add(VisualPB);
            panel2.Location = new Point(-1, 76);
            panel2.Name = "panel2";
            panel2.Size = new Size(1127, 689);
            panel2.TabIndex = 2;
            // 
            // devListTBox
            // 
            devListTBox.BackColor = Color.White;
            devListTBox.BorderStyle = BorderStyle.None;
            devListTBox.Font = new Font("Arial Narrow", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            devListTBox.ForeColor = SystemColors.Highlight;
            devListTBox.FormattingEnabled = true;
            devListTBox.Location = new Point(149, 11);
            devListTBox.Name = "devListTBox";
            devListTBox.Size = new Size(306, 32);
            devListTBox.TabIndex = 10;
            // 
            // CnctBTN
            // 
            CnctBTN.BackColor = Color.White;
            CnctBTN.FlatStyle = FlatStyle.Flat;
            CnctBTN.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            CnctBTN.ForeColor = Color.SeaGreen;
            CnctBTN.Location = new Point(461, 6);
            CnctBTN.Name = "CnctBTN";
            CnctBTN.Size = new Size(129, 39);
            CnctBTN.TabIndex = 9;
            CnctBTN.Text = "CONNECT";
            CnctBTN.UseVisualStyleBackColor = false;
            CnctBTN.Click += CnctBTN_Click_1;
            // 
            // LoadingPB
            // 
            LoadingPB.BackColor = Color.Transparent;
            LoadingPB.Image = (Image)resources.GetObject("LoadingPB.Image");
            LoadingPB.Location = new Point(596, 5);
            LoadingPB.Name = "LoadingPB";
            LoadingPB.Size = new Size(39, 41);
            LoadingPB.SizeMode = PictureBoxSizeMode.StretchImage;
            LoadingPB.TabIndex = 7;
            LoadingPB.TabStop = false;
            LoadingPB.Click += LoadingPB_Click;
            // 
            // searchBTN
            // 
            searchBTN.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            searchBTN.Density = MaterialSkin.Controls.MaterialButton.MaterialButtonDensity.Default;
            searchBTN.Depth = 0;
            searchBTN.HighEmphasis = true;
            searchBTN.Icon = null;
            searchBTN.Location = new Point(4, 8);
            searchBTN.Margin = new Padding(4, 6, 4, 6);
            searchBTN.MouseState = MaterialSkin.MouseState.HOVER;
            searchBTN.Name = "searchBTN";
            searchBTN.NoAccentTextColor = Color.Empty;
            searchBTN.Size = new Size(129, 36);
            searchBTN.TabIndex = 2;
            searchBTN.Text = "Search Vision";
            searchBTN.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Contained;
            searchBTN.UseAccentColor = false;
            searchBTN.UseVisualStyleBackColor = true;
            searchBTN.Click += searchBTN_Click;
            // 
            // VisioNeo
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackgroundImage = (Image)resources.GetObject("$this.BackgroundImage");
            BackgroundImageLayout = ImageLayout.Stretch;
            ClientSize = new Size(1649, 766);
            Controls.Add(panel2);
            Controls.Add(panel1);
            FormBorderStyle = FormBorderStyle.None;
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "VisioNeo";
            Text = "VisioNeo";
            panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)MaximizeBTN).EndInit();
            ((System.ComponentModel.ISupportInitialize)CloseBTN).EndInit();
            ((System.ComponentModel.ISupportInitialize)MinimizeBTN).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ((System.ComponentModel.ISupportInitialize)VisualPB).EndInit();
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)LoadingPB).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Panel panel1;
        private PictureBox CloseBTN;
        private PictureBox pictureBox2;
        private PictureBox pictureBox1;
        private PictureBox MinimizeBTN;
        private PictureBox MaximizeBTN;
        private PictureBox VisualPB;
        private Panel panel2;
        private MaterialSkin.Controls.MaterialButton searchBTN;
        private PictureBox LoadingPB;
        private Button CnctBTN;
        private ListBox devListTBox;
    }
}
