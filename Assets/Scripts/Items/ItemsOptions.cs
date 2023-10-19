using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemsOptions : MonoBehaviour
{
  //public int m_ID;
  private LevelEditorManager m_levelEditor;
  //public ItemManager m_item;
  //public GameObject m_colorCanvas;

  private void Start()
  {
    m_levelEditor = GameObject.FindGameObjectWithTag("LevelEditorManager").GetComponent<LevelEditorManager>();
  }
  public void Delete()
  {
    Destroy(m_levelEditor.m_itemID);
    m_levelEditor.m_itemButtons[m_levelEditor.m_currentButtonID].m_quantity++;
    m_levelEditor.m_itemButtons[m_levelEditor.m_currentButtonID].m_quantityText.text = m_levelEditor.m_itemButtons[m_levelEditor.m_currentButtonID].m_quantity.ToString();
  }
  public void DeleteEdit()
  {
    Destroy(m_levelEditor.m_itemID);
  }
  public void OnEdit()
  {
    m_levelEditor.m_editClicked = true;
    Vector2 screenPosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
    Vector2 worldPosition = Camera.main.ScreenToWorldPoint(screenPosition);
    var obj = Instantiate(m_levelEditor.m_itemSprite[m_levelEditor.m_currentButtonID], new Vector3(worldPosition.x, worldPosition.y, 0), Quaternion.identity);
    obj.GetComponent<SpriteFollowMouse>().ChangeDefaults(m_levelEditor.m_itemID.transform.rotation.eulerAngles, m_levelEditor.m_itemID.transform.localScale);
  }
  //public void ColorWheel()
  //{
  //  m_colorCanvas.SetActive(true);
  //}
}
