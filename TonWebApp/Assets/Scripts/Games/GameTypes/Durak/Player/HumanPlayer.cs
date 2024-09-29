namespace Games.GameTypes.Durak.Player
{
    public class HumanPlayer : DurakPlayer
    {
        public HumanPlayer(Durak durak) : base(durak)
        {
        }

        public HumanPlayer(Durak durak, bool canAttack) : base(durak, canAttack)
        {
        }
    }
}