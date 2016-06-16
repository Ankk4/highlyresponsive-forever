using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour
{
    [Header("Force applied to ball on hit")]
    public float Force = 100.0f;

	// Use this for initialization
	void Start ()
    {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (!GetComponent<SpriteRenderer>().isVisible)
        {
            Deactivate();
        }
    }

    public void Activate(Vector3 pos, Vector2 dir, float speed)
    {
        // Set bullet to active
        gameObject.SetActive(true);

        // Set local position
        transform.localPosition = pos;

        var rb = GetComponent<Rigidbody2D>();
        if (rb)
        {
            rb.velocity += dir * speed;
        }
    }

    public void Deactivate()
    {
        gameObject.SetActive(false);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Ball ball = null;
        if (ball = collision.gameObject.GetComponent<Ball>())
        {
            var rb = ball.GetComponent<Rigidbody2D>();
            if (rb)
            {
                /*Vector2 normal = collision.contacts[0].normal;
                Vector2 vel = rb.velocity;
                //float newSpeed = Mathf.Clamp(rb.velocity.magnitude * 1.5f, 0, ball.MaxSpeed);
                Vector2 newDir = Vector2.Reflect(vel.normalized, normal);

                // Enhance x axis movement
                //newDir.x *= 1.5f;

                rb.velocity = -newDir * ball.MaxSpeed * 0.5f;*/

                /*Vector2 vel = rb.velocity;
                Mathf.Abs(vel.y);
                vel.y *= 1.5f;

                rb.velocity = vel;*/
                /*Vector2 vel = rb.velocity;
                Vector2 contact = collision.contacts[0].point;
                Vector2 pos = transform.localPosition;
                Vector2 force = (contact - pos).normalized;

                // Change velocity to the proper facing
                if ((force.x > 0.0f && vel.x < 0.0f) || (force.x < 0.0f && vel.x > 0.0f))
                {
                    vel.x = -vel.x;
                }
                if ((force.y > 0.0f && vel.y < 0.0f) || (force.y < 0.0f && vel.y > 0.0f))
                {
                    vel.y = -vel.y;
                }

                // Apply impulse force to ball
                rb.AddForce(force * 100.0f, ForceMode2D.Impulse);*/

                Vector2 vel = rb.velocity;
                Vector2 ballPos = ball.transform.localPosition;
                Vector2 pos = transform.localPosition;
                bool leftForce = ballPos.x < pos.x;

                if ((ballPos.x > pos.x && vel.x < 0.0f) || (ballPos.x < pos.x && vel.x > 0.0f))
                {
                    vel.x = -vel.x;
                    
                }
                if ((ballPos.y > pos.y && vel.y < 0.0f) || (ballPos.y < pos.y && vel.y > 0.0f))
                {
                    vel.x = -vel.x;
                }

                rb.velocity = vel; // Assign temp vel to rb vel

                if (leftForce)
                {
                    rb.AddForce(new Vector2(-Force, Force * 2.0f), ForceMode2D.Impulse);
                }
                else
                {
                    rb.AddForce(new Vector2(Force, Force * 2.0f), ForceMode2D.Impulse);
                }
            }
            Deactivate();
        }
    }
}
