using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSM : MonoBehaviour
{
    public State m_onBuildState;
    public State m_onPlayState;
    public State m_currentState;

    //[SerializeField] bool m_onEditorState = false;

    // Start is called before the first frame update
    //void Start()
    //{
    //    if (!m_onEditorState)
    //    {
    //        StartFSM();
    //    }
    //}

    // Update is called once per frame
    void Update()
    {
        m_currentState.onUpdate();
    }
    public void SetState(State newState)
    {
        m_currentState.onExit();
        m_currentState = newState;
        m_currentState.onEnter();
    }

    public void StartFSM()
    {
        m_onBuildState = gameObject.AddComponent<BuildState>();
        m_onPlayState = gameObject.AddComponent<PlayState>();
        m_onBuildState.SetFSM(this);
        m_onPlayState.SetFSM(this);
        m_currentState = m_onBuildState;
        m_currentState.onEnter();
    }
}
