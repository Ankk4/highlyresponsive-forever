using UnityEngine;
using System.Collections;

public class Tile : MonoBehaviour
{

    // Use this for initialization
    protected virtual void Start()
    {

    }

    // Update is called once per frame
    protected virtual void Update()
    {

    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
    }

    public virtual void Reset()
    {

    }

    #region State check functions

    public virtual bool IsAlive()
    {
        return false;
    }

    #endregion State check functions
}
