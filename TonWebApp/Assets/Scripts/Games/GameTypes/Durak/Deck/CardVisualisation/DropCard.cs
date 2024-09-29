using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Games.GameTypes.Durak.Deck.CardVisualisation
{
    [RequireComponent(typeof(Image))]
    public class DropCard : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        public Image upperCardImage;
        
        [SerializeField] private Image _lowerCardImage;

        [SerializeField] private int _index;
        [SerializeField] private Games.GameTypes.Durak.Deck.DropCard _dropCard;
        [SerializeField] private Durak _durak;

        public void Init(Durak durak, int index)
        {
            _index = index;
            _durak = durak;
            _dropCard = durak.dropCards[index];

            _lowerCardImage.sprite =
                Resources.Load<Sprite>($"Cards/{_dropCard.LowerCard.CardType}/{_dropCard.LowerCard.Value}");
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (_durak != null)
                _durak.dropCardIndex = _index;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (_durak != null)
                _durak.dropCardIndex = -1;
        }

        public void SetSprite(Card upperCard)
        {
            var sprite = Resources.Load<Sprite>($"Cards/{upperCard.CardType}/{upperCard.Value}");
            upperCardImage.sprite = sprite;
        }
    }
}