using UnityEngine;
using System.Collections;

public class FSM : MonoBehaviour
{
    public FSMState m_CurrentState { get { return m_currentState; } }
    public FSMState m_currentState;

    public Queue m_StateQueue = new Queue();

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (m_currentState)
        {
            m_currentState.Update();
        }
        else
        {
            AssignNextInQueue();
        }
    }

    public void ChangeState(FSMState newState)
    {
        if (m_currentState)
        {
            m_currentState.Exit();
            DestroyImmediate(m_currentState);
        }
        m_currentState = newState;
        if (m_currentState)
        {
            m_currentState.Init(this);
        }
    }

    public void AddToQueue(FSMState state)
    {
        if (state)
        {
            m_StateQueue.Enqueue(state);
        }
    }

    public bool AssignNextInQueue()
    {
        if (m_StateQueue.Count > 0)
        {
            ChangeState((FSMState)m_StateQueue.Dequeue());
            return true;
        }
        return false;
    }
}
