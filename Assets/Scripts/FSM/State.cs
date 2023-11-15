using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State : MonoBehaviour
{
  public FSM m_stateMachine;
  public bool m_canPlay;
  public LevelEditorManager m_levelEditor;
  
  virtual public void onEnter()
  {

  }
  virtual public void onUpdate()
  {

  }
  virtual public void onExit()
  {

  }
 
  public void SetFSM(FSM fsm)
  {
    m_stateMachine = fsm;
  }

}
