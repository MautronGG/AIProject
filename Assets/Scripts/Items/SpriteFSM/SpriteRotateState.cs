using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteRotateState : SpriteStates
{
  float m_rotationDegree = 0f;
  float m_defaultRotateSpeed = 10f;
  float m_rotationSpeed;
  float m_runRotateSpeed = 25f;
  public Vector3 m_defaultRotation;

  bool m_noRotation1 = false;
  bool m_noRotation2 = false;
  private void Awake()
  {
    m_levelEditor = FindObjectOfType<LevelEditorManager>();
    m_sprite = FindObjectOfType<SpriteFollowMouse>();
  }
  public override void onEnter()
  {
    m_rotationDegree = 0f;
    m_rotationSpeed = m_defaultRotateSpeed;
  }

  public override void onExit()
  {

  }

  public override void onUpdate()
  {
    if (Input.GetKey(KeyCode.E))
    {
      m_noRotation1 = false;
      m_rotationDegree -= m_rotationSpeed * Time.deltaTime;
    }
    if (Input.GetKey(KeyCode.Q))
    {
      m_noRotation2 = false;
      m_rotationDegree += m_rotationSpeed * Time.deltaTime;
    }
    if (Input.GetKeyUp(KeyCode.E))
    {
      m_noRotation1 = true;
    }
    if (Input.GetKeyUp(KeyCode.Q))
    {
      m_noRotation2 = true;
    }

    transform.rotation = Quaternion.Euler(m_defaultRotation.x + m_rotationDegree, m_defaultRotation.y, m_defaultRotation.z);
    
    if (m_noRotation1 && m_noRotation2)
    {
      m_stateMachine.SetState(m_stateMachine.m_onIdleState);
    }
  }
}
