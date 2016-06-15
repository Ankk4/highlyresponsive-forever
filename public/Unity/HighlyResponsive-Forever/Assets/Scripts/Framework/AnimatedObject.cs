using UnityEngine;
using System.Collections;

public class AnimatedObject : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public virtual void PauseAnim(bool pause)
    {
        var anim = GetComponent<Animator>();
        if (anim)
        {
            anim.enabled = !pause;
        }
    }
}
