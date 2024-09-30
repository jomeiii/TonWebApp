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
        Debug.Log(GameObject.Find("Canvas").name);
        GameObject.Find("Canvas").GetComponent<LobbyManager>().JoinGame(RoomName);
    }
}
