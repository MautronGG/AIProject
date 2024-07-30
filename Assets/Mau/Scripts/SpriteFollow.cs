using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteFollow : MonoBehaviour
{
    LevelManager m_levelManager;
    public bool m_follow = true;
    ItemScript m_item;
    public string m_layer;
    //public GameObject m_child;

    private void Awake()
    {    
        m_levelManager = GameObject.FindObjectOfType<LevelManager>();
        m_item = GetComponent<ItemScript>();
        //ChangeDefaults(m_defaultRotation, m_defaultScale, 7);
    }

    void Update()
    {
        if (m_follow)
        {
            Vector2 screenPosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            Vector2 worldPosition = Camera.main.ScreenToWorldPoint(screenPosition);
            transform.position = worldPosition;
            //transform.rotation = Quaternion.Euler(m_defaultRotation.x + m_rotationDegree, m_defaultRotation.y, m_defaultRotation.z);
        }

    }
    public void StartFollow()
    {
        m_follow = true;
        m_levelManager.m_HUDBuildCanvas.SetActive(false);
        m_levelManager.m_isEditing = true;
        m_layer = LayerMask.LayerToName(m_item.gameObject.layer);
        m_item.gameObject.layer = LayerMask.NameToLayer("nocol");
    }
}
