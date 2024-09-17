using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : MonoBehaviour
{
  public float m_radius;
  public float m_density;
  private void Start()
  {
    var box = GetComponent<BoxCollider>();
    var x = box.bounds.size.x;
    var y = box.bounds.size.y;
    if (x >= y)
    {
      m_radius = x;
    }
    else
    {
      m_radius = y;
    }
  }
}
