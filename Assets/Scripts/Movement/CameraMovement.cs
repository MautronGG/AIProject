using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
  public float m_speed;
  // Start is called before the first frame update
  void Start()
  {

  }

  // Update is called once per frame
  void Update()
  {
    if (Input.GetKey(KeyCode.D))
    {
      transform.position += new Vector3(1f,0f,0f) * Time.deltaTime * m_speed;
    }
    if (Input.GetKey(KeyCode.A))
    {
      transform.position += new Vector3(-1f,0f,0f) * Time.deltaTime * m_speed;
    }
    if (Input.GetKey(KeyCode.W))
    {
      transform.position += new Vector3(0f,1f,0f) * Time.deltaTime * m_speed;
    }
    if (Input.GetKey(KeyCode.S))
    {
      transform.position += new Vector3(0f,-1f,0f) * Time.deltaTime * m_speed;
    }
  }
}
