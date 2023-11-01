using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
  float m_speed;
  public float m_runSpeed;
  public float m_defaultSpeed;
  public bool m_canMove = true;

  private void Start()
  {
    m_speed = m_defaultSpeed;
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
  }
}
