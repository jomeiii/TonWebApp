using System.Collections.Generic;
using Games.GameTypes.Durak.Player;
using UnityEngine;

namespace Games.GameTypes.Durak.Deck.CardVisualisation
{
    public class CardManager : MonoBehaviour
    {
        [SerializeField] private Vector2 _cardPosition;
        [SerializeField] private Transform _cardRotationPoint;
        [SerializeField] private float _cardSpacing;
        [SerializeField] private bool _currentPlayerOwner;
        [SerializeField] private Durak _durak;
        
        private DurakPlayer _durakPlayer;
        private Transform _cardContainer;
        private CardVisualisation _cardVisualisationPrefab;
        private List<CardVisualisation> _spawnCard;

        private void OnDisable()
        {
            _durakPlayer.CardAddedEvent -= CardChange;
        }

        public void Init(DurakPlayer durakPlayer)
        {
            _durakPlayer = durakPlayer;
            _durakPlayer.CardAddedEvent += CardChange;
            _cardContainer = transform;
            _cardVisualisationPrefab = Resources.Load<CardVisualisation>("Cards/CardPrefab");
            _spawnCard = new();
        }

        public void CardChange(List<Card> cards)
        {
            ClearSpawnCards();

            float cardWidth = _cardVisualisationPrefab.GetComponent<RectTransform>().rect.width;

            for (int i = 0; i < cards.Count; i++)
            {
                var cardObj = Instantiate(_cardVisualisationPrefab, _cardContainer);
                cardObj.name = $"Card {i + 1}";
                _spawnCard.Add(cardObj);

                // Рассчитываем позицию карты по X
                float cardX = _cardPosition.x + (i - cards.Count / 2f) * (cardWidth + _cardSpacing) + cardWidth / 2;
                cardObj.transform.localPosition = new Vector2(cardX, _cardPosition.y);
                // Рассчитываем угол поворота
                float dist1 = Vector3.Distance(cardObj.transform.localPosition, _cardRotationPoint.localPosition);
                float dist2 = Vector3.Distance(cardObj.transform.localPosition,
                    new Vector3(cardObj.transform.localPosition.x, _cardRotationPoint.transform.localPosition.y, 0));
                var angle = Mathf.Acos(dist2 / dist1) * Mathf.Rad2Deg;
                if (_currentPlayerOwner)
                    cardObj.transform.rotation =
                        i >= cards.Count / 2 ? Quaternion.Euler(0, 0, -angle) : Quaternion.Euler(0, 0, angle);
                else
                    cardObj.transform.rotation =
                        i >= cards.Count / 2 ? Quaternion.Euler(0, 0, angle) : Quaternion.Euler(0, 0, -angle);
                
                cardObj.Init(cards[i], cardObj.transform.localPosition, _currentPlayerOwner, _durak, i);
            }
        }

        private void ClearSpawnCards()
        {
            foreach (var card in _spawnCard)
            {
                Destroy(card.gameObject);
            }

            _spawnCard.Clear();
        }
    }
}