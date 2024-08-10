using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestFixObject : MonoBehaviour
{
    [Tooltip("Manager object Fix")]
    [SerializeField]
    FixColorManager m_fixColorManager;
    [SerializeField]
    GameObject m_gameObject;
    private SpriteRenderer spriteRenderer;
    private SpriteRenderer spriteRendererLink;
    private List<Sprite> temporalSprite;

    private string actualColor = "Black";

    // Start is called before the first frame update
    void Start()
    {
        GameObject spriteObject = new GameObject("TemporalSpriteObject");
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRendererLink = m_gameObject.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame

    private void OnMouseOver()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            temporalSprite = m_fixColorManager.getSpriteLinkObjects(1, typesObects.Portal, typesObects.KeyDoor, actualColor);
        }
        else if (Input.GetKeyDown(KeyCode.W))
        {
            temporalSprite = m_fixColorManager.getSpriteLinkObjects(0, typesObects.Portal, typesObects.Portal, actualColor);
        }
        else if (Input.GetKeyDown(KeyCode.B))
        {
            temporalSprite = m_fixColorManager.getSpriteLinkObjects(5, typesObects.Portal, typesObects.Portal, actualColor);
        }
        else if (Input.GetKeyDown(KeyCode.G))
        {
            temporalSprite = m_fixColorManager.getSpriteLinkObjects(3, typesObects.Portal, typesObects.Portal, actualColor);
        }
        else if (Input.GetKeyDown(KeyCode.O))
        {
            temporalSprite = m_fixColorManager.getSpriteLinkObjects(7, typesObects.Portal, typesObects.Portal, actualColor);
        }
        else if (Input.GetKeyDown(KeyCode.M))
        {
            temporalSprite = m_fixColorManager.getSpriteLinkObjects(6, typesObects.Portal, typesObects.Portal, actualColor);
        }
        else if (Input.GetKeyDown(KeyCode.C))
        {
            temporalSprite = m_fixColorManager.getSpriteLinkObjects(4, typesObects.Portal, typesObects.Portal, actualColor);
        }
        else if (Input.GetKeyDown(KeyCode.Y))
        {
            temporalSprite = m_fixColorManager.getSpriteLinkObjects(2, typesObects.Portal, typesObects.Portal, actualColor);
        }
        if (temporalSprite == null)
        {
            return;
        }
        actualColor = m_fixColorManager.getLastColor(typesObects.Portal);
        if (temporalSprite != null)
        {
            spriteRenderer.sprite = temporalSprite[0];
            spriteRendererLink.sprite = temporalSprite[1];
        }
        temporalSprite = null;
        
    }
}