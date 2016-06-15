using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

public class ResourcePool : MonoBehaviour
{
    [Header("Public Variables")]
    [Tooltip("Default resource count to load")]
    public uint m_ResourceValue = 10;
    [Tooltip("Resource limit")]
    public uint m_ResourceCap = 10;
    [Tooltip("Number of resources to load if not enough")]
    public uint m_AutoRaiseValue = 5;
    [Tooltip("Load resources to limit")]
    public bool m_PreloadAll = false;
    [Tooltip("Allow creating of resources during runtime if not enough")]
    public bool m_AutoRaiseCap = false;
    [Tooltip("Resource blueprint")]
    public GameObject m_Blueprint;

    // Organisation
    [Header("Scene organisation")]
    [Tooltip("Time between each scene organisation")]
    public float OrganizeInterval = 5.0f;
    private float OrganizeTimer = 0.0f;

    protected List<GameObject> m_resourcePool = new List<GameObject>();
    private List<Vector2> m_storedVelocity = new List<Vector2>();

    public List<GameObject> Resources
    {
        get
        {
            return m_resourcePool;
        }
    }

    public List<GameObject> ActiveResources
    {
        get
        {
            return (from item in m_resourcePool where item.activeSelf select item).ToList();
        }
    }
    public List<GameObject> InactiveResources
    {
        get
        {
            return (from item in m_resourcePool where !item.activeSelf select item).ToList();
        }
    }

	// Use this for initialization
	void Start ()
    {
	    if (m_PreloadAll)
        {
            expand(m_ResourceCap);
        }
        else
        {
            expand(m_ResourceValue);
        }
	}
	
	// Update is called once per frame
	void Update ()
    {
        float dt = TimeManager.DeltaTime;

	    // TODO: Scene organisation (Optional)
        if (OrganizeTimer < OrganizeInterval)
        {
            OrganizeTimer += dt;
        }
        else
        {
            StartCoroutine(SEQ_Organization());
        }
	}

    public void ToggleMovement(bool movement)
    {
        int count = 0;
        foreach (var go in m_resourcePool)
        {
            var rb = go.GetComponent<Rigidbody2D>();
            if (rb)
            {
                if (movement)
                {
                    rb.velocity = m_storedVelocity[count++];
                }
                else
                {
                    m_storedVelocity[count++] = rb.velocity;
                }
                rb.isKinematic = !movement;
            }

            var anim = go.GetComponent<AnimatedObject>();
            if (anim)
            {
                anim.PauseAnim(!movement);
            }
        }
    }

    public GameObject Fetch()
    {
        foreach (var i in m_resourcePool)
        {
            if (!i.activeSelf)
            {
                return i;
            }
        }

        bool expanded = expand(m_AutoRaiseValue);
        if (expanded)
        {
            return m_resourcePool[m_resourcePool.Count - 1];
        }
        return null;
    }

    public void ResetAll()
    {
        foreach (var i in m_resourcePool)
        {
            i.SetActive(false);
        }
    }

    public bool RaiseCap(uint newCap)
    {
        // Do not allow shrinking as it will cause problems with existing items
        if (newCap <= m_ResourceCap)
        {
            return false;
        }

        m_ResourceCap = newCap;

        return true;
    }

    private bool expand(uint expandBy)
    {
        uint newCount = (uint)m_resourcePool.Count + expandBy;
        
        // Do not allow expanding if above cap unless auto raise cap is enabled
        if (!m_AutoRaiseCap && newCount > m_ResourceCap)
        {
            return false;
        }
        RaiseCap(newCount);

         for (int i = 0; i < expandBy; ++i)
         {
             createNew();
         }
         return true;
    }

    private void createNew()
    {
        // Create the resource
        GameObject newObj = Instantiate(m_Blueprint);
        // Set it's activity to false
        newObj.SetActive(false);
        // Move this object under the Manager as child for Scene Organization
        newObj.transform.parent = transform;
        // Add into the list
        m_resourcePool.Add(newObj);
        m_storedVelocity.Add(Vector2.zero);
    }

    IEnumerator SEQ_Organization()
    {
        foreach (var res in InactiveResources)
        {
            if (!res.activeSelf)
            {
                // Organizing
                res.transform.parent = transform;
                yield return new WaitForSeconds(0);
            }
        }
        // Reset timer
        OrganizeTimer = 0.0f;
    }
}
