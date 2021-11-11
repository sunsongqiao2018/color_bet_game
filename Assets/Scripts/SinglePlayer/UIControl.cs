using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using DG.Tweening;
using UnityEngine.UI;
public class UIControl : MonoBehaviour
{

    [SerializeField] TextMeshProUGUI _totalChips;
    [SerializeField] TextMeshProUGUI _curtBetAmtText;
    [SerializeField] Image popUpPanel;

    TextMeshProUGUI popUpText;
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
        StateMachine.Instance.CardRevealed += ShowPopUpResult;
        SetTotalChips();
        popUpPanel.transform.localScale = Vector3.zero;
        try
        {
            popUpText = popUpPanel.GetComponentInChildren<TextMeshProUGUI>();
        }
        catch (Exception)
        {
            throw;
        }
    }

    private void UpdateValues(object sender, EventArgs e)
    {
        SetTotalChips();
    }

    private void ShowPopUpResult(object sender, BoolEventArgs e)
    {
        bool playerWon = e.value;
        popUpText.text = playerWon ? "You Win!" : "You lose";
        PopUpSequence(2);
    }

    private void PopUpSequence(float duration)
    {
        Sequence seq = DOTween.Sequence();
        seq.Append(popUpPanel.transform.DOScale(Vector3.one, .2f).SetEase(Ease.OutBounce));
        seq.AppendInterval(duration);
        seq.Append(popUpPanel.transform.DOScale(Vector3.zero, .2f).SetEase(Ease.OutExpo));
    }
}
