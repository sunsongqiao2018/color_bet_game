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
    async void Start()
    {
        _animator = gameObject.GetComponent<Animator>();
        idleStatHash = _animator.GetCurrentAnimatorStateInfo(0).shortNameHash;
        await TriggerIdleAnim();
    }
    private async Task TriggerIdleAnim()
    {
        //pause if not in idle state;
        while (_animator.GetCurrentAnimatorStateInfo(0).shortNameHash != idleStatHash)
        {
            await Task.Yield();
        }
        while (elapsedTime < idleTimer)
        {
            await Task.Yield();
            elapsedTime += Time.deltaTime;
        }
        _animator.SetTrigger("IdleAnim");
        elapsedTime = 0;
        await TriggerIdleAnim();
    }
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
