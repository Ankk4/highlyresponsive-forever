using UnityEngine;
using System.Collections;

public class Tile : MonoBehaviour
{
    public int HitsLeft;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (IsAlive())
        {
            if (collision.GetComponent<Ball>())
            {
                hit();
            }
        }
    }

    private void hit()
    {
        --HitsLeft;

        if (!IsAlive())
        {
            gameObject.SetActive(false);
        }
    }

    #region State check functions

    public bool IsAlive()
    {
        return HitsLeft > 0;
    }

    #endregion State check functions
}
