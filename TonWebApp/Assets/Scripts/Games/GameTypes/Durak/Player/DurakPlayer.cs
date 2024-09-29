using System;
using System.Collections.Generic;
using Games.GameTypes.Durak.Deck;
using Games.GameTypes.Durak.Deck.CardVisualisation;
using UnityEngine;
using DropCard = Games.GameTypes.Durak.Deck.DropCard;

namespace Games.GameTypes.Durak.Player
{
    [Serializable]
    public class DurakPlayer
    {
        public Durak durak;
        private CardManager _cardManager;
        [SerializeField] protected List<Card> cards;

        // Текущий статус игрока: может атаковать или защищается
        public bool canAttack;

        public List<Card> Cards => cards;

        public event Action<List<Card>> CardAddedEvent;

        protected DurakPlayer(Durak durak, CardManager cardManager)
        {
            cards = new List<Card>();
            this.durak = durak;
            _cardManager = cardManager;
            _cardManager.Init(this);
        }

        protected DurakPlayer(Durak durak, CardManager cardManager, bool canAttack) : this(durak, cardManager)
        {
            this.canAttack = canAttack;
        }

        public void AddCard(Card card)
        {
            cards.Add(card);
            CardAddedEvent?.Invoke(cards);
        }

        public bool RemoveCard(int cardIndex)
        {
            if (cardIndex >= 0 && cardIndex < cards.Count)
            {
                cards.RemoveAt(cardIndex);
                CardAddedEvent?.Invoke(cards);
                return true;
            }

            return false;
        }

        public void Attack(int cardIndex)
        {
            if (HasCard(cardIndex))
            {
                var playerCard = cards[cardIndex];

                if (durak.isFirstMove)
                {
                    StartAttack(playerCard, cardIndex);
                }
                else
                {
                    ContinueAttack(playerCard, cardIndex);
                }

                durak.OnPlayerMovedEvent();
            }
        }

        public bool Defence(int cardIndex, int dropCardIndex)
        {
            if (HasCard(cardIndex))
            {
                var playerCard = cards[cardIndex];
                if (CanDefence(playerCard, dropCardIndex))
                {
                    durak.dropCards[dropCardIndex].UpperCard = playerCard;
                    RemoveCard(cardIndex);
                    durak.OnPlayerMovedEvent();
                    return true;
                }
            }

            return false;
        }

        protected bool HasCard(int cardIndex)
        {
            return cardIndex >= 0 && cardIndex < Cards.Count;
        }

        protected void StartAttack(Card playerCard, int cardIndex)
        {
            durak.isFirstMove = false;
            durak.dropCards.Add(new(playerCard));
            durak.CreateDropCardVisualization(durak.dropCards.Count - 1);
            RemoveCard(cardIndex);
        }

        protected void ContinueAttack(Card playerCard, int cardIndex)
        {
            foreach (var dropCard in durak.dropCards)
            {
                if (CanAttack(playerCard, dropCard))
                {
                    durak.dropCards.Add(new(playerCard));
                    durak.CreateDropCardVisualization(durak.dropCards.Count - 1);
                    RemoveCard(cardIndex);
                    return;
                }
            }
        }

        protected bool CanAttack(Card playerCard, DropCard dropCard)
        {
            return dropCard.LowerCard.Value == playerCard.Value ||
                   (!dropCard.isUpperCardNull && dropCard.UpperCard.Value == playerCard.Value);
        }

        protected bool CanDefence(Card playerCard, int dropCardIndex)
        {
            var dropCard = durak.dropCards[dropCardIndex];
            if (dropCard.LowerCard.CardType == durak.TrumpCard.CardType)
            {
                return playerCard.CardType == durak.TrumpCard.CardType &&
                       playerCard.Value > dropCard.LowerCard.Value;
            }
            else
            {
                if (playerCard.CardType == durak.TrumpCard.CardType)
                {
                    return true;
                }
                else
                {
                    return playerCard.CardType == dropCard.LowerCard.CardType &&
                           playerCard.Value > dropCard.LowerCard.Value;
                }
            }
        }
    }
}