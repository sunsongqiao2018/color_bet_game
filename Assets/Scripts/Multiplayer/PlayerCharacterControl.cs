using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System;

public class PlayerCharacterControl : MonoBehaviourPunCallbacks
{
    [SerializeField] GameObject playerPrefab;
    GameObject animatedPlayer;
    // Start is called before the first frame update
    // bool secondPlayerActive;
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        photonView.RPC("AddPlayerCharacter", RpcTarget.Others);
        //HookPlayEvents(true);
    }
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        if (animatedPlayer != null) Destroy(animatedPlayer);
        //HookPlayEvents(false);
    }
    //private void HookPlayEvents(bool hook)
    //{
    //    if (hook)
    //    {
    //        StateMachine.Instance.GameStarted += AllPlayerCheer;
    //        StateMachine.Instance.BroadcastResult += PlayerReactResult;
    //    }
    //    else
    //    {
    //        StateMachine.Instance.GameStarted -= AllPlayerCheer;
    //        StateMachine.Instance.BroadcastResult -= PlayerReactResult;
    //    }
    //}

    //private void PlayerReactResult(object sender, BoolEventArgs e)
    //{
    //    bool iwon = e.value;
    //    //raise win/lose anim event 
    //}
    //private void AllPlayerCheer(object sender, EventArgs e)
    //{
    //    //raise win/lose anim event 
    //    playerCharAnim.CallPlayerAnim(PlayerAnim.ANIMSTATES.STATE_CHEER);
    //}

    [PunRPC]
    private void AddPlayerCharacter()
    {
        //instantiate "yourself" in the other client world
        animatedPlayer = PhotonNetwork.Instantiate(playerPrefab.name, transform.position, Quaternion.identity);
        //playerCharAnim = player.GetComponent<PlayerAnim>();
    }
}
