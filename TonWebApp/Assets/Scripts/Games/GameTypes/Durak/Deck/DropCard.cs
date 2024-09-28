using System;
using UnityEngine;

namespace Games.GameTypes.Durak.Deck
{
    [Serializable]
    public class DropCard
    {
        [SerializeField] private Card _lowerCard;
        [SerializeField] private Card _upperCard;

        public bool isUpperCardNull = true;

        public Card LowerCard => _lowerCard;

        public Card UpperCard
        {
            get => _upperCard;
            set
            {
                isUpperCardNull = false;
                _upperCard = value;
            }
        }

        public DropCard(Card lowerCard)
        {
            _lowerCard = lowerCard;
            _upperCard = null;
        }
    }
}