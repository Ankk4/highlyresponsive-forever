using UnityEngine;
using System.Collections;
using System;

public class BreakableTile : Tile
{
    public int DefaultHitsLeft;

    private int hitsLeft;

    // Use this for initialization
    protected override void Start()
    {
        base.Start();

        hitsLeft = DefaultHitsLeft;
        updateState();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }

    protected void hit()
    {
        --hitsLeft;

        updateState();
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);

        if (IsAlive())
        {
            if (collision.GetComponent<Ball>())
            {
                hit();
            }
        }

        // Remove itself from tile count if dead
        if (!IsAlive())
        {
            --(transform.root.GetComponent<GameManager>().TileCount);
        }
    }

    public override void Reset()
    {
        base.Reset();
        
        hitsLeft = DefaultHitsLeft;
        updateState();
    }

    private void updateState()
    {
        if (IsAlive())
        {
            gameObject.SetActive(true);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    #region State check functions

    public override bool IsAlive()
    {
        return hitsLeft > 0;
    }

    #endregion State check functions
}
