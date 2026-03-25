using MvCamCtrl.NET;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace VisioNeo_App.Services
{
    public class CameraService
    {
        private MyCamera camera = new MyCamera();
        private Thread grabThread;
        private bool isGrabbing = false;

        public bool IsConnected { get; private set; }

        // =========================
        // CONNECT
        // =========================
        public bool Connect(MyCamera.MV_CC_DEVICE_INFO deviceInfo)
        {
            int result;

            result = camera.MV_CC_CreateDevice_NET(ref deviceInfo);
            if (result != MyCamera.MV_OK) return false;

            result = camera.MV_CC_OpenDevice_NET();
            if (result != MyCamera.MV_OK) return false;

            camera.MV_CC_SetEnumValue_NET("TriggerMode", 0);

            IsConnected = true;
            return true;
        }

        // =========================
        // DISCONNECT
        // =========================
        public void Disconnect()
        {
            if (!IsConnected) return;

            isGrabbing = false;
            grabThread?.Join(500);

            camera.MV_CC_StopGrabbing_NET();
            camera.MV_CC_CloseDevice_NET();
            camera.MV_CC_DestroyDevice_NET();

            IsConnected = false;

        }

        // =========================
        // START GRABBING
        // =========================
        public void StartGrabbing(Action<Bitmap> onFrame)
        {
            if (!IsConnected) return;

            camera.MV_CC_StartGrabbing_NET();

            isGrabbing = true;

            grabThread = new Thread(() =>
            {
                GrabLoop(onFrame);
            });

            grabThread.IsBackground = true;
            grabThread.Start();
        }

        // =========================
        // GRAB LOOP
        // =========================
        private void GrabLoop(Action<Bitmap> onFrame)
        {
            MyCamera.MV_FRAME_OUT frame = new MyCamera.MV_FRAME_OUT();

            while (isGrabbing)
            {
                int result = camera.MV_CC_GetImageBuffer_NET(ref frame, 1000);

                if (result == MyCamera.MV_OK)
                {
                    try
                    {
                        Bitmap bmp = ConvertToBitmap(frame);

                        onFrame?.Invoke(bmp);
                    }
                    catch { }

                    camera.MV_CC_FreeImageBuffer_NET(ref frame);
                }
            }
        }

        // =========================
        // CONVERT FRAME → BITMAP
        // =========================
        private Bitmap ConvertToBitmap(MyCamera.MV_FRAME_OUT frame)
        {
            MyCamera.MV_PIXEL_CONVERT_PARAM convert = new MyCamera.MV_PIXEL_CONVERT_PARAM();

            convert.nWidth = frame.stFrameInfo.nWidth;
            convert.nHeight = frame.stFrameInfo.nHeight;
            convert.pSrcData = frame.pBufAddr;
            convert.nSrcDataLen = frame.stFrameInfo.nFrameLen;
            convert.enSrcPixelType = frame.stFrameInfo.enPixelType;

            convert.enDstPixelType = MyCamera.MvGvspPixelType.PixelType_Gvsp_BGR8_Packed;

            int bufferSize = convert.nWidth * convert.nHeight * 3;
            byte[] buffer = new byte[bufferSize];

            GCHandle handle = GCHandle.Alloc(buffer, GCHandleType.Pinned);

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

            Bitmap final = (Bitmap)bmp.Clone();

            handle.Free();

            return final;
        }

        // =========================
        // PARAMETERS
        // =========================
        public void SetExposure(float value)
        {
            camera.MV_CC_SetEnumValue_NET("ExposureAuto", 0);
            camera.MV_CC_SetFloatValue_NET("ExposureTime", value);
        }

        public float GetExposure()
        {
            MyCamera.MVCC_FLOATVALUE val = new MyCamera.MVCC_FLOATVALUE();
            camera.MV_CC_GetFloatValue_NET("ExposureTime", ref val);
            return val.fCurValue;
        }

        public void SetGain(float value)
        {
            camera.MV_CC_SetFloatValue_NET("Gain", value);
        }

        public float GetGain()
        {
            MyCamera.MVCC_FLOATVALUE val = new MyCamera.MVCC_FLOATVALUE();
            camera.MV_CC_GetFloatValue_NET("Gain", ref val);
            return val.fCurValue;
        }

        public bool IsFeatureAvailable(string feature)
        {
            MyCamera.MVCC_INTVALUE val = new MyCamera.MVCC_INTVALUE();
            int res = camera.MV_CC_GetIntValue_NET(feature, ref val);
            return res == MyCamera.MV_OK;
        }

        public void SetBrightness(int value)
        {
            camera.MV_CC_SetIntValue_NET("Brightness", (uint)value);
        }

        public int GetBrightness()
        {
            MyCamera.MVCC_INTVALUE val = new MyCamera.MVCC_INTVALUE();
            int res = camera.MV_CC_GetIntValue_NET("Brightness", ref val);

            return res == MyCamera.MV_OK ? (int)val.nCurValue : 0;
        }

        public void SetContrast(int value)
        {
            camera.MV_CC_SetIntValue_NET("Contrast", (uint)value);
        }

        public int GetContrast()
        {
            MyCamera.MVCC_INTVALUE val = new MyCamera.MVCC_INTVALUE();
            int res = camera.MV_CC_GetIntValue_NET("Contrast", ref val);

            return res == MyCamera.MV_OK ? (int)val.nCurValue : 0;
        }

        public void SetSharpness(int value)
        {
            camera.MV_CC_SetIntValue_NET("Sharpness", (uint)value);
        }

        public int GetSharpness()
        {
            MyCamera.MVCC_INTVALUE val = new MyCamera.MVCC_INTVALUE();
            int res = camera.MV_CC_GetIntValue_NET("Sharpness", ref val);

            return res == MyCamera.MV_OK ? (int)val.nCurValue : 0;
        }

        public void SetSaturation(int value)
        {
            camera.MV_CC_SetIntValue_NET("Saturation", (uint)value);
        }

        public int GetSaturation()
        {
            MyCamera.MVCC_INTVALUE val = new MyCamera.MVCC_INTVALUE();
            int res = camera.MV_CC_GetIntValue_NET("Saturation", ref val);

            return res == MyCamera.MV_OK ? (int)val.nCurValue : 0;
        }

        public void SetFrameRate(float value)
        {
            camera.MV_CC_SetBoolValue_NET("AcquisitionFrameRateEnable", true);
            camera.MV_CC_SetFloatValue_NET("AcquisitionFrameRate", value);
        }

        public float GetFrameRate()
        {
            MyCamera.MVCC_FLOATVALUE val = new MyCamera.MVCC_FLOATVALUE();
            int res = camera.MV_CC_GetFloatValue_NET("AcquisitionFrameRate", ref val);

            return res == MyCamera.MV_OK ? val.fCurValue : 0;
        }
    }
}