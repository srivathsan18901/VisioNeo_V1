using OpenCvSharp;
using OpenCvSharp.Extensions;

namespace VisioNeo_App.Services
{
    public class ObjectDetectionService
    {
        // Store template for tracking
        public Mat Template { get; private set; }

        public void SetTemplate(Bitmap bmp, Rectangle roi)
        {
            Mat mat = BitmapConverter.ToMat(bmp);

            Rect rect = new Rect(roi.X, roi.Y, roi.Width, roi.Height);
            rect = ClampRect(rect, mat.Width, mat.Height);

            Template = new Mat(mat, rect);
        }

        public Rectangle MatchTemplate(Bitmap frame)
        {
            if (Template == null) return Rectangle.Empty;

            Mat source = BitmapConverter.ToMat(frame);
            Mat result = new Mat();

            Cv2.MatchTemplate(source, Template, result, TemplateMatchModes.CCoeffNormed);

            Cv2.MinMaxLoc(result, out _, out double maxVal, out _, out OpenCvSharp.Point maxLoc);

            if (maxVal < 0.6) return Rectangle.Empty; // threshold

            return new Rectangle(maxLoc.X, maxLoc.Y, Template.Width, Template.Height);
        }

        private Rect ClampRect(Rect rect, int maxW, int maxH)
        {
            int x = Math.Max(0, rect.X);
            int y = Math.Max(0, rect.Y);

            int w = Math.Min(rect.Width, maxW - x);
            int h = Math.Min(rect.Height, maxH - y);

            return new Rect(x, y, w, h);
        }
    }
}