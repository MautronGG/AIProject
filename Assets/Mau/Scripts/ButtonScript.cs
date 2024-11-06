using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ButtonScript : MonoBehaviour
{
    [SerializeField] GameObject m_object;
    public bool m_isClicked = false;
    public LevelManager m_levelManager;
    GameObject m_obj;
    SpriteFollow m_spriteFollow;
    [SerializeField] int m_numBridges = 4;
    [SerializeField] TextMeshProUGUI m_text;

    //[Dropdown("m_editor.m_objectsList")]
    //public string m_name;
    //public ObjectsEnum m_ID = new ObjectsEnum();

    public enum ObjectsEnum
    {
        NormalBlock,
        SpikyBlock,
        Portal,
        Key,
        Spring,
        Enemy,
        Laser,
    };

    void Start()
    {
        m_levelManager = GameObject.FindObjectOfType<LevelManager>();
    }
    public void OnClick()
    {
        if (m_numBridges > 0)
        {
            m_levelManager.m_controlCanvas.SetActive(true);
            ChangeQuantity(false);
            Vector2 screenPosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            Vector2 worldPosition = Camera.main.ScreenToWorldPoint(screenPosition);
            m_isClicked = true;
            m_obj = Instantiate(m_object, new Vector3(worldPosition.x, worldPosition.y, 0), Quaternion.identity);
            m_spriteFollow = m_obj.GetComponent<SpriteFollow>();
            //m_obj = Instantiate(m_editor.m_itemPrefabs[(int)m_ID], new Vector3(worldPosition.x, worldPosition.y, 0), Quaternion.identity);
            m_spriteFollow.StartFollow();
            //m_editor.m_currentButtonID = (int)m_ID;
        }
    }
    public void ChangeQuantity(bool positive)
    {
        if (positive)
        {
            m_numBridges++;
        }
        else
        {
            m_numBridges--;
        }
        m_text.text = m_numBridges.ToString();
    }
}
