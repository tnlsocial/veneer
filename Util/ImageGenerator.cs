using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using System.Diagnostics;

namespace veneer.Util;

public static class ImageGenerator
{
    public static string RandomImage(string imagesDirectory)
    {
        var imageFiles = Directory.GetFiles(imagesDirectory, "*.jpg");
        if (imageFiles.Length == 0)
            throw new InvalidOperationException("No images found in the directory.");

        var random = new Random();
        string randomImageFile = imageFiles[random.Next(imageFiles.Length)];
        Image<Rgba32> image = Image.Load<Rgba32>(randomImageFile);

        using (var ms = new MemoryStream())
        {
            image.SaveAsJpeg(ms);
            var imageBytes = ms.ToArray();
            var base64String = Convert.ToBase64String(imageBytes);
            return "data:image/jpeg;base64," + base64String;
        }
    }

    public static string Generate(string imagesDirectory, string text, Font font)
    {
        var imageFiles = Directory.GetFiles(imagesDirectory, "*.jpg");
        if (imageFiles.Length == 0)
            throw new InvalidOperationException("No images found in the directory.");

        var random = new Random();
        string randomImageFile = imageFiles[random.Next(imageFiles.Length)];

        using (Image<Rgba32> image = Image.Load<Rgba32>(randomImageFile))
        {
            Size imgSize = image.Size;

            var options = new RichTextOptions(font)
            {
                Origin = new PointF(image.Width / 2f, image.Height - font.Size), // Central bottom alignment
                WrappingLength = image.Width - 50,  // 50 pixels padding
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Bottom
            };

            Color shadowColor = Color.Black.WithAlpha(0.6f); // Semi-transparent black
            Brush shadowBrush = Brushes.Solid(shadowColor);
            Brush whiteBrush = Brushes.Solid(Color.White);

            PointF shadowOffset = new PointF(2, 2);  // Shadow offset; adjust for larger or softer shadow

            // Prepare options for the shadow with offset origin
            var shadowOptions = new RichTextOptions(font)
            {
                Origin = new PointF(options.Origin.X + shadowOffset.X, options.Origin.Y + shadowOffset.Y),
                WrappingLength = options.WrappingLength,
                HorizontalAlignment = options.HorizontalAlignment,
                VerticalAlignment = options.VerticalAlignment
            };

            image.Mutate(x =>
            {
                // Draw shadow first
                x.DrawText(shadowOptions, text, shadowBrush);

                // Draw main text
                x.DrawText(options, text, whiteBrush);
            });

            using (var ms = new MemoryStream())
            {
                image.SaveAsJpeg(ms);
                var imageBytes = ms.ToArray();
                var base64String = Convert.ToBase64String(imageBytes);
                return "data:image/jpeg;base64," + base64String;
            }
        }
    }
}
