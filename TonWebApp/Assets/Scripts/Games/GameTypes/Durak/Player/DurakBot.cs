using Dynamic;
using Games.GameTypes.Durak.Deck;
using Games.GameTypes.Durak.Deck.CardVisualisation;
using UnityEngine;

namespace Games.GameTypes.Durak.Player
{
    public class DurakBot : DurakPlayer
    {
        private readonly int _botIndex;

        public DurakBot(Durak durak, CardManager cardManager, int botIndex) : base(durak, cardManager)
        {
            _botIndex = botIndex;
            durak.CurrentPlayerIndexChangedEvent += OnCurrentPlayerIndexChange;
            durak.PlayerMovedEvent += OnPlayerMoved;
        }

        public DurakBot(Durak durak, CardManager cardManager, int botIndex, bool canAttack) : base(durak, cardManager,
            canAttack)
        {
            _botIndex = botIndex;
            durak.CurrentPlayerIndexChangedEvent += OnCurrentPlayerIndexChange;
            durak.PlayerMovedEvent += OnPlayerMoved;
        }

        private void OnCurrentPlayerIndexChange(int currentPlayerIndex)
        {
            if (_botIndex == currentPlayerIndex)
            {
                int cardIndex = FindTheSmallestAttackCardIndex();
                Attack(cardIndex);
            }
        }

        private void OnPlayerMoved()
        {
            if (canAttack)
            {
                // Проверяем, есть ли карты для атаки
                if (HasAttackCard())
                {
                    // Если есть, то атакуем
                    Attack();
                }
                else
                {
                    if (durak.dropCards.Count > 0)
                    {
                        foreach (var dropCard in durak.dropCards)
                        {
                            if (dropCard.isUpperCardNull)
                            {
                                return;
                            }
                        }

                        durak.EndMoveButtonHandler(_botIndex);
                    }
                }
            }
            else
            {
                // Если нет возможности атаковать, то защищаемся
                Defend();
            }
        }

        private bool HasAttackCard()
        {
            // Проверяем, есть ли карта для атаки
            for (int i = 0; i < cards.Count; i++)
            {
                for (int j = 0; j < durak.dropCards.Count; j++)
                {
                    if (cards[i].Value == durak.dropCards[j].LowerCard.Value ||
                        (!durak.dropCards[j].isUpperCardNull &&
                         cards[i].Value == durak.dropCards[j].UpperCard.Value))
                    {
                        return true; // Если нашли подходящую карту, то возвращаем true
                    }
                }
            }

            return false; // Если подходящей карты не нашли, то возвращаем false
        }

        private void Attack()
        {
            // Атакуем, используя подходящую карту
            for (int i = 0; i < cards.Count; i++)
            {
                for (int j = 0; j < durak.dropCards.Count; j++)
                {
                    if (cards[i].Value == durak.dropCards[j].LowerCard.Value ||
                        (!durak.dropCards[j].isUpperCardNull &&
                         cards[i].Value == durak.dropCards[j].UpperCard.Value))
                    {
                        Attack(i);
                        i--; // Сдвигаем индекс на один назад, чтобы не пропустить следующую карту
                        break; // Выходим из внутреннего цикла, если нашли подходящую карту
                    }
                }
            }
        }

        private void Defend()
        {
            // Защищаемся от сброшенных карт
            for (int i = 0; i < durak.dropCards.Count; i++)
            {
                // Если карта не побита
                if (durak.dropCards[i].isUpperCardNull)
                {
                    int cardIndex = FindTheSmallestDefenceCard(durak.dropCards[i].LowerCard);
                    // Если карту нашли, то бьем
                    if (cardIndex != -1)
                    {
                        Defence(cardIndex, i);
                    }
                    // Если карту не нашли значит бот должен взять
                    else
                    {
                        durak.IsDefencePlayerWin = false;
                        return;
                    }
                }
            }
        }

        private int FindTheSmallestDefenceCard(Card dropCard)
        {
            // Ищем самую маленькую карту, подходящую для защиты:
            // 1. Сначала ищем козырь, если сбрасываемая карта - козырь
            // 2. Затем ищем карту того же типа, что и сбрасываемая карта

            int smallestTrumpCardIndex = -1;
            int smallestTrumpCardValue = int.MaxValue;

            int smallestDropCardTypeIndex = -1;
            int smallestDropCardTypeValue = int.MaxValue;

            // Если сбрасываемая карта - козырь, то ищем козырь в руке
            if (dropCard.CardType == durak.TrumpCard)
            {
                for (int i = 0; i < cards.Count; i++)
                {
                    var card = cards[i];

                    // Ищем козырь, который больше по значению, чем сбрасываемая карта
                    if (card.CardType == durak.TrumpCard &&
                        card.Value < smallestTrumpCardValue &&
                        card.Value > dropCard.Value)
                    {
                        smallestTrumpCardIndex = i;
                        smallestTrumpCardValue = card.Value;
                    }
                }

                // Если нашли козырь, то возвращаем его индекс
                if (smallestTrumpCardIndex != -1)
                {
                    return smallestTrumpCardIndex;
                }
            }

            // Иначе, если сбрасываемая карта не козырь, то ищем карту того же типа
            for (int i = 0; i < cards.Count; i++)
            {
                var card = cards[i];

                // Ищем козырь, если он есть
                if (card.CardType == durak.TrumpCard &&
                    card.Value < smallestTrumpCardValue)
                {
                    smallestTrumpCardIndex = i;
                    smallestTrumpCardValue = card.Value;
                }
                // Ищем карту того же типа, что и сбрасываемая карта
                else if (card.CardType == dropCard.CardType &&
                         card.Value < smallestDropCardTypeValue &&
                         card.Value > dropCard.Value)
                {
                    smallestDropCardTypeIndex = i;
                    smallestDropCardTypeValue = card.Value;
                }
            }

            // Возвращаем индекс найденной карты (козыря или карты того же типа)
            // Если ни одной подходящей карты не найдено, то возвращаем -1
            if (smallestDropCardTypeIndex != -1)
            {
                return smallestDropCardTypeIndex;
            }
            else
            {
                return smallestTrumpCardIndex;
            }
        }


        private int FindTheSmallestAttackCardIndex()
        {
            int smallestNonTrumpCardIndex = -1;
            int smallestNonTrumpCardValue = int.MaxValue;
            int smallestTrumpCardIndex = -1;
            int smallestTrumpCardValue = int.MaxValue;

            for (int i = 0; i < Cards.Count; i++)
            {
                var playerCard = Cards[i];

                // Сначала обрабатываем не козыри
                if (playerCard.CardType != durak.TrumpCard)
                {
                    if (playerCard.Value < smallestNonTrumpCardValue)
                    {
                        smallestNonTrumpCardIndex = i;
                        smallestNonTrumpCardValue = playerCard.Value;
                    }
                }
                // Потом козыри
                else
                {
                    if (playerCard.Value < smallestTrumpCardValue)
                    {
                        smallestTrumpCardIndex = i;
                        smallestTrumpCardValue = playerCard.Value;
                    }
                }
            }

            if (smallestNonTrumpCardIndex != -1)
            {
                return smallestNonTrumpCardIndex;
            }
            else if (smallestTrumpCardIndex != -1)
            {
                return smallestTrumpCardIndex;
            }
            else
            {
                return -1;
            }
        }
    }
}