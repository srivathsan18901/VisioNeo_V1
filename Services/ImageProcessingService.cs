using OpenCvSharp;
using OpenCvSharp.Extensions;
using System.Drawing.Imaging;

namespace VisioNeo_App.Services
{
    public enum DisplayMode
    {
        Normal,
        Grayscale,
        Heatmap
    }

    public class ImageProcessingService
    {
        public Bitmap Process(Bitmap input, DisplayMode mode)
        {
            try
            {
                if (input == null)
                    throw new ArgumentNullException(nameof(input), "Input image cannot be null");

                return mode switch
                {
                    DisplayMode.Grayscale => ConvertToGrayscale(input),
                    DisplayMode.Heatmap => ConvertToHeatmap(input),
                    DisplayMode.Normal => (Bitmap)input.Clone(),
                    _ => (Bitmap)input.Clone()
                };
            }
            catch (Exception ex)
            {
                throw new Exception($"Image processing failed for mode {mode}: {ex.Message}", ex);
            }
        }

        public Bitmap DeskewImage(Bitmap input)
        {
            if (input == null)
                throw new ArgumentNullException(nameof(input), "Input image cannot be null");

            try
            {
                using (Mat src = BitmapConverter.ToMat(input))
                using (Mat gray = new Mat())
                using (Mat binary = new Mat())
                using (Mat nonZero = new Mat())
                {
                    // Convert to grayscale
                    Cv2.CvtColor(src, gray, ColorConversionCodes.BGR2GRAY);

                    // Threshold
                    Cv2.Threshold(gray, binary, 0, 255,
                        ThresholdTypes.BinaryInv | ThresholdTypes.Otsu);

                    // Find non-zero pixels
                    Cv2.FindNonZero(binary, nonZero);

                    if (nonZero.Empty())
                        return (Bitmap)input.Clone();

                    // Directly use Mat (NO conversion to Point[])
                    RotatedRect box = Cv2.MinAreaRect(nonZero);

                    double angle = box.Angle;

                    if (angle < -45)
                        angle += 90;

                    // Rotate image
                    Point2f center = new Point2f(src.Width / 2f, src.Height / 2f);
                    Mat rotationMatrix = Cv2.GetRotationMatrix2D(center, angle, 1.0);

                    Mat rotated = new Mat();
                    Cv2.WarpAffine(src, rotated, rotationMatrix, src.Size(),
                        InterpolationFlags.Linear,
                        BorderTypes.Constant,
                        Scalar.White);

                    return BitmapConverter.ToBitmap(rotated);
                }
            }
            catch (Exception ex)
            {
                // Return original image if deskew fails
                System.Diagnostics.Debug.WriteLine($"Deskew failed: {ex.Message}");
                return (Bitmap)input.Clone();
            }
        }

        // =========================
        // GRAYSCALE
        // =========================
        private Bitmap ConvertToGrayscale(Bitmap original)
        {
            try
            {
                Bitmap gray = new Bitmap(original.Width, original.Height);

                using (Graphics g = Graphics.FromImage(gray))
                {
                    ColorMatrix colorMatrix = new ColorMatrix(new float[][]
                    {
                        new float[] {0.3f, 0.3f, 0.3f, 0, 0},
                        new float[] {0.59f,0.59f,0.59f,0,0},
                        new float[] {0.11f,0.11f,0.11f,0,0},
                        new float[] {0,0,0,1,0},
                        new float[] {0,0,0,0,1}
                    });

                    ImageAttributes attributes = new ImageAttributes();
                    attributes.SetColorMatrix(colorMatrix);

                    g.DrawImage(original,
                        new Rectangle(0, 0, original.Width, original.Height),
                        0, 0, original.Width, original.Height,
                        GraphicsUnit.Pixel, attributes);
                }

                return gray;
            }
            catch (Exception ex)
            {
                throw new Exception($"Grayscale conversion failed: {ex.Message}", ex);
            }
        }

        // =========================
        // HEATMAP
        // =========================
        private Bitmap ConvertToHeatmap(Bitmap original)
        {
            try
            {
                Bitmap heatmap = new Bitmap(original.Width, original.Height);

                for (int y = 0; y < original.Height; y++)
                {
                    for (int x = 0; x < original.Width; x++)
                    {
                        Color pixel = original.GetPixel(x, y);

                        int intensity = (pixel.R + pixel.G + pixel.B) / 3;

                        Color heatColor = GetHeatColor(intensity);

                        heatmap.SetPixel(x, y, heatColor);
                    }
                }

                return heatmap;
            }
            catch (Exception ex)
            {
                throw new Exception($"Heatmap conversion failed: {ex.Message}", ex);
            }
        }

        // =========================
        // HEATMAP COLOR
        // =========================
        private Color GetHeatColor(int value)
        {
            try
            {
                if (value < 85)
                    return Color.FromArgb(0, value * 3, 255); // Blue → Cyan
                else if (value < 170)
                    return Color.FromArgb((value - 85) * 3, 255, 255 - (value - 85) * 3); // Green → Yellow
                else
                    return Color.FromArgb(255, 255 - (value - 170) * 3, 0); // Yellow → Red
            }
            catch (Exception ex)
            {
                throw new Exception($"Heat color calculation failed for value {value}: {ex.Message}", ex);
            }
        }

        // Add disposal helper
        public void Dispose()
        {
            // Cleanup if needed
            GC.SuppressFinalize(this);
        }
    }
}