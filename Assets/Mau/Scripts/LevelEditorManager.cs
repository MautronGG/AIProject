using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.Analytics;
using UnityEngine.SceneManagement;

public class LevelEditorManager : MonoBehaviour
{
  public int m_currentButtonID;
  public bool m_canRepeatColor = false;

  [Header("Arrays")]
  public ItemController[] m_itemButtons;
  public GameObject[] m_itemPrefabs;
  public GameObject[] m_itemSprite;

  [Header("Graffiti")]
  public GameObject[] m_graffitiArray;
  public int m_graffiti;
  public GameObject[] m_wallArray;

  [Header("Canvas")]
  public GameObject m_optionsCanvas;
  public GameObject m_colorCanvas;
  public GameObject m_HUDCanvas;
  public GameObject m_winCanvas;
  public GameObject m_gameOverCanvas;
  public GameObject m_controlCanvas;
  public GameObject m_pleaseAssignColorCanvas;
  public GameObject m_cantRepeatColorCanvas;
  public GameObject m_pauseCanvas;
  public Button m_playButton;
  public Button m_bridgeButton;

  public TextMeshProUGUI m_points;

  [Header("Lists")]
  public List<ItemManager> m_itemsList;
  public List<ItemManager> m_bombsList;
  public List<ItemManager> m_enemyList;
  public List<Material> m_materialsBridgeArray;
  public List<Material> m_materialsPortalArray;
  public List<Material> m_materialsBombArray;
  public List<Material> m_materialsEnemyArray;

  [Header("Colors")]
  public Material m_red;
  public Material m_yellow;
  public Material m_green;
  public Material m_cyan;
  public Material m_blue;
  public Material m_magenta;
  public Material m_white;
  public Material m_black;

  public UnityEvent m_playEvents;
  public UnityEvent m_restartEvents;
  public GameObject m_itemID;
  public bool m_editClicked = false;

  public int m_reachedGoals = 0;
  public int m_finishedBoids = 0;

  public int m_personalID = 0;
  public bool m_canPlay;

  public bool m_resettingDeafults = false;
  float m_resetTime = 0f;
  int m_scene;
  public bool m_pause = false;

  public bool m_isEditing = false;

  public bool m_canWin = true;
  private void Start()
  {
    Time.timeScale = 1.0f;
    SpecialEvent(0);
  }
  private void Update()
  {
    if (m_resettingDeafults)
    {
      m_resetTime += Time.deltaTime;
      if (m_resetTime >= 0.5f)
      {
        m_resettingDeafults = false;
        m_resetTime = 0f;
      }
    }
    if (Input.GetKeyDown(KeyCode.Escape) && !m_pause && !m_isEditing)
    {
      Time.timeScale = 0.0f;
      m_pauseCanvas.SetActive(true);
      m_pause = true;
      m_HUDCanvas.SetActive(false);
    }

    Vector2 screenPosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
    Vector2 worldPosition = Camera.main.ScreenToWorldPoint(screenPosition);

    if (Input.GetMouseButtonDown(0) && m_itemButtons[m_currentButtonID].m_isClicked)
    {
            BridgeScript.BridgeActivate = false;
      InstantiatePrefab(worldPosition);
      m_itemButtons[m_currentButtonID].m_isClicked = false;
      m_isEditing = false;
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
      if (m_canWin)
      {
        if (m_reachedGoals > 0)
        {
          m_HUDCanvas.SetActive(false);
          m_winCanvas.SetActive(true);
          m_points.text = "Points " + m_reachedGoals + "/3";
          if (SceneManager.GetActiveScene().name == "MainMenu")
          {
            Analytics.CustomEvent("TutorialFinished");
          }
          
        }
        else
        {
          m_HUDCanvas.SetActive(false);
          m_gameOverCanvas.SetActive(true);
        }
      }
      else
      {
        if (m_reachedGoals < 3)
        {
          m_HUDCanvas.SetActive(false);
          m_gameOverCanvas.SetActive(true);
        }
        else
        {
          ResetDefaults();
          m_restartEvents.Invoke();
          m_wallArray[m_graffiti].SetActive(false);
          m_graffiti++;
          SpecialEvent(m_graffiti);
        }
      }
    }
  }
  public void SpecialEvent(int graffiti)
  {
    switch (graffiti)
    {
      case 0:
        m_graffitiArray[graffiti].SetActive(true);
        break;
      case 1:
        m_graffitiArray[graffiti].SetActive(true);
        break;
      case 2:
        m_graffitiArray[graffiti].SetActive(true);
        break;
      case 3:
        m_graffitiArray[graffiti].SetActive(true);
        break;
      case 4:
        m_graffitiArray[graffiti].SetActive(true);
        break;
      case 5:
        m_graffitiArray[graffiti].SetActive(true);
        break;
      case 6:
        m_graffitiArray[graffiti].SetActive(true);
        break;
      case 7:
        m_graffitiArray[graffiti].SetActive(true);
        break;
      case 8:
        m_graffitiArray[graffiti].SetActive(true);
        break;
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
    List<Material> array = m_materialsBridgeArray;
    var item = m_itemID.GetComponent<ItemManager>();
    var canChangeColor = true;
    var diffColors = false;
    var prevColor = ID;
    var layer = "Black";
    var material = m_black;
    if (item.m_colorID != ID)
    {
      prevColor = item.m_colorID;
      diffColors = true;
    }
    var render = m_itemID.GetComponent<Renderer>();
    var exitPortal = render;
    var bombWall = render;

    //if (item.m_changedColor)
    //{
    //  ID = item.m_colorID;
    //}

    if (item.m_ID == 0)
    {
      array = m_materialsBridgeArray;
    }
    else if (item.m_ID == 1)
    {
      array = m_materialsPortalArray;
      exitPortal = item.gameObject.GetComponent<PortalScript>().m_exitPortal.gameObject.GetComponent<Renderer>();
    }
    else if (item.m_ID == 2)
    {
      array = m_materialsBombArray;
      bombWall = item.gameObject.GetComponent<BombScript>().m_wall.gameObject.GetComponent<Renderer>();
    }
    else if (item.m_ID == 3)
    {
      array = m_materialsEnemyArray;
    }
    if (ID == 0)
    {
      if (!m_canRepeatColor)
      {
        if (!array.Contains(m_red))
        {
          array.Add(m_red);
        }
        else
        {
          CantChangeColor();
          canChangeColor = false;
          Debug.Log("Cant change color");
          return;
        }

      }
      material = m_red;
      layer = "Red";

      //m_isRed = true;
    }
    if (ID == 1)
    {
      if (!m_canRepeatColor)
      {
        if (!array.Contains(m_yellow))
        {
          array.Add(m_yellow);
        }
        else
        {
          CantChangeColor();
          canChangeColor = false;
          Debug.Log("Cant change color");
          return;

        }

      }
      material = m_yellow;
      layer = "Yellow";

      //m_isYellow = true;
    }
    if (ID == 2)
    {
      if (!m_canRepeatColor)
      {
        if (!array.Contains(m_green))
        {
          array.Add(m_green);
        }
        else
        {
          CantChangeColor();
          canChangeColor = false;
          Debug.Log("Cant change color");
          return;
        }
      }
      material = m_green;
      layer = "Green";

      //m_isGreen = true;
    }
    if (ID == 3)
    {
      if (!m_canRepeatColor)
      {
        if (!array.Contains(m_cyan))
        {
          array.Add(m_cyan);
        }
        else
        {
          CantChangeColor();
          canChangeColor = false;
          Debug.Log("Cant change color");
          return;
        }
      }
      material = m_cyan;
      layer = "Cyan";

      //m_isCyan = true;
    }
    if (ID == 4)
    {
      if (!m_canRepeatColor)
      {
        if (!array.Contains(m_blue))
        {
          array.Add(m_blue);
        }
        else
        {
          CantChangeColor();
          canChangeColor = false;
          Debug.Log("Cant change color");
          return;
        }
      }
      material = m_blue;
      layer = "Blue";

      //m_isBlue = true;
    }
    if (ID == 5)
    {
      if (!m_canRepeatColor)
      {
        if (!array.Contains(m_magenta))
        {
          array.Add(m_magenta);
        }
        else
        {
          CantChangeColor();
          canChangeColor = false;
          Debug.Log("Cant change color");
          return;
        }
      }
      material = m_magenta;
      layer = "Magenta";

      //m_isMagenta = true;
    }
    if (ID == 6)
    {
      if (!m_canRepeatColor)
      {
        if (!array.Contains(m_white))
        {
          array.Add(m_white);
        }
        else
        {
          CantChangeColor();
          canChangeColor = false;
          Debug.Log("Cant change color");
          return;
        }
      }
      material = m_white;
      layer = "White";
      //m_isWhite = true;
    }
    if (ID == 7)
    {

      material = m_black;
      layer = "Black";

      //m_isGreen = true;
    }
    if (canChangeColor)
    {
      item.m_colorID = ID;
      bombWall.GetComponent<ItemManager>().m_colorID = ID;
      exitPortal.GetComponent<ItemManager>().m_colorID = ID;
      render.material = material;
      render.gameObject.layer = LayerMask.NameToLayer(layer);
      exitPortal.material = material;
      exitPortal.gameObject.layer = LayerMask.NameToLayer(layer);
      bombWall.material = material;
    }
    if (item.m_changedColor && canChangeColor && diffColors && !m_canRepeatColor)
    {
      RemoveColor(prevColor, item.m_ID);
    }
    item.m_changedColor = true;
    bombWall.GetComponent<ItemManager>().m_changedColor = true;
    exitPortal.GetComponent<ItemManager>().m_changedColor = true;
  }

  public void RemoveColor(int colorID, int itemID)
  {
    var array = m_materialsBridgeArray;
    if (itemID == 1)
    {
      array = m_materialsPortalArray;
    }
    else if (itemID == 2)
    {
      array = m_materialsBombArray;
    }
    else if (itemID == 3)
    {
      array = m_materialsEnemyArray;
    }
    if (colorID == 0)
    {
      if (array.Contains(m_red))
      {
        array.Remove(m_red);
      }
    }
    if (colorID == 1)
    {
      if (array.Contains(m_yellow))
      {
        array.Remove(m_yellow);
      }
    }
    if (colorID == 2)
    {
      if (array.Contains(m_green))
      {
        array.Remove(m_green);
      }
    }
    if (colorID == 3)
    {
      if (array.Contains(m_cyan))
      {
        array.Remove(m_cyan);
      }
    }
    if (colorID == 4)
    {
      if (array.Contains(m_blue))
      {
        array.Remove(m_blue);
      }
    }
    if (colorID == 5)
    {
      if (array.Contains(m_magenta))
      {
        array.Remove(m_magenta);
      }
    }
    if (colorID == 6)
    {
      if (array.Contains(m_white))
      {
        array.Remove(m_white);
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
    m_canPlay = true;
    foreach (ItemManager obj in m_itemsList)
    {
      if (obj != null)
      {
        if (obj.m_changedColor)
        {
          continue;
        }
        else
        {
          m_canPlay = false;
          m_pleaseAssignColorCanvas.SetActive(true);
          break;
        }
      }
    }
  }
  public void ResetDefaults()
  {
    m_resettingDeafults = true;
    var camera = Camera.main.GetComponent<CameraMovement>();
    camera.m_canMove = true;
    camera.m_autoMove = false;
    m_reachedGoals = 0;
    m_finishedBoids = 0;
    m_editClicked = false;
    m_canPlay = false;
    foreach (ItemManager bomb in m_bombsList)
    {
      if (!bomb.gameObject.activeInHierarchy)
      {
        bomb.gameObject.SetActive(true);
      }
      else
      {
        bomb.ResetDeafualts();
      }
    }
    foreach (ItemManager enemy in m_enemyList)
    {
      if (!enemy.gameObject.activeInHierarchy)
      {
        enemy.gameObject.SetActive(true);
      }
      else
      {
        enemy.ResetDeafualts();
      }
    }
  }
  public void CantChangeColor()
  {
    m_cantRepeatColorCanvas.SetActive(true);
  }
  public void UnPause()
  {
    Time.timeScale = 1f;
    m_pause = false;
    m_HUDCanvas.SetActive(true);
    m_pauseCanvas.SetActive(false);
  }
}
