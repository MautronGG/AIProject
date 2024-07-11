using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombWallScript : ItemManager
{
  public BombScript m_bomb;

  private void OnTriggerEnter(Collider other)
  {
    if (m_bomb.m_player)
    {
      if (other.gameObject == m_bomb.m_player.gameObject)
      {
        m_bomb.m_player.gameObject.GetComponent<MinionsMovement>().m_bomb = true;
        m_bomb.gameObject.SetActive(false);
        gameObject.SetActive(false);
      }
    }
  }
}
