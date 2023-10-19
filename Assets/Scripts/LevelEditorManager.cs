using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelEditorManager : MonoBehaviour
{
  public ItemController[] m_itemButtons;
  public GameObject[] m_itemPrefabs;
  public GameObject[] m_itemSprite;
  public int m_currentButtonID;
  public GameObject m_optionsCanvas;

  [Header("Colors")]
  public Material m_red;
  public Material m_yellow;
  public Material m_green;
  public Material m_cyan;
  public Material m_blue;
  public Material m_magenta;
  public Material m_white;

  public GameObject m_itemID;
  public bool m_editClicked = false;

  private void Update()
  {
    Vector2 screenPosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
    Vector2 worldPosition = Camera.main.ScreenToWorldPoint(screenPosition);

    if (Input.GetMouseButtonDown(0) && m_itemButtons[m_currentButtonID].m_isClicked)
    {
      InstantiatePrefab(worldPosition);
      m_itemButtons[m_currentButtonID].m_isClicked = false;
    }
    else if (Input.GetMouseButtonDown(0) && m_editClicked)
    {
      InstantiatePrefab(worldPosition);
      m_editClicked = false;
    }
  }
  public void InstantiatePrefab(Vector3 worldPosition)
  {
    var obj = Instantiate(m_itemPrefabs[m_currentButtonID], new Vector3(worldPosition.x, worldPosition.y, 0), FindObjectOfType<SpriteFollowMouse>().transform.rotation);
    obj.transform.localScale = FindObjectOfType<SpriteFollowMouse>().transform.localScale;
    Destroy(GameObject.FindGameObjectWithTag("ItemSprite"));
  }
  public void ChangeColor(int ID)
  {
    var render = m_itemID.GetComponent<Renderer>();
    if (ID == 0)
    {
      gameObject.layer = LayerMask.NameToLayer("Red");
      render.gameObject.layer = LayerMask.NameToLayer("Red");
      render.material = m_red;
    }
    if (ID == 1)
    {
      gameObject.layer = LayerMask.NameToLayer("Yellow");
      render.gameObject.layer = LayerMask.NameToLayer("Yellow");
      render.material = m_yellow;
    }
    if (ID == 2)
    {
      gameObject.layer = LayerMask.NameToLayer("Green");
      render.gameObject.layer = LayerMask.NameToLayer("Green");
      render.material = m_green;
    }
    if (ID == 3)
    {
      gameObject.layer = LayerMask.NameToLayer("Cyan");
      render.gameObject.layer = LayerMask.NameToLayer("Cyan");
      render.material = m_cyan;
    }
    if (ID == 4)
    {
      gameObject.layer = LayerMask.NameToLayer("Blue");
      render.gameObject.layer = LayerMask.NameToLayer("Blue");
      render.material = m_blue;
    }
    if (ID == 5)
    {
      gameObject.layer = LayerMask.NameToLayer("Magenta");
      render.gameObject.layer = LayerMask.NameToLayer("Magenta");
      render.material = m_magenta;
    }
    if (ID == 6)
    {
      gameObject.layer = LayerMask.NameToLayer("White");
      render.gameObject.layer = LayerMask.NameToLayer("White");
      render.material = m_white;
    }
  }
}
