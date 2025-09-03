using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.Analytics;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;

public class EditorManager : MonoBehaviour
{
    [Header("Canvas")]
    public GameObject m_optionsCanvas;
    public GameObject m_HUDCanvas;
    public GameObject m_pauseCanvas;
    public Button m_playButton;
    //public GameObject m_typeButtons;
    //public GameObject m_ObjectsButtons;

    public ButtonSelectionTracker[] m_buttonSelectionTrackers;

    public bool m_pause = false;

    public bool m_isEditing = false;

    public EditorGridManager m_grid;
    public EditorItem m_item;
    public EditorSpriteFollow m_spriteFollow;

    public List<GameObject> m_editorItemPrefabs = new List<GameObject>();

    private void Awake()
    {
        Time.timeScale = 1.0f;
        m_buttonSelectionTrackers = m_HUDCanvas.GetComponentsInChildren<ButtonSelectionTracker>();  
    }
   //private void Start()
   //{
   //    //if (m_typeButtons)
   //    //{
   //    //    m_typeButtons.SetActive(false);
   //    //}
   //    //if (m_ObjectsButtons)
   //    //{
   //    //    m_ObjectsButtons.SetActive(false);
   //    //}
   //}
    private void Update()
    {
        ///To Pause Game
        if (Input.GetKeyDown(KeyCode.Escape) && !m_pause && !m_isEditing)
        {
            Time.timeScale = 0.0f;
            m_pauseCanvas.SetActive(true);
            m_pause = true;
            m_HUDCanvas.SetActive(false);
        }
        ///To move Sprite when placing in Level
        Vector2 screenPosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        Vector2 worldPosition = Camera.main.ScreenToWorldPoint(screenPosition);
    }

    public void UnPause()
    {
        Time.timeScale = 1f;
        m_pause = false;
        m_HUDCanvas.SetActive(true);
        m_pauseCanvas.SetActive(false);
    }

    
}
