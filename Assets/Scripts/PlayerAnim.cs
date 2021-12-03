using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerAnim : MonoBehaviour
{
    [SerializeField] float idleTimer = 10;
    float elapsedTime = 0;
    Animator _animator;
    int idleStatHash;
    public enum ANIMSTATES
    {
        //when he/she wins
        STATE_WIN,
        //when he/she lose
        STATE_LOSE,
        //when game starts
        STATE_CHEER
    }
    // Start is called before the first frame update
    private void Awake()
    {
        HookPlayEvents(true);
    }
    void Start()
    {
        _animator = gameObject.GetComponent<Animator>();
        idleStatHash = _animator.GetCurrentAnimatorStateInfo(0).shortNameHash;
        StartCoroutine(IdleAnimCoroutine());
    }
    private void OnDisable()
    {
        StopCoroutine(IdleAnimCoroutine());
        HookPlayEvents(false);
    }
    private void HookPlayEvents(bool hook)
    {
        if (hook)
        {
            StateMachine.Instance.GameStarted += AllPlayerCheer;
            StateMachine.Instance.CardRevealed += PlayerReactResult;
        }
        else
        {
            StateMachine.Instance.GameStarted -= AllPlayerCheer;
            StateMachine.Instance.CardRevealed -= PlayerReactResult;
        }
    }
    private void PlayerReactResult(object sender, BoolEventArgs e)
    {
        bool playerWin = e.value;
        if (playerWin) CallPlayerAnim(ANIMSTATES.STATE_WIN);
        else CallPlayerAnim(ANIMSTATES.STATE_LOSE);
    }
    private void AllPlayerCheer(object sender, EventArgs e)
    {
        //raise win/lose anim event 
        CallPlayerAnim(ANIMSTATES.STATE_CHEER);
    }
    private IEnumerator IdleAnimCoroutine()
    {
        while (_animator.GetCurrentAnimatorStateInfo(0).shortNameHash != idleStatHash)
        {
            yield return null;
        }
        while (elapsedTime < idleTimer)
        {
            yield return new WaitForEndOfFrame();
            elapsedTime += Time.deltaTime;
        }
        if (_animator != null) _animator.SetTrigger("IdleAnim");
        elapsedTime = 0;
        yield return IdleAnimCoroutine();
    }
    //private async Task TriggerIdleAnim()
    //{
    //    //pause if not in idle state;
    //    while (_animator.GetCurrentAnimatorStateInfo(0).shortNameHash != idleStatHash)
    //    {
    //        await Task.Yield();
    //    }
    //    while (elapsedTime < idleTimer)
    //    {
    //        await Task.Yield();
    //        elapsedTime += Time.deltaTime;
    //    }
    //    if (_animator != null) _animator.SetTrigger("IdleAnim");
    //    elapsedTime = 0;
    //    await TriggerIdleAnim();
    //}
    public void CallPlayerAnim(ANIMSTATES state)
    {
        string triggerStr = null;
        switch (state)
        {
            case ANIMSTATES.STATE_WIN:
                triggerStr = "Win";
                break;
            case ANIMSTATES.STATE_LOSE:
                triggerStr = "Lose";
                break;
            case ANIMSTATES.STATE_CHEER:
                triggerStr = "Cheer";
                break;
            default:
                Debug.Log("ANIM STATE NOT VALID");
                break;
        }
        if (!string.IsNullOrEmpty(triggerStr)) _animator.SetTrigger(triggerStr);
    }
}
