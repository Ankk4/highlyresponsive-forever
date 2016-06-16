using UnityEngine;
using System.Collections;
using System;

public class IdleState : FSMState
{
    public override void Init(FSM parent)
    {
        base.Init(parent);

        AnimatorWrapper.Play(m_parent.GetComponent<Animator>(), "Player_Idle");
    }

    public override void Update()
    {

    }

    public override void Exit()
    {

    }
}
