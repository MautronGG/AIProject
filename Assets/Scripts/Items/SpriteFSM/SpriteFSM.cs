using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteFSM : MonoBehaviour
{
  public SpriteStates m_onIdleState;
  public SpriteStates m_onRotateState;
  public SpriteStates m_onScaleState;
  public SpriteStates m_currentState;

  // Start is called before the first frame update
  void Start()
  {
    m_onRotateState = gameObject.AddComponent<SpriteRotateState>();
    m_onScaleState = gameObject.AddComponent<SpriteScaleState>();
    m_onIdleState = gameObject.AddComponent<SpriteIdleState>();
    m_onIdleState.SetFSM(this);
    m_onRotateState.SetFSM(this);
    m_onScaleState.SetFSM(this);
    m_currentState = m_onIdleState;
    m_currentState.onEnter();
  }

  // Update is called once per frame
  void Update()
  {
    m_currentState.onUpdate();
  }
  public void SetState(SpriteStates newState)
  {
    m_currentState.onExit();
    m_currentState = newState;
    m_currentState.onEnter();
  }
}
