using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteFollowMouse : MonoBehaviour
{
  public float m_rotationDegree = 0f;
  public float m_rotationSpeed;
  public float m_scaleDegree = 0f;
  public float m_scaleSpeed = 1;
  public GameObject m_child;
  void Update()
  {
    m_scaleDegree = 0f;
    Vector2 screenPosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
    Vector2 worldPosition = Camera.main.ScreenToWorldPoint(screenPosition);
    transform.position = worldPosition;
    transform.rotation = Quaternion.Euler(0f, 0f, m_rotationDegree);
    
    if (Input.GetKey(KeyCode.E))
    {
      m_rotationDegree -= m_rotationSpeed * Time.deltaTime;
    }
    if (Input.GetKey(KeyCode.Q))
    {
      m_rotationDegree += m_rotationSpeed * Time.deltaTime;
    }
    if (Input.GetKey(KeyCode.F))
    {
      m_scaleDegree += m_scaleSpeed * Time.deltaTime;
      transform.localScale += new Vector3( m_scaleDegree, 0f, 0f);
    }
    if (Input.GetKey(KeyCode.G))
    {
      m_scaleDegree -= m_scaleSpeed * Time.deltaTime;
      transform.localScale += new Vector3(m_scaleDegree, 0f, 0f);
    }
  }
}
