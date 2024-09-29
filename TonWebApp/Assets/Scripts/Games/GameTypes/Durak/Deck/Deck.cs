using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Games.GameTypes.Durak.Deck
{
    [Serializable]
    public class Deck
    {
        [SerializeField] private List<Card> _cards;

        public List<Card> Cards => _cards;

        public Deck(int deckSize)
        {
            _cards = new List<Card>();
            var cardTypesCount = Enum.GetValues(typeof(CardType)).Length;

            for (var i = 0; i < cardTypesCount; i++)
            {
                var cardType = (CardType)i;

                for (var j = Card.MaxValue; j > Card.MaxValue - deckSize / cardTypesCount; j--)
                {
                    if (j > 0)
                    {
                        var card = new Card(cardType, j);
                        _cards.Add(card);
                    }
                }
            }
            
            ShuffleDeck();
        }

        public bool TakeCard(out Card outCard)
        {
            if (_cards.Count == 0)
            {
                outCard = null;
                return false;
            }
            
            var card = _cards[0];
            _cards.Remove(card);
            outCard = card;
            return true;
        }
            
        // https://en.wikipedia.org/wiki/Fisher%E2%80%93Yates_shuffle
        private void ShuffleDeck()
        {
            for (int i = _cards.Count - 1; i > 0; i--)
            {
                int randomIndex = Random.Range(0, i + 1);
                (_cards[i], _cards[randomIndex]) = (_cards[randomIndex], _cards[i]);
            }
        }
    }
}