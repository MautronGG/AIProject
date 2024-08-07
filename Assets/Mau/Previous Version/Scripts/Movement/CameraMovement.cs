using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
  float m_speed;
  public float m_runSpeed;
  public float m_defaultSpeed;
  public bool m_canMove = true;
  public bool m_autoMove = false;
  public GameObject m_minion;
  public Vector3 m_defaultPosition;

  private void Start()
  {
    m_speed = m_defaultSpeed;
    m_defaultPosition = transform.position;
  }
  // Update is called once per frame
  void Update()
  {
    if(m_canMove)
    {
      if (Input.GetKey(KeyCode.D))
      {
        transform.position += new Vector3(1f, 0f, 0f) * Time.deltaTime * m_speed;
      }
      if (Input.GetKey(KeyCode.A))
      {
        transform.position += new Vector3(-1f, 0f, 0f) * Time.deltaTime * m_speed;
      }
      if (Input.GetKey(KeyCode.W))
      {
        transform.position += new Vector3(0f, 1f, 0f) * Time.deltaTime * m_speed;
      }
      if (Input.GetKey(KeyCode.S))
      {
        transform.position += new Vector3(0f, -1f, 0f) * Time.deltaTime * m_speed;
      }
      if (Input.GetKeyDown(KeyCode.LeftShift))
      {
        m_speed = m_runSpeed;
      }
      if (Input.GetKeyUp(KeyCode.LeftShift))
      {
        m_speed = m_defaultSpeed;
      }
    }
    if (m_autoMove)
    {
      transform.position = new Vector3(m_minion.transform.position.x + 3, m_defaultPosition.y + 3.240495f, transform.position.z);
    }
  }
  public void AutomaticMovement(bool Bool)
  {
    //m_canMove = false;
    m_autoMove = Bool;
  }
  public void ChangeMovement(bool newMove)
  {
    m_canMove = newMove;
  }
    public void ResetTransform()
    {
        transform.position = m_defaultPosition;
    }
}
