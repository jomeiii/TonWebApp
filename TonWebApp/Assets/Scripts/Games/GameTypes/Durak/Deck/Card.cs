using System;
using UnityEngine;

namespace Games.GameTypes.Durak.Deck
{
    [Serializable]
    public class Card
    {
        public const int MaxValue = 14;

        [SerializeField] private CardType _cardType;
        [SerializeField] private int _value;

        public CardType CardType => _cardType;
        public int Value => _value;

        public bool IsNull => CardType == CardType.Hearts && _value == 0;


        public Card(CardType cardType, int value)
        {
            _cardType = cardType;
            _value = value;
        }
    }
}