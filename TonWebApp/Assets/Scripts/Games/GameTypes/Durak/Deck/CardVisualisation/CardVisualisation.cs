using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Games.GameTypes.Durak.Deck.CardVisualisation
{
    public class CardVisualisation : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IBeginDragHandler,
        IDragHandler, IEndDragHandler
    {
        [SerializeField] private Image _image;
        [SerializeField] private Card _card;
        [SerializeField] private int _cardIndex;
        
        private Vector3 _startPosition;
        private Durak _durak;

        public void Init(Card card, Vector3 startPosition, bool isOwner, Durak durak, int cardIndex)
        {
            _card = card;
            Sprite imageSprite = Resources.Load<Sprite>(isOwner ? $"Cards/{card.CardType}/{card.Value}" : "Cards/Shirt");

            if (imageSprite != null)
                _image.sprite = imageSprite;

            _startPosition = startPosition;
            _durak = durak;
            _cardIndex = cardIndex;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            UpScale();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            DownScale();
        }


        public void OnBeginDrag(PointerEventData eventData)
        {
            // Отключаем блокировку взаимодействия с объектом
            gameObject.GetComponent<CanvasGroup>().blocksRaycasts = false;
            UpScale();
        }

        public void OnDrag(PointerEventData eventData)
        {
            // Перемещаем объект в позицию курсора
            RectTransform rectTransform = GetComponent<RectTransform>();
            rectTransform.anchoredPosition += eventData.delta / CanvasScaleFactor();
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            // Включаем блокировку взаимодействия с объектом
            gameObject.GetComponent<CanvasGroup>().blocksRaycasts = true;
            DownScale();
            transform.localPosition = _startPosition;
            _durak.cardIndex = _cardIndex;
            _durak.Move(0);
        }

        private float CanvasScaleFactor()
        {
            // Получаем масштаб Canvas для корректного перемещения
            return GetComponentInParent<Canvas>().scaleFactor;
        }

        private void UpScale()
        {
            transform.localScale *= 1.2f;
        }

        private void DownScale()
        {
            transform.localScale /= 1.2f;
        }
    }
}