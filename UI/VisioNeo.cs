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
                        MyCamera.MV_PIXEL_CONVERT_PARAM convert = new MyCamera.MV_PIXEL_CONVERT_PARAM();

                        convert.nWidth = frame.stFrameInfo.nWidth;
                        convert.nHeight = frame.stFrameInfo.nHeight;
                        convert.pSrcData = frame.pBufAddr;
                        convert.nSrcDataLen = frame.stFrameInfo.nFrameLen;
                        convert.enSrcPixelType = frame.stFrameInfo.enPixelType;
                        convert.enDstPixelType = MyCamera.MvGvspPixelType.PixelType_Gvsp_BGR8_Packed;

                        int bufferSize = convert.nWidth * convert.nHeight * 3;
                        byte[] rgbBuffer = new byte[bufferSize];

                        GCHandle handle = GCHandle.Alloc(rgbBuffer, GCHandleType.Pinned);
                        convert.pDstBuffer = handle.AddrOfPinnedObject();
                        convert.nDstBufferSize = (uint)bufferSize;

                        camera.MV_CC_ConvertPixelType_NET(ref convert);

                        Bitmap bmp = new Bitmap(
                            convert.nWidth,
                            convert.nHeight,
                            convert.nWidth * 3,
                            PixelFormat.Format24bppRgb,
                            convert.pDstBuffer
                        );

                        VisualPB.Invoke(new Action(() =>
                        {
                            if (!isConnected) return;

                            VisualPB.Image?.Dispose();
                            VisualPB.Image = (Bitmap)bmp.Clone();
                        }));

                        handle.Free();
                    }
                    catch { }

                    camera.MV_CC_FreeImageBuffer_NET(ref frame);
                }
            }
        }

    }
}
