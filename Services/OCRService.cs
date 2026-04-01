using Tesseract;

public class OCRService
{
    private TesseractEngine engine;

    public OCRService()
    {
        string tessPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "tessdata");

        if (!Directory.Exists(tessPath))
            throw new Exception($"Tessdata folder missing: {tessPath}");

        if (!File.Exists(Path.Combine(tessPath, "eng.traineddata")))
            throw new Exception("eng.traineddata missing!");

        engine = new TesseractEngine(tessPath, "eng", EngineMode.Default);
    }

    public string ReadText(Bitmap image)
    {
        using (var pix = PixConverter.ToPix(image))
        {
            using (var page = engine.Process(pix))
            {
                return page.GetText().Trim();
            }
        }
    }


}