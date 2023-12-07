using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIText : MonoBehaviour
{
  public Slider m_massSlider;
  public TMP_InputField m_massInput;
  public Slider m_volumeSlider;
  public TMP_InputField m_volumeInput;

  public Buoyancy m_buoy;

  private void Start()
  {
    m_massSlider.value = m_buoy.m_mass;
    m_volumeSlider.value = m_buoy.m_volume;
    m_volumeInput.text = System.Convert.ToString(m_buoy.m_volume);
    m_massInput.text = System.Convert.ToString(m_buoy.m_mass);
  }
  public void ChangeMassSlider()
  {
    m_massSlider.value = int.Parse(m_massInput.text);
  }
  public void ChangeVolumeSlider()
  {
    m_volumeSlider.value = int.Parse(m_volumeInput.text);
  }
  public void ChangeMassInput()
  {
    m_massInput.text = System.Convert.ToString(m_massSlider.value);
    
  }
  public void ChangeVolumeInput()
  {
    m_volumeInput.text = System.Convert.ToString(m_volumeSlider.value);
    
  }
  public void ChangeMass()
  {
    m_buoy.m_mass = m_massSlider.value;
    m_buoy.CalculatePhysics();
  }
  public void ChangeVolume()
  {
    m_buoy.m_volume = m_volumeSlider.value;
    m_buoy.CalculatePhysics();
  }
}
