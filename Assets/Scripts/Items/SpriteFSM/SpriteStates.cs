using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteStates : MonoBehaviour
{
  public SpriteFSM m_stateMachine;
  public LevelEditorManager m_levelEditor;
  public SpriteFollowMouse m_sprite;

  virtual public void onEnter()
  {

  }
  virtual public void onUpdate()
  {

  }
  virtual public void onExit()
  {

  }

  public void SetFSM(SpriteFSM fsm)
  {
    m_stateMachine = fsm;
  }
}
