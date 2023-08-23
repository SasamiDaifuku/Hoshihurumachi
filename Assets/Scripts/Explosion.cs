using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

public class Explosion : MonoBehaviour
{
    [SerializeField] private Animator animatorExplosion;

    private void Start()
    {
        /*
        Animator animator = this.GetComponent<Animator>();
        var trigger = animator.GetBehaviour<ObservableStateMachineTrigger>();
        trigger.OnStateEnterAsObservable()
            .Subscribe(_ => Debug.Log("開始"))
            .AddTo(this);
        trigger.OnStateExitAsObservable()
            .Subscribe(_ => Debug.Log("終了"))
            .AddTo(this);
        */
    }
    public void DeleteExplosion()
    {
        Destroy(gameObject);
    }
}
