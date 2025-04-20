using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Net.NetworkInformation;
using ZXing;

namespace SharedControls // Add this namespace
{
    public static class BarcodeHelper
    {
        public static byte[] GenerateBarcode(string content)
        {
            var writer = new BarcodeWriter
            {
                Format = BarcodeFormat.CODE_128,
                Options = new ZXing.Common.EncodingOptions
                {
                    Height = 100,
                    Width = 250,
                    Margin = 1
                }
            };

            using (Bitmap bitmap = writer.Write(content))
            using (MemoryStream ms = new MemoryStream())
            {
                bitmap.Save(ms, ImageFormat.Png);
                return ms.ToArray();
            }
        }
    }
}
