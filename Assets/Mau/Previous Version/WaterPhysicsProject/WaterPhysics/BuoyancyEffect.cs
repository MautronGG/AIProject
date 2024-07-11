using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuoyancyEffect : MonoBehaviour
{
  public Transform[] Floaters;
  public float UnderWaterDrag = 3f;
  public float UnderWaterAngularDrag = 1f;
  public float AirDrag = 0f;
  public float AirAngularDrag = 0.05f;
  public float WaterHeight = 0f;

  public float m_density;
  public float m_mass;
  public float m_volume;
  public float m_waterDensity = 15f;
  public double m_force;
  public float m_densitiesDiff;

  Rigidbody Rb;
  bool Underwater;
  int FloatersUnderWater;
  // Start is called before the first frame update
  void Start()
  {
    Rb = this.GetComponent<Rigidbody>();
  }

  // Update is called once per frame
  void FixedUpdate()
  {
    if( m_volume <= 0)
    {
      m_volume = 1;
    }
    if(m_mass <= 0)
    {
      m_mass = 1;
    }
    FloatersUnderWater = 0;
    var mass = m_mass / 1000;
    var volume = m_volume / 1000000;
    var water = m_waterDensity * 1000;
    m_density = mass / volume;
    m_densitiesDiff = water - m_density;
    var Cubicradius = (volume * 3) / (4 * 3.1416);
    var radius = System.Math.Pow(Cubicradius, (double)1 / 3);
    m_force = m_densitiesDiff * 9.81 * (4 / 3 * 3.1416f) * (float)Cubicradius;
    if (m_force < 0)
    {
      m_force *= -1f;
    }
    for (int i = 0; i < Floaters.Length; i++)
    {
      float diff = Floaters[i].position.y - WaterHeight;
      if (diff < 0)
      {
        if (m_densitiesDiff > 0)
        {
          Rb.useGravity = true;
          Rb.AddForceAtPosition(Vector3.up * (float)m_force * Mathf.Abs(diff) * 10, Floaters[i].position, ForceMode.Force);
        }
        else if (m_densitiesDiff < 0)
        {
          //m_force *= -1;
          Rb.useGravity = true;
          Rb.AddForceAtPosition(Vector3.down * (float)m_force * Mathf.Abs(diff) * 10, Floaters[i].position, ForceMode.Force);
        }
        else if (m_densitiesDiff == 0)
        {
          Rb.velocity = Vector3.zero;
          Rb.useGravity = false;
        }
        FloatersUnderWater += 1;
        if (!Underwater)
        {
          Underwater = true;
          SwitchState(true);
        }
      }
    }
    if (Underwater && FloatersUnderWater == 0)
    {
      Underwater = false;
      SwitchState(false);
    }
  }
  void SwitchState(bool isUnderwater)
  {
    if (isUnderwater)
    {
      Rb.drag = UnderWaterDrag;
      Rb.angularDrag = UnderWaterAngularDrag;
    }
    else
    {
      Rb.useGravity = true;
      Rb.drag = AirDrag;
      Rb.angularDrag = AirAngularDrag;
    }
  }

  private void Update()
  {
    Rb.mass = m_density;

  }
  //public void ChangeDensity()
  //{
  //}
}
