using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// GameControl controlls all player inputs.
/// </summary>
public class GameControl : MonoBehaviour
{
    //Player button inputs.
    [SerializeField] Button increBetBtn, decreBetBtn, changeBetBtn, readyBtn;
    [SerializeField] UIControl _uiControl;
    //=========Private Fields===================//
    private const int _stackPerBet = 10;
    //since the result only has red and green, we can just use a bool LETS SAY true => Green, false=>Red.
    private bool _playersBet = true;
    // Start is called before the first frame update
    public static EventHandler OnPlayStart, OnReadyBtnPressed;
    void Start()
    {
        //hook button events;
        increBetBtn.onClick.AddListener(() => ChangeChipBetAmount(_stackPerBet));
        decreBetBtn.onClick.AddListener(() => ChangeChipBetAmount(-_stackPerBet));
        changeBetBtn.onClick.AddListener(ToggleBet);
        //readyBtn.onClick.AddListener(PlayStart);

        //hook class events
        StateMachine.Instance.GameFinished += OnGameFinished;
        OnPlayStart += MultiPlayGameStart;
        OnReadyBtnPressed += DisableUIButtons;
        //initial set ups;
        StateMachine.Instance.SetPlayerBetAmount(_stackPerBet);
        _uiControl.UpdateBetAmount(_stackPerBet);
        ChipsPool.Instance.GetChip();
    }

    private void DisableUIButtons(object sender, EventArgs e)
    {
        ToggleButtonGroups(false);
    }

    private void MultiPlayGameStart(object sender, EventArgs e)
    {
        PlayStart();
    }

    private void PlayStart()
    {
        StateMachine.Instance.SetPlayGameInitData(_playersBet);
        _uiControl.SetTotalChips();
        StateMachine.Instance.OnPlayGame();
        ChipsPool.Instance.RemoveAllChips();

    }

    private void OnGameFinished(object sender, EventArgs e)
    {
        UpdateBetCap();
        ChipsPool.Instance.SetChips(StateMachine.Instance.playerBetAmount / _stackPerBet);
        ToggleButtonGroups(true);
    }

    private void UpdateBetCap()
    {
        if (StateMachine.Instance.playerBetAmount > StateMachine.Instance.playerChipStock)
        {
            _uiControl.UpdateBetAmount(StateMachine.Instance.playerChipStock);
        }
    }
    /// <summary>
    /// returns if player successfully changed bet amount
    /// </summary>
    /// <param name="value">takes value from increbet or decrebet</param>
    /// <returns></returns>
    private void ChangeChipBetAmount(int value)
    {
        int pendingBets = StateMachine.Instance.playerBetAmount;
        pendingBets += value;
        if (pendingBets < 10 || pendingBets > StateMachine.Instance.playerChipStock)
        {
            pendingBets -= value;
        }
        else
        {
            if (value > 0)
                ChipsPool.Instance.GetChip();
            else
                ChipsPool.Instance.RemoveChip();
        }
        StateMachine.Instance.SetPlayerBetAmount(pendingBets);
        _uiControl.UpdateBetAmount(pendingBets);
    }
    /// <summary>
    /// player can toggle what the bet whenever, the value will only be sent out on playgame().
    /// </summary>
    private void ToggleBet()
    {
        _playersBet = !_playersBet;
        StateMachine.Instance.SetPlayerBet(_playersBet);
        changeBetBtn.GetComponent<Image>().color = _playersBet ? Color.green : Color.red;
    }

    private void ToggleButtonGroups(bool isEnable)
    {
        increBetBtn.interactable = isEnable;
        decreBetBtn.interactable = isEnable;
        changeBetBtn.interactable = isEnable;
    }
}
