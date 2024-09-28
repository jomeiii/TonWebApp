using System;
using Games;
using Managers;
using UnityEngine;

namespace Menus.SelectGameMenu
{
    public class SelectGameController : MonoBehaviour
    {
        public event Action OnSelectedGameTypeEvent;
        
        private GameManager GameManager => GameManager.Instance;

        public void DurakButtonHandler()
        {
            SelectGameButtonHandler(GameType.Durak);
        }

        private void SelectGameButtonHandler(GameType gameType)
        {
            GameManager.currentGameType = gameType;
            OnSelectedGameTypeEvent?.Invoke();
        }
    }
}