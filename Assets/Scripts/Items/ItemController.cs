using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ItemController : MonoBehaviour
{
  public int m_ID;
  public int m_quantity;
  public TextMeshProUGUI m_quantityText;
  public bool m_isClicked = false;
  private LevelEditorManager m_levelEditor;
  public SpriteFollowMouse m_itemSprite;

  // Start is called before the first frame update
  void Start()
  {
    m_quantityText.text = m_quantity.ToString();
    m_levelEditor = GameObject.FindGameObjectWithTag("LevelEditorManager").GetComponent<LevelEditorManager>();
  }
  public void OnClick()
  {
    if (m_quantity > 0)
    {
      Vector2 screenPosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
      Vector2 worldPosition = Camera.main.ScreenToWorldPoint(screenPosition);
      m_isClicked = true;
      Instantiate(m_levelEditor.m_itemSprite[m_ID], new Vector3(worldPosition.x, worldPosition.y, 0), Quaternion.identity);
      m_quantity--;
      m_quantityText.text = m_quantity.ToString();
      m_levelEditor.m_currentButtonID = m_ID;
      m_levelEditor.m_HUDCanvas.SetActive(false);
      m_levelEditor.m_controlCanvas.SetActive(true);
    }
  }
}
