using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteIdleState : SpriteStates
{
  private void Awake()
  {
    m_levelEditor = FindObjectOfType<LevelEditorManager>();
    m_sprite = FindObjectOfType<SpriteFollowMouse>();
  }
  public override void onEnter()
  {
    
  }

  public override void onExit()
  {

  }

  public override void onUpdate()
  {
    if (Input.GetKey(KeyCode.E) || Input.GetKey(KeyCode.Q))
    {
      m_stateMachine.SetState(m_stateMachine.m_onRotateState);
    }
    if (Input.GetKey(KeyCode.F) || Input.GetKey(KeyCode.G))
    {
      m_stateMachine.SetState(m_stateMachine.m_onScaleState);
    }
  }
}
