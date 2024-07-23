using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorManager : MonoBehaviour
{
    [Tooltip("Sprite Red")]
    [SerializeField]
    private Sprite m_spriteRed;
    [Tooltip("Sprite Yellow")]
    [SerializeField]
    private Sprite m_spriteYellow;
    [Tooltip("Sprite Green")]
    [SerializeField]
    private Sprite m_spriteGreen;
    [Tooltip("Sprite Cyan")]
    [SerializeField]
    private Sprite m_spriteCyan;
    [Tooltip("Sprite Blue")]
    [SerializeField]
    private Sprite m_spriteBlue;
    [Tooltip("Sprite Magenta")]
    [SerializeField]
    private Sprite m_spriteMagenta;
    [Tooltip("Sprite White")]
    [SerializeField]
    private Sprite m_spriteWhite;
    [Tooltip("Sprite Black")]
    [SerializeField]
    private Sprite m_spriteBlack;

    private int lastColor = 7;

    private bool m_activeRed = true, m_activeYellow = true, m_activeGreen = true, m_activeCyan = true, m_activeBlue = true, m_activeMagenta = true, m_activeWhite = true, m_activeBlack = true;

    public Sprite getSprite(int colorFix, int actualColor)
    {
        if (colorFix == 0 && m_activeRed == true)
        {
            m_activeRed = false;
            lastColor = colorFix;
            activeColors(actualColor);
            return m_spriteRed;
        }
        else if (colorFix == 1 && m_activeYellow == true)
        {
            m_activeYellow = false;
            lastColor = colorFix;
            activeColors(actualColor);
            return m_spriteRed;
        }
        else if (colorFix == 2 && m_activeGreen == true)
        {
            m_activeGreen = false;
            lastColor = colorFix;
            return m_spriteBlue;
        }
        else if (colorFix == 3 && m_activeCyan == true)
        {
            m_activeCyan = false;
            lastColor = colorFix;
            activeColors(actualColor);
            return m_spriteRed;
        }
        else if (colorFix == 4 && m_activeBlue == true)
        {
            m_activeBlue = false;
            lastColor = colorFix;
            activeColors(actualColor);
            return m_spriteGreen;
        }
        else if (colorFix == 5 && m_activeMagenta == true)
        {
            m_activeMagenta = false;
            lastColor = colorFix;
            activeColors(actualColor);
            return m_spriteRed;
        }
        else if (colorFix == 6 && m_activeWhite == true)
        {
            m_activeWhite = false;
            lastColor = colorFix;
            activeColors(actualColor);
            return m_spriteWhite;
        }
        else 
        {
            return m_spriteBlack;
        }
    }
    private void activeColors(int actualColor)
    {
        if (actualColor == 0)
        {
            m_activeRed = true;
        }
        if (actualColor == 1)
        {
            m_activeRed = true;
        }
        else if (actualColor == 2)
        {
            m_activeGreen = true;
        }
        if (actualColor == 3)
        {
            m_activeRed = true;
        }
        else if (actualColor == 4)
        {
            m_activeBlue = true;
        }
        if (actualColor == 5)
        {
            m_activeRed = true;
        }
        else if (actualColor == 6)
        {
            m_activeWhite = true;
        }
    }
    public int getLastColor()
    {
        return lastColor;
    }
}
