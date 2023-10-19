using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemsOptions : MonoBehaviour
{
  public int m_ID;
  private LevelEditorManager m_levelEditor;
  public ItemManager m_item;
  public GameObject m_colorCanvas;

  private void Start()
  {
    m_levelEditor = GameObject.FindGameObjectWithTag("LevelEditorManager").GetComponent<LevelEditorManager>();
  }
  public void Delete()
  {
    Destroy(GetComponentInParent<ItemManager>().gameObject);
    m_levelEditor.m_itemButtons[m_ID].m_quantity++;
    m_levelEditor.m_itemButtons[m_ID].m_quantityText.text = m_levelEditor.m_itemButtons[m_ID].m_quantity.ToString();
  }
  //public void ColorWheel()
  //{
  //  m_colorCanvas.SetActive(true);
  //}
}
