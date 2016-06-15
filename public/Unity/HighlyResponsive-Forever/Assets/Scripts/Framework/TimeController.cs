using UnityEngine;
using System.Collections;

public class TimeController : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
	    if (Input.GetKey(KeyCode.Keypad3))
        {
            TimeManager.TimeScale = Mathf.Clamp(TimeManager.TimeScale + 5.0f * TimeManager.DeltaTime, 0, 100);
        }
        if (Input.GetKey(KeyCode.Keypad1))
        {
            TimeManager.TimeScale = Mathf.Clamp(TimeManager.TimeScale - 5.0f * TimeManager.DeltaTime, 0, 100);
        }
        if (Input.GetKey(KeyCode.Keypad2))
        {
            TimeManager.TimeScale = 1.0f;
        }
    }
}
