using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditorState : State
{
  private void Awake()
  {
    m_levelManager = FindObjectOfType<LevelManager>();
  }
  public override void onEnter()
  {
    m_levelManager.m_restartEvents.Invoke();
  }

  public override void onExit()
  {
  
  }

  public override void onUpdate()
  {
  }
}
