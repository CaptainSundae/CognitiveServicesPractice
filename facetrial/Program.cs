using System;
using System.IO;
using System.Net.Http.Headers;
using System.Net.Http;

namespace CSHttpClientSample
{
    static class Program
    {
        static void Main()
        {
            Console.Write("Enter the path to the JPEG image with faces to identify:");
            string imageFilePath = Console.ReadLine();

            MakeDetectRequest(imageFilePath);

            Console.WriteLine("\n\n\n ENTER to exit...\n\n\n");
            Console.ReadLine();
        }

        static byte[] GetImageAsByteArray(string imageFilePath)
        {
            FileStream fileStream = new FileStream(imageFilePath, FileMode.Open, FileAccess.Read);
            BinaryReader binaryReader = new BinaryReader(fileStream);
            return binaryReader.ReadBytes((int)fileStream.Length);
        }

        static async void MakeDetectRequest(string imageFilePath)
        {
            var client = new HttpClient();

            // Request headers 
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", "af343b3eafef43d0987a3d1a9ebf2448");

            // Request parameters and URI string.
            string queryString = "returnFaceId=true&returnFaceLandmarks=false&returnFaceAttributes=age,gender";
            string uri = "https://westus.api.cognitive.microsoft.com/face/v1.0/detect?" + queryString;

            HttpResponseMessage response;
            string responseContent;


            byte[] byteData = GetImageAsByteArray(imageFilePath);

            using (var content = new ByteArrayContent(byteData))
            {
                // This example uses content type "application/octet-stream".
                // The other content types you can use are "application/json" and "multipart/form-data".
                content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                response = await client.PostAsync(uri, content);
                responseContent = response.Content.ReadAsStringAsync().Result;
            }
            //A peak at the JSON response.
            Console.WriteLine(responseContent);
        }
    }
}