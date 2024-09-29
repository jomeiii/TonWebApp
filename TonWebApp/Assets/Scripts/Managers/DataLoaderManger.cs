using UnityEngine;
using Photon.Pun;

namespace Managers
{
    public class DataLoaderManger : MonoBehaviour
    {
        private void Awake()
        {
            var loader = PhotonNetwork.Instantiate("LoaderManager", Vector3.zero, Quaternion.identity, 0);
        }
    }
}