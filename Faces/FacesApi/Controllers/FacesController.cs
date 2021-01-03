using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace FacesApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FacesController : ControllerBase
    {
        [HttpPost]
        public async Task<List<byte[]>> ReadFaces()
        {
            using var ms = new MemoryStream(2048);

            await Request.Body.CopyToAsync(ms);
            var faces = GetFaces(ms.ToArray());
            return faces;
        }

        private List<byte[]> GetFaces(byte[] image)
        {
            Mat src = Cv2.ImDecode(image, ImreadModes.Color);

            // Convert byte Array to JPEG Image
            // And Save it for testing purposes
            src.SaveImage("image.jpg", new ImageEncodingParam(ImwriteFlags.JpegProgressive, 255));

            // Load Cascade File
            var cascadeFilePath = Path.Combine(Directory.GetCurrentDirectory(), "CascadeFiles", "haarcascade_frontalface_default.xml");
            var faceCascade = new CascadeClassifier();
            faceCascade.Load(cascadeFilePath);

            var faces = faceCascade.DetectMultiScale(src, 1.1, 6, HaarDetectionType.DoRoughSearch, new Size(60, 60));

            var faceList = new List<byte[]>();
            int num = 1;

            foreach (var rect in faces)
            {
                var faceImage = new Mat(src, rect);
                faceList.Add(faceImage.ToBytes(".jpg"));
                faceImage.SaveImage("face"+num+".jpg", new ImageEncodingParam(ImwriteFlags.JpegProgressive, 255));

                num++;
            }

            return faceList;
        }
    }
}
