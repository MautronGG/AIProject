using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalScript : ItemManager
{
  public Transform m_exitPortal;
  private void OnTriggerEnter(Collider other)
  {
    if (other.transform.tag == "Player")
    {
      other.gameObject.GetComponent<MinionsMovement>().PortalTrigger(m_exitPortal.position);
    }
  }
}
