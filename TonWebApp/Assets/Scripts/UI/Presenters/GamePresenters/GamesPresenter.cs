using Menus.SelectGameMenu;
using UnityEngine;

namespace UI.Presenters.GamePresenters
{
    public class GamesPresenter : MonoBehaviour
    {
        [SerializeField] private GameObject _gamesPanel;
        [SerializeField] private GameObject _selectGameTypeMenuPanel;
        [SerializeField] private SelectGameController _selectGameController;

        private void OnEnable()
        {
            _selectGameController.OnSelectedGameTypeEvent += OnSelectGameType;
        }

        private void OnDisable()
        {
            _selectGameController.OnSelectedGameTypeEvent -= OnSelectGameType;
        }

        private void OnSelectGameType()
        {
            _selectGameTypeMenuPanel.SetActive(false);
            _gamesPanel.SetActive(true);
        }
    }
}