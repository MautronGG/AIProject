using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;

public class LevelEditorManager : MonoBehaviour
{
  public ItemController[] m_itemButtons;
  public GameObject[] m_itemPrefabs;
  public GameObject[] m_itemSprite;
  public int m_currentButtonID;
  public GameObject m_optionsCanvas;
  public GameObject m_colorCanvas;
  public GameObject m_HUDCanvas;
  public GameObject m_winCanvas;
  public GameObject m_gameOverCanvas;
  public GameObject m_controlCanvas;
  public GameObject m_pleaseAssignColorCanvas;
  public GameObject m_cantRepeatColorCanvas;

  public TextMeshProUGUI m_points;

  public List<ItemManager> m_itemsList;
  public List<Material> m_materialsBridgeArray;

  [Header("Colors")]
  public Material m_red;
  public Material m_yellow;
  public Material m_green;
  public Material m_cyan;
  public Material m_blue;
  public Material m_magenta;
  public Material m_white;
  public Material m_black;

  //public bool m_isRed;
  //public bool m_isYellow;
  //public bool m_isGreen;
  //public bool m_isCyan;
  //public bool m_isBlue;
  //public bool m_isMagenta;
  //public bool m_isWhite;

  public UnityEvent m_playEvents;
  public GameObject m_itemID;
  public bool m_editClicked = false;

  public int m_reachedGoals = 0;
  public int m_finishedBoids = 0;

  public int m_personalID = 0;

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
      //InstantiatePrefab(worldPosition);
      var item = m_itemsList[m_itemID.GetComponent<ItemManager>().m_personalID];
      item.gameObject.SetActive(true);
      m_editClicked = false;
      var sprite = item.GetComponent<ItemManager>().m_mySprite;
      ChangePrefab(item.gameObject, sprite.GetComponent<SpriteFollowMouse>(), worldPosition);
      sprite.SetActive(false);
    }

    if (m_finishedBoids == 3)
    {
      Camera.main.GetComponent<CameraMovement>().m_canMove = false;
      if (m_reachedGoals > 0)
      {
        m_HUDCanvas.SetActive(false);
        m_winCanvas.SetActive(true);
        m_points.text = "Points " + m_reachedGoals + "/3";
      }
      else
      {
        m_HUDCanvas.SetActive(false);
        m_gameOverCanvas.SetActive(true);
      }
    }
  }
  public void InstantiatePrefab(Vector3 worldPosition)
  {
    var sprite = FindObjectOfType<SpriteFollowMouse>();
    var obj = Instantiate(m_itemPrefabs[m_currentButtonID], new Vector3(worldPosition.x, worldPosition.y, 0), sprite.transform.rotation);
    var script = obj.GetComponent<ItemManager>();
    m_itemsList.Add(script);
    script.m_mySprite = sprite.gameObject;
    script.m_mySprite.SetActive(false);
    script.m_personalID = m_personalID;
    m_personalID++;
    ChangePrefab(obj, sprite, worldPosition);
    //Destroy(GameObject.FindGameObjectWithTag("ItemSprite"));
    //obj.GetComponent<Renderer>().material = material;
  }
  public void ChangePrefab(GameObject obj, SpriteFollowMouse sprite, Vector3 worldPosition)
  {
    obj.transform.localScale = sprite.transform.localScale;
    ChangeSpriteColor(sprite.m_colorID, obj);
    m_HUDCanvas.SetActive(true);    
    m_controlCanvas.SetActive(false);
    obj.transform.position = new Vector3(worldPosition.x, worldPosition.y, 0);
    obj.transform.rotation = sprite.transform.rotation;
  }
  public void ChangeColor(int ID)
  {
    //var tempID = ID;
    var item = m_itemID.GetComponent<ItemManager>();
    if (item.m_changedColor && item.m_colorID != ID)
    {
      RemoveColor(item.m_colorID);
    }

    item.m_colorID = ID;
    var render = m_itemID.GetComponent<Renderer>();
    
    //if (item.m_changedColor)
    //{
    //  ID = item.m_colorID;
    //}

    if (ID == 0)
    {

      if (m_currentButtonID == 0)
      {
        if (!m_materialsBridgeArray.Contains(m_red))
        {
          m_materialsBridgeArray.Add(m_red);
        }
        else
        {
          CantChangeColor();
          Debug.Log("Cant change color");
          return;
        }
      }
      gameObject.layer = LayerMask.NameToLayer("Red");
      render.gameObject.layer = LayerMask.NameToLayer("Red");
      render.material = m_red;
      
      //m_isRed = true;
    }
    if (ID == 1)
    {

      if (m_currentButtonID == 0)
      {
        if (!m_materialsBridgeArray.Contains(m_yellow))
        {
          m_materialsBridgeArray.Add(m_yellow);
        }
        else
        {
          CantChangeColor();
          Debug.Log("Cant change color");
          return;
        }
      }
      gameObject.layer = LayerMask.NameToLayer("Yellow");
      render.gameObject.layer = LayerMask.NameToLayer("Yellow");
      render.material = m_yellow;
      
      //m_isYellow = true;
    }
    if (ID == 2)
    {

      if (m_currentButtonID == 0)
      {
        if (!m_materialsBridgeArray.Contains(m_green))
        {
          m_materialsBridgeArray.Add(m_green);
        }
        else
        {
          CantChangeColor();
          Debug.Log("Cant change color");
          return;
        }
      }
      gameObject.layer = LayerMask.NameToLayer("Green");
      render.gameObject.layer = LayerMask.NameToLayer("Green");
      render.material = m_green;
      
      //m_isGreen = true;
    }
    if (ID == 3)
    {

      if (m_currentButtonID == 0)
      {
        if (!m_materialsBridgeArray.Contains(m_cyan))
        {
          m_materialsBridgeArray.Add(m_cyan);
        }
        else
        {
          CantChangeColor();
          Debug.Log("Cant change color");
          return;
        }
      }
      gameObject.layer = LayerMask.NameToLayer("Cyan");
      render.gameObject.layer = LayerMask.NameToLayer("Cyan");
      render.material = m_cyan;
      
      //m_isCyan = true;
    }
    if (ID == 4)
    {

      if (m_currentButtonID == 0)
      {
        if (!m_materialsBridgeArray.Contains(m_blue))
        {
          m_materialsBridgeArray.Add(m_blue);
        }
        else
        {
          CantChangeColor();
          Debug.Log("Cant change color");
          return;
        }
      }
      gameObject.layer = LayerMask.NameToLayer("Blue");
      render.gameObject.layer = LayerMask.NameToLayer("Blue");
      render.material = m_blue;
      
      //m_isBlue = true;
    }
    if (ID == 5)
    {
      if (m_currentButtonID == 0)
      {
        if (!m_materialsBridgeArray.Contains(m_magenta))
        {
          m_materialsBridgeArray.Add(m_magenta);
        }
        else
        {
          CantChangeColor();
          Debug.Log("Cant change color");
          return;
        }
      }
      gameObject.layer = LayerMask.NameToLayer("Magenta");
      render.gameObject.layer = LayerMask.NameToLayer("Magenta");
      render.material = m_magenta;  
      //m_isMagenta = true;
    }
    if (ID == 6)
    {
      if (m_currentButtonID == 0)
      {
        if (!m_materialsBridgeArray.Contains(m_white))
        {
          m_materialsBridgeArray.Add(m_white);
        }
        else
        {
          CantChangeColor();
          Debug.Log("Cant change color");
          return;
        }
      }

      gameObject.layer = LayerMask.NameToLayer("White");
      render.gameObject.layer = LayerMask.NameToLayer("White");
      render.material = m_white;
      
      //m_isWhite = true;
    }
    item.m_changedColor = true;
  }

  public void RemoveColor(int ID)
  {
    if (m_currentButtonID == 0)
    {
      if (ID == 0)
      {
        if (m_materialsBridgeArray.Contains(m_red))
        {
          m_materialsBridgeArray.Remove(m_red);
        }
      }
      if (ID == 1)
      {
        if (m_materialsBridgeArray.Contains(m_yellow))
        {
          m_materialsBridgeArray.Remove(m_yellow);
        }
      }
      if (ID == 2)
      {
        if (m_materialsBridgeArray.Contains(m_green))
        {
          m_materialsBridgeArray.Remove(m_green);
        }
      }
      if (ID == 3)
      {
        if (m_materialsBridgeArray.Contains(m_cyan))
        {
          m_materialsBridgeArray.Remove(m_cyan);
        }
      }
      if (ID == 4)
      {
        if (m_materialsBridgeArray.Contains(m_blue))
        {
          m_materialsBridgeArray.Remove(m_blue);
        }
      }
      if (ID == 5)
      {
        if (m_materialsBridgeArray.Contains(m_magenta))
        {
          m_materialsBridgeArray.Remove(m_magenta);
        }
      }
      if (ID == 6)
      {
        if (m_materialsBridgeArray.Contains(m_white))
        {
          m_materialsBridgeArray.Remove(m_white);
        }
      }
    }
  }
  public void ChangeSpriteColor(int ID, GameObject obj)
  {
    var render = obj.GetComponent<Renderer>();
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
    if (ID == 7)
    {
      gameObject.layer = LayerMask.NameToLayer("Black");
      render.gameObject.layer = LayerMask.NameToLayer("Black");
      render.material = m_black;
    }

  }
  public void PlayEvents()
  {
    var canPlay = true;
    foreach (ItemManager obj in m_itemsList)
    {
      if (obj.m_changedColor)
      {
        continue;
      }
      else
      {
        canPlay = false;
        break;
      }
    }
    if (canPlay)
    {
      m_playEvents.Invoke();
    }
    else
    {
      m_pleaseAssignColorCanvas.SetActive(true);
    }
  }
  public void ResetDefaults()
  {
    m_reachedGoals = 0;
    m_finishedBoids = 0;
    m_editClicked = false;
}
  public void CantChangeColor()
  {
    m_cantRepeatColorCanvas.SetActive(true);
  }
}
