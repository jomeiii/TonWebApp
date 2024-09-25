using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace Cliker
{
    public class PlayerDataFetcher : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _usernameText;
        [SerializeField] private Image _avatarImage;
        [SerializeField] private TextMeshProUGUI _playerIDText;

        [SerializeField] private string _playerID;
        [SerializeField] private string _botToken;

        private const string APIUrl = "http://localhost:5000";

        private void Awake()
        {
            if (Application.platform == RuntimePlatform.WebGLPlayer)
            {
                Debug.Log("WebGL Player");
                GetPlayerIdFromUrl(); // Если это WebGL, получаем ID из URL
            }
            else
            {
                Debug.Log("Unity Player");
                _playerID = "860859651"; // Установка значения ID для других платформ
            }

            _playerIDText.text = _playerID;
            StartCoroutine(LoadBotToken());
            StartCoroutine(GetUserProfileCoroutine(_playerID));
        }

        private void GetPlayerIdFromUrl()
        {
            string url = Application.absoluteURL;
            int index = url.IndexOf("?user_id=", StringComparison.Ordinal);
            if (index != -1)
            {
                string userIdParam = url.Substring(index + 9); // 9 - длина "?user_id="
                _playerID = userIdParam;
                Debug.Log("Player ID: " + _playerID); // Выводим ID в консоль
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
                    // Парсим JSON-ответ
                    string jsonResponse = webRequest.downloadHandler.text;
                    UserProfile userProfile = JsonUtility.FromJson<UserProfile>(jsonResponse);

                    // Устанавливаем имя пользователя
                    _usernameText.text = userProfile.username;

                    // Загружаем аватарку
                    StartCoroutine(LoadAvatar(_playerID));
                }
                else
                {
                    // Ошибка при выполнении запроса
                    Debug.LogError("Error: " + webRequest.error);
                }
            }
        }

        private IEnumerator LoadAvatar(string userId)
        {
            // URL к эндпоинту для получения аватара
            string url = $"{APIUrl}/get_user_avatar/{userId}"; // Замените на свой URL

            using (UnityWebRequest webRequest = UnityWebRequestTexture.GetTexture(url))
            {
                yield return webRequest.SendWebRequest();

                if (webRequest.result == UnityWebRequest.Result.Success)
                {
                    // Если загрузка успешна, создаем текстуру
                    Texture2D texture = DownloadHandlerTexture.GetContent(webRequest);
                    _avatarImage.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
                }
                else
                {
                    Debug.LogError("Error loading avatar: " + webRequest.error);
                }
            }
        }


        private IEnumerator LoadBotToken()
        {
            string url = APIUrl + "/token";
            using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
            {
                yield return webRequest.SendWebRequest();

                if (webRequest.result == UnityWebRequest.Result.Success)
                {
                    string token = webRequest.downloadHandler.text;
                    _botToken = token;
                }
                else
                {
                    Debug.LogError("Error loading bot token: " + webRequest.error);
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