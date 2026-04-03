using Tesseract;

namespace VisioNeo_App.Services
{
    public class OCRService : IDisposable
    {
        private TesseractEngine engine;
        private bool disposed = false;
        public bool IsInitialized => engine != null;

        public OCRService()
        {
            try
            {
                string tessPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "tessdata");

                if (!Directory.Exists(tessPath))
                    throw new DirectoryNotFoundException($"Tessdata folder missing: {tessPath}");

                string trainedDataPath = Path.Combine(tessPath, "eng.traineddata");
                if (!File.Exists(trainedDataPath))
                    throw new FileNotFoundException($"eng.traineddata missing at: {trainedDataPath}");

                engine = new TesseractEngine(tessPath, "eng", EngineMode.Default);

                // REMOVE strict whitelist OR expand it
                engine.SetVariable("tessedit_char_whitelist",
                    "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789.,:;!?@#$₹%&()[]{}+-=*/\\\"' ");

                // VERY IMPORTANT for spacing
                engine.SetVariable("preserve_interword_spaces", "1");
                engine.SetVariable("tessedit_pageseg_mode", "3"); // Fully automatic
                engine.SetVariable("textord_space_size_is_variable", "1");
                // Better segmentation (try both)
                engine.DefaultPageSegMode = PageSegMode.Auto;
                // OR if single block text:
                //engine.DefaultPageSegMode = PageSegMode.SingleBlock;
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to initialize OCR engine: {ex.Message}", ex);
            }
        }

        public List<(string word, float confidence, Rect bounds)> ReadWordsWithConfidence(Bitmap image)
        {
            if (image == null)
                throw new ArgumentNullException(nameof(image));

            if (engine == null)
                throw new InvalidOperationException("OCR engine is not initialized");

            var results = new List<(string, float, Rect)>();

            using (var pix = PixConverter.ToPix(image))
            using (var page = engine.Process(pix))
            using (var iter = page.GetIterator())
            {
                iter.Begin();

                do
                {
                    string word = iter.GetText(PageIteratorLevel.Word);
                    float conf = iter.GetConfidence(PageIteratorLevel.Word);

                    if (!string.IsNullOrWhiteSpace(word))
                    {
                        if (iter.TryGetBoundingBox(PageIteratorLevel.Word, out var rect))
                        {
                            results.Add((word, conf, rect));
                        }
                    }

                } while (iter.Next(PageIteratorLevel.Word));
            }

            return results;
        }

        private Color GetConfidenceColor(float confidence)
        {
            // confidence: 0 → 100

            if (confidence >= 85)
                return Color.Black;        // very strong
            else if (confidence >= 70)
                return Color.DarkGreen;    // good
            else if (confidence >= 50)
                return Color.Orange;       // medium
            else
                return Color.Red;          // poor
        }

        public Bitmap DrawConfidenceOverlay(Bitmap image, List<(string word, float confidence, Rect bounds)> words)
        {
            Bitmap output = new Bitmap(image);

            using (Graphics g = Graphics.FromImage(output))
            {
                foreach (var item in words)
                {
                    var color = GetConfidenceColor(item.confidence);

                    using (Pen pen = new Pen(color, 5))
                    using (Brush brush = new SolidBrush(color))
                    using (Font font = new Font("Segoe UI", 10, FontStyle.Bold))
                    {
                        var rect = new Rectangle(
                                    item.bounds.X1,
                                    item.bounds.Y1,
                                    item.bounds.X2 - item.bounds.X1,
                                    item.bounds.Y2 - item.bounds.Y1
                                );

                        // Draw bounding box
                        g.DrawRectangle(pen, rect);

                        // Draw text + confidence
                        string label = $"{item.word} ({item.confidence:0})";
                        g.DrawString(label, font, brush, rect.X, rect.Y - 15);
                    }
                }
            }

            return output;
        }

        public string ReconstructText(List<(string word, float confidence, Rect bounds)> words)
        {
            // 1. Sort by Y (top to bottom)
            var sorted = words.OrderBy(w => w.bounds.Y1).ToList();

            List<List<(string word, float conf, Rect bounds)>> lines = new();

            int lineThreshold = 15; // adjust if needed

            foreach (var w in sorted)
            {
                bool added = false;

                foreach (var line in lines)
                {
                    // Compare Y with first word in line
                    if (Math.Abs(line[0].bounds.Y1 - w.bounds.Y1) < lineThreshold)
                    {
                        line.Add(w);
                        added = true;
                        break;
                    }
                }

                if (!added)
                {
                    lines.Add(new List<(string, float, Rect)> { w });
                }
            }

            // 2. Sort each line by X (left to right)
            var finalText = new List<string>();

            foreach (var line in lines)
            {
                var ordered = line.OrderBy(w => w.bounds.X1).ToList();

                string lineText = "";

                for (int i = 0; i < ordered.Count; i++)
                {
                    lineText += ordered[i].word;

                    if (i < ordered.Count - 1)
                    {
                        int gap = ordered[i + 1].bounds.X1 - ordered[i].bounds.X2;

                        // Dynamic spacing
                        if (gap > 40)
                            lineText += "   ";   // big gap
                        else if (gap > 20)
                            lineText += "  ";    // medium gap
                        else
                            lineText += " ";     // normal space
                    }
                }

                finalText.Add(lineText);
            }

            return string.Join(Environment.NewLine, finalText);
        }

        // Dispose pattern for proper cleanup
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    if (engine != null)
                    {
                        try
                        {
                            engine.Dispose();
                        }
                        catch (Exception ex)
                        {
                            System.Diagnostics.Debug.WriteLine($"Error disposing OCR engine: {ex.Message}");
                        }
                        finally
                        {
                            engine = null;
                        }
                    }
                }
                disposed = true;
            }
        }
    }
}