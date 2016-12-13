using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Xml;
using System.Xml.Linq;

namespace CarDiaryWeb
{
    public class AbbyyReader
    {
        protected static string GetExtension(string exportFormat)
        {
            var extension = string.Empty;
            switch (exportFormat.ToLower())
            {
                case "txt":
                    extension = "txt";
                    break;
                case "rtf":
                    extension = "rtf";
                    break;
                case "docx":
                    extension = "docx";
                    break;
                case "xlsx":
                    extension = "xlsx";
                    break;
                case "pptx":
                    extension = "pptx";
                    break;
                case "pdfsearchable":
                case "pdftextandimages":
                    extension = "pdf";
                    break;
                case "xml":
                    extension = "xml";
                    break;
            }
            return extension;
        }

        public static string GetResultUrl(XDocument doc)
        {
            var resultUrl = string.Empty;
            var task = doc.Root.Element("task");
            if (task != null)
            {
                resultUrl = task.Attribute("resultUrl") != null ? task.Attribute("resultUrl").Value : string.Empty;
            }
            return resultUrl;
        }

        public static string GetStatus(XDocument doc)
        {
            var status = string.Empty;
            var task = doc.Root.Element("task");
            if (task != null)
            {
                status = task.Attribute("status").Value;
            }
            return status;
        }

        public static HttpWebRequest CreateRequest(string url, string method, ICredentials credentials, IWebProxy proxy)
        {
            var request = (HttpWebRequest)HttpWebRequest.Create(url);
            request.ContentType = "application/octet-stream";
            request.Credentials = credentials;
            request.Method = method;
            request.Proxy = proxy;
            return request;
        }

        public static void FillRequestWithContent(HttpWebRequest request, string contentPath)
        {
            using (BinaryReader reader = new BinaryReader(File.OpenRead(contentPath)))
            {
                request.ContentLength = reader.BaseStream.Length;
                using (Stream stream = request.GetRequestStream())
                {
                    byte[] buffer = new byte[reader.BaseStream.Length];
                    while (true)
                    {
                        int bytesRead = reader.Read(buffer, 0, buffer.Length);
                        if (bytesRead == 0)
                        {
                            break;
                        }
                        stream.Write(buffer, 0, bytesRead);
                    }
                }
            }
        }

        public static XDocument GetResponse(HttpWebRequest request)
        {
            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            {
                using (Stream stream = response.GetResponseStream())
                {
                    return XDocument.Load(new XmlTextReader(stream));
                }
            }
        }

        public static string GetTaskId(XDocument doc)
        {
            var id = string.Empty;
            var task = doc.Root.Element("task");
            if (task != null)
            {
                id = task.Attribute("id").Value;
            }
            return id;
        }
    }
}