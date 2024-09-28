using System.Collections;
using Managers;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace Games
{
    public class Game : MonoBehaviour
    {
        [SerializeField] private Image _userAvatar;
        [SerializeField] private Image _opponentAvatar;
        
        private GameManager GameManager => GameManager.Instance;

        protected virtual void Awake()
        {
            Init("5501139939");
        }

        protected virtual void Init(string opponentID)
        {
            var avatarTexture = GameManager.telegramData.avatar; 
            _userAvatar.sprite = Sprite.Create(avatarTexture, new Rect(0, 0, avatarTexture.width, avatarTexture.height), Vector2.zero);
            
            StartCoroutine(LoadOpponentAvatar(opponentID));
        }
        
        private IEnumerator LoadOpponentAvatar(string userId)
        {
            string url = $"{ServerSetting.APIUrl}/get_user_avatar/{userId}";

            using (UnityWebRequest webRequest = UnityWebRequestTexture.GetTexture(url))
            {
                yield return webRequest.SendWebRequest();

                if (webRequest.result == UnityWebRequest.Result.Success)
                {
                    Texture2D texture = DownloadHandlerTexture.GetContent(webRequest);
                    _opponentAvatar.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
                }
                else
                {
                    Debug.LogError("Error loading avatar: " + webRequest.error);
                }
            }
        }
    }
}