using Menus.MainMenu;
using UnityEngine;

namespace UI.Presenters
{
    public class SelectGameTypeMenuPresenter : MonoBehaviour
    {
        [SerializeField] private GameObject _mainMenu;
        [SerializeField] private GameObject _selectGameTypeMenu;
        [SerializeField] private MainMenuController _managementController;

        private void OnEnable()
        {
            _managementController.OnMainMenuButtonPressedEvent += OnMainMenuButtonPressed;
        }

        private void OnDisable()
        {
            _managementController.OnMainMenuButtonPressedEvent -= OnMainMenuButtonPressed;
        }

        private void OnMainMenuButtonPressed()
        {
            _mainMenu.SetActive(false);
            _selectGameTypeMenu.SetActive(true);
        }
    }
}