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

        public string ReadText(Bitmap image)
        {
            if (image == null)
                throw new ArgumentNullException(nameof(image), "Image cannot be null");

            if (engine == null)
                throw new InvalidOperationException("OCR engine is not initialized");

            try
            {
                using (var pix = PixConverter.ToPix(image))
                {
                    if (pix == null)
                        throw new InvalidOperationException("Failed to convert image to Pix format");

                    using (var page = engine.Process(pix))
                    {
                        if (page == null)
                            throw new InvalidOperationException("Failed to process image");

                        string text = page.GetText();
                        return string.IsNullOrEmpty(text) ? string.Empty : text.Trim();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"OCR text extraction failed: {ex.Message}", ex);
            }
        }

        public async Task<string> ReadTextAsync(Bitmap image)
        {
            return await Task.Run(() => ReadText(image));
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