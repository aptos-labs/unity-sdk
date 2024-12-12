namespace Aptos
{
    using System.Net;
    using System.Net.Http;
    using System.Runtime.CompilerServices;
    using System.Text;
    using System.Threading.Tasks;
    using UnityEngine;
    using UnityEngine.Networking;

    public static class UnityWebRequestExtensions
    {
        ///<summmary>
        /// This extension to allow using await with async web requests
        ///</summmary>
        public static TaskAwaiter GetAwaiter(this AsyncOperation asyncOp)
        {
            var tcs = new TaskCompletionSource<object>();
            asyncOp.completed += obj =>
            {
                tcs.SetResult(null);
            };
            return ((Task)tcs.Task).GetAwaiter();
        }

        public static HttpResponseMessage ToHttpResponseMessage(
            this UnityWebRequest unityWebRequest
        )
        {
            if (
                unityWebRequest.result == UnityWebRequest.Result.ConnectionError
                || unityWebRequest.result == UnityWebRequest.Result.ProtocolError
            )
            {
                // Handle UnityWebRequest error
                throw new HttpRequestException(unityWebRequest.error);
            }

            // Create HttpResponseMessage
            var responseMessage = new HttpResponseMessage(
                (HttpStatusCode)unityWebRequest.responseCode
            )
            {
                ReasonPhrase = unityWebRequest.error, // Optional: Maps error messages
                Content = new StringContent(
                    unityWebRequest.downloadHandler?.text ?? string.Empty,
                    Encoding.UTF8
                ),
            };

            // Transfer headers
            foreach (var header in unityWebRequest.GetResponseHeaders())
            {
                responseMessage.Headers.TryAddWithoutValidation(header.Key, header.Value);
            }

            return responseMessage;
        }
    }
}
