using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemScript : MonoBehaviour
{
    //public int m_ID;
    //public int m_colorID = 7;
    //public GameObject m_optionsCanvas;
    //public GameObject m_colorCanvas;
    public LevelManager m_levelManager;
    //public GameObject m_child;
    //public bool m_changedColor = false;
    //public GameObject m_mySprite;
    public int m_personalID;

    public bool m_canOpenOptions = true;
    //public bool m_canEdit = true;
    public bool m_canDelete = true;

    public Vector3 m_defaultPosition;
    public Quaternion m_defaultRotation;
    public Vector3 m_defaultScale;

    bool m_checks = true;
    public bool m_canClick = true;
    public SpriteFollow m_spriteFollow;
    string m_layer;
    private void Awake()
    {
        m_levelManager = GameObject.FindObjectOfType<LevelManager>();
        m_spriteFollow = GetComponent<SpriteFollow>();
        m_levelManager.m_isEditing = true;
    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && m_spriteFollow.m_follow && m_canClick)
        {
            PlaceDown();
        }
        m_canClick = true;
    }
    private void OnMouseOver()
    {
        if (!m_spriteFollow.m_follow)
        {
            m_checks = true;
            if (m_levelManager.m_optionsCanvas.activeInHierarchy)
            {
                m_checks = false;
            }
            else if (m_levelManager.m_colorCanvas.activeInHierarchy)
            {
              m_checks = false;
            }
            else if (m_levelManager.m_winCanvas.activeInHierarchy)
            {
              m_checks = false;
            }
            else if (m_levelManager.m_gameOverCanvas.activeInHierarchy)
            {
              m_checks = false;
            }
            else if (m_levelManager.m_controlCanvas.activeInHierarchy)
            {
              m_checks = false;
            }
            else if (!m_canOpenOptions)
            {
                m_checks = false;
            }
            else if (m_levelManager.m_isEditing)
            {
                m_checks = false;
            }
            else if (m_levelManager.m_playButton.GetComponent<ButtonSelectionTracker>().IsSelected)
            {
                m_checks = false;
            }
            else if (m_levelManager.m_bridgeButton.GetComponent<ButtonSelectionTracker>().IsSelected)
            {
              m_checks = false;
            }
            else if (m_levelManager.m_pauseCanvas.activeInHierarchy)
            {
                m_checks = false;
            }
            if (Input.GetMouseButtonDown(0) && m_checks)
            {
                m_levelManager.m_spriteFollow = m_spriteFollow;
                m_levelManager.m_item = this;
                m_levelManager.m_optionsCanvas.SetActive(true);
            }
        }
    }
    public virtual void ResetDeafualts()
    {
        transform.position = m_defaultPosition;
        transform.rotation = m_defaultRotation;
        transform.localScale = m_defaultScale;
    }
    //private void OnEnable()
    //{
    //    ResetDeafualts();
    //}
    private void PlaceDown()
    {
        m_spriteFollow.m_follow = false;
        gameObject.layer = LayerMask.NameToLayer(m_spriteFollow.m_layer);
        //m_editor.m_itemButtons[m_editor.m_currentButtonID].m_isClicked = false;
        m_levelManager.m_isEditing = false;
        m_levelManager.m_HUDBuildCanvas.SetActive(true);
        m_levelManager.m_controlCanvas.SetActive(false);
    }
    
    public void SaveDefaults()
    {
        m_defaultPosition = transform.position;
        m_defaultScale = transform.localScale;
        m_defaultRotation = transform.rotation;
    }
}
