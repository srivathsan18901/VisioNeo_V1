namespace VisioNeo_App
{
    using MvCamCtrl.NET;
    using System.IO;
    using System.Runtime.InteropServices;
    using VisioNeo_App.Services;


    public partial class VisioNeo : Form
    {
        private CameraService cameraService = new CameraService();
        private ImageProcessingService imageService = new ImageProcessingService();
        private Services.DisplayMode currentDisplayMode = Services.DisplayMode.Normal;
        MyCamera.MV_CC_DEVICE_INFO_LIST deviceList;
        bool isConnected = false;
        private OCRService ocrService = new OCRService();
        private Bitmap lastFrame = null;
        private readonly object frameLock = new object();
        private bool isFrozen = false;
        private bool isHandlingException = false;
        // Color Detection
        private bool isDrawing = false;
        private bool isTeachMode = false;
        private bool isInspectionMode = false;
        private Point startPoint;
        private List<Rectangle> roiList = new List<Rectangle>();
        private List<Color> taughtColors = new List<Color>();
        private Rectangle currentROI; // 🔥 for drawing only

        private ObjectDetectionService objService = new ObjectDetectionService();
        private int roiCounter = 1;
        private bool isObjectTeachMode = false;
        private bool isTrackingMode = false;
        // 🔥 Calibration Mode
        private bool isCalibrationMode = false;
        private List<Point> calibPoints = new List<Point>();

        private List<Rectangle> objRois = new List<Rectangle>();
        private List<string> objLabels = new List<string>();
        private Point taughtCenter;   // reference point
        private double pixelToMM = 0.05; // 🔥 change based on calibration

        private List<string> roiLabels = new List<string>();
        private List<bool> roiResults = new List<bool>();


        public VisioNeo()
        {
            InitializeComponent();
            Application.ApplicationExit += OnApplicationExit;
            TabCntl.SizeMode = TabSizeMode.Fixed;
            LoadingPB.Visible = false;
            CnctBTN.Visible = false;
            Param_Panel.Visible = false;
            ToolsPanel.Visible = false;
            devListTBox.Visible = false;
            checkCD_btn.Visible = false;
            ClearCD_btn.Visible = false;
            OCR_Panel.Visible = false;
            track_btn.Visible = false;
            ClearOD_btn.Visible = false;
            CD_Panel.Visible = false;
            //TabCntl.Visible = false;
            VisualPB.SizeMode = PictureBoxSizeMode.StretchImage;

            // Brightness
            tbBrightness.Minimum = 0;
            tbBrightness.Maximum = 255;

            // Contrast
            tbContrast.Minimum = 0;
            tbContrast.Maximum = 255;

            // Sharpness
            tbSharpness.Minimum = 0;
            tbSharpness.Maximum = 100;

            // Saturation
            tbSaturation.Minimum = 0;
            tbSaturation.Maximum = 255;

            // Frame Rate
            tbFrameRate.Minimum = 1;
            tbFrameRate.Maximum = 60;

            cbDisplayMode.Items.Add("Normal");
            cbDisplayMode.Items.Add("Grayscale");
            cbDisplayMode.Items.Add("Heatmap");

            cbDisplayMode.SelectedIndex = 0;
            this.TabCntl.DrawItem += new DrawItemEventHandler(this.tabControl1_DrawItem);

            VisualPB.MouseDown += VisualPB_MouseDown;
            VisualPB.MouseMove += VisualPB_MouseMove;
            VisualPB.MouseUp += VisualPB_MouseUp;
            VisualPB.Paint += VisualPB_Paint;

            typeof(PictureBox).InvokeMember("DoubleBuffered",
                System.Reflection.BindingFlags.NonPublic |
                System.Reflection.BindingFlags.Instance |
                System.Reflection.BindingFlags.SetProperty,
                null, VisualPB, new object[] { true });
        }

        private void OnApplicationExit(object sender, EventArgs e)
        {
            try
            {
                if (isConnected)
                {
                    cameraService.Disconnect();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error during application exit: {ex.Message}");
            }
        }

        private void tabControl1_DrawItem(object sender, DrawItemEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;



            TabPage tabPage = TabCntl.TabPages[e.Index];
            Rectangle tabRect = TabCntl.GetTabRect(e.Index);

            bool isSelected = (e.Index == TabCntl.SelectedIndex);

            // Colors
            Color bgColor = isSelected ? Color.FromArgb(66, 109, 178) : Color.FromArgb(114, 162, 182);
            Color textColor = Color.White;

            // Fill tab
            using (SolidBrush brush = new SolidBrush(bgColor))
            {
                g.FillRectangle(brush, tabRect);
            }

            // Text
            using (Font font = new Font("Segoe UI",
                                       isSelected ? 10.5f : 9.5f,
                                       isSelected ? FontStyle.Bold : FontStyle.Italic))
            using (SolidBrush textBrush = new SolidBrush(textColor))
            {
                StringFormat sf = new StringFormat
                {
                    Alignment = StringAlignment.Center,
                    LineAlignment = StringAlignment.Center
                };

                g.DrawString(tabPage.Text, font, textBrush, tabRect, sf);
            }
        }

        private void MinimizeBTN_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void MaximizeBTN_Click(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Maximized)
            {
                this.WindowState = FormWindowState.Normal;
            }
            else
            {
                this.WindowState = FormWindowState.Maximized;
            }
        }

        private void CloseBTN_Click(object sender, EventArgs e)
        {
            clearCD();
            cameraService.Disconnect();
            Application.Exit();
        }

        private async void searchBTN_Click(object sender, EventArgs e)
        {
            // Step 1: Show loader
            LoadingPB.Visible = true;
            CnctBTN.Visible = false;
            devListTBox.Items.Clear();

            // Step 2: Run camera search in background
            var devices = await Task.Run(() => SearchCamera());

            // Step 3: Update UI after search
            LoadingPB.Visible = false;

            if (devices.Count > 0)
            {
                foreach (var dev in devices)
                {
                    devListTBox.Items.Add(dev);
                }
                devListTBox.Visible = true;
                CnctBTN.Visible = true; // show connect button
            }

            else
            {
                devListTBox.Items.Clear();
                devListTBox.Items.Add("No camera found");
            }
        }

        private void LoadingPB_Click(object sender, EventArgs e)
        {

        }

        private void VisualPB_Click(object sender, EventArgs e)
        {

        }

        private async void CnctBTN_Click_1(object sender, EventArgs e)
        {
            // =========================
            // DISCONNECT MODE
            // =========================
            if (isConnected)
            {
                try
                {
                    isConnected = false;
                    cameraService.Disconnect();

                    VisualPB.Invoke(() =>
                    {
                        VisualPB.Image?.Dispose();
                        VisualPB.Image = null;
                        Image gif = Properties.Resources.No_Data_Founds;
                        VisualPB.Image = gif;
                        ImageAnimator.Animate(gif, (s, ev) => { VisualPB.Invalidate(); });
                    });

                    CnctBTN.Text = "CONNECT";
                    CnctBTN.ForeColor = Color.SeaGreen;
                    isConnected = false;
                    isFrozen = false;
                    clearCD(); // Reset color detection state on disconnect
                    clearOD();
                    Param_Panel.Visible = false;
                    ToolsPanel.Visible = false;
                }
                catch (Exception ex)
                {
                    HandleException(ex, "Disconnect");
                }
                return;
            }

            // =========================
            // CONNECT MODE
            // =========================
            int index = devListTBox.SelectedIndex;
            if (index < 0)
            {
                MessageBox.Show("Please select a device!");
                return;
            }

            try
            {
                MyCamera.MV_CC_DEVICE_INFO deviceInfo =
                        (MyCamera.MV_CC_DEVICE_INFO)Marshal.PtrToStructure(
                            deviceList.pDeviceInfo[index],
                            typeof(MyCamera.MV_CC_DEVICE_INFO));

                bool connected = cameraService.Connect(deviceInfo);

                if (!connected)
                {
                    MessageBox.Show("Camera connection failed");
                    return;
                }

                // Wrap the frame callback to catch exceptions
                cameraService.StartGrabbing(frame =>
                {
                    try
                    {
                        if (isFrozen) return;
                        Bitmap outputFrame;
                        if (isTrackingMode)
                        {
                            Rectangle tracked = objService.MatchTemplate(frame);
                            outputFrame = (Bitmap)frame.Clone();
                            if (tracked != Rectangle.Empty)
                            {
                                int centerX = tracked.X + tracked.Width / 2;
                                int centerY = tracked.Y + tracked.Height / 2;

                                int dx = centerX - taughtCenter.X;
                                int dy = centerY - taughtCenter.Y;

                                double dxMM = dx * pixelToMM;
                                double dyMM = dy * pixelToMM;

                                double distanceMM = Math.Sqrt(dxMM * dxMM + dyMM * dyMM);

                                double tolerance = 2.0; // mm (adjust)

                                bool isOK = distanceMM < tolerance;

                                Bitmap drawFrame = (Bitmap)frame.Clone();

                                using (Graphics g = Graphics.FromImage(outputFrame))
                                {
                                    // 🟡 Draw tracked object
                                    using (Pen pen = new Pen(isOK ? Color.Lime : Color.Red, 3))
                                    {
                                        g.DrawRectangle(pen, tracked);
                                    }

                                    // 🟢 Draw taught center (REFERENCE)
                                    using (Pen refPen = new Pen(Color.Blue, 2))
                                    {
                                        int size = 10;

                                        g.DrawLine(refPen,
                                            taughtCenter.X - size, taughtCenter.Y,
                                            taughtCenter.X + size, taughtCenter.Y);

                                        g.DrawLine(refPen,
                                            taughtCenter.X, taughtCenter.Y - size,
                                            taughtCenter.X, taughtCenter.Y + size);
                                    }

                                    // 🔵 Draw line between taught and current
                                    using (Pen linePen = new Pen(Color.Cyan, 2))
                                    {
                                        g.DrawLine(linePen,
                                            taughtCenter.X, taughtCenter.Y,
                                            centerX, centerY);
                                    }
                                }
                                // 🔥 UPDATE LABEL (IMPORTANT: UI THREAD)
                                VisualPB.Invoke(() =>
                                {
                                    //VisualPB.Image?.Dispose();
                                    //VisualPB.Image = drawFrame;
                                    if (!isObjectTeachMode && !isTrackingMode) { Res_CD_Lbl.Text = ""; }
                                    else
                                    {
                                        Res_CD_Lbl.Text =
                                           $"Ref: ({taughtCenter.X},{taughtCenter.Y})  |  " +
                                           $"Cur: ({centerX},{centerY})  |  " +
                                           $"DX: {dxMM:F2} mm  DY: {dyMM:F2} mm  |  Dist: {distanceMM:F2} mm";

                                        Res_CD_Lbl.ForeColor = isOK ? Color.Green : Color.Red;
                                    }

                                });
                            }
                        }

                        else
                        {
                            outputFrame = imageService.Process(frame, currentDisplayMode);
                        }

                        lock (frameLock)
                        {
                            lastFrame?.Dispose();
                            lastFrame = (Bitmap)outputFrame.Clone();
                        }

                        VisualPB.BeginInvoke((Action)(() =>
                        {
                            var old = VisualPB.Image;
                            VisualPB.Image = outputFrame;
                            old?.Dispose();
                        }));
                    }
                    catch (Exception ex)
                    {
                        VisualPB.Invoke(() => HandleException(ex, "Frame Processing"));
                    }
                });

                // Load camera parameters with error handling
                try
                {
                    float currentExposure = cameraService.GetExposure();
                    exposureTrackBar.Minimum = 10000;
                    exposureTrackBar.Maximum = 1000000;
                    currentExposure = Math.Max(exposureTrackBar.Minimum, currentExposure);
                    currentExposure = Math.Min(exposureTrackBar.Maximum, currentExposure);
                    exposureTrackBar.Value = (int)currentExposure;
                    Exp_lbl.Text = $"{currentExposure:F0}";

                    float currentGain = cameraService.GetGain();
                    gainTrackBar.Minimum = 0;
                    gainTrackBar.Maximum = 20;
                    currentGain = Math.Max(gainTrackBar.Minimum, currentGain);
                    currentGain = Math.Min(gainTrackBar.Maximum, currentGain);
                    gainTrackBar.Value = (int)currentGain;
                    Gain_lbl.Text = $"{currentGain:F1}";

                    // Optional parameters
                    int brightness = cameraService.GetBrightness();
                    tbBrightness.Value = Math.Min(tbBrightness.Maximum, brightness);
                    lblBrightness.Text = brightness.ToString();

                    int contrast = cameraService.GetContrast();
                    tbContrast.Value = Math.Min(tbContrast.Maximum, contrast);
                    lblContrast.Text = contrast.ToString();

                    int sharpness = cameraService.GetSharpness();
                    tbSharpness.Value = Math.Min(tbSharpness.Maximum, sharpness);
                    lblSharpness.Text = sharpness.ToString();

                    int saturation = cameraService.GetSaturation();
                    tbSaturation.Value = Math.Min(tbSaturation.Maximum, saturation);
                    lblSaturation.Text = saturation.ToString();

                    float fps = cameraService.GetFrameRate();
                    tbFrameRate.Value = (int)Math.Min(tbFrameRate.Maximum, fps);
                    lblFPS.Text = $"{fps:F1}";

                    tbBrightness.Enabled = cameraService.IsFeatureAvailable("Brightness");
                    tbContrast.Enabled = cameraService.IsFeatureAvailable("Contrast");
                    tbSharpness.Enabled = cameraService.IsFeatureAvailable("Sharpness");
                    tbSaturation.Enabled = cameraService.IsFeatureAvailable("Saturation");
                }
                catch (Exception ex)
                {
                    HandleException(ex, "Loading Camera Parameters");
                }

                // UI Update
                CnctBTN.Text = "DISCONNECT";
                CnctBTN.ForeColor = Color.Red;
                isConnected = true;
                Param_Panel.Visible = true;
                ToolsPanel.Visible = true;
                TabCntl.Visible = true;
            }
            catch (Exception ex)
            {
                HandleException(ex, "Camera Connection");

                // Ensure camera is disconnected if connection fails
                try
                {
                    cameraService.Disconnect();
                }
                catch { /* Ignore */ }
            }
        }

        private List<string> SearchCamera()
        {
            try
            {
                List<string> devices = new List<string>();
                deviceList = new MyCamera.MV_CC_DEVICE_INFO_LIST();

                int result = MyCamera.MV_CC_EnumDevices_NET(
                    MyCamera.MV_GIGE_DEVICE | MyCamera.MV_USB_DEVICE,
                    ref deviceList);

                if (result != MyCamera.MV_OK)
                {
                    return devices;
                }

                for (int i = 0; i < deviceList.nDeviceNum; i++)
                {
                    MyCamera.MV_CC_DEVICE_INFO deviceInfo =
                        (MyCamera.MV_CC_DEVICE_INFO)Marshal.PtrToStructure(
                            deviceList.pDeviceInfo[i],
                            typeof(MyCamera.MV_CC_DEVICE_INFO));

                    if (deviceInfo.nTLayerType == MyCamera.MV_GIGE_DEVICE)
                    {
                        var gigeInfo = (MyCamera.MV_GIGE_DEVICE_INFO)
                            MyCamera.ByteToStruct(deviceInfo.SpecialInfo.stGigEInfo,
                            typeof(MyCamera.MV_GIGE_DEVICE_INFO));

                        string ip = $"{(gigeInfo.nCurrentIp & 0xff)}." +
                                    $"{((gigeInfo.nCurrentIp >> 8) & 0xff)}." +
                                    $"{((gigeInfo.nCurrentIp >> 16) & 0xff)}." +
                                    $"{((gigeInfo.nCurrentIp >> 24) & 0xff)}";

                        string model = gigeInfo.chModelName?.Trim('\0');
                        string name = gigeInfo.chUserDefinedName?.Trim('\0');

                        devices.Add($"Hikvision - {model} - {name} ({ip})");
                    }
                }
                return devices;
            }
            catch (Exception ex)
            {
                HandleException(ex, "Camera Search");
                return new List<string>();
            }
        }

        private void Gain_Click(object sender, EventArgs e)
        {

        }

        private void exposureTrackBar_Scroll_1(object sender, EventArgs e)
        {
            try
            {
                float exposure = exposureTrackBar.Value;
                cameraService.SetExposure(exposure);
                Exp_lbl.Text = $"({exposure:F0} µs)";
            }
            catch (Exception ex)
            {
                HandleException(ex, "Setting Exposure");
            }
        }

        private void gainTrackBar_Scroll(object sender, EventArgs e)
        {
            try
            {
                float gain = gainTrackBar.Value;
                cameraService.SetGain(gain);
                Gain_lbl.Text = $"{gain:F1}";
            }
            catch (Exception ex)
            {
                HandleException(ex, "Setting Gain");
            }
        }

        private void tbBrightness_Scroll(object sender, EventArgs e)
        {
            try  // Add this
            {
                int val = tbBrightness.Value;
                cameraService.SetBrightness(val);
                lblBrightness.Text = val.ToString();
            }
            catch (Exception ex)
            {
                HandleException(ex, "Setting Brightness");
            }
        }

        private void tbContrast_Scroll(object sender, EventArgs e)
        {
            int val = tbContrast.Value;
            cameraService.SetContrast(val);
            lblContrast.Text = val.ToString();
        }

        private void tbSharpness_Scroll(object sender, EventArgs e)
        {
            int val = tbSharpness.Value;
            cameraService.SetSharpness(val);
            lblSharpness.Text = val.ToString();
        }

        private void tbSaturation_Scroll(object sender, EventArgs e)
        {
            int val = tbSaturation.Value;
            cameraService.SetSaturation(val);
            lblSaturation.Text = val.ToString();
        }

        private void tbFrameRate_Scroll(object sender, EventArgs e)
        {
            float val = tbFrameRate.Value;
            cameraService.SetFrameRate(val);
            lblFPS.Text = $"{val:F1}";
        }

        private void cbDisplayMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            currentDisplayMode = (Services.DisplayMode)cbDisplayMode.SelectedIndex;
        }

        private void Param_Panel_Paint(object sender, PaintEventArgs e)
        {

        }

        private async void OCR_btn_Click(object sender, EventArgs e)
        {
            try  // Add overall try-catch
            {
                if (lastFrame == null)
                {
                    MessageBox.Show("No frame available!");
                    return;
                }

                isFrozen = true;

                Bitmap captured = (Bitmap)lastFrame.Clone();
                Bitmap deskewed = imageService.DeskewImage(captured);

                // This UI update is fine (called from UI thread)
                VisualPB.Image?.Dispose();
                VisualPB.Image = (Bitmap)deskewed.Clone();
                var words = await Task.Run(() => ocrService.ReadWordsWithConfidence(deskewed));

                // 🔥 Draw overlay
                Bitmap overlay = ocrService.DrawConfidenceOverlay(deskewed, words);

                VisualPB.Image?.Dispose();
                VisualPB.Image = overlay;

                string formattedText = ocrService.ReconstructText(words);
                txtOCRResult.Text = formattedText;
                OCR_Panel.Visible = true;


                deskewed.Dispose();
                captured.Dispose();
            }
            catch (Exception ex)
            {
                HandleException(ex, "OCR Processing");
                isFrozen = false; // Reset frozen state on error
            }
        }

        private void Resume_btn_Click(object sender, EventArgs e)
        {
            isFrozen = false;
            OCR_Panel.Visible = false;
        }

        private void HandleException(Exception ex, string context = "")
        {
            if (isHandlingException) return; // Prevent recursive calls
            isHandlingException = true;

            try
            {
                string errorMsg = $"Error in {context}:\n{ex.Message}\n\n{ex.StackTrace}";

                // Log to file
                try
                {
                    string logPath = Path.Combine(Application.StartupPath, "error_log.txt");
                    File.AppendAllText(logPath, $"{DateTime.Now}: {errorMsg}\n\n");
                }
                catch { /* Ignore logging errors */ }

                // Show message box (use Invoke if needed)
                if (InvokeRequired)
                {
                    Invoke(new Action(() => MessageBox.Show(errorMsg, "Camera Error", MessageBoxButtons.OK, MessageBoxIcon.Error)));
                }
                else
                {
                    MessageBox.Show(errorMsg, "Camera Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                // Try to disconnect camera if connected
                if (isConnected)
                {
                    try
                    {
                        cameraService.Disconnect();
                        isConnected = false;

                        if (InvokeRequired)
                        {
                            Invoke(new Action(() =>
                            {
                                VisualPB.Image?.Dispose();
                                Image gif = Properties.Resources.No_Data_Founds;
                                VisualPB.Image = gif;
                                ImageAnimator.Animate(gif, (s, ev) => { VisualPB.Invalidate(); });
                                CnctBTN.Text = "CONNECT";
                                CnctBTN.ForeColor = Color.SeaGreen;
                                Param_Panel.Visible = false;
                                ToolsPanel.Visible = false;
                            }));
                        }
                        else
                        {
                            VisualPB.Image?.Dispose();
                            Image gif = Properties.Resources.No_Data_Founds;
                            VisualPB.Image = gif;
                            ImageAnimator.Animate(gif, (s, ev) => { VisualPB.Invalidate(); });
                            CnctBTN.Text = "CONNECT";
                            CnctBTN.ForeColor = Color.SeaGreen;
                            Param_Panel.Visible = false;
                            ToolsPanel.Visible = false;
                        }
                    }
                    catch (Exception disconnectEx)
                    {
                        MessageBox.Show($"Error during disconnect: {disconnectEx.Message}");
                    }
                }
            }
            finally
            {
                isHandlingException = false;
            }
        }

        private void CD_Btn_Click(object sender, EventArgs e)
        {
            if (lastFrame == null)
            {
                MessageBox.Show("No frame available!");
                return;
            }

            isFrozen = true;
            isTeachMode = true;
            isInspectionMode = false;
            CD_Panel.Visible = true;

            dgvCD.Rows.Clear();
            roiCounter = 1;
            ClearCD_btn.Visible = true;

            VisualPB.Cursor = Cursors.Cross;
        }

        private void VisualPB_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                if (isCalibrationMode)
                {
                    calibPoints.Add(e.Location);

                    if (calibPoints.Count == 1)
                    {
                        Res_CD_Lbl.Text = "Select Point 2";
                    }
                    else if (calibPoints.Count == 2)
                    {
                        CalculatePixelToMM();
                    }

                    VisualPB.Invalidate();
                    return;
                }

                if (!isTeachMode && !isObjectTeachMode) return;

                isDrawing = true;
                startPoint = e.Location;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error starting ROI draw: {ex.Message}");
            }

        }

        private void VisualPB_MouseMove(object sender, MouseEventArgs e)
        {
            try
            {
                if (!isDrawing) return;

                int x = Math.Max(0, Math.Min(startPoint.X, e.X));
                int y = Math.Max(0, Math.Min(startPoint.Y, e.Y));

                int w = Math.Abs(startPoint.X - e.X);
                int h = Math.Abs(startPoint.Y - e.Y);

                if (w < 2 || h < 2) return;

                currentROI = new Rectangle(x, y, w, h);

                VisualPB.Invalidate();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error during ROI drawing: {ex.Message}");
            }
        }

        private void VisualPB_MouseUp(object sender, MouseEventArgs e)
        {
            if (!isTeachMode && !isObjectTeachMode) return;

            isDrawing = false;

            if (isObjectTeachMode)
            {
                string label = Prompt.ShowDialog("Enter Object Name:", "Object Label");

                if (string.IsNullOrWhiteSpace(label))
                    label = "Object";
                track_btn.Visible = true;
                ClearOD_btn.Visible = true;
                AddObjectROI(label);
            }
            else if (isTeachMode)
            {
                checkCD_btn.Visible = true;
                ClearCD_btn.Visible = true;

                AddROIAndTeach();
            }
        }

        private void AddObjectROI(string label)
        {
            if (lastFrame == null || currentROI.Width < 5 || currentROI.Height < 5)
                return;

            Bitmap bmp;
            lock (frameLock)
            {
                bmp = (Bitmap)lastFrame.Clone();
            }

            float scaleX = (float)bmp.Width / VisualPB.Width;
            float scaleY = (float)bmp.Height / VisualPB.Height;

            Rectangle realROI = new Rectangle(
                (int)(currentROI.X * scaleX),
                (int)(currentROI.Y * scaleY),
                (int)(currentROI.Width * scaleX),
                (int)(currentROI.Height * scaleY)
            );

            taughtCenter = new Point(
                realROI.X + realROI.Width / 2,
                realROI.Y + realROI.Height / 2
            );

            objRois.Add(currentROI);
            objLabels.Add(label);

            // 🔥 IMPORTANT → Save template for tracking
            objService.SetTemplate(bmp, realROI);

            currentROI = Rectangle.Empty;

            // 🔥 IMPORTANT FIX
            isFrozen = false;   // resume live camera

            isObjectTeachMode = false; // stop teaching mode

            VisualPB.Invalidate();
        }

        private void VisualPB_Paint(object sender, PaintEventArgs e)
        {
            for (int i = 0; i < roiList.Count; i++)
            {
                var roi = roiList[i];
                var label = roiLabels[i];

                Color borderColor = Color.Lime;

                if (i < roiResults.Count)
                {
                    borderColor = roiResults[i] ? Color.Lime : Color.Red;
                }

                using (Pen pen = new Pen(borderColor, 2))
                    e.Graphics.DrawRectangle(pen, roi);

                using (Font font = new Font("Segoe UI", 7))
                using (Brush textBrush = new SolidBrush(borderColor))
                {
                    string resultText = roiResults[i] ? "PASS" : "FAIL";
                    e.Graphics.DrawString($"{label} ({resultText})", font, textBrush, roi.X, roi.Y - 12);
                }
            }

            if (calibPoints.Count > 0)
            {
                using (Pen pen = new Pen(Color.Yellow, 2))
                using (Brush brush = new SolidBrush(Color.Yellow))
                {
                    foreach (var p in calibPoints)
                    {
                        e.Graphics.FillEllipse(brush, p.X - 4, p.Y - 4, 8, 8);
                    }

                    if (calibPoints.Count == 2)
                    {
                        e.Graphics.DrawLine(pen, calibPoints[0], calibPoints[1]);
                    }
                }
            }

            // 🔵 Draw current ROI while dragging
            if (isDrawing && currentROI != Rectangle.Empty)
            {
                using (Pen pen = new Pen(Color.Blue, 2))
                {
                    e.Graphics.DrawRectangle(pen, currentROI);
                }
            }
        }

        private void AddROIAndTeach()
        {
            try
            {
                if (lastFrame == null || currentROI.Width < 5 || currentROI.Height < 5)
                    return;

                Bitmap bmp;
                lock (frameLock)
                {
                    if (lastFrame == null) return;
                    bmp = (Bitmap)lastFrame.Clone();
                }

                float scaleX = (float)bmp.Width / VisualPB.Width;
                float scaleY = (float)bmp.Height / VisualPB.Height;

                Rectangle realROI = new Rectangle(
                    (int)(currentROI.X * scaleX),
                    (int)(currentROI.Y * scaleY),
                    (int)(currentROI.Width * scaleX),
                    (int)(currentROI.Height * scaleY)
                );

                realROI = ClampRectangle(realROI, bmp.Width, bmp.Height);

                if (realROI.Width <= 0 || realROI.Height <= 0)
                    return;

                Color color = GetDominantColorSafe(bmp, realROI);

                // ✅ STORE MULTIPLE
                roiList.Add(currentROI);
                taughtColors.Add(color);
                string roiName = $"A{roiCounter++}";

                roiLabels.Add(roiName);
                roiResults.Add(true);

                // ✅ Add to TABLE
                dgvCD.Rows.Add(
                    roiName,
                    $"R:{color.R} G:{color.G} B:{color.B}",
                    "PASS"
                );

                // reset current ROI so next draw starts fresh
                currentROI = Rectangle.Empty;

                isFrozen = true;
                isTeachMode = true; // 🔥 KEEP DRAW MODE ACTIVE

                VisualPB.Invalidate();
            }
            catch (Exception ex)
            {
                HandleException(ex, "Multi ROI Teach");
            }
        }

        private Color GetDominantColorSafe(Bitmap bmp, Rectangle roi)
        {
            long r = 0, g = 0, b = 0;
            int count = 0;

            for (int y = roi.Top; y < roi.Bottom; y++)
            {
                if (y < 0 || y >= bmp.Height) continue;

                for (int x = roi.Left; x < roi.Right; x++)
                {
                    if (x < 0 || x >= bmp.Width) continue;

                    Color pixel = bmp.GetPixel(x, y);

                    r += pixel.R;
                    g += pixel.G;
                    b += pixel.B;

                    count++;
                }
            }

            if (count == 0) return Color.Black;

            return Color.FromArgb(
                (int)(r / count),
                (int)(g / count),
                (int)(b / count)
            );
        }

        private double ColorDistance(Color c1, Color c2)
        {
            int dr = c1.R - c2.R;
            int dg = c1.G - c2.G;
            int db = c1.B - c2.B;

            return Math.Sqrt(dr * dr + dg * dg + db * db);
        }

        private Rectangle ClampRectangle(Rectangle rect, int maxW, int maxH)
        {
            int x = Math.Max(0, rect.X);
            int y = Math.Max(0, rect.Y);

            int w = Math.Min(rect.Width, maxW - x);
            int h = Math.Min(rect.Height, maxH - y);

            return new Rectangle(x, y, w, h);
        }

        private void checkCD_btn_Click(object sender, EventArgs e)
        {
            try
            {
                if (lastFrame == null || roiList.Count == 0)
                {
                    MessageBox.Show("No ROI or frame available!");
                    return;
                }

                // 🔒 Freeze current frame
                isFrozen = false;
                bool overallPass = true;

                Bitmap bmp;
                lock (frameLock)
                {
                    bmp = (Bitmap)lastFrame.Clone();
                }

                for (int i = 0; i < roiList.Count; i++)
                {
                    Rectangle roi = MapToImageROIForList(bmp, roiList[i]);

                    Color currentColor = GetDominantColorSafe(bmp, roi);
                    double diff = ColorDistance(currentColor, taughtColors[i]);

                    bool isPass = diff < 40;
                    roiResults[i] = isPass;
                    dgvCD.Rows[i].Cells[2].Value = isPass ? "PASS" : "FAIL";

                    // Optional color highlight
                    dgvCD.Rows[i].Cells[2].Style.ForeColor = isPass ? Color.Green : Color.Red;
                    if (!isPass)
                        overallPass = false;

                    using (Graphics g = Graphics.FromImage(bmp))
                    {
                        Color overlayColor = isPass ? Color.Lime : Color.Red;

                        using (Pen pen = new Pen(overlayColor, 2))
                            g.DrawRectangle(pen, roi);
                    }
                }

                Res_CD_Lbl.Text = overallPass ? "Result : PASS" : "Result : FAIL";
                Res_CD_Lbl.ForeColor = overallPass ? Color.Green : Color.Red;
                VisualPB.Invalidate();
                VisualPB.Image?.Dispose();
                VisualPB.Image = bmp;
            }
            catch (Exception ex)
            {
                HandleException(ex, "Manual Inspection");
            }
        }

        private Rectangle MapToImageROIForList(Bitmap bmp, Rectangle roiPB)
        {
            float scaleX = (float)bmp.Width / VisualPB.Width;
            float scaleY = (float)bmp.Height / VisualPB.Height;

            return ClampRectangle(new Rectangle(
                (int)(roiPB.X * scaleX),
                (int)(roiPB.Y * scaleY),
                (int)(roiPB.Width * scaleX),
                (int)(roiPB.Height * scaleY)
            ), bmp.Width, bmp.Height);
        }

        private void ClearCD_btn_Click(object sender, EventArgs e)
        {
            clearCD();
        }

        private void clearCD()
        {

            try
            {
                // 🔥 Reset all color detection data
                roiList.Clear();
                taughtColors.Clear();
                roiLabels.Clear();
                currentROI = Rectangle.Empty;

                isTeachMode = false;
                roiResults.Clear();
                isInspectionMode = false;
                isDrawing = false;

                // 🔓 Resume live camera
                isFrozen = false;
                dgvCD.Rows.Clear();
                roiCounter = 1;

                // ✅ HIDE PANEL
                CD_Panel.Visible = false;

                checkCD_btn.Visible = false;
                ClearCD_btn.Visible = false;

                Res_CD_Lbl.Text = "";               // ✅ clear completely
                Res_CD_Lbl.ForeColor = Color.Black; // optional reset

                VisualPB.Cursor = Cursors.Default;

                // 🔄 Force redraw (removes rectangle + text)
                VisualPB.Invalidate();

                // 🧹 OPTIONAL: clear current image overlay (reload last frame cleanly)
                if (lastFrame != null)
                {
                    lock (frameLock)
                    {
                        VisualPB.Image?.Dispose();
                        VisualPB.Image = (Bitmap)lastFrame.Clone();
                    }
                }
            }
            catch (Exception ex)
            {
                HandleException(ex, "Clear Color Detection");
            }
        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void objDec_btn_Click(object sender, EventArgs e)
        {
            if (lastFrame == null)
            {
                MessageBox.Show("No frame available!");
                return;
            }

            //isFrozen = true;

            isObjectTeachMode = true;
            isTrackingMode = false;
            isTeachMode = false;   // 🔥 IMPORTANT

            VisualPB.Cursor = Cursors.Cross;
        }

        private void track_btn_Click(object sender, EventArgs e)
        {
            if (objLabels.Count == 0)
            {
                MessageBox.Show("No object trained!");
                return;
            }

            isTrackingMode = true;
            isFrozen = false;
        }

        public static class Prompt
        {
            public static string ShowDialog(string text, string caption)
            {
                Form prompt = new Form()
                {
                    Width = 300,
                    Height = 150,
                    Text = caption
                };

                Label lbl = new Label() { Left = 10, Top = 10, Text = text };
                TextBox input = new TextBox() { Left = 10, Top = 40, Width = 260 };

                Button ok = new Button() { Text = "OK", Left = 200, Width = 70, Top = 70 };
                ok.Click += (sender, e) => { prompt.Close(); };

                prompt.Controls.Add(lbl);
                prompt.Controls.Add(input);
                prompt.Controls.Add(ok);

                prompt.ShowDialog();

                return input.Text;
            }
        }

        private void ClearOD_btn_Click(object sender, EventArgs e)
        {
            clearOD();
        }

        private void clearOD()
        {
            try
            {
                // 🔥 Clear object data
                objRois.Clear();
                objLabels.Clear();

                // 🔥 Reset tracking/template
                objService = new ObjectDetectionService();

                // 🔥 Reset modes
                isObjectTeachMode = false;
                isTrackingMode = false;
                isDrawing = false;

                // =========================
                // 🔥 NEW: RESET CALIBRATION
                // =========================
                isCalibrationMode = false;
                calibPoints.Clear();

                pixelToMM = 0.0; // or default like 0.05 if you prefer

                // 🔓 Resume live camera
                isFrozen = false;

                // 🔥 Clear label
                Res_CD_Lbl.Text = "";
                Res_CD_Lbl.ForeColor = Color.Black;

                // 🎯 Reset UI
                track_btn.Visible = false;
                ClearOD_btn.Visible = false;

                VisualPB.Cursor = Cursors.Default;

                // 🔄 Refresh screen
                VisualPB.Invalidate();

                // 🧹 Reload clean frame
                if (lastFrame != null)
                {
                    lock (frameLock)
                    {
                        VisualPB.Image?.Dispose();
                        VisualPB.Image = (Bitmap)lastFrame.Clone();
                    }
                }
            }
            catch (Exception ex)
            {
                HandleException(ex, "Clear Object Detection");
            }
        }

        private void dgvCD_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void Res_btn_Click(object sender, EventArgs e)
        {
            if (lastFrame == null)
            {
                MessageBox.Show("No frame available!");
                return;
            }

            isFrozen = true;
            isCalibrationMode = true;
            calibPoints.Clear();

            VisualPB.Cursor = Cursors.Cross;
            ClearOD_btn.Visible = true;

            Res_CD_Lbl.Text = "Select Point 1";
        }

        private void CalculatePixelToMM()
        {
            if (calibPoints.Count < 2) return;

            Point p1 = calibPoints[0];
            Point p2 = calibPoints[1];

            double dx = p2.X - p1.X;
            double dy = p2.Y - p1.Y;

            double pixelDistance = Math.Sqrt(dx * dx + dy * dy);

            // 🔥 Ask user for real distance
            string input = Prompt.ShowDialog("Enter real distance (mm):", "Calibration");

            if (!double.TryParse(input, out double realDistance) || realDistance <= 0)
            {
                MessageBox.Show("Invalid input!");
                return;
            }

            pixelToMM = realDistance / pixelDistance;

            Res_CD_Lbl.Text = $"pixelToMM = {pixelToMM:F5} mm/pixel";

            // 🔥 Reset mode
            isCalibrationMode = false;
            VisualPB.Cursor = Cursors.Default;
        }
    }
}
