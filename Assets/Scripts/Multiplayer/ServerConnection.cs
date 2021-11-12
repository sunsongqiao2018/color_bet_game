using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class ServerConnection : MonoBehaviourPunCallbacks
{
    [SerializeField] GameObject loadingGO, LobbyGO;
    // Start is called before the first frame update
    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
        ToggleLoadingAndLobby(true);
    }
    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
    }
    public override void OnJoinedLobby()
    {
        ToggleLoadingAndLobby(false);
    }
    private void ToggleLoadingAndLobby(bool value)
    {
        loadingGO.SetActive(value);
        LobbyGO.SetActive(!value);
    }
}
