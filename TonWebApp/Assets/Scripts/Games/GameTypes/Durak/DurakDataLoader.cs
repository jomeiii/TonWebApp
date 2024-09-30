using Games.GameTypes.Durak.Deck;
using Photon.Pun;
using UnityEngine;

namespace Games.GameTypes.Durak
{
    public class DurakDataLoader : MonoBehaviourPunCallbacks
    {
        [SerializeField] private Durak _durak;
        [SerializeField] PhotonView _photonView;

        private void Start()
        {
            _photonView = GetComponent<PhotonView>();

            if (!_photonView.IsMine)
            {
                enabled = false;
                return;
            }

            _durak = FindObjectOfType<Durak>();
            if (PhotonNetwork.CurrentRoom.PlayerCount == PhotonNetwork.CurrentRoom.MaxPlayers)
            {
                _photonView.RPC(nameof(ActivateReadyButton), RpcTarget.All, null);
            }

            // var deck = new Deck.Deck(36);
            // deck.TakeCard(out var trumpCard);
            // SendDeckData(photonViewObj, deck);
            // SendTrumpCard(photonViewObj ,trumpCard);
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
            _durak.Deck = JsonUtility.FromJson<Deck.Deck>(deckJson);
        }

        [PunRPC]
        private void LoadTrumpCard(string cardJson)
        {
            _durak.TrumpCard = JsonUtility.FromJson<Card>(cardJson);
        }

        [PunRPC]
        private void ActivateReadyButton()
        {
            _durak.readyButton.gameObject.SetActive(true);
            _durak.readyButton.onClick.AddListener(ReadyButtonHandler);
        }

        [PunRPC]
        private void LoadPlayerCount()
        {
            if (PhotonNetwork.CurrentRoom.MaxPlayers - 1 == _durak.playersCount)
            {
                var deck = new Deck.Deck(36);
                deck.TakeCard(out var trumpCard);
                SendDeckData(_photonView, deck);
                SendTrumpCard(_photonView, trumpCard);
            }

            _durak.playersCount++;
        }

        private void ReadyButtonHandler()
        {
            _photonView.RPC(nameof(LoadPlayerCount), RpcTarget.All, null);
        }
    }
}