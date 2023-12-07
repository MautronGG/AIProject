using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteScaleState : SpriteStates
{
  float m_scaleDegree = 0f;
  float m_defaultScaleSpeed = 1f;
  float m_scaleSpeed;
  float m_runScaleSpeed = 8f;
  public Vector3 m_defaultScale;
  bool m_noScale1 = false;
  bool m_noScale2 = false;
  private void Awake()
  {
    m_levelEditor = FindObjectOfType<LevelEditorManager>();
    m_sprite = FindObjectOfType<SpriteFollowMouse>();
  }
  public override void onEnter()
  {
    m_scaleDegree = 0f;
    m_scaleSpeed = m_defaultScaleSpeed;
  }

  public override void onExit()
  {

  }

  public override void onUpdate()
  {
    if (Input.GetKey(KeyCode.F))
    {
      m_noScale1 = false;
      m_scaleDegree += m_scaleSpeed * Time.deltaTime;
    }
    if (Input.GetKey(KeyCode.G))
    {
      m_noScale2 = false;
      m_scaleDegree -= m_scaleSpeed * Time.deltaTime;
    }
    if (Input.GetKeyUp(KeyCode.F))
    {
      m_noScale1 = true;
    }
    if (Input.GetKeyUp(KeyCode.G))
    {
      m_noScale2 = true;
    }

    transform.localScale += new Vector3(0f, m_scaleDegree, 0f);

    if (m_noScale1 && m_noScale2)
    {
      m_stateMachine.SetState(m_stateMachine.m_onIdleState);
    }
  }
}
