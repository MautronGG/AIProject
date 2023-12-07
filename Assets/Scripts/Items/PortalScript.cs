using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalScript : ItemManager
{
  public Transform m_exitPortal;
  private void OnTriggerEnter(Collider other)
  {
    var player = other.gameObject.GetComponent<MinionsMovement>();
    if (other.transform.tag == "Player" && !player.m_portaled)
    {
      player.PortalTrigger(m_exitPortal.position);
      player.m_portaled = true;
    }
  }
}
