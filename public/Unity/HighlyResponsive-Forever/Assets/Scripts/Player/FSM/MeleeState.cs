using UnityEngine;
using System.Collections;
using System;

public class MeleeState : FSMState
{
    public override void Init(FSM parent)
    {
        base.Init(parent);
        
        AnimatorWrapper.Play(m_parent.GetComponent<Animator>(), "Player_Melee");
    }

    public override void Update()
    {
        if (AnimatorWrapper.IsDone(m_parent.GetComponent<Animator>()))
        {
            var idleState = ScriptableObject.CreateInstance<IdleState>();
            idleState.Init(m_parent);
            m_parent.ChangeState(idleState);
        }
    }

    public override void Exit()
    {

    }
}
