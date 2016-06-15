using UnityEngine;
using System.Collections;

public class AnimatorWrapper
{
    private static int Default_Layer = 0;

    public static bool IsDone(Animator anim, int loopCount = 1)
    {
        if (anim)
        {
            return anim.GetCurrentAnimatorStateInfo(Default_Layer).normalizedTime >= (float)loopCount;
        }
        return true;
    }

    public static bool IsName(Animator anim,string name)
    {
        if (anim)
        {
            return anim.GetCurrentAnimatorStateInfo(Default_Layer).IsName(name);
        }
        return false;
    }

    public static float GetNormalizedTime(Animator anim)
    {
        return anim.GetCurrentAnimatorStateInfo(Default_Layer).normalizedTime;
    }

    public static bool Play(Animator anim, string name, float speed = 1.0f, float time = 0.0f)
    {
        if (!anim)
        {
            return false;
        }
        
        // Enable animator
        if (!anim.enabled)
        {
            anim.enabled = true;
        }

        anim.speed = speed;
        anim.Play(name, -1, time);
        anim.Update(0.0f); // Update for the latest state

        return true;
    }

    public static void Pause(Animator anim)
    {
        if (!anim)
        {
            return;
        }

        anim.enabled = false;
    }

    public static void Resume(Animator anim)
    {
        if (!anim)
        {
            return;
        }

        anim.enabled = true;
    }
}
