using Games.GameTypes.Durak.Deck;
using Photon.Pun;
using UnityEngine;

namespace Games.GameTypes.Durak
{
    public class DurakDataLoader : MonoBehaviourPunCallbacks
    {
        [SerializeField] private Durak _durak;

        private void Start()
        {
            // Получаем компонент PhotonView
            var photonViewObj = GetComponent<PhotonView>();

            // Проверяем, принадлежит ли объект текущему игроку
            if (!photonViewObj.IsMine)
            {
                enabled = false; // Отключаем компонент, если это не наш объект
                return; // Выходим из метода
            }

            // Находим объект Durak в сцене
            _durak = FindObjectOfType<Durak>();

            // Создаем колоду карт и отправляем данные другим игрокам
            var deck = new Deck.Deck(36);
            deck.TakeCard(out var trumpCard);
            SendDeckData(photonViewObj, deck);
            SendTrumpCard(photonViewObj ,trumpCard);
        }

        private void SendDeckData(PhotonView photonViewObj, Deck.Deck deck)
        {
            string deckJson = JsonUtility.ToJson(deck);
            photonViewObj.RPC(nameof(LoadData), RpcTarget.All, deckJson);
        }

        private void SendTrumpCard(PhotonView photonViewObj, Card card)
        {
            string cardJson = JsonUtility.ToJson(card);
            photonViewObj.RPC(nameof(LoadTrumpCard), RpcTarget.All, cardJson);
        }

        [PunRPC]
        private void LoadData(string deckJson)
        {
            // Десериализуем данные колоды и присваиваем их объекту Durak
            _durak.Deck = JsonUtility.FromJson<Deck.Deck>(deckJson);
        }

        [PunRPC]
        private void LoadTrumpCard(string cardJson)
        {
            _durak.TrumpCard = JsonUtility.FromJson<Card>(cardJson);
        }
    }
}