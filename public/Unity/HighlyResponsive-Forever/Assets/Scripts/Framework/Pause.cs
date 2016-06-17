using UnityEngine;
using System.Collections;

public class Pause : MonoBehaviour
{
    private Vector2 vel;

    public void Toggle(bool pause)
    {
        ToggleVel(pause);
        ToggleAnim(pause);
    }

    public void ToggleVel(bool pause)
    {
        var rb = GetComponent<Rigidbody2D>();
        if (!rb)
        {
            return;
        }

        if (pause)
        {
            // Pause
            vel = rb.velocity;

            // Stop all movement
            rb.isKinematic = true;
        }
        else
        {
            // Resume
            // Enable movement
            rb.isKinematic = false;

            rb.velocity = vel;
        }
    }

    public void ToggleAnim(bool pause)
    {
        var anim = GetComponent<Animator>();
        if (!anim)
        {
            return;
        }

        anim.enabled = !pause;
    }
}
