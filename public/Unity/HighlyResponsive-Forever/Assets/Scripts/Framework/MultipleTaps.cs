using UnityEngine;
using System.Collections.Generic;

public class MultipleTaps : MonoBehaviour
{
    private float offset;
    public float percentOffset = 0.1f;
    public float TimeTillDisable = 0.25f;

    public List<Tap> DoubleTaps { get { return doubleTaps; } }
    private List<Tap> doubleTaps = new List<Tap>();
    private List<Tap> prevTaps = new List<Tap>();

	// Use this for initialization
	void Start ()
    {
        offset = Screen.width * percentOffset;
	}
	
	// Update is called once per frame
	void Update ()
    {
        // Clear all double taps at the start of next frame
        Reset();

        // Update timer for taps and mark any expired taps for removal
        List<Tap> toRemove = new List<Tap>();
        foreach (var tap in prevTaps)
        {
            tap.DisableTimer.Update();
            if (tap.DisableTimer.IsTime())
            {
                toRemove.Add(tap);
            }
        }

        // Remove taps
        foreach (var tap in toRemove)
        {
            prevTaps.Remove(tap);
        }

        // Check new touch with last touch
	    foreach (var touch in Input.touches)
        {
            if (touch.phase == TouchPhase.Began)
            {
                if (!checkDoubleTap(touch))
                {
                    // Unused, add to newPrev
                    var newTap = ScriptableObject.CreateInstance<Tap>();
                    newTap.Init(touch.position, TimeTillDisable);
                    prevTaps.Add(newTap);
                }
            }
        }
	}

    void Reset()
    {
        doubleTaps.Clear();
    }

    bool checkDoubleTap(Touch touch)
    {
        foreach (var prevTouch in prevTaps)
        {
            if ((touch.position - prevTouch.Position).sqrMagnitude <= offset * offset)
            {
                // Add to double tap
                var newTap = ScriptableObject.CreateInstance<Tap>();
                newTap.Init(touch.position, TimeTillDisable);

                doubleTaps.Add(newTap);

                // Stop checking as this touch is used
                return true;
            }
        }

        return false;
    }

    public bool HasDoubleTap()
    {
        return doubleTaps.Count > 0;
    }
}