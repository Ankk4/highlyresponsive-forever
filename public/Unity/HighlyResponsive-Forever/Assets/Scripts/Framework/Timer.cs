using UnityEngine;
using System.Collections;

public class Timer : ScriptableObject
{
    public float CurrentTime
    {
        get
        {
            return m_timer;
        }
    }

    public float MaxTime
    {
        get
        {
            return m_maxTime;
        }
    }

    public float RemainingTime
    {
        get
        {
            return MaxTime - CurrentTime;
        }
    }

    public float NormalisedTime
    {
        get
        {
            if (MaxTime <= 0.0f)
            {
                return 1.0f;
            }
            return CurrentTime / MaxTime;
        }
    }

    private float m_timer = 0.0f;
    private float m_maxTime = 0.0f;
    private bool m_pause = false;

	// Use this for initialization
	void Start ()
    {
	
	}
	
	// Update is called once per frame
	public void Update ()
    {
        if (m_pause)
        {
            return;
        }

        float dt = TimeManager.DeltaTime;
        
        if (m_timer < m_maxTime)
        {
            m_timer += dt;
        }
	}

    public void Init(float maxTime, bool pause)
    {
        m_timer = 0.0f;
        m_maxTime = maxTime;
        m_pause = pause;
    }

    public void Reset()
    {
        m_timer = 0.0f;
    }

    public void ResetAll()
    {
        Reset();
        m_maxTime = 0.0f;
        m_pause = false;
    }

    public bool IsTime()
    {
        return m_timer >= m_maxTime;
    }

    public void Pause()
    {
        m_pause = true;
    }

    public void Resume()
    {
        m_pause = false;
    }

    public bool IsPause()
    {
        return m_pause;
    }

    public void SetIsTime()
    {
        m_timer = m_maxTime;
    }
}
