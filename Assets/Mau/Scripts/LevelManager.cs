using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.Analytics;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    //public List<string> m_objectsList;

    //public int m_currentButtonID;
    //public int m_personalID = 0;
    //
    //[Header("Arrays")]
    //public EditorItem[] m_itemButtons;
    //public GameObject[] m_itemPrefabs;
    ////public GameObject[] m_itemSprite;

    [Header("Canvas")]
    public GameObject m_optionsCanvas;
    public GameObject m_HUDBuildCanvas;
    public GameObject m_HUDPlayCanvas;
    public GameObject m_pauseCanvas;
    public GameObject m_colorCanvas;
    public GameObject m_winCanvas;
    public GameObject m_gameOverCanvas;
    public GameObject m_controlCanvas;
    public GameObject m_pleaseAssignColorCanvas;
    public GameObject m_cantRepeatColorCanvas;
    public Button m_playButton;
    public Button m_resetButton;
    public Button m_bridgeButton;
    public Button m_cancelEditButton;
    public TextMeshProUGUI m_points;

    public GameObject m_currentStateCanvas;

    [Header("Lists")]
    [SerializeField] public Object_Parent[] m_objects;
    [SerializeField] public List<Object_Bridge> m_bridges;
    [SerializeField] public Object_FinalDoor[] m_doors;
    public GameObject[] m_voidsList;
    //public List<ItemManager> m_itemsList;
    //public List<ItemManager> m_bombsList;
    //public List<ItemManager> m_enemyList;
    //public List<Material> m_materialsBridgeArray;
    //public List<Material> m_materialsPortalArray;
    //public List<Material> m_materialsBombArray;
    //public List<Material> m_materialsEnemyArray;

    public bool m_pause = false;

    public bool m_isEditing = false;

    public Object_Bridge m_item;
    public SpriteFollow m_spriteFollow;

    public int m_reachedGoals = 0;
    public int m_playerEnded = 0;

    FSM m_myFSM;

    public MinionMovement m_Red;
    public MinionMovement m_Green;
    public MinionMovement m_Blue;

    public bool m_canPlay = false;
    public UnityEvent m_playEvents;
    public UnityEvent m_restartEvents;

    public CameraMovement m_camera;

    public bool m_doorsLocked = false;

    private void Start()
    {
        Time.timeScale = 1.0f;
        m_myFSM = GetComponent<FSM>();
        m_objects = FindObjectsOfType<Object_Parent>();
        m_doors = FindObjectsOfType<Object_FinalDoor>();
        Initialized();
        foreach (Object_Parent obj in m_objects)
        {
            if (obj.actualColor != ColorEnum.Black)
            {
                int a = ((int)obj.m_object);
                int b = ((int)obj.actualColor);
                if (obj.actualColor == ColorEnum.White)
                {
                    GetComponent<FixColorManager>().m_listSpriteObjects[a].m_activeWhite = false;
                }
                else if (obj.actualColor == ColorEnum.Red)
                {
                    GetComponent<FixColorManager>().m_listSpriteObjects[a].m_activeRed = false;
                }
                else if (obj.actualColor == ColorEnum.Yellow)
                {
                    GetComponent<FixColorManager>().m_listSpriteObjects[a].m_activeYellow = false;
                }
                else if (obj.actualColor == ColorEnum.Green)
                {
                    GetComponent<FixColorManager>().m_listSpriteObjects[a].m_activeGreen = false;
                }
                else if (obj.actualColor == ColorEnum.Cyan)
                {
                    GetComponent<FixColorManager>().m_listSpriteObjects[a].m_activeCyan = false;
                }
                else if (obj.actualColor == ColorEnum.Blue)
                {
                    GetComponent<FixColorManager>().m_listSpriteObjects[a].m_activeBlue = false;
                }
                else if (obj.actualColor == ColorEnum.Magenta)
                {
                    GetComponent<FixColorManager>().m_listSpriteObjects[a].m_activeMagenta = false;
                }
            }
        }
    }

    private void Update()
    {
        ///To Pause Game
        if (Input.GetKeyDown(KeyCode.Escape) && !m_pause && !m_isEditing)
        {
            Pause();
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && m_pause)
        {
            UnPause();
        }
        if (m_playerEnded == 3)
        {
            m_currentStateCanvas.SetActive(false);
            m_camera.ChangeMovement(false);
            if (m_reachedGoals > 0)
            {
                m_winCanvas.SetActive(true);
                m_points.text = "Points " + m_reachedGoals + "/3";
            }
            else
            {
                m_gameOverCanvas.SetActive(true);
            }
        }

        ///To move Sprite when placing in Level
        Vector2 screenPosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        Vector2 worldPosition = Camera.main.ScreenToWorldPoint(screenPosition);
    }
    public void Pause()
    {
        Time.timeScale = 0.0f;
        m_pauseCanvas.SetActive(true);
        m_pause = true;
        m_currentStateCanvas.SetActive(false);
    }
    public void UnPause()
    {
        Time.timeScale = 1f;
        m_pause = false;
        m_currentStateCanvas.SetActive(true);
        m_pauseCanvas.SetActive(false);
    }
    protected void Initialized()
    {
        m_playEvents.AddListener(() =>
        {
            m_Red.EnableMovement(true);
            m_Green.EnableMovement(true);
            m_Blue.EnableMovement(true);
            m_HUDBuildCanvas.SetActive(false);
            m_HUDPlayCanvas.SetActive(true);
            m_camera.ChangeMovement(false);
            m_camera.AutomaticMovement(true);
            foreach (Object_Parent obj in m_objects)
            {
                obj.StartObject();
            }
        });
        m_playButton.onClick.AddListener(() =>
        {
            m_myFSM.SetState(m_myFSM.m_onPlayState);
        });

        m_resetButton.onClick.AddListener(() =>
        {
            m_myFSM.SetState(m_myFSM.m_onEditorState);
        });
        m_restartEvents.AddListener(() =>
        {
            m_Red.ResetTransform();
            m_Green.ResetTransform();
            m_Blue.ResetTransform();
            m_HUDBuildCanvas.SetActive(true);
            m_HUDPlayCanvas.SetActive(false);
            m_camera.ChangeMovement(true);
            m_camera.AutomaticMovement(false);
            m_camera.ResetTransform();
            ResetDefaults();
            foreach (Object_Parent _object in m_objects)
            {
                _object.ResetDeafualts();
            }
        }); 
        m_cancelEditButton.onClick.AddListener(() =>
        {
            m_currentStateCanvas.SetActive(true);
        });
    }
    public void ResetDefaults()
    {
        m_reachedGoals = 0;
        m_playerEnded = 0;
        m_canPlay = false;
    }

    public void CantChangeColor()
    {
        m_cantRepeatColorCanvas.SetActive(true);
    }
    public void CheckColors()
    {
        m_doorsLocked = false;
        foreach (Object_Parent obj in m_objects)
        {
            if (obj.actualColor == ColorEnum.Black)
            {
                m_doorsLocked = true;
                break;
            }
        }   
        if (m_doorsLocked == false)
        {
            foreach (Object_Bridge obj in m_bridges)
            {
                if (obj.actualColor == ColorEnum.Black)
                {
                    m_doorsLocked = true;
                    break;
                }
            }
        }
        foreach (Object_FinalDoor door in m_doors)
        {
            if (m_doorsLocked == false)
            {
                door.ChangeSprite(true);
            }
            else
            {
                door.ChangeSprite(false);
            }
        }
    }
}
