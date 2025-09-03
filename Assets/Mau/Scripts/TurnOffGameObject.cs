using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnOffGameObject : MonoBehaviour
{
    [SerializeField] bool m_state;
    bool m_firstFrame = true;
    // Start is called before the first frame update
    void Start()
    {
        gameObject.SetActive(m_state);
        m_firstFrame = false;
    }

    public void ChangeState()
    {
        m_state = !m_state;
        gameObject.SetActive(m_state);
    }

    private void OnDisable()
    {
        if (!m_firstFrame)
        {
            m_state = false;
        }
    }

    private void OnEnable()
    {
        if (!m_firstFrame)
        {
            m_state = true;
        }
    }
}
