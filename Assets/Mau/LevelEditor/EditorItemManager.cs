using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditorItemManager : MonoBehaviour
{
  //public int m_ID;
  //public int m_colorID = 7;
  //public GameObject m_optionsCanvas;
  //public GameObject m_colorCanvas;
  public EditorManager m_editor;
  //public GameObject m_child;
  //public bool m_changedColor = false;
  //public GameObject m_mySprite;
  public int m_personalID;

  public bool m_canOpenOptions = true;
  //public bool m_canEdit = true;
  public bool m_canDelete = true;

  public Vector3 m_defaultPosition;
  public Quaternion m_defaultRotation;
  public Vector3 m_defaultScale;

  bool m_checks = true;
  bool m_canClick = true;
  EditorSpriteFollow m_spriteFollow;
  private void Awake()
  {
    m_editor = GameObject.FindGameObjectWithTag("EditorManager").GetComponent<EditorManager>();
    m_defaultPosition = transform.position;
    m_defaultScale = transform.localScale;
    m_defaultRotation = transform.rotation;
    m_spriteFollow = GetComponent<EditorSpriteFollow>();
    m_editor.m_isEditing = true;
  }
  private void Update()
  {
    if (Input.GetMouseButtonDown(0) && m_spriteFollow.m_follow && m_canClick)
    {
      PlaceDown();
    }
    m_canClick = true;
  }
  private void OnMouseOver()
  {
    if (!m_spriteFollow.m_follow)
    {
      m_checks = true;
      if (m_editor.m_optionsCanvas.activeInHierarchy)
      {
        m_checks = false;
      }
      ////else if (!m_editor.m_colorCanvas.activeInHierarchy)
      //{
      //  m_checks = false;
      //}
      ////else if (!m_editor.m_winCanvas.activeInHierarchy)
      //{
      //  m_checks = false;
      //}
      ////else if (!m_editor.m_gameOverCanvas.activeInHierarchy)
      //{
      //  m_checks = false;
      //}
      ////else if (!m_editor.m_controlCanvas.activeInHierarchy)
      //{
      //  m_checks = false;
      //}
      else if (!m_canOpenOptions)
      {
        m_checks = false;
      }
      else if (m_editor.m_isEditing)
      {
        m_checks = false;
      }
      else if (m_editor.m_playButton.GetComponent<ButtonSelectionTracker>().IsSelected)
      {
        m_checks = false;
      }
      ////else if (!m_editor.m_bridgeButton.GetComponent<ButtonSelectionTracker>().IsSelected)
      //{
      //  m_checks = false;
      //}
      else if (m_editor.m_pauseCanvas.activeInHierarchy)
      {
        m_checks = false;
      }
      if (Input.GetMouseButtonDown(0) && m_checks)
      {
        PickUp();
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
  private void PickUp()
  {
    //m_editor.m_itemID = this.gameObject;
    //m_editor.m_optionsCanvas.SetActive(true);
    m_editor.m_HUDCanvas.SetActive(false);
    m_spriteFollow.m_follow = true;
    m_editor.m_isEditing = true;
    m_canClick = false;
  }
  private void PlaceDown()
  {
    m_spriteFollow.m_follow = false;
    gameObject.layer = LayerMask.NameToLayer("Black");
    m_editor.m_itemButtons[m_editor.m_currentButtonID].m_isClicked = false;
    m_editor.m_isEditing = false;
    m_editor.m_HUDCanvas.SetActive(true);
  }
}
