using UnityEngine;
using System.Collections.Generic;

public class ResourcePoolManager : MonoBehaviour
{
    //public List<ResourcePool> Pools = new List<ResourcePool>();
    public ResourcePool[] Pools;
	// Use this for initialization
	protected virtual void Start ()
    {
	
	}
	
	// Update is called once per frame
    protected virtual void Update()
    {
	
	}

    public ResourcePool FetchPool(int type)
    {
        if (Pools == null)
        {
            return null;
        }
        if (type < 0 || type >= Pools.Length)
        {
            return null;
        }
        return Pools[type];
    }

    public void ToggleMovement(bool movement)
    {
        foreach (var pool in Pools)
        {
            pool.ToggleMovement(movement);
        }
    }
}
