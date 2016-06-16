using UnityEngine;
using System.Collections;

public abstract class FSMState : ScriptableObject
{
    public FSM m_Parent { set { m_parent = value; } }
    protected FSM m_parent;

    // Use this for initialization
    public virtual void Init(FSM parent)
    {
        m_parent = parent;
    }

    // Update is called once per frame
    public abstract void Update();

    public abstract void Exit();
}
