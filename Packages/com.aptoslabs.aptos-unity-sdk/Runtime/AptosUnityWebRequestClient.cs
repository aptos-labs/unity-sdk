using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine.Networking;

namespace Aptos
{
    public class AptosUnityWebRequestClient : RequestClient
    {
        private string GetReasonPhrase(int responseCode)
        {
            if (responseCode >= 200 && responseCode < 300)
            {
                return "Success";
            }
            else if (responseCode >= 300 && responseCode < 400)
            {
                return "The user has been redirected.";
            }
            else if (responseCode >= 400 && responseCode < 500)
            {
                return "Bad Request.";
            }
            else if (responseCode >= 500 && responseCode < 600)
            {
                return "Internal server error.";
            }
            else
            {
                return "Unknown";
            }
        }

        public override async Task<ClientResponse<Res>> Get<Res>(ClientRequest request)
            where Res : class
        {
            var webRequest = UnityWebRequest.Get(request.Url);

            // Add headers
            if (request.Headers != null)
            {
                foreach (KeyValuePair<string, string> header in request.Headers)
                {
                    webRequest.SetRequestHeader(header.Key, header.Value);
                }
            }

            // Add query params
            if (request.QueryParams != null)
            {
                UriBuilder uriBuilder = new(request.Url);
                string query = uriBuilder.Query; // Existing query
                StringBuilder queryBuilder =
                    new(string.IsNullOrEmpty(query) ? "" : query.Substring(1)); // Remove leading '?'

                foreach (KeyValuePair<string, string> queryParam in request.QueryParams)
                {
                    if (queryBuilder.Length > 0)
                    {
                        queryBuilder.Append('&');
                    }
                    queryBuilder.Append(
                        $"{Uri.EscapeDataString(queryParam.Key)}={Uri.EscapeDataString(queryParam.Value)}"
                    );
                }

                uriBuilder.Query = queryBuilder.ToString();
                webRequest.uri = uriBuilder.Uri;
            }

            try
            {
                await webRequest.SendWebRequest();
                HttpResponseMessage response = webRequest.ToHttpResponseMessage();
                var content = webRequest.downloadHandler.text;
                return new ClientResponse<Res>(
                    (int)webRequest.responseCode,
                    GetReasonPhrase((int)webRequest.responseCode),
                    webRequest.result == UnityWebRequest.Result.Success
                        ? JsonConvert.DeserializeObject<Res>(content)!
                        : null,
                    !(webRequest.result == UnityWebRequest.Result.Success)
                        ? JsonConvert.DeserializeObject<JObject>(content)
                        : null,
                    response,
                    response.Headers.ToDictionary(h => h.Key, h => string.Join(", ", h.Value))
                );
            }
            catch (Exception e)
            {
                throw new Exception($"Error making request to {request.Url}: {e.Message}");
            }
        }

        public override async Task<ClientResponse<Res>> Post<Res>(ClientRequest request)
            where Res : class
        {
            var webRequest = new UnityWebRequest(request.Url, "POST");
            webRequest.downloadHandler = new DownloadHandlerBuffer();

            // Add headers
            if (request.Headers != null)
            {
                foreach (KeyValuePair<string, string> header in request.Headers)
                {
                    webRequest.SetRequestHeader(header.Key, header.Value);
                }
            }

            // Add content
            if (request.Body != null)
            {
                byte[] bodyData;
                if (request.Body is byte[] bytes)
                {
                    bodyData = bytes;
                }
                else
                {
                    string jsonBody = JsonConvert.SerializeObject(request.Body);
                    bodyData = Encoding.UTF8.GetBytes(jsonBody);
                    webRequest.SetRequestHeader("Content-Type", "application/json");
                }

                webRequest.uploadHandler = new UploadHandlerRaw(bodyData);

                if (request.ContentType != null)
                {
                    webRequest.SetRequestHeader("Content-Type", request.ContentType);
                }
            }

            // Add query params
            if (request.QueryParams != null)
            {
                UriBuilder uriBuilder = new(request.Url);
                string query = uriBuilder.Query;
                StringBuilder queryBuilder =
                    new(string.IsNullOrEmpty(query) ? "" : query.Substring(1));

                foreach (KeyValuePair<string, string> queryParam in request.QueryParams)
                {
                    if (queryBuilder.Length > 0)
                    {
                        queryBuilder.Append('&');
                    }
                    queryBuilder.Append(
                        $"{Uri.EscapeDataString(queryParam.Key)}={Uri.EscapeDataString(queryParam.Value)}"
                    );
                }

                uriBuilder.Query = queryBuilder.ToString();
                webRequest.uri = uriBuilder.Uri;
            }

            try
            {
                await webRequest.SendWebRequest();
                HttpResponseMessage response = webRequest.ToHttpResponseMessage();
                var content = webRequest.downloadHandler.text;
                return new ClientResponse<Res>(
                    (int)webRequest.responseCode,
                    GetReasonPhrase((int)webRequest.responseCode),
                    webRequest.result == UnityWebRequest.Result.Success
                        ? JsonConvert.DeserializeObject<Res>(content)!
                        : null,
                    !(webRequest.result == UnityWebRequest.Result.Success)
                        ? JsonConvert.DeserializeObject<JObject>(content)
                        : null,
                    response,
                    response.Headers.ToDictionary(h => h.Key, h => string.Join(", ", h.Value))
                );
            }
            catch (Exception e)
            {
                throw new Exception($"Error making request to {request.Url}", e);
            }
        }
    }
}
