using UnityEngine;
using System.Collections;

public class Ball : MonoBehaviour
{
    [Header("Public variables")]
    public float LaunchSpeed = 50.0f;
    public float MaxSpeed = 50.0f;
    public GameObject RefFloor;

	// Use this for initialization
	void Start ()
    {
        // Generate random direction on start
        var rb = GetComponent<Rigidbody2D>();
        if (rb)
        {
            float angle = Random.Range(0, 360);
            Vector2 vel = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad)) * LaunchSpeed;
            rb.velocity = vel;
        }
	}
	
	// Update is called once per frame
	void Update ()
    {
    }

    void FixedUpdate()
    {
        // Check speed is within limits
        var rb = GetComponent<Rigidbody2D>();
        if (rb)
        {
            // Clamp speed to max
            float sqrSpeed = rb.velocity.sqrMagnitude;
            if (sqrSpeed > MaxSpeed * MaxSpeed)
            {
                Vector2 newVel = rb.velocity.normalized * MaxSpeed;
                rb.velocity = newVel;
            }

            // Reduce y velocity
            var vel = rb.velocity;
            //vel.y *= 0.99f;
            rb.velocity = vel;
        }
    }

    void OnCollisionEnter2D (Collision2D collision)
    {
        if (collision.gameObject == RefFloor)
        {
            var rb = GetComponent<Rigidbody2D>();
            if (rb)
            {
                /*var vel = rb.velocity;
                vel.y *= 0.98f;
                rb.velocity = vel;*/
            }
        }
    }
}
