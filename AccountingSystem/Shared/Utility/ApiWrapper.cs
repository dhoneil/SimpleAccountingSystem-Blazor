
using AccountingSystem.Shared.ViewModels;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AccountingSystem.Shared.Utility
{
    public class ApiWrapper
    {
        private static JsonSerializerSettings JsonSettings()
        {

            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.Converters.Add(new StringEnumConverter());
            settings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            return settings;
        }

        /// <summary>
        /// Delete
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="url"></param>
        /// <returns></returns>
        public async static Task<T> Delete<T>(string url, object parameter, string contentType = "application/json")
        {
            string p = contentType == "application/json" ? JsonConvert.SerializeObject(parameter, JsonSettings()) : parameter.ToString();
            string token = string.Empty;
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Delete, url);
            request.Content = new StringContent(p, Encoding.UTF8, contentType);

            client.Timeout = TimeSpan.FromHours(2);
            var result = await client.SendAsync(request);

            string resultStr = string.Empty;
            using (HttpContent content = result.Content)
                resultStr = content.ReadAsStringAsync().Result;

            if (result.IsSuccessStatusCode)
                return JsonConvert.DeserializeObject<T>(resultStr, JsonSettings());
            else
            {
                if (!string.IsNullOrEmpty(resultStr))
                {
                    var error = JsonConvert.DeserializeObject<StatusCodeResponseVM>(resultStr, JsonSettings());
                    string msg = string.IsNullOrEmpty(error.Detail) ? error.Message : error.Detail;
                    throw new Exception(msg);
                }
                throw new Exception(result.ReasonPhrase);
            }
        }

        /// <summary>
        /// Put
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="url"></param>
        /// <param name="parameter"></param>
        /// <param name="contentType"></param>
        /// <returns></returns>
        public async static Task<T> Put<T>(string url, object parameter, string contentType = "application/json")
        {
            string p = contentType == "application/json" ? JsonConvert.SerializeObject(parameter, JsonSettings()) : parameter.ToString();

            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(contentType));

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Put, url);
            request.Content = new StringContent(p, Encoding.UTF8, contentType);

            client.Timeout = TimeSpan.FromHours(2);
            var result = await client.SendAsync(request);

            string resultStr = string.Empty;
            using (HttpContent content = result.Content)
                resultStr = content.ReadAsStringAsync().Result;

            if (result.IsSuccessStatusCode)
                return JsonConvert.DeserializeObject<T>(resultStr, JsonSettings());
            else
            {
                if (!string.IsNullOrEmpty(resultStr))
                {
                    var error = JsonConvert.DeserializeObject<StatusCodeResponseVM>(resultStr, JsonSettings());
                    string msg = string.IsNullOrEmpty(error.Detail) ? error.Message : error.Detail;
                    throw new Exception(msg);
                }
                throw new Exception(result.ReasonPhrase);
            }
        }

        /// <summary>
        /// Post
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="url"></param>
        /// <param name="parameter"></param>
        /// <param name="contentType"></param>
        /// <param name="isToken"></param>
        /// <returns></returns>
        public async static Task<T> Post<T>(string url, object parameter = null, string contentType = "application/json", bool isToken = false)
        {
            string p = contentType == "application/json" ? JsonConvert.SerializeObject(parameter, JsonSettings()) : parameter.ToString();
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(contentType));

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, url);
            request.Content = new StringContent(p, Encoding.UTF8, contentType);

            client.Timeout = TimeSpan.FromHours(2);
            var result = await client.SendAsync(request);

            string resultStr = string.Empty;
            using (HttpContent content = result.Content)
                resultStr = content.ReadAsStringAsync().Result;


            if (!result.IsSuccessStatusCode)
            {
                if (!string.IsNullOrEmpty(resultStr))
                {
                    var error = JsonConvert.DeserializeObject<StatusCodeResponseVM>(resultStr, JsonSettings());
                    string msg = string.IsNullOrEmpty(error.Detail) ? error.Message : error.Detail;
                    throw new Exception(msg);
                }
                throw new Exception(result.ReasonPhrase);
            }
            else
            {
                if (isToken)
                {
                    resultStr = Regex.Replace(resultStr, @"\\", "");
                    resultStr = Regex.Replace(resultStr, @"""{", "{");
                    resultStr = Regex.Replace(resultStr, @"}""", "}");
                }
                return JsonConvert.DeserializeObject<T>(resultStr, JsonSettings());
            }
        }


        /// <summary>
        /// Get
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="url"></param>
        /// <returns></returns>
        public async static Task<T> Get<T>(string url, object parameter = null)
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, url);
            if (parameter != null)
            {
                string p = JsonConvert.SerializeObject(parameter, JsonSettings());
                request.Content = new StringContent(p, Encoding.UTF8, "application/json");
            }
            client.Timeout = TimeSpan.FromHours(2);
            var result = await client.SendAsync(request);

            string resultStr = string.Empty;
            using (HttpContent content = result.Content)
                resultStr = content.ReadAsStringAsync().Result;

            if (result.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<T>(resultStr, JsonSettings());
            }
            else
            {
                if (!string.IsNullOrEmpty(resultStr))
                {
                    var error = JsonConvert.DeserializeObject<StatusCodeResponseVM>(resultStr, JsonSettings());
                    string msg = string.IsNullOrEmpty(error.Detail) ? error.Message : error.Detail;
                    throw new Exception(msg);
                }
                throw new Exception(result.ReasonPhrase);
            }
        }

        public async static Task<Stream> GetStream(string url, object parameter = null)
        {
            Stream responseStream = null;
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, url);
            if (parameter != null)
            {
                string p = JsonConvert.SerializeObject(parameter, JsonSettings());
                request.Content = new StringContent(p, Encoding.UTF8, "application/json");
            }
            client.Timeout = TimeSpan.FromHours(2);
            var result = await client.SendAsync(request);

            if (result.IsSuccessStatusCode)
            {
                responseStream = await result.Content.ReadAsStreamAsync();
            }

            return responseStream;
        }

        public async static Task<Stream> PostStream(string url, object parameter = null)
        {
            Stream responseStream = null;
            string p = JsonConvert.SerializeObject(parameter, JsonSettings());
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, url);
            request.Content = new StringContent(p, Encoding.UTF8, "application/json");

            client.Timeout = TimeSpan.FromHours(2);
            var result = await client.SendAsync(request);

            if (result.IsSuccessStatusCode)
            {
                responseStream = await result.Content.ReadAsStreamAsync();
            }

            return responseStream;
        }

        public async static Task<string> Upload(string url, Stream stream, string fileName, string contentType)
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Clear();
            MultipartFormDataContent form = new MultipartFormDataContent();
            HttpContent content = new StringContent("file");
            form.Add(content, "file");

            content = new StreamContent(stream);
            content.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
            {
                Name = "file",
                FileName = fileName,
                Size = stream.Length
            };
            form.Add(content);
            content.Headers.ContentType = new MediaTypeHeaderValue(contentType);

            client.Timeout = TimeSpan.FromHours(2);
            var result = await client.PostAsync(url, form);
            var resultStr = result.Content.ReadAsStringAsync().Result;

            if (result.IsSuccessStatusCode)
            {
                return resultStr;
            }
            else
            {
                if (!string.IsNullOrEmpty(resultStr))
                {
                    var error = JsonConvert.DeserializeObject<StatusCodeResponseVM>(resultStr, JsonSettings());
                    string msg = string.IsNullOrEmpty(error.Detail) ? error.Message : error.Detail;
                    throw new Exception(msg);
                }
                throw new Exception(result.ReasonPhrase);
            }
        }
    }
}
