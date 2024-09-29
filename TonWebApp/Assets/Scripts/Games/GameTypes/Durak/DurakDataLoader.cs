using Photon.Pun;
using UnityEngine;

namespace Games.GameTypes.Durak
{
    public class DurakDataLoader : MonoBehaviourPunCallbacks
    {
        [SerializeField] private Durak _durak;
        
        private void Awake()
        {
            var photonViewObj = GetComponent<PhotonView>();
            if (!photonViewObj.IsMine)
            {
                enabled = false;
            }
            
            _durak = GameObject.FindGameObjectWithTag("Durak").GetComponent<Durak>();
            photonViewObj.RPC("LoadData", RpcTarget.Others, new Deck.Deck(36));
        }

        [PunRPC]
        private void LoadData(Deck.Deck deck)
        {
            _durak.Deck = deck;
        }
    }
}