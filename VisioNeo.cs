namespace VisioNeo_App
{
    using MvCamCtrl.NET;
    using System.Drawing.Imaging;
    using System.Runtime.InteropServices;

    public partial class VisioNeo : Form
    {

        MyCamera camera = new MyCamera();
        MyCamera.MV_CC_DEVICE_INFO_LIST deviceList;
        Thread grabThread;
        bool isGrabbing = false;
        bool isConnected = false;


        public VisioNeo()
        {
            InitializeComponent();
            LoadingPB.Visible = false;
            CnctBTN.Visible = false;
            devListTBox.Visible = false;
            VisualPB.Image = Properties.Resources.No_Data_Founds;
            VisualPB.SizeMode = PictureBoxSizeMode.StretchImage;
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
                    isGrabbing = false;

                    grabThread?.Join(500);

                    camera.MV_CC_StopGrabbing_NET();
                    camera.MV_CC_CloseDevice_NET();
                    camera.MV_CC_DestroyDevice_NET();

                    // Show GIF
                    VisualPB.Image = new Bitmap(Properties.Resources.No_Data_Founds);

                    ImageAnimator.Animate(VisualPB.Image, (s, ev) =>
                    {
                        VisualPB.Invalidate();
                    });

                    // UI Update
                    CnctBTN.Text = "CONNECT";
                    CnctBTN.ForeColor = Color.SeaGreen;

                    isConnected = false;
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

                int result;

                result = camera.MV_CC_CreateDevice_NET(ref deviceInfo);
                if (result != MyCamera.MV_OK)
                {
                    MessageBox.Show("Create handle failed");
                    return;
                }

                result = camera.MV_CC_OpenDevice_NET();
                if (result != MyCamera.MV_OK)
                {
                    MessageBox.Show("Open device failed");
                    return;
                }

                camera.MV_CC_SetEnumValue_NET("TriggerMode", 0);

                camera.MV_CC_StartGrabbing_NET();

                isGrabbing = true;
                grabThread = new Thread(GrabLoop);
                grabThread.IsBackground = true;
                grabThread.Start();

                // UI Update
                CnctBTN.Text = "DISCONNECT";
                CnctBTN.ForeColor = Color.Red;

                isConnected = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void GrabLoop()
        {
            MyCamera.MV_FRAME_OUT frame = new MyCamera.MV_FRAME_OUT();

            while (isGrabbing)
            {
                int result = camera.MV_CC_GetImageBuffer_NET(ref frame, 1000);

                if (result == MyCamera.MV_OK)
                {
                    try
                    {
                        int width = frame.stFrameInfo.nWidth;
                        int height = frame.stFrameInfo.nHeight;

                        byte[] buffer = new byte[frame.stFrameInfo.nFrameLen];
                        Marshal.Copy(frame.pBufAddr, buffer, 0, buffer.Length);

                        Bitmap bmp = new Bitmap(width, height, PixelFormat.Format8bppIndexed);

                        // Set grayscale palette
                        ColorPalette palette = bmp.Palette;
                        for (int i = 0; i < 256; i++)
                        {
                            palette.Entries[i] = Color.FromArgb(i, i, i);
                        }
                        bmp.Palette = palette;

                        BitmapData bmpData = bmp.LockBits(
                            new Rectangle(0, 0, width, height),
                            ImageLockMode.WriteOnly,
                            bmp.PixelFormat);

                        Marshal.Copy(buffer, 0, bmpData.Scan0, buffer.Length);
                        bmp.UnlockBits(bmpData);

                        VisualPB.Invoke(new Action(() =>
                        {
                            if (!isConnected) return; // 🔥 IMPORTANT FIX

                            VisualPB.Image?.Dispose();
                            VisualPB.Image = bmp;
                        }));
                    }
                    catch { }

                    camera.MV_CC_FreeImageBuffer_NET(ref frame);
                }
            }
        }

    }
}
