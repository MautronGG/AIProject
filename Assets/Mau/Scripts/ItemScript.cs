using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemScript : MonoBehaviour
{
    [SerializeField]
    public bool m_isDouble = false;
    //public int m_ID;
    //public int m_colorID = 7;
    //public GameObject m_optionsCanvas;
    //public GameObject m_colorCanvas;
    public LevelManager m_levelManager;
    //public GameObject m_child;
    //public bool m_changedColor = false;
    //public GameObject m_mySprite;
    public int m_personalID;

    public Vector3 m_defaultPosition;
    public Quaternion m_defaultRotation;
    public Vector3 m_defaultScale;

    public bool m_checks = true;
    public bool m_canClick = true;
    string m_layer;

    [Tooltip("Manager object Fix")]
    [SerializeField]
    public FixColorManager m_fixColorManager;

    public CursorSet color;

    [SerializeField]
    public typesObects m_object;
    public ItemScript otherObject;

    private SpriteRenderer spriteRenderer;
    private Sprite temporalSprite;
    public List<Sprite> doubleSprites;

    public string actualColor = "Black";
    public virtual void Awake()
    {
        m_levelManager = GameObject.FindObjectOfType<LevelManager>();
        color = FindObjectOfType<CursorSet>();
        m_fixColorManager = FindObjectOfType<FixColorManager>();
        GameObject spriteObject = new GameObject("TemporalSpriteObject");
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    public virtual void Update()
    {
        m_canClick = true;
    }
    public virtual void OnMouseOver()
    {
        int theColor = color.color;
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
        //if (Input.GetMouseButtonDown(0) && m_checks)
        //{
        //    m_levelManager.m_spriteFollow = m_spriteFollow;
        //    m_levelManager.m_item = this;
        //    m_levelManager.m_optionsCanvas.SetActive(true);
        //}
        
        if (Input.GetMouseButtonDown(0) && theColor < 8 && m_checks)
        {
            if (!m_isDouble)
            {
                temporalSprite = m_fixColorManager.getSprite(theColor, m_object, actualColor);
                if (temporalSprite == null)
                {
                    return;
                }
                actualColor = m_fixColorManager.getLastColor(m_object);
                gameObject.layer = LayerMask.NameToLayer(actualColor);
                spriteRenderer.sprite = temporalSprite;
                temporalSprite = null;
            }
            else
            {
                doubleSprites = m_fixColorManager.getSpriteLinkObjects(theColor, m_object, otherObject.m_object, actualColor);
                if (doubleSprites == null)
                {
                    return;
                }
                actualColor = m_fixColorManager.getLastColor(m_object);
                spriteRenderer.sprite = doubleSprites[0];
                otherObject.spriteRenderer.sprite = doubleSprites[1];
                gameObject.layer = LayerMask.NameToLayer(actualColor);
                otherObject.gameObject.layer = LayerMask.NameToLayer(actualColor);
                doubleSprites = null;
            } 
        }
    }
    public virtual void ResetDeafualts()
    {
        transform.position = m_defaultPosition;
        transform.rotation = m_defaultRotation;
        transform.localScale = m_defaultScale;
        gameObject.SetActive(true);
    }
    //private void OnEnable()
    //{
    //    ResetDeafualts();
    //}
    public virtual void SaveDefaults()
    {
        m_defaultPosition = transform.position;
        m_defaultScale = transform.localScale;
        m_defaultRotation = transform.rotation;
    }
}
