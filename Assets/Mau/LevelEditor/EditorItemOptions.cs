using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditorItemOptions : MonoBehaviour
{
  //public int m_ID;
  private EditorManager m_editor;
  //public ItemManager m_item;
  //public GameObject m_colorCanvas;

  private void Start()
  {
    m_editor = GameObject.FindGameObjectWithTag("EditorManager").GetComponent<EditorManager>();
  }

  public void Delete()
  {
    var me = m_editor.m_item.gameObject;
    var parent = me.transform.parent.gameObject;
    if (parent != null)
    {
      Destroy(parent);
    }
    else
    {
      Destroy(me);

    }
  }
  public void PickUp()
  {
    //m_editor.m_itemID = this.gameObject;
    //m_editor.m_optionsCanvas.SetActive(true);
    m_editor.m_HUDCanvas.SetActive(false);
    m_editor.m_spriteFollow.m_follow = true;
    m_editor.m_isEditing = true;
    m_editor.m_item.m_canClick = false;
  }
}
