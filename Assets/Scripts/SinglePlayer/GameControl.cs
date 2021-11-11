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
    //Buttons recieve player inputs.
    [SerializeField] Button increBetBtn, decreBetBtn, changeBetBtn, readyBtn;

    [SerializeField] UIControl _uiControl;
    //=========Private Fields===================//
    private const int stackPerBet = 10;
    //since the result only has red and green, we can just use a bool LETS SAY true => Green, false=>Red.
    private bool _playersBet = true;
    private int _totalChips, _betChips;



    // Start is called before the first frame update
    void Start()
    {
        //hook button events;
        increBetBtn.onClick.AddListener(() => ChangeChipBetAmount(stackPerBet));
        decreBetBtn.onClick.AddListener(() => ChangeChipBetAmount(-stackPerBet));
        changeBetBtn.onClick.AddListener(ToggleBet);
        readyBtn.onClick.AddListener(PlayStart);

        //hook class events
        StateMachine.Instance.GameFinished += OnGameFinished;
        //initial set ups;
        _totalChips = StateMachine.Instance.playerChipStock;
        _betChips = stackPerBet;
        _uiControl.UpdateBetAmount(_betChips);
        ChipsPool.Instance.GetChip();
    }

    private void PlayStart()
    {
        StateMachine.Instance.SetPlayGameInitData(_playersBet, _betChips);
        _uiControl.SetTotalChips();
        StateMachine.Instance.OnPlayGame();
        ChipsPool.Instance.RemoveAllChips();

        readyBtn.interactable = false;
    }

    private void OnGameFinished(object sender, EventArgs e)
    {
        _totalChips = StateMachine.Instance.playerChipStock;
        if (_betChips > _totalChips)
        {
            _betChips = _totalChips;
            _uiControl.UpdateBetAmount(_betChips);
        }
        ChipsPool.Instance.SetChips(_betChips / stackPerBet);
        readyBtn.interactable = true;
    }
    /// <summary>
    /// returns if player successfully changed bet amount
    /// </summary>
    /// <param name="value">takes value from increbet or decrebet</param>
    /// <returns></returns>
    private void ChangeChipBetAmount(int value)
    {
        _betChips += value;
        if (_betChips < 10 || _betChips > _totalChips)
        {
            _betChips -= value;
        }
        else
        {
            if (value > 0)
                ChipsPool.Instance.GetChip();
            else
                ChipsPool.Instance.RemoveChip();
        }
        _uiControl.UpdateBetAmount(_betChips);
    }
    /// <summary>
    /// player can toggle what the bet whenever, the value will only be sent out on playgame().
    /// </summary>
    private void ToggleBet()
    {
        _playersBet = !_playersBet;
        changeBetBtn.GetComponent<Image>().color = _playersBet ? Color.green : Color.red;
    }
}
