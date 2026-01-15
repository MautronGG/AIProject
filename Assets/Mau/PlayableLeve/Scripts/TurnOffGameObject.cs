using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnOffGameObject : MonoBehaviour
{
    [SerializeField] bool m_startingState;
    bool m_currentState;
    bool m_firstFrame = true;
    [SerializeField] bool m_onEditorState = false;
    // Start is called before the first frame update
    void Start()
    {
        m_currentState = m_startingState;
        m_firstFrame = false;
        gameObject.SetActive(m_currentState);
    }

    public void ChangeState()
    {
        m_currentState = !m_currentState;
        gameObject.SetActive(m_currentState);
    }

    private void OnDisable()
    {
        if (!m_firstFrame || m_onEditorState)
        {
            m_currentState = false;
            Debug.Log(this + "OFF" + " , " + m_firstFrame + " , " + m_onEditorState);
        }
    }

    private void OnEnable()
    {
        if (!m_firstFrame || m_onEditorState)
        {
            m_currentState = true;
            Debug.Log(this + "ON" + " , " + m_firstFrame + " , " + m_onEditorState);
        }
    }

    public void ResetState()
    {
        m_currentState = m_startingState;
        gameObject.SetActive(m_currentState);
    }
}
