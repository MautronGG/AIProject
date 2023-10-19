using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelEditorManager : MonoBehaviour
{
  public ItemController[] m_itemButtons;
  public GameObject[] m_itemPrefabs;
  public GameObject[] m_itemSprite;
  public int m_currentButtonID;

  [Header("Colors")]
  public Material m_red;
  public Material m_yellow;
  public Material m_green;
  public Material m_cyan;
  public Material m_blue;
  public Material m_magenta;
  public Material m_white;
  public int m_colorID;

  private void Update()
  {
    Vector2 screenPosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
    Vector2 worldPosition = Camera.main.ScreenToWorldPoint(screenPosition);

    if (Input.GetMouseButtonDown(0) && m_itemButtons[m_currentButtonID].m_isClicked)
    {
      m_itemButtons[m_currentButtonID].m_isClicked = false;
      var obj = Instantiate(m_itemPrefabs[m_currentButtonID], new Vector3(worldPosition.x, worldPosition.y, 0), Quaternion.Euler(Quaternion.identity.x, Quaternion.identity.y, FindObjectOfType<SpriteFollowMouse>().m_rotationDegree));
      obj.GetComponent<ItemManager>().transform.localScale = FindObjectOfType<SpriteFollowMouse>().transform.localScale;
      Destroy(GameObject.FindGameObjectWithTag("ItemSprite"));
    }
  }
}
