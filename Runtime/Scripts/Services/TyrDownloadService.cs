using UnityEngine;
using System.Threading.Tasks;
using UnityEngine.Networking;

namespace TyrDK
{
    public class TyrDownloadService : TyrClassService<TyrDownloadService>
    {
        public async Task<Sprite> DownloadImage(string url)
        {
            using var www = UnityWebRequestTexture.GetTexture(url);
            var operation = www.SendWebRequest();
            
            while (!operation.isDone)
            {
                await Task.Yield();
            }
            
            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError($"Error downloading image: {www.error}");
                return null;
            }
            
            var texture = DownloadHandlerTexture.GetContent(www);
            return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
        }
    }
}