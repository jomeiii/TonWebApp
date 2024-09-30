using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using Photon.Realtime;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    string Scene;
    [SerializeField] GameObject content;

    [SerializeField] GameObject roomListPrefab;
    string GameType = "";

    [Header("Fool")]
    [SerializeField] Toggle FlipedUp;
    [SerializeField] Toggle Translated;
    [SerializeField] Dropdown CountOfCards;
    [SerializeField] Dropdown CountOfPlayers;

    private void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
        Debug.Log("connected");
    }

    public void GetType(string type)
    {
        GameType = type;
    }

    public void JoinGame(string name)
    {
        string[] getScene;
        PhotonNetwork.JoinRoom(name);
        getScene = name.Split("/");
        Scene = getScene[0];
    }

    public override void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel(Scene);
    }

    public void JoingRandomRoom()
    {
        PhotonNetwork.JoinRandomRoom();
    }

    public void CreateRoom(string type, int CountOfPlayers, string GameName)
    {
        Scene = type;
        int name = PhotonNetwork.CountOfRooms + 1;
        Debug.Log(GameName + name);
        PhotonNetwork.CreateRoom(GameName + name, new Photon.Realtime.RoomOptions { MaxPlayers = CountOfPlayers });
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        string[] SharedName;
        Debug.Log(roomList.Count);
        for (int i = 0; i < roomList.Count; i++)
        {
            SharedName = roomList[i].Name.Split("/");
            switch (GameType)
            {
                case "TestCliker":
                    Debug.Log("case");
                    if (SharedName[0] == GameType)
                    {
                        Debug.Log("type");
                        if (SharedName[SharedName.Length - 2] == CountOfPlayers.options[CountOfPlayers.value].text)
                        {
                            if (FlipedUp.isOn == true)
                            {
                                foreach (string name in SharedName)
                                {
                                    if (name == "flip-up")
                                    {
                                        GameObject room = Instantiate(roomListPrefab, Vector3.zero, Quaternion.identity, content.transform);
                                        room.GetComponent<RoomManager>().GetName(SharedName[1], roomList[i].Name);
                                    }
                                }
                            }

                            if (Translated.isOn == true)
                            {
                                foreach (string name in SharedName)
                                {
                                    if (name == "translated")
                                    {
                                        GameObject room = Instantiate(roomListPrefab, Vector3.zero, Quaternion.identity, content.transform);
                                        room.GetComponent<RoomManager>().GetName(SharedName[1], roomList[i].Name);
                                    }
                                }
                            }
                        }
                    }
                    break;
            }
            Debug.Log(roomList[i].Name);
        }
    }
}
