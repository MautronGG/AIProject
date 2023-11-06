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
    var color = m_levelEditor.m_itemID.GetComponent<ItemManager>().m_colorID;
    m_levelEditor.RemoveColor(color);
    //m_levelEditor.m_itemsList.Remove(m_levelEditor.m_itemID.GetComponent<ItemManager>());
    Destroy(m_levelEditor.m_itemID.GetComponent<ItemManager>().m_mySprite);
    Destroy(m_levelEditor.m_itemID);
    m_levelEditor.m_itemButtons[m_levelEditor.m_currentButtonID].m_quantity++;
    m_levelEditor.m_itemButtons[m_levelEditor.m_currentButtonID].m_quantityText.text = m_levelEditor.m_itemButtons[m_levelEditor.m_currentButtonID].m_quantity.ToString();

   
  }
  public void DeleteEdit()
  {
    //m_levelEditor.m_itemsList.Remove(m_levelEditor.m_itemID.GetComponent<ItemManager>());
    //Destroy(m_levelEditor.m_itemID);
    m_levelEditor.m_itemsList[m_levelEditor.m_itemID.GetComponent<ItemManager>().m_personalID].gameObject.SetActive(false);
    //Remove(m_levelEditor.m_itemID.GetComponent<ItemManager>());

  }
  public void OnEdit()
  {
    m_levelEditor.m_editClicked = true;
    Vector2 screenPosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
    Vector2 worldPosition = Camera.main.ScreenToWorldPoint(screenPosition);
    var sprite = m_levelEditor.m_itemsList[m_levelEditor.m_itemID.GetComponent<ItemManager>().m_personalID].m_mySprite;
    sprite.SetActive(true);
    sprite.GetComponent<SpriteFollowMouse>().ChangeDefaults(m_levelEditor.m_itemID.transform.rotation.eulerAngles, m_levelEditor.m_itemID.transform.localScale, m_levelEditor.m_itemID.GetComponent<ItemManager>().m_colorID);
    //var obj = Instantiate(m_levelEditor.m_itemSprite[m_levelEditor.m_currentButtonID], new Vector3(worldPosition.x, worldPosition.y, 0), Quaternion.identity);
    //obj.GetComponent<SpriteFollowMouse>().ChangeDefaults(m_levelEditor.m_itemID.transform.rotation.eulerAngles, m_levelEditor.m_itemID.transform.localScale, m_levelEditor.m_itemID.GetComponent<ItemManager>().m_colorID);
  }
  //public void ColorWheel()
  //{
  //  m_colorCanvas.SetActive(true);
  //}
}
