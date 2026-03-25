namespace VisioNeo_App
{
    using MvCamCtrl.NET;
    using System.Runtime.InteropServices;
    using VisioNeo_App.Services;

    public partial class VisioNeo : Form
    {
        private CameraService cameraService = new CameraService();
        private ImageProcessingService imageService = new ImageProcessingService();
        private Services.DisplayMode currentDisplayMode = Services.DisplayMode.Normal;
        MyCamera.MV_CC_DEVICE_INFO_LIST deviceList;
        bool isConnected = false;

        public VisioNeo()
        {
            InitializeComponent();
            LoadingPB.Visible = false;
            CnctBTN.Visible = false;
            Param_Panel.Visible = false;
            devListTBox.Visible = false;
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


        private void CnctBTN_Click_1(object sender, EventArgs e)
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

                        ImageAnimator.Animate(gif, (s, ev) =>
                        {
                            VisualPB.Invalidate();
                        });
                    });


                    // UI Update
                    CnctBTN.Text = "CONNECT";
                    CnctBTN.ForeColor = Color.SeaGreen;

                    isConnected = false;
                    Param_Panel.Visible = false;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
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

                cameraService.StartGrabbing(frame =>
                {
                    if (!cameraService.IsConnected) return;

                    Bitmap finalImage = imageService.Process(frame, currentDisplayMode);

                    VisualPB.Invoke(() =>
                    {
                        if (!cameraService.IsConnected) return;
                        VisualPB.Image?.Dispose();
                        VisualPB.Image = finalImage;
                    });
                });

                // 🔥 LOAD PARAMETERS FROM CAMERA
                float currentExposure = cameraService.GetExposure();

                // Example range (adjust as per your camera)
                exposureTrackBar.Minimum = 10000;
                exposureTrackBar.Maximum = 1000000;

                // Clamp (safety)
                currentExposure = Math.Max(exposureTrackBar.Minimum, currentExposure);
                currentExposure = Math.Min(exposureTrackBar.Maximum, currentExposure);

                exposureTrackBar.Value = (int)currentExposure;

                Exp_lbl.Text = $"{currentExposure:F0}";

                float currentGain = cameraService.GetGain();

                // Typical Hikvision range (adjust if needed)
                gainTrackBar.Minimum = 0;
                gainTrackBar.Maximum = 20;

                // Clamp
                currentGain = Math.Max(gainTrackBar.Minimum, currentGain);
                currentGain = Math.Min(gainTrackBar.Maximum, currentGain);

                gainTrackBar.Value = (int)currentGain;

                Gain_lbl.Text = $"{currentGain:F1}";

                // Optional params (may not work on Hikvision)
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

                // UI Update
                CnctBTN.Text = "DISCONNECT";
                CnctBTN.ForeColor = Color.Red;

                isConnected = true;
                Param_Panel.Visible = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private List<string> SearchCamera()
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

        private void Gain_Click(object sender, EventArgs e)
        {

        }

        private void exposureTrackBar_Scroll_1(object sender, EventArgs e)
        {
            float exposure = exposureTrackBar.Value;
            cameraService.SetExposure(exposure);
            Exp_lbl.Text = $"({exposure:F0} µs)";
        }

        private void gainTrackBar_Scroll(object sender, EventArgs e)
        {
            float gain = gainTrackBar.Value;
            cameraService.SetGain(gain);
            Gain_lbl.Text = $"{gain:F1}";
        }


        private void tbBrightness_Scroll(object sender, EventArgs e)
        {
            int val = tbBrightness.Value;
            cameraService.SetBrightness(val);
            lblBrightness.Text = val.ToString();
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
    }
}
