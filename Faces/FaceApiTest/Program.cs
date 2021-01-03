using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace FaceApiTest
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var imagepath = @"oscar.jpg";
            var urlAddress = @"http://localhost:6000/api/faces";

            ImageUtility imageUtility = new ImageUtility();
            var bytes = imageUtility.ConvertToBytes(imagepath);

            List<byte[]> faceList = null;
            var byteContent = new ByteArrayContent(bytes);
            byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");

            HttpClientHandler clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };

            using var httpClient = new HttpClient(clientHandler);
            using var response = await httpClient.PostAsync(urlAddress, byteContent);

            string apiResponse = await response.Content.ReadAsStringAsync();
            faceList = JsonConvert.DeserializeObject<List<byte[]>>(apiResponse);

            if (faceList.Count == 0) throw new Exception("Face Not Found");

            for (int i = 0; i < faceList.Count; i++)
            {
                imageUtility.FromBytesToImage(faceList[0], $"face{i}.jpg");
            }

            Console.WriteLine($"{faceList.Count} Face(s) Detected.");
        }
    }
}
