using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;

public class RoomItem : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI rmNameTmp;

    public void SetRoomName(string _name)
    {
        rmNameTmp.text = _name;
    }
    public string GetRoomName()
    {
        return rmNameTmp.text;
    }
    public void JoinRoom() 
    {

        LobbyManager.instance.JoinRoom();
    }
}
