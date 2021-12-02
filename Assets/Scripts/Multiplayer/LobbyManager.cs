using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using TMPro;
using Photon.Realtime;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    [SerializeField] TMP_InputField createInput, joinInput;
    [SerializeField] GameObject roomItemPrefab;
    List<RoomItem> activeRooms;
    [SerializeField] GameObject rmListContent;
    public static LobbyManager instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    private void Start()
    {
        activeRooms = new List<RoomItem>();
    }
    public void CreateRoom()
    {
        if (string.IsNullOrEmpty(createInput.text)) return;
        RoomOptions ro = new RoomOptions();
        ro.MaxPlayers = 2;
        PhotonNetwork.CreateRoom(createInput.text, ro);
    }
    public void JoinRoom()
    {
        RoomItem rmItem = EventSystem.current.currentSelectedGameObject.GetComponent<RoomItem>();
        if (rmItem == null) return;

        PhotonNetwork.JoinRoom(rmItem.GetRoomName());
    }

    public override void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel("Game");
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        RefreshRmList(roomList);
    }
    void RefreshRmList(List<RoomInfo> rmList)
    {
        //clear previous active rooms;
        foreach (var item in activeRooms)
        {
            Destroy(item.gameObject);
        }
        activeRooms.Clear();
        foreach (RoomInfo roominfo in rmList)
        {
            if (roominfo.PlayerCount == 0) return;
            RoomItem rItem = Instantiate(roomItemPrefab, rmListContent.transform).GetComponent<RoomItem>();
            rItem.SetRoomName(roominfo.Name);
        }
    }
}
