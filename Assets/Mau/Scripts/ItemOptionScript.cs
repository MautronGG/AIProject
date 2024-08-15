using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemOptionScript : MonoBehaviour
{
    //public int m_ID;
    private LevelManager m_levelManager;
    //public ItemManager m_item;
    //public GameObject m_colorCanvas;

    private void Start()
    {
        m_levelManager = GameObject.FindObjectOfType<LevelManager>();
    }

    public void PickUp()
    {
        //m_editor.m_itemID = this.gameObject;
        //m_editor.m_optionsCanvas.SetActive(true);
        m_levelManager.m_item.m_spriteFollow.StartFollow();
        m_levelManager.m_item.m_canClick = false;
    }

    public void Delete()
    {
        m_levelManager.m_item.Delete();
    }
}
