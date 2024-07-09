using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestFixObject : MonoBehaviour
{
    [Tooltip("Manager object Fix")]
    [SerializeField]
    FixColorManager m_fixColorManager;

    private SpriteRenderer spriteRenderer;
    private Sprite temporalSprite;

    private string actualColor = "Black";

    // Start is called before the first frame update
    void Start()
    {
        GameObject spriteObject = new GameObject("TemporalSpriteObject");
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            temporalSprite = m_fixColorManager.getSprite("Red", actualColor);
        }
        else if (Input.GetKeyDown(KeyCode.W))
        {
            temporalSprite = m_fixColorManager.getSprite("White", actualColor);
        }
        else if (Input.GetKeyDown(KeyCode.B))
        {
            temporalSprite = m_fixColorManager.getSprite("Blue", actualColor);
        }
        else if (Input.GetKeyDown(KeyCode.G))
        {
            temporalSprite = m_fixColorManager.getSprite("Green", actualColor);
        }
        if (temporalSprite == null) 
        {
            return;
        }
        actualColor = m_fixColorManager.getLastColor();
        spriteRenderer.sprite = temporalSprite;
        temporalSprite = null;
    }
}
