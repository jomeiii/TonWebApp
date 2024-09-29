using System;
using Games.GameTypes.Durak.Deck.CardVisualisation;

namespace Games.GameTypes.Durak.Player
{
    [Serializable]
    public class HumanPlayer : DurakPlayer
    {
        public HumanPlayer(Durak durak, CardManager cardManager) : base(durak, cardManager)
        {
        }

        public HumanPlayer(Durak durak, CardManager cardManager, bool canAttack) : base(durak, cardManager ,canAttack)
        {
        }
    }
}