using UnityEngine;
using Photon.Realtime;
using Photon.Pun;
using System;
using ExitGames.Client.Photon;

public class GameDice : MonoBehaviour, IOnEventCallback
{
    public const byte SetGameResultFromMasterClient = 1;
    bool dice;
    private void OnEnable()
    {
        PhotonNetwork.AddCallbackTarget(this);
    }

    private void OnDisable()
    {
        PhotonNetwork.RemoveCallbackTarget(this);
    }
    public void Start()
    {
        StateMachine.Instance.GameStarted += CallFlipCoin;
    }
    private void CallFlipCoin(object sender, EventArgs e)
    {
        //when solo play or is master client
        if (PhotonNetwork.PlayerList.Length == 1 || PhotonNetwork.IsMasterClient)
        {
            FlipCoin();
            StateMachine.Instance.GameResult = dice;
            StateMachine.Instance.ResultReceived = true;
            MasterBroadcastResult();
        }
    }
    private void FlipCoin()
    {
        int coin = UnityEngine.Random.Range(0, 2);
        dice = coin == 0;
    }
    private void MasterBroadcastResult()
    {
        if (!PhotonNetwork.IsMasterClient) return;
        object[] content = new object[] { dice };
        RaiseEventOptions raiseEvtOpt = new RaiseEventOptions { Receivers = ReceiverGroup.Others };
        PhotonNetwork.RaiseEvent(SetGameResultFromMasterClient, content, raiseEvtOpt, SendOptions.SendReliable);
    }
    public void OnEvent(EventData photonEvent)
    {
        byte eventCode = photonEvent.Code;
        if (eventCode == SetGameResultFromMasterClient)
        {
            object[] data = (object[])photonEvent.CustomData;
            dice = (bool)data[0];
            StateMachine.Instance.GameResult = dice;
            StateMachine.Instance.ResultReceived = true;
        }
    }
}
