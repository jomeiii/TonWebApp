using System;
using Managers;
using UnityEngine;

namespace Menus.MainMenu
{
    public class MainMenuController : MonoBehaviour
    {
        public event Action OnMainMenuButtonPressedEvent;
        
        public void HandleMainMenuButtonPress()
        {
            if (GameManager.Instance.isDataLoaded)
            {
                OnMainMenuButtonPressedEvent?.Invoke();
            }
        }
    }
}