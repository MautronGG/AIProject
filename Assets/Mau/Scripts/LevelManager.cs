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

    //[Header("Lists")]
    //public List<ItemManager> m_itemsList;
    //public List<ItemManager> m_bombsList;
    //public List<ItemManager> m_enemyList;
    //public List<Material> m_materialsBridgeArray;
    //public List<Material> m_materialsPortalArray;
    //public List<Material> m_materialsBombArray;
    //public List<Material> m_materialsEnemyArray;

    //[Header("Colors")]
    //public Material m_red;
    //public Material m_yellow;
    //public Material m_green;
    //public Material m_cyan;
    //public Material m_blue;
    //public Material m_magenta;
    //public Material m_white;
    //public Material m_black;

    public bool m_pause = false;

    public bool m_isEditing = false;

    public NewBridgeScript m_item;
    public SpriteFollow m_spriteFollow;

    public int m_reachedGoals = 0;
    public int m_playerEnded = 0;

    FSM m_myFSM;

    public SphereScript m_Red;
    public SphereScript m_Green;
    public SphereScript m_Blue;

    public bool m_canPlay = false;
    public UnityEvent m_playEvents;
    public UnityEvent m_restartEvents;

    public CameraMovement m_camera;
    [SerializeField] GameObject m_object;
    private void Awake()
    {

    }
    private void Start()
    {
        Time.timeScale = 1.0f;
        m_myFSM = GetComponent<FSM>();

        Initialized();
    }

    private void Update()
    {
        ///To Pause Game
        if (Input.GetKeyDown(KeyCode.Escape) && !m_pause && !m_isEditing)
        {
            Pause();
        }
        if (Input.GetKeyDown(KeyCode.Escape) && m_pause)
        {
            UnPause();
        }
        if (m_playerEnded == 3)
        {
            if (m_reachedGoals > 0)
            {
                m_winCanvas.SetActive(true);
            }
            else
            {
                m_gameOverCanvas.SetActive(true);
            }
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            m_myFSM.SetState(m_myFSM.m_onPlayState);
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            m_myFSM.SetState(m_myFSM.m_onEditorState);
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            Vector2 screenPosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            Vector2 worldPosition = Camera.main.ScreenToWorldPoint(screenPosition);
            //m_isClicked = true;
            var obj = Instantiate(m_object, new Vector3(worldPosition.x, worldPosition.y, 0), Quaternion.identity);
            m_spriteFollow = obj.GetComponent<SpriteFollow>();
            //m_obj = Instantiate(m_editor.m_itemPrefabs[(int)m_ID], new Vector3(worldPosition.x, worldPosition.y, 0), Quaternion.identity);
            m_spriteFollow.StartFollow();
        }
        ///To move Sprite when placing in Level
       //Vector2 screenPosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
       //Vector2 worldPosition = Camera.main.ScreenToWorldPoint(screenPosition);
    }
    public void Pause()
    {
        Time.timeScale = 0.0f;
        m_pauseCanvas.SetActive(true);
        m_pause = true;

        if (m_myFSM.m_currentState == m_myFSM.m_onEditorState)
        {
            m_HUDBuildCanvas.SetActive(false);
        }
        if (m_myFSM.m_currentState == m_myFSM.m_onPlayState)
        {
            m_HUDPlayCanvas.SetActive(false);
        }
    }
    public void UnPause()
    {
        Time.timeScale = 1f;
        m_pause = false;
        if (m_myFSM.m_currentState == m_myFSM.m_onEditorState)
        {
            m_HUDBuildCanvas.SetActive(true);
        }
        if (m_myFSM.m_currentState == m_myFSM.m_onPlayState)
        {
            m_HUDPlayCanvas.SetActive(true);
        }
        m_pauseCanvas.SetActive(false);
    }
    protected void Initialized()
    {
        m_playEvents.AddListener(() =>
        {
            m_Red.EnableMovement(true);
            m_Green.EnableMovement(true);
            m_Blue.EnableMovement(true);
            //m_HUDBuildCanvas.SetActive(false);
            //m_HUDPlayCanvas.SetActive(true);
            //m_camera.ChangeMovement(false);
            m_camera.AutomaticMovement(true);
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
            //m_HUDBuildCanvas.SetActive(true);
            //m_HUDPlayCanvas.SetActive(false);
            m_camera.ChangeMovement(true);
            m_camera.AutomaticMovement(false);
            //m_camera.ResetTransform();
            //ResetDefaults();
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
}
