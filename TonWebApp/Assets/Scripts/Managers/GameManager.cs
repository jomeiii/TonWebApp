using Games;
using UnityEngine;

namespace Managers
{
    public class GameManager : Singleton<GameManager>
    {
        public TelegramData telegramData;
        
        public GameType currentGameType;
        
        public bool isDataLoaded;

        protected override void Awake()
        {
            base.Awake();
            telegramData = new();
        }
    }

    public class TelegramData
    {
        public string playerID;
        public string playerUserName;
        public Texture2D avatar;
    }
}