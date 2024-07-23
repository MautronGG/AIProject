using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeColor : MonoBehaviour
{
    [Tooltip("Manager object Fix")]
    [SerializeField]
    ColorManager m_ColorManager;

    private SpriteRenderer spriteRenderer;
    private Sprite temporalSprite;

    private int actualColor = 7;

    // Start is called before the first frame update
    void Start()
    {
        GameObject spriteObject = new GameObject("TemporalSpriteObject");
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (temporalSprite == null)
        {
            return;
        }
        actualColor = m_ColorManager.getLastColor();
        spriteRenderer.sprite = temporalSprite;
        temporalSprite = null;
    }

    public void ColorChange(int newColor)
    {
        switch (newColor)
        {
            case 0:
                temporalSprite = m_ColorManager.getSprite(0, actualColor);
                break;
            case 1:
                temporalSprite = m_ColorManager.getSprite(1, actualColor);
                break;
            case 2:
                temporalSprite = m_ColorManager.getSprite(2, actualColor);
                break;
            case 3:
                temporalSprite = m_ColorManager.getSprite(3, actualColor);
                break;
            case 4:
                temporalSprite = m_ColorManager.getSprite(4, actualColor);
                break;
            case 5:
                temporalSprite = m_ColorManager.getSprite(5, actualColor);
                break;
            case 6:
                temporalSprite = m_ColorManager.getSprite(6, actualColor);
                break;
            case 7:
                temporalSprite = m_ColorManager.getSprite(7, actualColor);
                break;
        }
    }
}
