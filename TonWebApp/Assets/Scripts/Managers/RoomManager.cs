using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomManager : MonoBehaviour
{
    [SerializeField] Text Name;
    string RoomName;

    public void GetName(string name, string roomname)
    {
        Name.text = name;
        RoomName = roomname;
    }

    public void Joing()
    {
        GameObject.Find("Convas").GetComponent<LobbyManager>().JoinGame(RoomName);
    }
}
