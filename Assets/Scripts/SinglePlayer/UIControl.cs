using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class UIControl : MonoBehaviour
{

    [SerializeField] TextMeshProUGUI _totalChips;
    [SerializeField] TextMeshProUGUI _curtBetAmtText;
    int betChips;
    public void SetTotalChips()
    {
        _totalChips.text = StateMachine.Instance.playerChipStock.ToString();
    }

    public void UpdateBetAmount(int amount)
    {
        _curtBetAmtText.text = amount.ToString();
        betChips = amount;
    }

    private void Start()
    {
        StateMachine.Instance.GameFinished += UpdateValues;
        SetTotalChips();
        UpdateBetAmount(10);
    }

    private void UpdateValues(object sender, EventArgs e)
    {
        SetTotalChips();
        //int playerMaxBet = StateMachine.Instance.playerChipStock;
        //if (playerMaxBet < betChips)
        //    UpdateBetAmount(playerMaxBet);
    }
}
