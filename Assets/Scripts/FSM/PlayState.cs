using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayState : State
{
  private void Awake()
  {
    m_levelEditor = FindObjectOfType<LevelEditorManager>();
  }
  public override void onEnter()
  {
    m_levelEditor.m_canPlay = true;
    m_levelEditor.m_playEvents.Invoke();
  }

  public override void onExit()
  {
  
  }

  public override void onUpdate()
  {
    if (!m_levelEditor.m_canPlay)
    {
      m_stateMachine.SetState(m_stateMachine.m_onEditorState);
    }
  }
}
