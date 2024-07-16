using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.Analytics;
using UnityEngine.SceneManagement;

public class EditorManager : MonoBehaviour
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
    public GameObject m_HUDCanvas;
    public GameObject m_pauseCanvas;
    public Button m_playButton;

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

    public EditorItemManager m_item;
    public EditorSpriteFollow m_spriteFollow;

    private void Start()
    {
        Time.timeScale = 1.0f;
    }
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
