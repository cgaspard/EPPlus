using System;
using System.IO;
using SkiaSharp;

namespace OfficeOpenXml.Compatibility
{
    internal class ImageCompat
    {
        internal static byte[] GetImageAsByteArray(SKBitmap bitmap)
        {
            using var ms = new MemoryStream();
            SKEncodedImageFormat format;

            // Determine the format based on the color type of the bitmap.
            // Note: this is a simplification, the color type does not necessarily
            // determine the original image format.
            switch (bitmap.ColorType)
            {
                case SKColorType.Bgra8888:  // Bgra8888 color type is used in GIF, BMP, and PNG
                    format = SKEncodedImageFormat.Png;
                    break;
                case SKColorType.Alpha8:  // Alpha8 color type can be used in BMP
                    format = SKEncodedImageFormat.Bmp;
                    break;
                default:
                    format = SKEncodedImageFormat.Jpeg;
                    break;
            }

            using var skImage = SKImage.FromBitmap(bitmap);
            var encoded = skImage.Encode(format, 100);
            encoded.SaveTo(ms);

            return ms.ToArray();
        }
    }
}
