using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBridgeScript : ItemScript
{
    public SpriteFollow m_spriteFollow;

    // Start is called before the first frame update
    public override void Awake()
    {
        m_spriteFollow = GetComponent<SpriteFollow>();
        base.Awake();
        m_levelManager.m_isEditing = true;
    }
    // Update is called once per frame
    public override void Update()
    {
        if (Input.GetMouseButtonDown(0) && m_spriteFollow.m_follow && m_canClick)
        {
            PlaceDown();
        }
        base.Update();
    }
    public override void OnMouseOver()
    {
        int theColor = color.color;
        if (!m_spriteFollow.m_follow)
        {
            base.OnMouseOver();
            if (Input.GetMouseButtonDown(0) && theColor == 8 && m_checks && !m_spriteFollow.m_follow)
            {
                m_levelManager.m_spriteFollow = m_spriteFollow;
                m_levelManager.m_item = this;
                m_levelManager.m_item.m_canClick = false;
                m_levelManager.m_optionsCanvas.SetActive(true);
            }
        }
        
    }
    public override void ResetDeafualts()
    {
        base.ResetDeafualts();
    }
    public override void SaveDefaults()
    {
        base.SaveDefaults();
    }
    private void PlaceDown()
    {
        m_spriteFollow.m_follow = false;
        gameObject.layer = LayerMask.NameToLayer(m_spriteFollow.m_layer);
        m_levelManager.m_isEditing = false;
        m_levelManager.m_HUDBuildCanvas.SetActive(true);
        m_levelManager.m_controlCanvas.SetActive(false);
    }
    public void Delete()
    {
        m_fixColorManager.getSprite(7, m_object, actualColor);
        actualColor = m_fixColorManager.getLastColor(m_object);
        Destroy(gameObject);

    }
}
