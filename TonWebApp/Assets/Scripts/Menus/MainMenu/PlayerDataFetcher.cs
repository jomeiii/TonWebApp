using System;
using System.Collections;
using Games;
using Managers;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace Menus.MainMenu
{
    public class PlayerDataFetcher : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _usernameText;
        [SerializeField] private Image _avatarImage;

        private string APIUrl => ServerSetting.APIUrl;
        private GameManager GameManager => GameManager.Instance;

        private void Awake()
        {
            if (Application.platform == RuntimePlatform.WebGLPlayer)
            {
                Debug.Log("WebGL Player");
                GetPlayerIdFromUrl();
            }
            else
            {
                Debug.Log("Unity Player");
                GameManager.telegramData.playerID = "860859651";
            }

            StartCoroutine(GetUserProfileCoroutine(GameManager.telegramData.playerID));
        }

        private void GetPlayerIdFromUrl()
        {
            string url = Application.absoluteURL;
            int index = url.IndexOf("?user_id=", StringComparison.Ordinal);
            if (index != -1)
            {
                string userIdParam = url.Substring(index + 9); // 9 - длина "?user_id="
                GameManager.telegramData.playerID = userIdParam;
                Debug.Log("Player ID: " + userIdParam);
            }
            else
            {
                Debug.LogError("Player ID not found in URL.");
            }
        }

        private IEnumerator GetUserProfileCoroutine(string userId)
        {
            var fullUrl = APIUrl + "/user/" + userId;

            using (UnityWebRequest webRequest = UnityWebRequest.Get(fullUrl))
            {
                yield return webRequest.SendWebRequest();

                if (webRequest.result == UnityWebRequest.Result.Success)
                {
                    string jsonResponse = webRequest.downloadHandler.text;
                    UserProfile userProfile = JsonUtility.FromJson<UserProfile>(jsonResponse);
    
                    GameManager.telegramData.playerUserName = userProfile.username;
                    GameManager.telegramData.playerUserName = userProfile.username;
                    _usernameText.text = userProfile.username;

                    yield return StartCoroutine(LoadAvatar(GameManager.telegramData.playerID));
                    GameManager.isDataLoaded = true;
                }
                else
                {
                    Debug.LogError("Error: " + webRequest.error);
                }
            }
        }

        private IEnumerator LoadAvatar(string userId)
        {
            string url = $"{APIUrl}/get_user_avatar/{userId}";

            using (UnityWebRequest webRequest = UnityWebRequestTexture.GetTexture(url))
            {
                yield return webRequest.SendWebRequest();

                if (webRequest.result == UnityWebRequest.Result.Success)
                {
                    Texture2D texture = DownloadHandlerTexture.GetContent(webRequest);
                    GameManager.telegramData.avatar = texture;
                    _avatarImage.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
                }
                else
                {
                    Debug.LogError("Error loading avatar: " + webRequest.error);
                }
            }
        }
    }

    [System.Serializable]
    public class UserProfile
    {
        public string first_name;
        public string last_name;
        public string username;
        public string avatar_url;
    }
}