using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{
    #region Controls
    
    // Key code for moving left
    public KeyCode MoveLeftKey = KeyCode.A;
    // Key code for moving right
    public KeyCode MoveRightKey = KeyCode.D;
    // Key code for shooting
    public KeyCode ShootKey = KeyCode.Space;

    #endregion Controls

    #region Public variables

    public float Speed = 2.0f;
    public float ShootInterval = 0.4f;

    #endregion Public Variables

    public ResourcePool RefBulletPool;

    private Timer ShootTimer;

    // Use this for initialization
    void Start ()
    {
	    if (!ShootTimer)
        {
            ShootTimer = ScriptableObject.CreateInstance<Timer>();
        }
        ShootTimer.Init(ShootInterval, false);
	}
	
	// Update is called once per frame
	void Update ()
    {
        // Shoot timer updates
        ShootTimer.Update();

        // Movement updates
        movementUpdates();

        // Shoot
        if (ShootTimer.IsTime() && RefBulletPool && Input.GetKey(ShootKey))
        {
            var bullet = RefBulletPool.Fetch();
            if (bullet)
            {
                bullet.GetComponent<Bullet>().Activate(transform.localPosition, Vector2.up, 30.0f);
                ShootTimer.Reset();
            }
        }
	}

    private void movementUpdates()
    {
        if (Input.GetKey(MoveLeftKey))
        {
            moveLeft();
        }
        else if (Input.GetKey(MoveRightKey))
        {
            moveRight();
        }
    }

    #region Movement

    private void moveLeft()
    {
        var localPos = transform.localPosition;
        localPos.x -= Speed * TimeManager.DeltaTime;
        transform.localPosition = localPos;
    }

    private void moveRight()
    {
        var localPos = transform.localPosition;
        localPos.x += Speed * TimeManager.DeltaTime;
        transform.localPosition = localPos;
    }

    #endregion Movement

    void OnTriggerEnter2D(Collider2D collision)
    {
        
    }
}
