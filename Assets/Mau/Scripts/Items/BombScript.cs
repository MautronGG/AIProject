using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombScript : ItemManager
{
  public GameObject m_wall;
  bool m_onPlayer = false;
  public Transform m_player;
  private void Update()
  {
    if (m_onPlayer)
    {
      transform.position = new Vector3(m_player.position.x, m_player.position.y + 1, transform.position.z);
    }
  }
  private void OnTriggerEnter(Collider other)
  {
    if (other.transform.tag == "Player" && !m_levelEditor.m_resettingDeafults)
    {
      m_player = other.transform;
      //m_player.gameObject.GetComponent<MinionsMovement>().m_bomb = true; 
      m_onPlayer = true;
      transform.localScale = new Vector3(15f, 15f, 15f);
    }
  }
  public override void ResetDeafualts()
  {
    base.ResetDeafualts();
    m_onPlayer = false;
    m_player = null;
  }
}
