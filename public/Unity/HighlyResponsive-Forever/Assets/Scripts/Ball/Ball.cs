using UnityEngine;
using System.Collections;

public class Ball : MonoBehaviour
{
    [Header("Movement Speed")]
    public float LaunchSpeed = 50.0f;
    public float MaxSpeed = 50.0f;

    [Header("Rotation")]
    public float RotationSpeed = 10.0f;

    [Header("Properties to stop bouncing")]
    public float MaxDrag = 5.0f;
    public float DragIncrement = 0.3f;

    [Header("Properties to create movement when not bouncing")]
    [Tooltip("Constant force to apply to ball so that it always moves left or right")]
    public float ConstantXForce = 50.0f;
    [Tooltip("Max y vel needed to start applying constant force to the x-axis for the ball to always move left or right")]
    public float MaxVelY = 2.0f;

    [Header("Object references")]
    public GameObject RefFloor;

    private Vector3 defaultPos;

	// Use this for initialization
	void Start ()
    {
        defaultPos = transform.localPosition;

        // Generate random direction on start
        var rb = GetComponent<Rigidbody2D>();
        if (rb)
        {
            float angle = 135.0f;//Random.Range(0, 360);
            Vector2 vel = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad)) * LaunchSpeed;
            rb.velocity = vel;
        }
    }
	
	// Update is called once per frame
	void Update ()
    {
        // Rotation
        var rb = GetComponent<Rigidbody2D>();
        if (rb)
        {
            if (rb.velocity.x > 0.0f)
            {
                // Closewise rotation
                transform.Rotate(0, 0, RotationSpeed * TimeManager.DeltaTime);
            }
            else
            {
                // Anti-clockwise rotation
                transform.Rotate(0, 0, -(RotationSpeed * TimeManager.DeltaTime));
            }
        }
    }

    void FixedUpdate()
    {
        // Check speed is within limits
        var rb = GetComponent<Rigidbody2D>();
        if (rb)
        {
            var vel = rb.velocity;

            // Disable y velocity if on ground
            if (rb.drag >= MaxDrag)
            {
                // Set position to exactly on top of floor
                Vector3 pos = transform.localPosition;
                pos.y = RefFloor.transform.localPosition.y + RefFloor.transform.localScale.y * 0.5f + transform.localScale.y * 0.5f; // Y position of floor plus half scale of floor and ball
                transform.localPosition = pos;

                // Remove y velocity
                vel.y = 0.0f;
                rb.velocity = vel;
            }

            // Clamp speed to max
            float sqrSpeed = rb.velocity.sqrMagnitude;
            if (sqrSpeed > MaxSpeed * MaxSpeed)
            {
                Vector2 newVel = rb.velocity.normalized * MaxSpeed;
                rb.velocity = newVel;
            }

            // Add force to x when not bouncing
            if (vel.y < MaxVelY && vel.y > -MaxVelY)
            {
                if (vel.x > 0.0f)
                {
                    rb.AddForce(new Vector2(ConstantXForce, 0));
                }
                else if (vel.x < 0.0f)
                {
                    rb.AddForce(new Vector2(-ConstantXForce, 0));
                }
            }
        }
    }

    void OnCollisionEnter2D (Collision2D collision)
    {
        if (collision.gameObject == RefFloor)
        {
            var rb = GetComponent<Rigidbody2D>();
            if (rb)
            {
                // Add linear drag
                rb.drag = Mathf.Clamp(rb.drag + DragIncrement, 0, MaxDrag);
            }
        }
        else if (collision.gameObject.GetComponent<Bullet>())
        {
            var rb = GetComponent<Rigidbody2D>();
            if (rb)
            {
                // Reset linear drag when bullet hits
                rb.drag = 0;
            }
        }
    }

    public void Reset()
    {
        transform.localPosition = defaultPos;

        // Generate random direction on start
        var rb = GetComponent<Rigidbody2D>();
        if (rb)
        {
            float angle = 135.0f;//Random.Range(0, 360);
            Vector2 vel = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad)) * LaunchSpeed;
            rb.velocity = vel;

            rb.drag = 0.0f;
        }
    }
}
