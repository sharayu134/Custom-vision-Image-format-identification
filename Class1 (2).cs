using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Threading;
using rountingnumber;

namespace CSHttpClientSample
{
    static class Program
    {
        const string subscriptionKey = "a02378056afc41c886ff93c7f5cbb17a";


        const string uriBase = "https://westcentralus.api.cognitive.microsoft.com/vision/v2.0/analyze";

        static void Main()
        {
            //input
            string imageFilePath = @"D:\as.jpg";

            string sdn = MakeAnalysisRequest(imageFilePath).Result;

            var oMycustomclassname = Newtonsoft.Json.JsonConvert.DeserializeObject<RootObject>(sdn);

            //image format  output
            Console.WriteLine(oMycustomclassname.metadata.format);
            Thread.Sleep(100000);

        }


        /// <param name="imageFilePath">The image file to analyze.</param>
        static async Task<string> MakeAnalysisRequest(string imageFilePath)
        {

            HttpClient client = new HttpClient();

            client.DefaultRequestHeaders.Add(
                "Ocp-Apim-Subscription-Key", subscriptionKey);


            string requestParameters =
                "visualFeatures=Categories,Description,Color";

            string uri = uriBase + "?" + requestParameters;

            HttpResponseMessage response;


            byte[] byteData = GetImageAsByteArray(imageFilePath);

            using (ByteArrayContent content = new ByteArrayContent(byteData))
            {

                content.Headers.ContentType =
                    new MediaTypeHeaderValue("application/octet-stream");

                response = await client.PostAsync(uri, content);
            }

            string contentString;  //= await response.Content.ReadAsStringAsync();

            
            return (contentString = await response.Content.ReadAsStringAsync());


        }


        /// <param name="imageFilePath">The image file to read.</param>

        static byte[] GetImageAsByteArray(string imageFilePath)
        {
            using (FileStream fileStream =
                new FileStream(imageFilePath, FileMode.Open, FileAccess.Read))
            {
                BinaryReader binaryReader = new BinaryReader(fileStream);
                return binaryReader.ReadBytes((int)fileStream.Length);
            }
        }
    }
}