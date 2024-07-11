using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SpringScriptV1 : ItemManager
{
  public Transform m_point1;
  public Transform m_point2;
  public Transform m_point3;
  public List<Transform> m_list;
  public int m_uses;
  int m_debugUses;
  public TextMeshProUGUI m_countText;
  public bool m_canChangeDirection;
  public bool m_hidden;
  private void Start()
  {
    m_countText.text = System.Convert.ToString(m_uses) + " uses left";
    m_debugUses = m_uses;
    m_list.Add(m_point1);
    m_list.Add(m_point2);
    m_list.Add(m_point3);
    m_hidden = false;
  }
  private void Update()
  {
    if (m_debugUses <= 0 && !m_hidden)
    {
      m_hidden = true;
      this.gameObject.SetActive(false);
    }
  }
  private void OnTriggerEnter(Collider other)
  {
    if (other.transform.tag == "Player" && m_debugUses > 0)
    {
      var a = other.GetComponent<MinionsMovement>();
      a.m_controller.enabled = false;
      a.m_isFollowPath = true;
      a.m_pathList = m_list;
      a.m_pathIndexNext = 0;
      a.m_pathIndexPrev = 0;
      m_debugUses--;
      m_countText.text = System.Convert.ToString(m_debugUses) + " uses left";
      if (m_canChangeDirection)
      {
        a.ChangeDirection();
      }
    }

  }
  public override void ResetDeafualts()
  {
    base.ResetDeafualts();
    m_debugUses = m_uses;
    m_countText.text = System.Convert.ToString(m_uses) + " uses left";
    m_hidden = false;
  }
}
