using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

public enum E_CARRY_TYPE
{
    NONE = 0,
    KEY,
    COUNT
}


public class Carriable : MonoBehaviour
{

    
    [SerializeField] protected E_CARRY_TYPE m_type;

    private GameObject m_target;

    public E_CARRY_TYPE type
    {
        get { return m_type;  }
        set { m_type = value; }
    }

    protected virtual void Start()
    {
        m_type = E_CARRY_TYPE.NONE;
    }


    protected virtual void Update()
    {
        if (m_target != null)
        {
            transform.position = m_target.transform.position;
        }
    }

    public void AttachTo(GameObject target)
    {
        m_target = target;
        transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
    }

    public void UnAttach()
    {
        m_target = null;
        transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
    }

}
