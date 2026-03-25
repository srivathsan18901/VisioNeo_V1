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
            return mode switch
            {
                DisplayMode.Grayscale => ConvertToGrayscale(input),
                DisplayMode.Heatmap => ConvertToHeatmap(input),
                _ => (Bitmap)input.Clone()
            };
        }

        // =========================
        // GRAYSCALE
        // =========================
        private Bitmap ConvertToGrayscale(Bitmap original)
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

        // =========================
        // HEATMAP
        // =========================
        private Bitmap ConvertToHeatmap(Bitmap original)
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

        // =========================
        // HEATMAP COLOR
        // =========================
        private Color GetHeatColor(int value)
        {
            if (value < 85)
                return Color.FromArgb(0, value * 3, 255); // Blue → Cyan

            else if (value < 170)
                return Color.FromArgb((value - 85) * 3, 255, 255 - (value - 85) * 3); // Green → Yellow

            else
                return Color.FromArgb(255, 255 - (value - 170) * 3, 0); // Yellow → Red
        }
    }
}