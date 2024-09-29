using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JoingRoomManager : MonoBehaviour
{
    [SerializeField] string GameName;

    string obstacle;

    [Header("Fool")]
    [SerializeField] InputField RoomName;
    [SerializeField] Dropdown GameType;
    [SerializeField] Dropdown CountOfCards;
    [SerializeField] Dropdown CountOfPlayers;


    // Start is called before the first frame update
    void Start()
    {
        obstacle = "/";
        RoomName.text = "Room";
    }

    // Update is called once per frame
    public void CreateOrJoingRoom()
    {
        string gameName = GameName;
        switch (GameName)
        {
            case "Fool":
                gameName += obstacle + RoomName.text + obstacle + GameType.options[GameType.value].text + obstacle + CountOfCards.options[CountOfCards.value].text + obstacle  + CountOfPlayers.options[CountOfPlayers.value].text + obstacle;
                break;

        }
            
        GetComponentInParent<LobbyManager>().CreateRoom(GameName, Convert.ToInt32(CountOfPlayers.options[CountOfPlayers.value].text), gameName);
    }
}
