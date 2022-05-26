namespace Orc.Automation.TestSync
{
    using System;
    using System.Collections.Specialized;
    using System.IO;
    using System.Net;
    using System.Net.Http;
    using System.Text;
    using System.Threading.Tasks;
    using RestSharp;

    public static class HttpRequestHelper
    {
        public static async Task<string> PostTestResultsAsync(string uri, string resultsFilePath, string token)
        {
            using var client = new RestClient();
            var request = new RestRequest(uri, Method.Post);
            request.AddHeader("Authorization", $"Bearer {token}");
            request.AddFile("file", resultsFilePath);
            var response = await client.ExecuteAsync(request);

            return response.Content;
        }
        
        public static string Get(string uri, string token = null)
        {
            var request = (HttpWebRequest)WebRequest.Create(uri);
            if (!string.IsNullOrWhiteSpace(token))
            {
                request.Headers.Add("Authorization", token);
            }

            request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;

            using var response = (HttpWebResponse)request.GetResponse();
            using var stream = response.GetResponseStream();
            if (stream is null)
            {
                return null;
            }

            using var reader = new StreamReader(stream);

            return reader.ReadToEnd();
        }

        public static async Task<string> GetAsync(string uri, string token = null)
        {
            var request = (HttpWebRequest)WebRequest.Create(uri);
            if (!string.IsNullOrWhiteSpace(token))
            {
                request.Headers.Add("Authorization", token);
            }

            request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;

            using var response = (HttpWebResponse)await request.GetResponseAsync();
            await using var stream = response.GetResponseStream();

            if (stream is null)
            {
                return null;
            }

            using var reader = new StreamReader(stream);
            return await reader.ReadToEndAsync();
        }


        public static string PostMultiPart(string uri, string data, string token = null, string method = "POST")
        {
            var dataBytes = Encoding.UTF8.GetBytes(data);
            using var form = new MultipartFormDataContent();
            form.Add(new ByteArrayContent(dataBytes, 0, dataBytes.Length), "file");


            var request = (HttpWebRequest)WebRequest.Create(uri);
            request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
            request.ContentLength = dataBytes.Length;
            request.ContentType = "multipart/form-data";
            request.Method = method;

            if (!string.IsNullOrWhiteSpace(token))
            {
                request.Headers.Add("Authorization", token);
            }

            using (var requestBody = request.GetRequestStream())
            {
                requestBody.Write(dataBytes, 0, dataBytes.Length);
            }

            using var response = (HttpWebResponse)request.GetResponse();
            using var stream = response.GetResponseStream();

            if (stream is null)
            {
                return null;
            }

            using var reader = new StreamReader(stream);
            return reader.ReadToEnd();
        }

        //public static string HttpPostRequest(string url, byte[] dataBytes, string token = null)
        //{
        //    var request = (HttpWebRequest)HttpWebRequest.Create(url);
        //    request.Method = "POST";

        //    using var formData = new MultipartFormDataContent();
        //    formData.Add(new ByteArrayContent(dataBytes, 0, dataBytes.Length), "file");

        //    request.ContentType = "binary";
        //    request.ContentLength = formData.Headers.ContentLength.Value;
        //    request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
        //    request.Method = "POST";

        //    if (!string.IsNullOrWhiteSpace(token))
        //    {
        //        request.Headers.Add("Authorization", token);
        //    }

        //    using var requestStream = request.GetRequestStream();
        //    var i = 0;
        //    attachAttempt:
        //    try
        //    {
        //        requestStream.Write(dataBytes, 0, dataBytes.Length);
        //    }
        //    catch (IOException)
        //    {
        //        if (i < 5)
        //        {
        //            i++;
        //            goto attachAttempt;
        //        }

        //        return "";
        //    }
        //    requestStream.Close();

        //    using var myHttpWebResponse = (HttpWebResponse)request.GetResponse();
        //    using var responseStream = myHttpWebResponse.GetResponseStream();
        //    using var myStreamReader = new StreamReader(responseStream, Encoding.Default);
        //    var pageContent = myStreamReader.ReadToEnd();

        //    myStreamReader.Close();
        //    responseStream.Close();

        //    myHttpWebResponse.Close();

        //    return pageContent;
        //}
        
        //public static async Task PostMultiPartAsync(string url, string data, string token = null)
        //{
        //    using (var httpClient = new HttpClient())
        //    {
        //        using var form = new MultipartFormDataContent();

        //       // form.Add(new StringContent(data), "file");
        //        //   form.Add(new StringContent(useremail), "email");
        //        //   form.Add(new StringContent(password), "password");

        //        var dataBytes = Encoding.UTF8.GetBytes(data);
        //        form.Add(new ByteArrayContent(dataBytes, 0, dataBytes.Length), "file");

        //        if (!string.IsNullOrWhiteSpace(token))
        //        {
        //            httpClient.DefaultRequestHeaders.Add("Authorization", token);
        //        }

        //        try
        //        {
        //            using var response = await httpClient.PostAsync(url, form);
        //            response.EnsureSuccessStatusCode();
        //        }
        //        catch (Exception e)
        //        {
        //            Console.WriteLine(e);
        //            throw;
        //        }
        //    }

        //    return;
        //}

        public static string PostB(string uri, byte[] dataBytes, string contentType, string token = null, string boundary = null, string method = "POST")
        {
            var request = (HttpWebRequest)WebRequest.Create(uri);
            request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
            request.ContentType = contentType;
            request.Method = method;
            request.KeepAlive = true;

            if (!string.IsNullOrWhiteSpace(token))
            {
                request.Headers.Add("Authorization", token);
            }

            using (var streamWriter = new StreamWriter(request.GetRequestStream()))
            {
                var header = $"Content-Disposition: form-data; name=\"file\"\r\nContent-Type: application/zip\r\n\r\n";
                var bytes = Encoding.UTF8.GetBytes(header);
                var length = bytes.Length;
                streamWriter.Write(bytes);

                streamWriter.Write(dataBytes);
                length += dataBytes.Length;

                var ending = $"\r\n--{boundary}--";
                bytes = Encoding.UTF8.GetBytes(ending);
                length += bytes.Length;

                streamWriter.Write(bytes);

                request.ContentLength = length;
            }

            var str = ConvertHttpWebRequestToString(request);

            try
            {
                using var response = (HttpWebResponse)request.GetResponse();
                using var stream = response.GetResponseStream();

                if (stream is null)
                {
                    return null;
                }

                using var reader = new StreamReader(stream);
                return reader.ReadToEnd();
            }
            catch (Exception e)
            { 
                Console.WriteLine(e);
                throw;
            }
        }

        private static string ConvertHttpWebRequestToString(HttpWebRequest loHttp)
        {
            string format = "";
            format += loHttp.Method.ToString() + " / HTTP/" + loHttp.ProtocolVersion.ToString() + Environment.NewLine;
            format += "Host: " + loHttp.RequestUri.Host + Environment.NewLine;
            for (int i = 0; i < loHttp.Headers.AllKeys.Length; i++)
                format += loHttp.Headers.AllKeys[i] + ": " + loHttp.Headers[loHttp.Headers.AllKeys[i]] + Environment.NewLine;
           
            if (loHttp.KeepAlive == true)
            {
                format += "Keep-Alive: 115" + Environment.NewLine;
                format += "Connection: keep-alive" + Environment.NewLine;
            }
      
            return format;
        }

        public static void HttpUploadFile(string url, string file, string paramName, string contentType, NameValueCollection nvc, string token)
        {
            var boundary = "---------------------------" + DateTime.Now.Ticks.ToString("x");
            var boundarybytes = System.Text.Encoding.ASCII.GetBytes("\r\n--" + boundary + "\r\n");

            var wr = (HttpWebRequest)WebRequest.Create(url);
            wr.ContentType = "multipart/form-data; boundary=" + boundary;
            wr.Method = "POST";
            wr.KeepAlive = true;

            if (!string.IsNullOrWhiteSpace(token))
            {
                wr.Headers.Add("Authorization", token);
            }

            //wr.Credentials = CredentialCache.DefaultCredentials;

            using var rs = wr.GetRequestStream();

            var formdataTemplate = "Content-Disposition: form-data; name=\"{0}\"\r\n\r\n{1}";
            foreach (string key in nvc.Keys)
            {
                rs.Write(boundarybytes, 0, boundarybytes.Length);
                var formitem = string.Format(formdataTemplate, key, nvc[key]);
                var formitembytes = Encoding.UTF8.GetBytes(formitem);
                rs.Write(formitembytes, 0, formitembytes.Length);
            }
            rs.Write(boundarybytes, 0, boundarybytes.Length);

            var headerTemplate = "Content-Disposition: form-data; name=\"{0}\"; Content-Type: {1}\r\n\r\n";
            var header = string.Format(headerTemplate, paramName, contentType);
            var headerbytes = Encoding.UTF8.GetBytes(header);
            rs.Write(headerbytes, 0, headerbytes.Length);

            using var fileStream = new FileStream(file, FileMode.Open, FileAccess.Read);
            var buffer = new byte[4096];
            var bytesRead = 0;
            while ((bytesRead = fileStream.Read(buffer, 0, buffer.Length)) != 0)
            {
                rs.Write(buffer, 0, bytesRead);
            }
            fileStream.Close();

            var trailer = Encoding.ASCII.GetBytes("\r\n--" + boundary + "--\r\n");
            rs.Write(trailer, 0, trailer.Length);
            rs.Close();

            WebResponse wresp = null;
            try
            {
                using (wresp = wr.GetResponse())
                {
                    var stream2 = wresp.GetResponseStream();
                    using var reader2 = new StreamReader(stream2);
                }
            }
#pragma warning disable CS0168
            catch (Exception ex)
#pragma warning restore CS0168
            {
                if (wresp is not null)
                {
                    wresp.Close();
                }
            }
            finally
            {
                wr = null;
            }
        }

        public static string Post(string uri, string data, string contentType, string token = null, string method = "POST")
        {
            var dataBytes = Encoding.UTF8.GetBytes(data);

            var request = (HttpWebRequest)WebRequest.Create(uri);
            request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
            request.ContentLength = dataBytes.Length;
            request.ContentType = contentType;
            request.Method = method;

            if (!string.IsNullOrWhiteSpace(token))
            {
                request.Headers.Add("Authorization", token);
            }

            using (var requestBody = request.GetRequestStream())
            {
                requestBody.Write(dataBytes, 0, dataBytes.Length);
            }

            using var response = (HttpWebResponse)request.GetResponse();
            using var stream = response.GetResponseStream();

            if (stream is null)
            {
                return null;
            }

            using var reader = new StreamReader(stream);
            return reader.ReadToEnd();
        }

        public static async Task<string> PostAsync(string uri, string data, string contentType, string token = null, string method = "POST")
        {
            var dataBytes = Encoding.UTF8.GetBytes(data);

            var request = (HttpWebRequest)WebRequest.Create(uri);
            request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
            request.ContentLength = dataBytes.Length;
            request.ContentType = contentType;
            request.Method = method;

            if (!string.IsNullOrWhiteSpace(token))
            {
                request.Headers.Add("Authorization", token);
            }

            await using (var requestBody = request.GetRequestStream())
            {
                await requestBody.WriteAsync(dataBytes, 0, dataBytes.Length);
            }

            using var response = (HttpWebResponse)await request.GetResponseAsync();
            await using var stream = response.GetResponseStream();

            if (stream is null)
            {
                return null;
            }

            using var reader = new StreamReader(stream);
            return await reader.ReadToEndAsync();
        }
    }
}
