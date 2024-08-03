using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test2Fix : MonoBehaviour
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
        if (Input.GetKeyDown(KeyCode.T))
        {
            temporalSprite = m_fixColorManager.getSprite("Red", "Portal", actualColor);
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            temporalSprite = m_fixColorManager.getSprite("White", "Portal", actualColor);
        }
        else if (Input.GetKeyDown(KeyCode.N))
        {
            temporalSprite = m_fixColorManager.getSprite("Blue", "Portal", actualColor);
        }
        else if (Input.GetKeyDown(KeyCode.H))
        {
            temporalSprite = m_fixColorManager.getSprite("Green", "Portal", actualColor);
        }
        if (temporalSprite == null)
        {
            return;
        }
        actualColor = m_fixColorManager.getLastColor("Portal");
        spriteRenderer.sprite = temporalSprite;
        temporalSprite = null;
    }
}
