using Games;
using Managers;
using Menus.SelectGameMenu;
using UnityEngine;

namespace UI.Presenters.GamePresenters
{
    public class GamePresenter : MonoBehaviour
    {
        [SerializeField] private GameObject _game;
        
        [SerializeField] private GameType _gameType;
        [SerializeField] private SelectGameController _selectGameController;

        private GameManager GameManager => GameManager.Instance;
        
        private void OnEnable()
        {
            _selectGameController.OnSelectedGameTypeEvent += OnGameSelected;
        }

        private void OnDisable()
        {
            _selectGameController.OnSelectedGameTypeEvent -= OnGameSelected;
        }

        private void OnGameSelected()
        {
            if (GameManager.currentGameType == _gameType)
            {
                _game.SetActive(true);
            }
        }
    }
}