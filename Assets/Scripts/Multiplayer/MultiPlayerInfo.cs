using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using Photon.Realtime;
using System;

[RequireComponent(typeof(PhotonView))]
public class MultiPlayerInfo : MonoBehaviourPunCallbacks, IPunObservable
{
    [SerializeField] GameObject multiUi;
    Text playerTotalChipText;
    Image playerPickIcon;
    int playerChips;
    bool playerPicks;
    private void Start()
    {
        //other player's info is tied with when the player pressed the play button, we can update it in different time but just to keep it simple here.
        StateMachine.Instance.BroadcastPlayerInfo += UpdatePlayerInfo;
    }

    private void UpdatePlayerInfo(object sender, PlayerInfoEventArgs e)
    {
        if (photonView.IsMine)
        {
            playerChips = e.totalChips;
            playerPicks = e.pick;
        }
        else
        {
            photonView.RPC("SetPlayerValues", RpcTarget.Others, e.pick, e.totalChips);
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(playerPicks);
            stream.SendNext(playerChips);
        }
        else
        {
            playerPickIcon.color = (bool)stream.ReceiveNext() ? Color.green : Color.red;
            playerTotalChipText.text = ((int)stream.ReceiveNext()).ToString();
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        photonView.RPC("AddMultiUi", RpcTarget.All);
    }
    public override void OnPlayerLeftRoom(Player newPlayer)
    {
        GameObject ui = GameObject.FindGameObjectWithTag("MultiCanvas");
        if (ui != null) Destroy(ui);
    }
    [PunRPC]
    private void AddMultiUi()
    {
        GameObject ui = Instantiate(multiUi);
        MultiUiInfo uiInfo = ui.GetComponent<MultiUiInfo>();
        playerTotalChipText = uiInfo.playerChipsTxt;
        playerPickIcon = uiInfo.playerPickImage;
    }
    [PunRPC]
    private void SetPlayerValues(bool pick, int value)
    {
        playerPickIcon.color = pick ? Color.green : Color.red;
        playerTotalChipText.text = value.ToString();
    }
}
