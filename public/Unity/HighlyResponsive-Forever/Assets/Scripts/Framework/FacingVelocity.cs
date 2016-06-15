using UnityEngine;
using System.Collections;

public class FacingVelocity : MonoBehaviour
{
    [Header("Public variables")]
    [Tooltip("Prevents all updates")]
    public bool m_PauseUpdate = false;
    [Tooltip("Time between each update")]
    public float m_UpdateInterval = 1.0f;
    [Tooltip("Rotation offset from right")]
    public float m_Offset = 0.0f;
    private Timer m_updateTimer;

    // Set up scriptable object
    void Awake ()
    {
        m_updateTimer = ScriptableObject.CreateInstance<Timer>();
    }

	// Use this for initialization
	void Start ()
    {
        m_updateTimer.Init(m_UpdateInterval, false);
	}
	
	// Update is called once per frame
	void Update ()
    {
        // Check if update is allowed and if it is time to update
	    if (m_PauseUpdate || !m_updateTimer.IsTime())
        {
            return;
        }

        UpdateRotation(); // Update rotation
        m_updateTimer.Reset(); // Reset timer
	}

    // Manually trigger new rotation
    public void UpdateRotation(Vector2? facingDir = null)
    {
        Vector2 dir = Vector2.zero;

        // Setup direction
        if (facingDir == null)
        {
            // No direction given, take direction from rigidbody
            var rb = GetComponent<Rigidbody2D>();
            if (rb)
            {
                dir = rb.velocity.normalized;
            }
        }
        else
        {
            // Use the direction given
            dir = facingDir.GetValueOrDefault(Vector2.zero);
        }

        if (dir == Vector2.zero)
        {
            // No rotation if no dir
            //transform.rotation = Quaternion.Euler(0, 0, 0);
            return;
        }
        float degree = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - m_Offset;
        transform.rotation = Quaternion.Euler(0, 0, degree);
    }
}
