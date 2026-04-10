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
            Res_CD_Lbl = new Label();
            CnctBTN = new Button();
            LoadingPB = new PictureBox();
            searchBTN = new MaterialSkin.Controls.MaterialButton();
            tableLayoutPanel1 = new TableLayoutPanel();
            TabCntl = new TabControl();
            Parameters = new TabPage();
            Param_Panel = new Panel();
            cbDisplayMode = new ComboBox();
            label2 = new Label();
            label3 = new Label();
            lblFPS = new Label();
            label11 = new Label();
            tbFrameRate = new TrackBar();
            lblSaturation = new Label();
            label9 = new Label();
            tbSaturation = new TrackBar();
            lblSharpness = new Label();
            label7 = new Label();
            tbSharpness = new TrackBar();
            lblContrast = new Label();
            label5 = new Label();
            tbContrast = new TrackBar();
            lblBrightness = new Label();
            tbBrightness = new TrackBar();
            Gain_lbl = new Label();
            Exp_lbl = new Label();
            Gain = new Label();
            Exposure = new Label();
            label1 = new Label();
            gainTrackBar = new TrackBar();
            exposureTrackBar = new TrackBar();
            Tools = new TabPage();
            ToolsPanel = new Panel();
            objDec_btn = new Button();
            ClearCD_btn = new PictureBox();
            checkCD_btn = new Button();
            CD_Btn = new Button();
            OCR_Panel = new Panel();
            Resume_btn = new PictureBox();
            label6 = new Label();
            txtOCRResult = new RichTextBox();
            label4 = new Label();
            OCR_btn = new Button();
            Imaging = new TabPage();
            panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)MaximizeBTN).BeginInit();
            ((System.ComponentModel.ISupportInitialize)CloseBTN).BeginInit();
            ((System.ComponentModel.ISupportInitialize)MinimizeBTN).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)VisualPB).BeginInit();
            panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)LoadingPB).BeginInit();
            tableLayoutPanel1.SuspendLayout();
            TabCntl.SuspendLayout();
            Parameters.SuspendLayout();
            Param_Panel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)tbFrameRate).BeginInit();
            ((System.ComponentModel.ISupportInitialize)tbSaturation).BeginInit();
            ((System.ComponentModel.ISupportInitialize)tbSharpness).BeginInit();
            ((System.ComponentModel.ISupportInitialize)tbContrast).BeginInit();
            ((System.ComponentModel.ISupportInitialize)tbBrightness).BeginInit();
            ((System.ComponentModel.ISupportInitialize)gainTrackBar).BeginInit();
            ((System.ComponentModel.ISupportInitialize)exposureTrackBar).BeginInit();
            Tools.SuspendLayout();
            ToolsPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)ClearCD_btn).BeginInit();
            OCR_Panel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)Resume_btn).BeginInit();
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
            VisualPB.Location = new Point(5, 53);
            VisualPB.Name = "VisualPB";
            VisualPB.Size = new Size(1249, 643);
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
            panel2.Controls.Add(Res_CD_Lbl);
            panel2.Controls.Add(CnctBTN);
            panel2.Controls.Add(LoadingPB);
            panel2.Controls.Add(searchBTN);
            panel2.Controls.Add(VisualPB);
            panel2.Location = new Point(3, 3);
            panel2.Name = "panel2";
            panel2.Size = new Size(1257, 699);
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
            // Res_CD_Lbl
            // 
            Res_CD_Lbl.AutoSize = true;
            Res_CD_Lbl.BackColor = Color.Transparent;
            Res_CD_Lbl.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold | FontStyle.Italic, GraphicsUnit.Point, 0);
            Res_CD_Lbl.Location = new Point(1096, 15);
            Res_CD_Lbl.Name = "Res_CD_Lbl";
            Res_CD_Lbl.Size = new Size(0, 21);
            Res_CD_Lbl.TabIndex = 11;
            // 
            // CnctBTN
            // 
            CnctBTN.BackColor = Color.Transparent;
            CnctBTN.BackgroundImageLayout = ImageLayout.None;
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
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            tableLayoutPanel1.BackColor = Color.Transparent;
            tableLayoutPanel1.ColumnCount = 2;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle());
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle());
            tableLayoutPanel1.Controls.Add(panel2, 0, 0);
            tableLayoutPanel1.Controls.Add(TabCntl, 1, 0);
            tableLayoutPanel1.Location = new Point(2, 69);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 1;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.Size = new Size(1646, 705);
            tableLayoutPanel1.TabIndex = 3;
            // 
            // TabCntl
            // 
            TabCntl.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            TabCntl.Controls.Add(Parameters);
            TabCntl.Controls.Add(Tools);
            TabCntl.Controls.Add(Imaging);
            TabCntl.DrawMode = TabDrawMode.OwnerDrawFixed;
            TabCntl.Font = new Font("Calibri", 9.75F, FontStyle.Bold | FontStyle.Italic, GraphicsUnit.Point, 0);
            TabCntl.Location = new Point(1266, 3);
            TabCntl.Name = "TabCntl";
            TabCntl.Padding = new Point(12, 8);
            TabCntl.SelectedIndex = 0;
            TabCntl.Size = new Size(377, 699);
            TabCntl.SizeMode = TabSizeMode.Fixed;
            TabCntl.TabIndex = 3;
            // 
            // Parameters
            // 
            Parameters.BackgroundImage = (Image)resources.GetObject("Parameters.BackgroundImage");
            Parameters.BackgroundImageLayout = ImageLayout.Stretch;
            Parameters.Controls.Add(Param_Panel);
            Parameters.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            Parameters.ForeColor = SystemColors.Highlight;
            Parameters.Location = new Point(4, 34);
            Parameters.Name = "Parameters";
            Parameters.Padding = new Padding(3);
            Parameters.Size = new Size(369, 661);
            Parameters.TabIndex = 0;
            Parameters.Text = "Parameters";
            Parameters.UseVisualStyleBackColor = true;
            // 
            // Param_Panel
            // 
            Param_Panel.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            Param_Panel.AutoScroll = true;
            Param_Panel.BackColor = Color.White;
            Param_Panel.Controls.Add(cbDisplayMode);
            Param_Panel.Controls.Add(label2);
            Param_Panel.Controls.Add(label3);
            Param_Panel.Controls.Add(lblFPS);
            Param_Panel.Controls.Add(label11);
            Param_Panel.Controls.Add(tbFrameRate);
            Param_Panel.Controls.Add(lblSaturation);
            Param_Panel.Controls.Add(label9);
            Param_Panel.Controls.Add(tbSaturation);
            Param_Panel.Controls.Add(lblSharpness);
            Param_Panel.Controls.Add(label7);
            Param_Panel.Controls.Add(tbSharpness);
            Param_Panel.Controls.Add(lblContrast);
            Param_Panel.Controls.Add(label5);
            Param_Panel.Controls.Add(tbContrast);
            Param_Panel.Controls.Add(lblBrightness);
            Param_Panel.Controls.Add(tbBrightness);
            Param_Panel.Controls.Add(Gain_lbl);
            Param_Panel.Controls.Add(Exp_lbl);
            Param_Panel.Controls.Add(Gain);
            Param_Panel.Controls.Add(Exposure);
            Param_Panel.Controls.Add(label1);
            Param_Panel.Controls.Add(gainTrackBar);
            Param_Panel.Controls.Add(exposureTrackBar);
            Param_Panel.Location = new Point(3, 3);
            Param_Panel.Name = "Param_Panel";
            Param_Panel.Size = new Size(363, 655);
            Param_Panel.TabIndex = 3;
            Param_Panel.Paint += Param_Panel_Paint;
            // 
            // cbDisplayMode
            // 
            cbDisplayMode.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            cbDisplayMode.FlatStyle = FlatStyle.Popup;
            cbDisplayMode.FormattingEnabled = true;
            cbDisplayMode.Location = new Point(190, 53);
            cbDisplayMode.Name = "cbDisplayMode";
            cbDisplayMode.Size = new Size(121, 23);
            cbDisplayMode.TabIndex = 34;
            cbDisplayMode.SelectedIndexChanged += cbDisplayMode_SelectedIndexChanged;
            // 
            // label2
            // 
            label2.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            label2.AutoSize = true;
            label2.Font = new Font("Calibri", 12F, FontStyle.Bold | FontStyle.Italic);
            label2.ForeColor = SystemColors.Highlight;
            label2.Location = new Point(39, 53);
            label2.Name = "label2";
            label2.Size = new Size(99, 19);
            label2.TabIndex = 33;
            label2.Text = "Display Mode";
            // 
            // label3
            // 
            label3.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            label3.AutoSize = true;
            label3.Font = new Font("Calibri", 12F, FontStyle.Bold | FontStyle.Italic);
            label3.ForeColor = SystemColors.Highlight;
            label3.Location = new Point(35, 265);
            label3.Name = "label3";
            label3.Size = new Size(78, 19);
            label3.TabIndex = 32;
            label3.Text = "Brightness";
            // 
            // lblFPS
            // 
            lblFPS.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            lblFPS.AutoSize = true;
            lblFPS.Font = new Font("Calibri", 12F, FontStyle.Bold | FontStyle.Italic);
            lblFPS.ForeColor = SystemColors.Highlight;
            lblFPS.Location = new Point(294, 619);
            lblFPS.Name = "lblFPS";
            lblFPS.Size = new Size(17, 19);
            lblFPS.TabIndex = 31;
            lblFPS.Text = "0";
            // 
            // label11
            // 
            label11.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            label11.AutoSize = true;
            label11.Font = new Font("Calibri", 12F, FontStyle.Bold | FontStyle.Italic);
            label11.ForeColor = SystemColors.Highlight;
            label11.Location = new Point(37, 619);
            label11.Name = "label11";
            label11.Size = new Size(86, 19);
            label11.TabIndex = 30;
            label11.Text = "Frame Rate";
            // 
            // tbFrameRate
            // 
            tbFrameRate.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            tbFrameRate.BackColor = SystemColors.ControlLightLight;
            tbFrameRate.Location = new Point(39, 650);
            tbFrameRate.Minimum = 1;
            tbFrameRate.Name = "tbFrameRate";
            tbFrameRate.Size = new Size(287, 45);
            tbFrameRate.SmallChange = 5;
            tbFrameRate.TabIndex = 29;
            tbFrameRate.Value = 1;
            tbFrameRate.Scroll += tbFrameRate_Scroll;
            // 
            // lblSaturation
            // 
            lblSaturation.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            lblSaturation.AutoSize = true;
            lblSaturation.Font = new Font("Calibri", 12F, FontStyle.Bold | FontStyle.Italic);
            lblSaturation.ForeColor = SystemColors.Highlight;
            lblSaturation.Location = new Point(294, 532);
            lblSaturation.Name = "lblSaturation";
            lblSaturation.Size = new Size(17, 19);
            lblSaturation.TabIndex = 28;
            lblSaturation.Text = "0";
            // 
            // label9
            // 
            label9.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            label9.AutoSize = true;
            label9.Font = new Font("Calibri", 12F, FontStyle.Bold | FontStyle.Italic);
            label9.ForeColor = SystemColors.Highlight;
            label9.Location = new Point(39, 532);
            label9.Name = "label9";
            label9.Size = new Size(77, 19);
            label9.TabIndex = 27;
            label9.Text = "Saturation";
            // 
            // tbSaturation
            // 
            tbSaturation.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            tbSaturation.BackColor = SystemColors.ControlLightLight;
            tbSaturation.Location = new Point(39, 563);
            tbSaturation.Minimum = 1;
            tbSaturation.Name = "tbSaturation";
            tbSaturation.Size = new Size(287, 45);
            tbSaturation.SmallChange = 5;
            tbSaturation.TabIndex = 26;
            tbSaturation.Value = 1;
            tbSaturation.Scroll += tbSaturation_Scroll;
            // 
            // lblSharpness
            // 
            lblSharpness.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            lblSharpness.AutoSize = true;
            lblSharpness.Font = new Font("Calibri", 12F, FontStyle.Bold | FontStyle.Italic);
            lblSharpness.ForeColor = SystemColors.Highlight;
            lblSharpness.Location = new Point(294, 441);
            lblSharpness.Name = "lblSharpness";
            lblSharpness.Size = new Size(17, 19);
            lblSharpness.TabIndex = 25;
            lblSharpness.Text = "0";
            // 
            // label7
            // 
            label7.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            label7.AutoSize = true;
            label7.Font = new Font("Calibri", 12F, FontStyle.Bold | FontStyle.Italic);
            label7.ForeColor = SystemColors.Highlight;
            label7.Location = new Point(39, 441);
            label7.Name = "label7";
            label7.Size = new Size(74, 19);
            label7.TabIndex = 24;
            label7.Text = "Sharpness";
            // 
            // tbSharpness
            // 
            tbSharpness.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            tbSharpness.BackColor = SystemColors.ControlLightLight;
            tbSharpness.Location = new Point(39, 472);
            tbSharpness.Minimum = 1;
            tbSharpness.Name = "tbSharpness";
            tbSharpness.Size = new Size(287, 45);
            tbSharpness.SmallChange = 5;
            tbSharpness.TabIndex = 23;
            tbSharpness.Value = 1;
            tbSharpness.Scroll += tbSharpness_Scroll;
            // 
            // lblContrast
            // 
            lblContrast.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            lblContrast.AutoSize = true;
            lblContrast.Font = new Font("Calibri", 12F, FontStyle.Bold | FontStyle.Italic);
            lblContrast.ForeColor = SystemColors.Highlight;
            lblContrast.Location = new Point(294, 352);
            lblContrast.Name = "lblContrast";
            lblContrast.Size = new Size(17, 19);
            lblContrast.TabIndex = 22;
            lblContrast.Text = "0";
            // 
            // label5
            // 
            label5.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            label5.AutoSize = true;
            label5.Font = new Font("Calibri", 12F, FontStyle.Bold | FontStyle.Italic);
            label5.ForeColor = SystemColors.Highlight;
            label5.Location = new Point(37, 352);
            label5.Name = "label5";
            label5.Size = new Size(65, 19);
            label5.TabIndex = 21;
            label5.Text = "Contrast";
            // 
            // tbContrast
            // 
            tbContrast.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            tbContrast.BackColor = SystemColors.ControlLightLight;
            tbContrast.Location = new Point(39, 383);
            tbContrast.Minimum = 1;
            tbContrast.Name = "tbContrast";
            tbContrast.Size = new Size(287, 45);
            tbContrast.SmallChange = 5;
            tbContrast.TabIndex = 20;
            tbContrast.Value = 1;
            tbContrast.Scroll += tbContrast_Scroll;
            // 
            // lblBrightness
            // 
            lblBrightness.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            lblBrightness.AutoSize = true;
            lblBrightness.Font = new Font("Calibri", 12F, FontStyle.Bold | FontStyle.Italic);
            lblBrightness.ForeColor = SystemColors.Highlight;
            lblBrightness.Location = new Point(294, 265);
            lblBrightness.Name = "lblBrightness";
            lblBrightness.Size = new Size(17, 19);
            lblBrightness.TabIndex = 19;
            lblBrightness.Text = "0";
            // 
            // tbBrightness
            // 
            tbBrightness.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            tbBrightness.BackColor = SystemColors.ControlLightLight;
            tbBrightness.Location = new Point(39, 296);
            tbBrightness.Minimum = 1;
            tbBrightness.Name = "tbBrightness";
            tbBrightness.Size = new Size(287, 45);
            tbBrightness.SmallChange = 5;
            tbBrightness.TabIndex = 17;
            tbBrightness.Value = 1;
            tbBrightness.Scroll += tbBrightness_Scroll;
            // 
            // Gain_lbl
            // 
            Gain_lbl.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            Gain_lbl.AutoSize = true;
            Gain_lbl.Font = new Font("Calibri", 12F, FontStyle.Bold | FontStyle.Italic);
            Gain_lbl.ForeColor = SystemColors.Highlight;
            Gain_lbl.Location = new Point(294, 179);
            Gain_lbl.Name = "Gain_lbl";
            Gain_lbl.Size = new Size(17, 19);
            Gain_lbl.TabIndex = 16;
            Gain_lbl.Text = "0";
            // 
            // Exp_lbl
            // 
            Exp_lbl.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            Exp_lbl.AutoSize = true;
            Exp_lbl.Font = new Font("Calibri", 12F, FontStyle.Bold | FontStyle.Italic);
            Exp_lbl.ForeColor = SystemColors.Highlight;
            Exp_lbl.Location = new Point(294, 100);
            Exp_lbl.Name = "Exp_lbl";
            Exp_lbl.Size = new Size(17, 19);
            Exp_lbl.TabIndex = 15;
            Exp_lbl.Text = "0";
            // 
            // Gain
            // 
            Gain.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            Gain.AutoSize = true;
            Gain.Font = new Font("Calibri", 12F, FontStyle.Bold | FontStyle.Italic);
            Gain.ForeColor = SystemColors.Highlight;
            Gain.Location = new Point(39, 179);
            Gain.Name = "Gain";
            Gain.Size = new Size(39, 19);
            Gain.TabIndex = 4;
            Gain.Text = "Gain";
            Gain.Click += Gain_Click;
            // 
            // Exposure
            // 
            Exposure.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            Exposure.AutoSize = true;
            Exposure.Font = new Font("Calibri", 12F, FontStyle.Bold | FontStyle.Italic);
            Exposure.ForeColor = SystemColors.Highlight;
            Exposure.Location = new Point(39, 100);
            Exposure.Name = "Exposure";
            Exposure.Size = new Size(68, 19);
            Exposure.TabIndex = 3;
            Exposure.Text = "Exposure";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 12F, FontStyle.Bold | FontStyle.Italic, GraphicsUnit.Point, 0);
            label1.ForeColor = SystemColors.Highlight;
            label1.Location = new Point(6, 9);
            label1.Name = "label1";
            label1.Size = new Size(110, 21);
            label1.TabIndex = 2;
            label1.Text = "PARAMETERS";
            // 
            // gainTrackBar
            // 
            gainTrackBar.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            gainTrackBar.BackColor = SystemColors.ControlLightLight;
            gainTrackBar.Location = new Point(33, 217);
            gainTrackBar.Name = "gainTrackBar";
            gainTrackBar.Size = new Size(287, 45);
            gainTrackBar.SmallChange = 10;
            gainTrackBar.TabIndex = 1;
            gainTrackBar.Scroll += gainTrackBar_Scroll;
            // 
            // exposureTrackBar
            // 
            exposureTrackBar.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            exposureTrackBar.BackColor = SystemColors.ControlLightLight;
            exposureTrackBar.Location = new Point(33, 131);
            exposureTrackBar.Minimum = 1;
            exposureTrackBar.Name = "exposureTrackBar";
            exposureTrackBar.Size = new Size(287, 45);
            exposureTrackBar.SmallChange = 5;
            exposureTrackBar.TabIndex = 0;
            exposureTrackBar.Value = 1;
            exposureTrackBar.Scroll += exposureTrackBar_Scroll_1;
            // 
            // Tools
            // 
            Tools.BackgroundImage = (Image)resources.GetObject("Tools.BackgroundImage");
            Tools.BackgroundImageLayout = ImageLayout.Stretch;
            Tools.Controls.Add(ToolsPanel);
            Tools.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            Tools.ForeColor = SystemColors.Highlight;
            Tools.Location = new Point(4, 34);
            Tools.Name = "Tools";
            Tools.Padding = new Padding(3);
            Tools.Size = new Size(369, 661);
            Tools.TabIndex = 1;
            Tools.Text = "Tools";
            Tools.UseVisualStyleBackColor = true;
            // 
            // ToolsPanel
            // 
            ToolsPanel.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            ToolsPanel.AutoScroll = true;
            ToolsPanel.BackColor = Color.White;
            ToolsPanel.Controls.Add(objDec_btn);
            ToolsPanel.Controls.Add(ClearCD_btn);
            ToolsPanel.Controls.Add(checkCD_btn);
            ToolsPanel.Controls.Add(CD_Btn);
            ToolsPanel.Controls.Add(OCR_Panel);
            ToolsPanel.Controls.Add(label4);
            ToolsPanel.Controls.Add(OCR_btn);
            ToolsPanel.Location = new Point(3, 3);
            ToolsPanel.Name = "ToolsPanel";
            ToolsPanel.Size = new Size(363, 655);
            ToolsPanel.TabIndex = 0;
            // 
            // objDec_btn
            // 
            objDec_btn.BackgroundImageLayout = ImageLayout.None;
            objDec_btn.FlatStyle = FlatStyle.Flat;
            objDec_btn.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold | FontStyle.Italic, GraphicsUnit.Point, 0);
            objDec_btn.Location = new Point(12, 141);
            objDec_btn.Name = "objDec_btn";
            objDec_btn.Size = new Size(123, 37);
            objDec_btn.TabIndex = 13;
            objDec_btn.Text = "Object Detection";
            objDec_btn.TextAlign = ContentAlignment.MiddleLeft;
            objDec_btn.UseVisualStyleBackColor = true;
            // 
            // ClearCD_btn
            // 
            ClearCD_btn.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            ClearCD_btn.BackColor = Color.Transparent;
            ClearCD_btn.Image = (Image)resources.GetObject("ClearCD_btn.Image");
            ClearCD_btn.Location = new Point(199, 103);
            ClearCD_btn.Name = "ClearCD_btn";
            ClearCD_btn.Size = new Size(25, 25);
            ClearCD_btn.SizeMode = PictureBoxSizeMode.StretchImage;
            ClearCD_btn.TabIndex = 7;
            ClearCD_btn.TabStop = false;
            ClearCD_btn.Click += ClearCD_btn_Click;
            // 
            // checkCD_btn
            // 
            checkCD_btn.BackgroundImageLayout = ImageLayout.None;
            checkCD_btn.FlatStyle = FlatStyle.Flat;
            checkCD_btn.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold | FontStyle.Italic, GraphicsUnit.Point, 0);
            checkCD_btn.Location = new Point(138, 98);
            checkCD_btn.Name = "checkCD_btn";
            checkCD_btn.Size = new Size(55, 37);
            checkCD_btn.TabIndex = 12;
            checkCD_btn.Text = "Check";
            checkCD_btn.TextAlign = ContentAlignment.MiddleLeft;
            checkCD_btn.UseVisualStyleBackColor = true;
            checkCD_btn.Click += checkCD_btn_Click;
            // 
            // CD_Btn
            // 
            CD_Btn.BackgroundImageLayout = ImageLayout.None;
            CD_Btn.FlatStyle = FlatStyle.Flat;
            CD_Btn.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold | FontStyle.Italic, GraphicsUnit.Point, 0);
            CD_Btn.Location = new Point(11, 98);
            CD_Btn.Name = "CD_Btn";
            CD_Btn.Size = new Size(123, 37);
            CD_Btn.TabIndex = 10;
            CD_Btn.Text = "Colour Detection";
            CD_Btn.TextAlign = ContentAlignment.MiddleLeft;
            CD_Btn.UseVisualStyleBackColor = true;
            CD_Btn.Click += CD_Btn_Click;
            // 
            // OCR_Panel
            // 
            OCR_Panel.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            OCR_Panel.BackgroundImage = (Image)resources.GetObject("OCR_Panel.BackgroundImage");
            OCR_Panel.BackgroundImageLayout = ImageLayout.Stretch;
            OCR_Panel.Controls.Add(Resume_btn);
            OCR_Panel.Controls.Add(label6);
            OCR_Panel.Controls.Add(txtOCRResult);
            OCR_Panel.Location = new Point(7, 240);
            OCR_Panel.Name = "OCR_Panel";
            OCR_Panel.Size = new Size(339, 405);
            OCR_Panel.TabIndex = 9;
            // 
            // Resume_btn
            // 
            Resume_btn.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            Resume_btn.BackColor = Color.Transparent;
            Resume_btn.Image = (Image)resources.GetObject("Resume_btn.Image");
            Resume_btn.Location = new Point(311, 3);
            Resume_btn.Name = "Resume_btn";
            Resume_btn.Size = new Size(25, 25);
            Resume_btn.SizeMode = PictureBoxSizeMode.StretchImage;
            Resume_btn.TabIndex = 7;
            Resume_btn.TabStop = false;
            Resume_btn.Click += Resume_btn_Click;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.BackColor = Color.Transparent;
            label6.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold | FontStyle.Italic);
            label6.Location = new Point(5, 11);
            label6.Name = "label6";
            label6.Size = new Size(99, 17);
            label6.TabIndex = 8;
            label6.Text = "RESULT PANEL";
            label6.Click += label6_Click;
            // 
            // txtOCRResult
            // 
            txtOCRResult.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            txtOCRResult.BorderStyle = BorderStyle.None;
            txtOCRResult.Location = new Point(4, 42);
            txtOCRResult.Name = "txtOCRResult";
            txtOCRResult.Size = new Size(329, 355);
            txtOCRResult.TabIndex = 4;
            txtOCRResult.Text = "";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Segoe UI", 12F, FontStyle.Bold | FontStyle.Italic, GraphicsUnit.Point, 0);
            label4.ForeColor = SystemColors.Highlight;
            label4.Location = new Point(6, 9);
            label4.Name = "label4";
            label4.Size = new Size(60, 21);
            label4.TabIndex = 3;
            label4.Text = "TOOLS";
            // 
            // OCR_btn
            // 
            OCR_btn.BackgroundImageLayout = ImageLayout.None;
            OCR_btn.FlatStyle = FlatStyle.Flat;
            OCR_btn.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold | FontStyle.Italic, GraphicsUnit.Point, 0);
            OCR_btn.Location = new Point(12, 55);
            OCR_btn.Name = "OCR_btn";
            OCR_btn.Size = new Size(222, 37);
            OCR_btn.TabIndex = 1;
            OCR_btn.Text = "Optical Character Recognization ";
            OCR_btn.TextAlign = ContentAlignment.MiddleLeft;
            OCR_btn.UseVisualStyleBackColor = true;
            OCR_btn.Click += OCR_btn_Click;
            // 
            // Imaging
            // 
            Imaging.BackColor = Color.Transparent;
            Imaging.BackgroundImage = (Image)resources.GetObject("Imaging.BackgroundImage");
            Imaging.BackgroundImageLayout = ImageLayout.Stretch;
            Imaging.Font = new Font("Segoe UI", 9F);
            Imaging.ForeColor = SystemColors.Highlight;
            Imaging.Location = new Point(4, 34);
            Imaging.Name = "Imaging";
            Imaging.Size = new Size(369, 661);
            Imaging.TabIndex = 2;
            Imaging.Text = "Imaging";
            // 
            // VisioNeo
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackgroundImage = (Image)resources.GetObject("$this.BackgroundImage");
            BackgroundImageLayout = ImageLayout.Stretch;
            ClientSize = new Size(1649, 766);
            Controls.Add(tableLayoutPanel1);
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
            tableLayoutPanel1.ResumeLayout(false);
            TabCntl.ResumeLayout(false);
            Parameters.ResumeLayout(false);
            Param_Panel.ResumeLayout(false);
            Param_Panel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)tbFrameRate).EndInit();
            ((System.ComponentModel.ISupportInitialize)tbSaturation).EndInit();
            ((System.ComponentModel.ISupportInitialize)tbSharpness).EndInit();
            ((System.ComponentModel.ISupportInitialize)tbContrast).EndInit();
            ((System.ComponentModel.ISupportInitialize)tbBrightness).EndInit();
            ((System.ComponentModel.ISupportInitialize)gainTrackBar).EndInit();
            ((System.ComponentModel.ISupportInitialize)exposureTrackBar).EndInit();
            Tools.ResumeLayout(false);
            ToolsPanel.ResumeLayout(false);
            ToolsPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)ClearCD_btn).EndInit();
            OCR_Panel.ResumeLayout(false);
            OCR_Panel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)Resume_btn).EndInit();
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
        private TableLayoutPanel tableLayoutPanel1;
        private Panel Param_Panel;
        private TrackBar gainTrackBar;
        private TrackBar exposureTrackBar;
        private Label Gain;
        private Label Exposure;
        private Label label1;
        private Label Gain_lbl;
        private Label Exp_lbl;
        private Label lblFPS;
        private Label label11;
        private TrackBar tbFrameRate;
        private Label lblSaturation;
        private Label label9;
        private TrackBar tbSaturation;
        private Label lblSharpness;
        private Label label7;
        private TrackBar tbSharpness;
        private Label lblContrast;
        private Label label5;
        private TrackBar tbContrast;
        private Label lblBrightness;
        private TrackBar tbBrightness;
        private Label label3;
        private ComboBox cbDisplayMode;
        private Label label2;
        private TabControl TabCntl;
        private TabPage Parameters;
        private TabPage Tools;
        private TabPage Imaging;
        private Panel ToolsPanel;
        private Button OCR_btn;
        private Label label4;
        private RichTextBox txtOCRResult;
        private PictureBox Resume_btn;
        private Panel OCR_Panel;
        private Label label6;
        private Label Res_CD_Lbl;
        private Button CD_Btn;
        private Button checkCD_btn;
        private PictureBox ClearCD_btn;
        private Button objDec_btn;
    }
}
