using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{
    #region Controls
    
    // Key code for moving left
    public KeyCode MoveLeftKey;
    // Key code for moving right
    public KeyCode MoveRightKey;
    // Key code for shooting
    public KeyCode ShootKey;
    // Key code for melee
    public KeyCode MeleeKey;

    #endregion Controls

    #region Public variables

    [Header("Player data")]
    public float NormalSpeed;
    public float MeleeSpeed;
    public float ShootInterval;
    public int MaxLives;

    [Header("Force applied to ball on hit")]
    public float Force = 100.0f;

    #endregion Public Variables

    [Header("Object references")]
    public ResourcePool RefBulletPool;

    private FSM fsm;
    private Timer shootTimer;
    public float CurrentSpeed { set { currentSpeed = value; } }
    private float currentSpeed;
    private float horizontalBoundSize;
    private bool? lastDir; // Left = false | Right = true
    private int Lives;

    // Use this for initialization
    void Start ()
    {
        // Set up shoot timer
	    if (!shootTimer)
        {
            shootTimer = ScriptableObject.CreateInstance<Timer>();
        }
        shootTimer.Init(ShootInterval, false);

        // Set up horizontal bound to clamp player's movement
        horizontalBoundSize = ScreenScript.GetScreenSize().x;

        // Set up current speed
        currentSpeed = NormalSpeed;

        // Set up FSM
        fsm = GetComponent<FSM>();
        var idleState = ScriptableObject.CreateInstance<IdleState>();
        idleState.Init(fsm);
        fsm.ChangeState(idleState);

        lastDir = null;

        Lives = MaxLives;
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (TimeManager.IsPause())
        {
            return;
        }

        // Shoot timer updates
        shootTimer.Update();

        // Movement updates
        movementUpdates();

        attackUpdates();
	}

    private void movementUpdates()
    {
        // Stop movement updates if not allowed to move
        if (!canMove())
        {
            return;
        }

        if (Input.GetKey(MoveLeftKey))
        {
            MoveLeft();
        }
        if (Input.GetKey(MoveRightKey))
        {
            MoveRight();
        }
    }

    private void attackUpdates()
    {
        // Shoot
        if (shootTimer.IsTime() && canShoot() && RefBulletPool && Input.GetKeyDown(ShootKey))
        {
            var bullet = RefBulletPool.Fetch();
            if (bullet)
            {
                bullet.GetComponent<Bullet>().Activate(transform.localPosition, Vector2.up, 30.0f);
                shootTimer.Reset();
            }
        }

        // Melee
        var anim = GetComponent<Animator>();
        if (!isMelee())
        {
            if (Input.GetKeyDown(MeleeKey))
            {
                if (Input.GetKey(MoveLeftKey) || Input.GetKey(MoveRightKey))
                {
                    if ((Input.GetKey(MoveLeftKey) && Input.GetKey(MoveRightKey)))
                    {
                        // Left and Right key pressed, melee
                        var meleeState = ScriptableObject.CreateInstance<MeleeState>();
                        meleeState.Init(fsm);
                        fsm.ChangeState(meleeState);
                    }
                    else
                    {
                        var kickState = ScriptableObject.CreateInstance<KickState>();
                        kickState.Init(fsm, lastDir.GetValueOrDefault());
                        fsm.ChangeState(kickState);
                    }
                }
                else
                {
                    var meleeState = ScriptableObject.CreateInstance<MeleeState>();
                    meleeState.Init(fsm);
                    fsm.ChangeState(meleeState);
                }
            }
        }
    }

    #region Movement

    public void MoveLeft()
    {
        var localPos = transform.localPosition;
        localPos.x -= currentSpeed * TimeManager.DeltaTime;
        clampMovement(ref localPos.x);
        transform.localPosition = localPos;

        lastDir = false;
    }

    public void MoveRight()
    {
        var localPos = transform.localPosition;
        localPos.x += currentSpeed * TimeManager.DeltaTime;
        clampMovement(ref localPos.x);
        transform.localPosition = localPos;

        lastDir = true;
    }

    private void clampMovement(ref float x)
    {
        float halfBoundX = horizontalBoundSize * 0.5f;
        float halfScaleX = transform.localScale.x * 0.5f;
        x = Mathf.Clamp(x, -halfBoundX + halfScaleX, halfBoundX - halfScaleX);
    }

    #endregion Movement

    void OnTriggerEnter2D(Collider2D collision)
    {
        Ball ball = null;
        if (ball = collision.gameObject.GetComponent<Ball>())
        {
            var rb = ball.GetComponent<Rigidbody2D>();
            if (rb)
            {
                if (isMelee())
                {
                    // Bounce ball away
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

                    rb.drag = 0; // Reset linear drag
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
                else
                {
                    // Hit, reduce lives
                    var pos = transform.localPosition;
                    pos.x = 0.0f;
                    transform.localPosition = pos;

                    Lives = Mathf.Clamp(Lives - 1, 0, MaxLives);
                }
            }
        }
    }

    #region State check functions

    public bool IsAlive()
    {
        return Lives > 0;
    }

    bool isMelee()
    {
        return fsm.m_currentState is MeleeState || fsm.m_currentState is KickState;
    }

    bool canMove()
    {
        return !TimeManager.IsPause() && !isMelee();
    }

    bool canShoot()
    {
        return !TimeManager.IsPause() && !isMelee();
    }

    #endregion State check functions

    public void Reset()
    {
        // Set x position to middle
        var pos = transform.localPosition;
        pos.x = 0.0f;
        transform.localPosition = pos;

        // Reset current speed
        currentSpeed = NormalSpeed;
        
        // Reset FSM
        fsm = GetComponent<FSM>();
        var idleState = ScriptableObject.CreateInstance<IdleState>();
        idleState.Init(fsm);
        fsm.ChangeState(idleState);

        lastDir = null;

        Lives = MaxLives; 
    }
}
