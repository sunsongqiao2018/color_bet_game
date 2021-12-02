using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using System;
/// <summary>
/// sync player ready info to server once players in ready == total player number the game will start.
/// </summary>
[RequireComponent(typeof(Button))]
public class SyncPlayReady : MonoBehaviourPunCallbacks, IPunObservable
{
    private ExitGames.Client.Photon.Hashtable _PlayerReadyProperty = new ExitGames.Client.Photon.Hashtable();
    Button readyBtn;
    private void Start()
    {
        SetPlayerReadyProperty("Ready", false);
        readyBtn = gameObject.GetComponent<Button>();
        readyBtn.onClick.AddListener(OnReadyButtonPressed);
        StateMachine.Instance.GameFinished += ResetReadyButton;
    }

    private void ResetReadyButton(object sender, EventArgs e)
    {
        SetPlayerReadyProperty("Ready", false);
        readyBtn.interactable = true;
    }

    public void OnReadyButtonPressed()
    {
        SetPlayerReadyProperty("Ready", true);
        readyBtn.interactable = false;
        GameControl.OnReadyBtnPressed.Invoke(this, new EventArgs());
    }
    private void SetPlayerReadyProperty(string key, bool value)
    {
        bool isReady = value;
        _PlayerReadyProperty[key] = isReady;
        PhotonNetwork.LocalPlayer.SetCustomProperties(_PlayerReadyProperty);
        if (!PhotonNetwork.IsMasterClient) return;
        CanStartGame();
    }
    public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {
        //only checks from master clients on with "Ready" property
        if (!PhotonNetwork.IsMasterClient) return;
        if (!changedProps.ContainsKey("Ready")) return;
        CanStartGame();
    }
    private void CanStartGame()
    {
        Player[] playerlist = PhotonNetwork.PlayerList;
        //if all player are ready, after iteration it stays true.
        bool canStart = true;
        foreach (var player in playerlist)
        {
            if (player.CustomProperties.ContainsKey("Ready"))
            {
                canStart = canStart && (bool)player.CustomProperties["Ready"];
            }
            else
            {
                canStart = false;
            }
        }
        if (canStart)
        {
            photonView.RPC("CallPlayStart", RpcTarget.All);
            foreach (var player in playerlist)
            {
                player.CustomProperties["Ready"] = false;
            }
        }
    }

    [PunRPC]
    private void CallPlayStart()
    {
        GameControl.OnPlayStart.Invoke(this, new EventArgs());
    }
    //if player left the room and other player is ready, play the game
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        CanStartGame();
    }
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {

    }
}
