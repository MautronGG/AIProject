using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
  public int m_ID;
  public int m_colorID = 6;
  //public GameObject m_optionsCanvas;
  //public GameObject m_colorCanvas;
  private LevelEditorManager m_levelEditor;
  //public GameObject m_child;
  public bool m_changedColor = false;


  private void Awake()
  {
    m_levelEditor = GameObject.FindGameObjectWithTag("LevelEditorManager").GetComponent<LevelEditorManager>();
  }
  private void OnMouseOver()
  {
    if(!m_levelEditor.m_optionsCanvas.activeInHierarchy && !m_levelEditor.m_colorCanvas.activeInHierarchy && !m_levelEditor.m_winCanvas.activeInHierarchy && !m_levelEditor.m_gameOverCanvas.activeInHierarchy && !m_levelEditor.m_controlCanvas.activeInHierarchy)
    {
      if (Input.GetMouseButtonDown(0))
      {
        m_levelEditor.m_itemID = this.gameObject;
        m_levelEditor.m_optionsCanvas.SetActive(true);
        m_levelEditor.m_HUDCanvas.SetActive(false);
      }
    }
  }
 
  //public void ChangeColor(int ID)
  //{
  //  var render = m_child.GetComponent<Renderer>();
  //  if (ID == 0)
  //  {
  //    gameObject.layer = LayerMask.NameToLayer("Red");
  //    render.gameObject.layer = LayerMask.NameToLayer("Red");
  //    render.material = m_levelEditor.m_red;
  //  }
  //  if (ID == 1)
  //  {
  //    gameObject.layer = LayerMask.NameToLayer("Yellow");
  //    render.gameObject.layer = LayerMask.NameToLayer("Yellow");
  //    render.material = m_levelEditor.m_yellow;
  //  }
  //  if (ID == 2)
  //  {
  //    gameObject.layer = LayerMask.NameToLayer("Green");
  //    render.gameObject.layer = LayerMask.NameToLayer("Green");
  //    render.material = m_levelEditor.m_green;
  //  }
  //  if (ID == 3)
  //  {
  //    gameObject.layer = LayerMask.NameToLayer("Cyan");
  //    render.gameObject.layer = LayerMask.NameToLayer("Cyan");
  //    render.material = m_levelEditor.m_cyan;
  //  }
  //  if (ID == 4)
  //  {
  //    gameObject.layer = LayerMask.NameToLayer("Blue");
  //    render.gameObject.layer = LayerMask.NameToLayer("Blue");
  //    render.material = m_levelEditor.m_blue;
  //  }
  //  if (ID == 5)
  //  {
  //    gameObject.layer = LayerMask.NameToLayer("Magenta");
  //    render.gameObject.layer = LayerMask.NameToLayer("Magenta");
  //    render.material = m_levelEditor.m_magenta;
  //  }
  //  if (ID == 6)
  //  {
  //    gameObject.layer = LayerMask.NameToLayer("White");
  //    render.gameObject.layer = LayerMask.NameToLayer("White");
  //    render.material = m_levelEditor.m_white;
  //  }
  //}
}
