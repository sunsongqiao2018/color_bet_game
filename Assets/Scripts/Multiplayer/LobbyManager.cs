using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using TMPro;
using Photon.Realtime;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    [SerializeField] TMP_InputField createInput, joinInput;


    public void CreateRoom()
    {
        if (string.IsNullOrEmpty(createInput.text)) return;
        RoomOptions ro = new RoomOptions();
        ro.MaxPlayers = 2;
        PhotonNetwork.CreateRoom(createInput.text, ro);
    }
    public void JoinRoom()
    {
        if (string.IsNullOrEmpty(joinInput.text)) return;

        PhotonNetwork.JoinRoom(joinInput.text);
    }

    public override void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel("Game");
    }
}
