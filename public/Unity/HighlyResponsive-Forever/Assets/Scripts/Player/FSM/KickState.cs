using UnityEngine;
using System.Collections;
using System;

public class KickState : FSMState
{
    private bool dir;

    public virtual void Init(FSM parent, bool dir)
    {
        base.Init(parent);

        this.dir = dir;
        Player p = m_parent.GetComponent<Player>();
        p.CurrentSpeed = p.MeleeSpeed;

        // Animation
        AnimatorWrapper.Play(m_parent.GetComponent<Animator>(), "Player_Kick");
    }

    public override void Update()
    {
        if (AnimatorWrapper.IsDone(m_parent.GetComponent<Animator>()))
        {
            var idleState = ScriptableObject.CreateInstance<IdleState>();
            idleState.Init(m_parent);
            m_parent.ChangeState(idleState);
        }
        else
        {
            if (dir)
            {
                m_parent.GetComponent<Player>().MoveRight();
            }
            else
            {
                m_parent.GetComponent<Player>().MoveLeft();
            }
        }
    }

    public override void Exit()
    {
        Player p = m_parent.GetComponent<Player>();
        p.CurrentSpeed = p.NormalSpeed;
    }
}
