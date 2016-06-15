using UnityEngine;
using System.Collections;

public class TimeManager : MonoBehaviour
{
    public static float DeltaTime
    {
        get
        {
            return Time.deltaTime;
        }
    }

    public static float TimeScale
    {
        set
        {
            Time.timeScale = value;
        }

        get
        {
            return Time.timeScale;
        }
    }

    public static bool IsPause()
    {
        return TimeScale == 0;
    }

    public static void Pause(bool pause)
    {
        if (pause)
        {
            TimeScale = 0.0f;
        }
        else
        {
            TimeScale = 1.0f;
        }
    }
}
