using Photon.Pun;
using UnityEngine;

namespace Games.GameTypes.Durak
{
    public class DurakDataLoader : MonoBehaviourPunCallbacks
    {
        [SerializeField] private Durak _durak;
        
        private void Start()
        {
            var photonViewObj = GetComponent<PhotonView>();
            if (!photonViewObj.IsMine)
            {
                enabled = false;
            }

            _durak = FindObjectOfType<Durak>();
            var deck = new Deck.Deck(36);
            photonViewObj.RPC(nameof(LoadData), RpcTarget.Others, JsonUtility.ToJson(deck));
        }

        [PunRPC]
        private void LoadData(Deck.Deck a)
        {
            _durak.Deck = a;
        }
    }
}