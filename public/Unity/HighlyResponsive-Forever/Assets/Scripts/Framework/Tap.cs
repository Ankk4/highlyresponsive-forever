using UnityEngine;
using System.Collections;

public class Tap : ScriptableObject
{
    public Vector2 Position;
    public Timer DisableTimer;

    public void Init(Vector2 pos, float disableTime)
    {
        if (!DisableTimer)
        {
            DisableTimer = ScriptableObject.CreateInstance<Timer>();
        }
        DisableTimer.Init(disableTime, false);

        Position = pos;
    }
}
