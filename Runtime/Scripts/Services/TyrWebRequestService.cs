using UnityEngine.Networking;
using System;
using System.Collections;
using UnityEngine;

namespace TyrDK
{
    public class TyrWebRequestService : TyrClassService<TyrWebRequestService>
    {
        private UnityWebRequest WebRequest;

        public void CreatePostWebRequest(string url, string body = null)
        {
            if (string.IsNullOrEmpty(url))
            {
                WebRequest = null;
                throw new ArgumentException("URL cannot be null or empty.");
            }
            WebRequest = new UnityWebRequest(url, "POST");
            if (!string.IsNullOrEmpty(body))
            {
                byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(body);
                WebRequest.uploadHandler = new UploadHandlerRaw(bodyRaw);
            }
            else
            {
                WebRequest.uploadHandler = new UploadHandlerRaw(new byte[0]);
            }
            
            WebRequest.downloadHandler = new DownloadHandlerBuffer();
        }
        
        public void CreateGetWebRequest(string url)
        {
            if (string.IsNullOrEmpty(url))
            {
                WebRequest = null;
                throw new ArgumentException("URL cannot be null or empty.");
            }
            WebRequest = UnityWebRequest.Get(url);
        }

        public void AddHeader(string key, string value)
        {
            if (WebRequest == null)
            {
                throw new ArgumentException("WebRequest is not initialized. Please initialize it before adding headers.");
            }
            WebRequest.SetRequestHeader(key, value);
        }
        
        public void SetRequestBody(string body)
        {
            if (WebRequest == null)
            {
                throw new ArgumentException("WebRequest is not initialized. Please initialize it before adding headers.");
            }
            WebRequest.uploadHandler = new UploadHandlerRaw(System.Text.Encoding.UTF8.GetBytes(body));
            WebRequest.downloadHandler = new DownloadHandlerBuffer();
        }

        public IEnumerator SendRequest(Action<string> onSuccess, Action<string> onError)
        {
            if (WebRequest == null)
            {
                throw new ArgumentException("WebRequest is not initialized. Please initialize it before sending.");
            }
            yield return WebRequest.SendWebRequest();
            
            if (WebRequest.result == UnityWebRequest.Result.Success)
            {
                onSuccess?.Invoke(WebRequest.downloadHandler.text);
            }
            else
            {
                if (WebRequest.result == UnityWebRequest.Result.ConnectionError)
                {
                    Debug.LogError($"Connection error: {WebRequest.error}");
                }
                else if (WebRequest.result == UnityWebRequest.Result.ProtocolError)
                {
                    Debug.LogError($"Protocol error: {WebRequest.error}");
                }
                else if (WebRequest.result == UnityWebRequest.Result.DataProcessingError)
                {
                    Debug.LogError($"Data processing error: {WebRequest.error}");
                }
                else if (WebRequest.result == UnityWebRequest.Result.InProgress)
                {
                    Debug.LogError($"Request in progress: {WebRequest.error}");
                }
                else
                {
                    Debug.LogError($"Unknown error: {WebRequest.error}");
                }
            }
        }
    }
}