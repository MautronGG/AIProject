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

    private ColorEnum actualColor = ColorEnum.Black;

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
            temporalSprite = m_fixColorManager.getSpriteLinkObjects(1, typesObjects.Portal, typesObjects.Key, actualColor);
        }
        else if (Input.GetKeyDown(KeyCode.W))
        {
            temporalSprite = m_fixColorManager.getSpriteLinkObjects(0, typesObjects.Portal, typesObjects.Portal, actualColor);
        }
        else if (Input.GetKeyDown(KeyCode.B))
        {
            temporalSprite = m_fixColorManager.getSpriteLinkObjects(5, typesObjects.Portal, typesObjects.Portal, actualColor);
        }
        else if (Input.GetKeyDown(KeyCode.G))
        {
            temporalSprite = m_fixColorManager.getSpriteLinkObjects(3, typesObjects.Portal, typesObjects.Portal, actualColor);
        }
        else if (Input.GetKeyDown(KeyCode.O))
        {
            temporalSprite = m_fixColorManager.getSpriteLinkObjects(7, typesObjects.Portal, typesObjects.Portal, actualColor);
        }
        else if (Input.GetKeyDown(KeyCode.M))
        {
            temporalSprite = m_fixColorManager.getSpriteLinkObjects(6, typesObjects.Portal, typesObjects.Portal, actualColor);
        }
        else if (Input.GetKeyDown(KeyCode.C))
        {
            temporalSprite = m_fixColorManager.getSpriteLinkObjects(4, typesObjects.Portal, typesObjects.Portal, actualColor);
        }
        else if (Input.GetKeyDown(KeyCode.Y))
        {
            temporalSprite = m_fixColorManager.getSpriteLinkObjects(2, typesObjects.Portal, typesObjects.Portal, actualColor);
        }
        if (temporalSprite == null)
        {
            return;
        }
        actualColor = m_fixColorManager.getLastColor(typesObjects.Portal);
        if (temporalSprite != null)
        {
            spriteRenderer.sprite = temporalSprite[0];
            spriteRendererLink.sprite = temporalSprite[1];
        }
        temporalSprite = null;
        
    }
}