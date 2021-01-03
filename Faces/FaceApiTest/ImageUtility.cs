using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaceApiTest
{
    class ImageUtility
    {
        public byte[] ConvertToBytes(string imagePath)
        {
            using MemoryStream memoryStream = new MemoryStream();
            
            using FileStream fileStream = new FileStream(imagePath, FileMode.Open);
            fileStream.CopyTo(memoryStream);

            var bytes = memoryStream.ToArray();
            return bytes;
        }

        public void FromBytesToImage(byte[] imagesBytes, string fileName)
        {
            using MemoryStream memoryStream = new MemoryStream(imagesBytes);

            Image img = Image.FromStream(memoryStream);
            img.Save($"{fileName}.jpg", ImageFormat.Jpeg);
        }
    }
}
