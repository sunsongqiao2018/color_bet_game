using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System;

public class ServerConnection : MonoBehaviourPunCallbacks
{
    [SerializeField] GameObject loadingGO, LobbyGO;
    [SerializeField] float bufferTime;
    bool connected = false;

    // Start is called before the first frame update
    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
        ToggleLoadingAndLobby(true);
        //StartCoroutine(OfflineBuffer());
    }

    //private IEnumerator OfflineBuffer()
    //{
    //    yield return new WaitForSeconds(bufferTime);
    //    if (!connected) 
    //    {

    //    }
    //}

    public override void OnConnectedToMaster()
    {
        connected = true;
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
