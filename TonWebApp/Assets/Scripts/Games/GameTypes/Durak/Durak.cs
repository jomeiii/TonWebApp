using System;
using System.Collections.Generic;
using Dynamic;
using Games.GameTypes.Durak.Deck;
using Games.GameTypes.Durak.Player;
using UnityEngine;

namespace Games.GameTypes.Durak
{
    public class Durak : Game
    {
        private const int SizeHands = 6;

        public int cardIndex;
        public int dropCardIndex;

        public bool isFirstMove = true;

        public List<DropCard> dropCards;

        [SerializeField] private Deck.Deck _deck;

        [SerializeField] private Card _trumpCard;

        [SerializeField] private int _currentPlayerIndex;
        [SerializeField] private List<DurakPlayer> _players;
        private bool _isDefencePlayerWin = true;

        public event Action<int> CurrentPlayerIndexChangedEvent;
        public event Action CurrentPlayerLoseEvent;
        public event Action PlayerMovedEvent;

        public Card TrumpCard => _trumpCard;

        public bool IsDefencePlayerWin
        {
            get => _isDefencePlayerWin;
            set => _isDefencePlayerWin = value;
        }

        protected override void Awake()
        {
            base.Awake();

            _deck = new Deck.Deck(36);
            _deck.TakeCard(out _trumpCard);

            // Создаем игроков
            _players.Add(new HumanPlayer(durak: this, canAttack: true)); // Реальный игрок
            _players.Add(new DurakBot(durak: this, _players.Count)); // Бот

            DealCard();
        }

        /// <summary>
        /// Обработчик хода только для живых игроков
        /// </summary>
        /// <param name="playerIndex">Индекс игрока, который только что сходил или попытался</param>
        public void Move(int playerIndex)
        {
            if (playerIndex >= 0 && playerIndex < _players.Count)
            {
                if (IsDefencePlayerIndex(playerIndex))
                {
                    DynamicDebug.Debug(nameof(Durak), nameof(Move), "Current player defence");
                    _players[playerIndex].Defence(cardIndex, dropCardIndex);
                }
                else
                {
                    // (stepa) TODO: Добавить еще условие для подкидывают только соседи
                    DynamicDebug.Debug(nameof(Durak), nameof(Move), "Current player attack");
                    _players[playerIndex].Attack(cardIndex);
                }
            }
        }

        public void EndMoveButtonHandler(int playerIndex)
        {
            if (playerIndex != _currentPlayerIndex) return;

            if (!_isDefencePlayerWin)
            {
                int defencePlayerIndex = (_currentPlayerIndex + 1) % _players.Count;

                foreach (var dropCard in dropCards)
                {
                    _players[defencePlayerIndex].AddCard(dropCard.LowerCard);
                    if (!dropCard.UpperCard.IsNull) _players[defencePlayerIndex].AddCard(dropCard.UpperCard);
                }

                _players[defencePlayerIndex].canAttack = true;
                _currentPlayerIndex = (_currentPlayerIndex + 2) % _players.Count;
                _players[(_currentPlayerIndex + 1) % _players.Count].canAttack = false;
            }
            else
            {
                _currentPlayerIndex = (_currentPlayerIndex + 1) % _players.Count;
                _players[_currentPlayerIndex].canAttack = true;
                _players[(_currentPlayerIndex + 1) % _players.Count].canAttack = false;
            }

            dropCards.Clear();
            DealCard();

            _isDefencePlayerWin = true;
            isFirstMove = true;

            CurrentPlayerIndexChangedEvent?.Invoke(_currentPlayerIndex);
        }

        public void LoseButtonHandler(int playerIndex)
        {
            if (!IsDefencePlayerIndex(playerIndex)) return;
            _isDefencePlayerWin = false;
            EndMoveButtonHandler(_currentPlayerIndex);

            CurrentPlayerLoseEvent?.Invoke();
        }

        public virtual void OnPlayerMovedEvent()
        {
            PlayerMovedEvent?.Invoke();
        }

        private void DealCard()
        {
            for (int i = _currentPlayerIndex; i < _players.Count + _currentPlayerIndex; i++)
            {
                int playerIndex = i % _players.Count;
                for (int j = _players[playerIndex].Cards.Count; j < SizeHands; j++)
                {
                    if (_deck.TakeCard(out Card card))
                    {
                        _players[playerIndex].AddCard(card);
                    }
                    else
                    {
                        if (_trumpCard != null && !_trumpCard.IsNull)
                        {
                            _players[playerIndex].AddCard(_trumpCard);
                            _trumpCard = null;
                        }
                        else
                        {
                            return;
                        }
                    }
                }
            }
        }

        private bool IsDefencePlayerIndex(int playerIndex)
        {
            return playerIndex == (_currentPlayerIndex + 1) % _players.Count;
        }
    }
}