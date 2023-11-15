using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSM : MonoBehaviour
{
  public State m_onEditorState;
  public State m_onPlayState;
  public State m_currentState;

  // Start is called before the first frame update
  void Start()
  {
    m_onEditorState = gameObject.AddComponent<EditorState>();
    m_onPlayState = gameObject.AddComponent<PlayState>();
    m_onEditorState.SetFSM(this);
    m_onPlayState.SetFSM(this);
    m_currentState = m_onEditorState;
    m_currentState.onEnter();
  }

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
}
