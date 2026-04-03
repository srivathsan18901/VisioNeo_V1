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
        private string lastDetectedText = "";
        private Bitmap lastFrame = null;
        private bool isFrozen = false;
        private bool isHandlingException = false;

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

                        Bitmap finalImage = imageService.Process(frame, currentDisplayMode);

                        lastFrame?.Dispose();
                        lastFrame = (Bitmap)finalImage.Clone();

                        VisualPB.Invoke(() =>
                        {
                            try
                            {
                                VisualPB.Image?.Dispose();
                                VisualPB.Image = finalImage;
                            }
                            catch (Exception ex)
                            {
                                // Handle UI update errors silently or log them
                                System.Diagnostics.Debug.WriteLine($"UI Update Error: {ex.Message}");
                            }
                        });
                    }
                    catch (Exception ex)
                    {
                        // Critical error in frame processing - disconnect
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

        private void OCV_btn_Click(object sender, EventArgs e)
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

                string text = await Task.Run(() => ocrService.ReadText(deskewed));

                // This runs on UI thread after await, so it's safe
                txtOCRResult.Text = text;

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

    }
}
