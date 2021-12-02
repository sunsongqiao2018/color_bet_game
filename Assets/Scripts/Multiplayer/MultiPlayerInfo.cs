using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using Photon.Realtime;
using System;
using TMPro;
[RequireComponent(typeof(PhotonView))]
public class MultiPlayerInfo : MonoBehaviourPunCallbacks, IPunObservable
{
    [SerializeField] GameObject multiUi;
    TextMeshProUGUI playerTotalChipTxt, playerBetAmountTxt;
    Image playerPickIcon;
    int playerTotalChips;
    int playerBetAmount;
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
            playerPicks = e.pick;
            playerTotalChips = e.totalChips;
            playerBetAmount = e.betChips;
        }
        else
        {
            photonView.RPC("SetPlayerValues", RpcTarget.Others, e.pick, e.totalChips, e.betChips);
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(playerPicks);
            stream.SendNext(playerTotalChips);
            stream.SendNext(playerBetAmount);
        }
        else
        {
            playerPickIcon.color = (bool)stream.ReceiveNext() ? Color.green : Color.red;
            playerTotalChipTxt.text = ((int)stream.ReceiveNext()).ToString();
            playerBetAmountTxt.text = ((int)stream.ReceiveNext()).ToString();
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
        playerTotalChipTxt = uiInfo.playerChipsTxt;
        playerBetAmountTxt = uiInfo.playerBetAmount;
        playerPickIcon = uiInfo.playerPickImage;

    }
    [PunRPC]
    private void SetPlayerValues(bool pick, int total, int betValue)
    {
        playerPickIcon.color = pick ? Color.green : Color.red;
        playerTotalChipTxt.text = total.ToString();
        playerBetAmountTxt.text = betValue.ToString();
    }
}
