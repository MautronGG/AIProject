using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
  public int m_ID;
  public int m_colorID = 7;
  //public GameObject m_optionsCanvas;
  //public GameObject m_colorCanvas;
  public LevelEditorManager m_levelEditor;
  //public GameObject m_child;
  public bool m_changedColor = false;
  public GameObject m_mySprite;
  public int m_personalID;

  public bool m_canOpenOptions = true;
  public bool m_canEdit = true;
  public bool m_canDelete = true;

  public Vector3 m_defaultPosition;
  public Quaternion m_defaultRotation;
  public Vector3 m_defaultScale;
  

  private void Awake()
  {
    m_levelEditor = GameObject.FindGameObjectWithTag("LevelEditorManager").GetComponent<LevelEditorManager>();
    m_defaultPosition = transform.position;
    m_defaultScale = transform.localScale;
    m_defaultRotation = transform.rotation;
  }
  private void OnMouseOver()
  {
    if(!m_levelEditor.m_optionsCanvas.activeInHierarchy && !m_levelEditor.m_colorCanvas.activeInHierarchy && !m_levelEditor.m_winCanvas.activeInHierarchy && !m_levelEditor.m_gameOverCanvas.activeInHierarchy && !m_levelEditor.m_controlCanvas.activeInHierarchy && m_canOpenOptions && !m_levelEditor.m_isEditing && !m_levelEditor.m_playButton.GetComponent<ButtonSelectionTracker>().IsSelected && !m_levelEditor.m_bridgeButton.GetComponent<ButtonSelectionTracker>().IsSelected && !m_levelEditor.m_pauseCanvas.activeInHierarchy) 
    {
      if (Input.GetMouseButtonDown(0))
      {
        m_levelEditor.m_itemID = this.gameObject;
        m_levelEditor.m_optionsCanvas.SetActive(true);
        m_levelEditor.m_HUDCanvas.SetActive(false);
      }
    }
  }
  public virtual void ResetDeafualts()
  {
    transform.position = m_defaultPosition;
    transform.rotation = m_defaultRotation;
    transform.localScale = m_defaultScale;
  }
  private void OnEnable()
  {
    ResetDeafualts();
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
