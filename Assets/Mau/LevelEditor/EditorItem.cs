using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditorItem : MonoBehaviour
{
  
  public bool m_isClicked = false;
  public EditorManager m_editor;
  GameObject m_obj;
  EditorSpriteFollow m_spriteFollow;
  
  //[Dropdown("m_editor.m_objectsList")]
  //public string m_name;
  public ObjectsEnum m_ID = new ObjectsEnum();

  public enum ObjectsEnum
  {
    NormalBlock,
    SpikyBlock,
    Portal,
    Key,
    Spring,
    Enemy,
    Laser
  };
  
  void Start()
  {
    m_editor = GameObject.FindGameObjectWithTag("EditorManager").GetComponent<EditorManager>();
    m_spriteFollow = GetComponent<EditorSpriteFollow>();
  }
  private void Update()
  {
    if (Input.GetMouseButtonDown(0) && m_editor.m_itemButtons[m_editor.m_currentButtonID].m_isClicked)
    {
      m_spriteFollow.m_follow = false;
      m_obj.gameObject.layer = LayerMask.NameToLayer("Black");
      m_editor.m_itemButtons[m_editor.m_currentButtonID].m_isClicked = false;
      m_editor.m_isEditing = false;
    }
  }
  public void OnClick()
  {
    Vector2 screenPosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
    Vector2 worldPosition = Camera.main.ScreenToWorldPoint(screenPosition);
    m_isClicked = true;
    m_obj = Instantiate(m_editor.m_itemPrefabs[(int)m_ID], new Vector3(worldPosition.x, worldPosition.y, 0), Quaternion.identity);
    m_obj.gameObject.layer = LayerMask.NameToLayer("nocol");
    m_editor.m_currentButtonID = (int)m_ID;
    m_editor.m_HUDCanvas.SetActive(false);
    m_editor.m_isEditing = true;
  }
 
 
}
