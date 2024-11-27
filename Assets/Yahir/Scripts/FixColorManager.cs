using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum typesObjects
{
    Bridge = 0,
    Spring,
    Portal,
    Key,
    Door,
    Monster,
    Laser
}
public enum ColorEnum
{
    White,
    Red,
    Yellow,
    Green,
    Cyan,
    Blue,
    Magenta,
    Black,
    Null
}
public class spriteObjects
{

    public typesObjects m_typeObject;

    public ColorEnum lastColor = ColorEnum.Black;

    public Sprite m_spriteRed;

    public Sprite m_spriteBlue;

    public Sprite m_spriteGreen;

    public Sprite m_spriteWhite;

    public Sprite m_spriteBlack;

    public Sprite m_spriteYellow;

    public Sprite m_spriteCyan;

    public Sprite m_spriteMagenta;

    public bool m_activeRed = true, m_activeBlue = true, m_activeGreen = true, m_activeWhite = true,
       m_activeBlack = true, m_activeMagenta = true, m_activeCyan = true, m_activeYellow = true;
}

public class FixColorManager : MonoBehaviour
{
    //[SerializeField]
    //List<Sprite> m_sprite = new List<Sprite>();

    [Tooltip("Sprite White")]
    [SerializeField]
    private List<Sprite> m_spriteWhite;
    [Tooltip("Sprite Red")]
    [SerializeField]
    private List<Sprite> m_spriteRed;
    [Tooltip("Sprite Yellow")]
    [SerializeField]
    private List<Sprite> m_spriteYellow;
    [Tooltip("Sprite Green")]
    [SerializeField]
    private List<Sprite> m_spriteGreen;
    [Tooltip("Sprite Cyan")]
    [SerializeField]
    private List<Sprite> m_spriteCyan;
    [Tooltip("Sprite Blue")]
    [SerializeField]
    private List<Sprite> m_spriteBlue;
    [Tooltip("Sprite Magenta")]
    [SerializeField]
    private List<Sprite> m_spriteMagenta;
    [Tooltip("Sprite Black")]
    [SerializeField]
    private List<Sprite> m_spriteBlack;
    [Tooltip("Sprite Yellow")]
    [SerializeField]
    private List<typesObjects> m_listTypeObjectId;

    public List<spriteObjects> m_listSpriteObjects = new List<spriteObjects>();

    // Start is called before the first frame update
    void Awake()
    {
        for (int i = 0; i < m_spriteRed.Count; i++)
        {
            spriteObjects newSpriteObjects = new spriteObjects();
            newSpriteObjects.m_typeObject = m_listTypeObjectId[i];
            newSpriteObjects.m_spriteCyan = m_spriteCyan[i];
            newSpriteObjects.m_spriteGreen = m_spriteGreen[i];
            newSpriteObjects.m_spriteWhite = m_spriteWhite[i];
            newSpriteObjects.m_spriteMagenta = m_spriteMagenta[i];
            newSpriteObjects.m_spriteBlack = m_spriteBlack[i];
            newSpriteObjects.m_spriteBlue = m_spriteBlue[i];
            newSpriteObjects.m_spriteRed = m_spriteRed[i];
            newSpriteObjects.m_spriteYellow = m_spriteYellow[i];
            m_listSpriteObjects.Add(newSpriteObjects);
        }
    }
    public Sprite getSprite(int colorFix, typesObjects idTypeObject, ColorEnum actualColor)
    {
        for (int i = 0; i < m_listSpriteObjects.Count; i++)
        {
            if (m_listSpriteObjects[i].m_typeObject == idTypeObject)
            {
                if (colorFix == 1 && m_listSpriteObjects[i].m_activeRed == true)
                {
                    m_listSpriteObjects[i].m_activeRed = false;
                    m_listSpriteObjects[i].lastColor = ColorEnum.Red;
                    activeColors(actualColor, i);
                    return m_listSpriteObjects[i].m_spriteRed;
                }
                else if (colorFix == 5 && m_listSpriteObjects[i].m_activeBlue == true)
                {
                    m_listSpriteObjects[i].m_activeBlue = false;
                    m_listSpriteObjects[i].lastColor = ColorEnum.Blue;
                    activeColors(actualColor, i);
                    return m_listSpriteObjects[i].m_spriteBlue;
                }
                else if (colorFix == 3 && m_listSpriteObjects[i].m_activeGreen == true)
                {
                    m_listSpriteObjects[i].m_activeGreen = false;
                    m_listSpriteObjects[i].lastColor = ColorEnum.Green;
                    activeColors(actualColor, i);
                    return m_listSpriteObjects[i].m_spriteGreen;
                }
                else if (colorFix == 0 && m_listSpriteObjects[i].m_activeWhite == true)
                {
                    m_listSpriteObjects[i].m_activeWhite = false;
                    m_listSpriteObjects[i].lastColor = ColorEnum.White;
                    activeColors(actualColor, i);
                    return m_listSpriteObjects[i].m_spriteWhite;
                }
                else if (colorFix == 4 && m_listSpriteObjects[i].m_activeCyan == true)
                {
                    m_listSpriteObjects[i].m_activeCyan = false;
                    m_listSpriteObjects[i].lastColor = ColorEnum.Cyan;
                    activeColors(actualColor, i);
                    return m_listSpriteObjects[i].m_spriteCyan;
                }
                else if (colorFix == 2 && m_listSpriteObjects[i].m_activeYellow == true)
                {
                    m_listSpriteObjects[i].m_activeYellow = false;
                    m_listSpriteObjects[i].lastColor = ColorEnum.Yellow;
                    activeColors(actualColor, i);
                    return m_listSpriteObjects[i].m_spriteYellow;
                }
                else if (colorFix == 6 && m_listSpriteObjects[i].m_activeMagenta == true)
                {
                    m_listSpriteObjects[i].m_activeMagenta = false;
                    m_listSpriteObjects[i].lastColor = ColorEnum.Magenta;
                    activeColors(actualColor, i);
                    return m_listSpriteObjects[i].m_spriteMagenta;
                }
                else if (colorFix == 7)
                {
                    m_listSpriteObjects[i].lastColor = ColorEnum.Black;
                    activeColors(actualColor, i);
                    return m_listSpriteObjects[i].m_spriteBlack;
                }
            }
        }
        return null;
    }

    public List<Sprite> getSpriteLinkObjects(int colorFix, typesObjects enumObject, typesObjects enumLinkObject, ColorEnum actualColor)
    {
        List<Sprite> newObject = new List<Sprite>();
        for (int i = 0; i < m_listSpriteObjects.Count; i++)
        {
            if (m_listSpriteObjects[i].m_typeObject == enumObject)
            {
                if (colorFix == 1 && m_listSpriteObjects[i].m_activeRed == true)
                {
                    m_listSpriteObjects[i].lastColor = ColorEnum.Red;
                    activeColors(actualColor, i);
                    newObject.Add(m_listSpriteObjects[i].m_spriteRed);
                    newObject.Add(getSprite(colorFix, enumLinkObject, actualColor));
                    m_listSpriteObjects[i].m_activeRed = false;
                }
                else if (colorFix == 5 && m_listSpriteObjects[i].m_activeBlue == true)
                {
                    m_listSpriteObjects[i].lastColor = ColorEnum.Blue;
                    activeColors(actualColor, i);
                    newObject.Add(m_listSpriteObjects[i].m_spriteBlue);
                    newObject.Add(getSprite(colorFix, enumLinkObject, actualColor));
                    m_listSpriteObjects[i].m_activeBlue = false;
                }
                else if (colorFix == 3 && m_listSpriteObjects[i].m_activeGreen == true)
                {
                    m_listSpriteObjects[i].lastColor = ColorEnum.Green;
                    activeColors(actualColor, i);
                    newObject.Add(m_listSpriteObjects[i].m_spriteGreen);
                    newObject.Add(getSprite(colorFix, enumLinkObject, actualColor));
                    m_listSpriteObjects[i].m_activeGreen = false;
                }
                else if (colorFix == 0 && m_listSpriteObjects[i].m_activeWhite == true)
                {
                    m_listSpriteObjects[i].lastColor = ColorEnum.White;
                    activeColors(actualColor, i);
                    newObject.Add(m_listSpriteObjects[i].m_spriteWhite);
                    newObject.Add(getSprite(colorFix, enumLinkObject, actualColor));
                    m_listSpriteObjects[i].m_activeWhite = false;
                }
                else if (colorFix == 4 && m_listSpriteObjects[i].m_activeCyan == true)
                {
                    m_listSpriteObjects[i].lastColor = ColorEnum.Cyan;
                    activeColors(actualColor, i);
                    newObject.Add(m_listSpriteObjects[i].m_spriteCyan);
                    newObject.Add(getSprite(colorFix, enumLinkObject, actualColor));
                    m_listSpriteObjects[i].m_activeCyan = false;
                }
                else if (colorFix == 2 && m_listSpriteObjects[i].m_activeYellow == true)
                {
                    m_listSpriteObjects[i].lastColor = ColorEnum.Yellow;
                    activeColors(actualColor, i);
                    newObject.Add(m_listSpriteObjects[i].m_spriteYellow);
                    newObject.Add(getSprite(colorFix, enumLinkObject, actualColor));
                    m_listSpriteObjects[i].m_activeYellow = false;
                }
                else if (colorFix == 6 && m_listSpriteObjects[i].m_activeMagenta == true)
                {
                    m_listSpriteObjects[i].lastColor = ColorEnum.Magenta;
                    activeColors(actualColor, i);
                    newObject.Add(m_listSpriteObjects[i].m_spriteMagenta);
                    newObject.Add(getSprite(colorFix, enumLinkObject, actualColor));
                    m_listSpriteObjects[i].m_activeMagenta = false;
                }
                else if (colorFix == 7 && m_listSpriteObjects[i].m_activeBlack == true)
                {
                    m_listSpriteObjects[i].lastColor = ColorEnum.Black;
                    activeColors(actualColor, i);
                    newObject.Add(m_listSpriteObjects[i].m_spriteBlack);
                    newObject.Add(getSprite(colorFix, enumLinkObject, actualColor));
                }
                else
                {
                    newObject = null;
                }
            }
        }
        return newObject;
    }

    private void activeColors(ColorEnum actualColor, int iter)
    {
        if (actualColor == ColorEnum.Red)
        {
            m_listSpriteObjects[iter].m_activeRed = true;
        }
        else if (actualColor == ColorEnum.Blue)
        {
            m_listSpriteObjects[iter].m_activeBlue = true;
        }
        else if (actualColor == ColorEnum.Green)
        {
            m_listSpriteObjects[iter].m_activeGreen = true;
        }
        else if (actualColor == ColorEnum.White)
        {
            m_listSpriteObjects[iter].m_activeWhite = true;
        }
        else if (actualColor == ColorEnum.Black)
        {
            m_listSpriteObjects[iter].m_activeBlack = true;
        }
        else if (actualColor == ColorEnum.Cyan)
        {
            m_listSpriteObjects[iter].m_activeCyan = true;
        }
        else if (actualColor == ColorEnum.Yellow)
        {
            m_listSpriteObjects[iter].m_activeYellow = true;
        }
        else if (actualColor == ColorEnum.Magenta)
        {
            m_listSpriteObjects[iter].m_activeMagenta = true;
        }
    }
    public ColorEnum getLastColor(int iter)
    { 
        return m_listSpriteObjects[iter].lastColor; 
    }
    public ColorEnum getLastColor(typesObjects idObject)
    {
        for (int i = 0; i < m_listSpriteObjects.Count; i++)
        {
            if (m_listSpriteObjects[i].m_typeObject == idObject)
            {
                return m_listSpriteObjects[i].lastColor;
            }
        }
        return ColorEnum.Null;
    }
}
