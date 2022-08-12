using System.Diagnostics;
using System.Net;
using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace MoChengHttp
{
    public class HttpUtil
    {
        public static void HttpDownload(string url, string filePath, Action<float> progressAction = null)
        {

            FileStream fs = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.Write);

            long currentFileLength = fs.Length;

            long totalLength = GetFileSize(url);



            float progress = 0f;
            while (currentFileLength < totalLength)
            {
                HttpWebRequest request = HttpWebRequest.Create(url) as HttpWebRequest;
                request.AddRange((int)currentFileLength);

                HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                Stream dataStream = response.GetResponseStream();

                byte[] buffer = new byte[4096];

                int length = dataStream.Read(buffer, 0, buffer.Length);
                while (length > 0)
                {

                    fs.Seek(currentFileLength, SeekOrigin.Begin);
                    fs.Write(buffer, 0, length);
                    currentFileLength += length;
                    Console.WriteLine(currentFileLength + " " + totalLength);
                    progress = (float)currentFileLength / (float)totalLength;
                    progressAction?.Invoke(progress);
                    length = dataStream.Read(buffer, 0, buffer.Length);
                }
                dataStream.Close();
                dataStream.Dispose();
                response.Close();
                response.Dispose();

            }
            fs.Close();
            fs.Dispose();
            if (currentFileLength != totalLength)
            {
                throw new Exception($"something wrong about the downloaded file:{filePath}");
            }



        }

        static long GetFileSize(string url)
        {
            // ServicePointManager.ServerCertificateValidationCallback = MyRemoteCertificateValidationCallback;
            HttpWebRequest requet = HttpWebRequest.Create(url) as HttpWebRequest;
            requet.Method = "HEAD";
            HttpWebResponse response = requet.GetResponse() as HttpWebResponse;
            return response.ContentLength;
        }
        public static float GetDownloadSize(string url)
        {
            long length = GetFileSize(url);
            float m = (float)(length >> 10) / (float)1024;

            return (float)Math.Round(m, 1);
        }



        public static string HttpContent(string url)
        {
            string content = null;
            try
            {

                HttpWebRequest requet = HttpWebRequest.Create(url) as HttpWebRequest;
                HttpWebResponse response = requet.GetResponse() as HttpWebResponse;
                using (StreamReader sr = new StreamReader(response.GetResponseStream()))
                {
                    content = sr.ReadToEnd();
                }
                response.Dispose();
            }
            catch (Exception)
            {

            }

            return content;
        }

    }


}