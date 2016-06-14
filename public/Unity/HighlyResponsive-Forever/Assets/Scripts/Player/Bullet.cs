using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour
{
    private Vector2 dir;
    private float speed;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        Vector2 localPos = transform.localPosition;
        localPos += dir * speed * TimeManager.DeltaTime;
        transform.localPosition = localPos;
	}

    public void Activate(Vector3 pos, Vector2 dir, float speed)
    {
        // Set bullet to active
        gameObject.SetActive(true);

        // Set local position
        transform.localPosition = pos;

        this.dir = dir;
        this.speed = speed;

        /*var rb = GetComponent<Rigidbody2D>();
        if (rb)
        {
            rb.velocity += dir * speed;
        }*/
    }
}
