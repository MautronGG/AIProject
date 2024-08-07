using System.Collections;
using System.Collections.Generic;
using UnityEngine;


class spriteObjects
{
    public string m_typeObject;

    public string lastColor = "Black";

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
    private List<string> m_listTypeObjectId;


    [SerializeField]
    private List<spriteObjects> m_listSpriteObjests = new List<spriteObjects>();

    // Start is called before the first frame update
    void Start()
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
            m_listSpriteObjests.Add(newSpriteObjects);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Sprite getSprite(int colorFix, string idTypeObject, string actualColor)
    {
        for (int i = 0; i < m_listSpriteObjests.Count; i++)
        {
            if (m_listSpriteObjests[i].m_typeObject == idTypeObject)
            {
                if (colorFix == 1 && m_listSpriteObjests[i].m_activeRed == true)
                {
                    m_listSpriteObjests[i].m_activeRed = false;
                    m_listSpriteObjests[i].lastColor = "Red";
                    activeColors(actualColor, i);
                    return m_listSpriteObjests[i].m_spriteRed;
                }
                else if (colorFix == 5 && m_listSpriteObjests[i].m_activeBlue == true)
                {
                    m_listSpriteObjests[i].m_activeBlue = false;
                    m_listSpriteObjests[i].lastColor = "Blue";
                    activeColors(actualColor, i);
                    return m_listSpriteObjests[i].m_spriteBlue;
                }
                else if (colorFix == 3 && m_listSpriteObjests[i].m_activeGreen == true)
                {
                    m_listSpriteObjests[i].m_activeGreen = false;
                    m_listSpriteObjests[i].lastColor = "Green";
                    activeColors(actualColor, i);
                    return m_listSpriteObjests[i].m_spriteGreen;
                }
                else if (colorFix == 0 && m_listSpriteObjests[i].m_activeWhite == true)
                {
                    m_listSpriteObjests[i].m_activeWhite = false;
                    m_listSpriteObjests[i].lastColor = "White";
                    activeColors(actualColor, i);
                    return m_listSpriteObjests[i].m_spriteWhite;
                }
                else if (colorFix == 4 && m_listSpriteObjests[i].m_activeCyan == true)
                {
                    m_listSpriteObjests[i].m_activeCyan = false;
                    m_listSpriteObjests[i].lastColor = "Cyan";
                    activeColors(actualColor, i);
                    return m_listSpriteObjests[i].m_spriteCyan;
                }
                else if (colorFix == 2 && m_listSpriteObjests[i].m_activeYellow == true)
                {
                    m_listSpriteObjests[i].m_activeYellow = false;
                    m_listSpriteObjests[i].lastColor = "Yellow";
                    activeColors(actualColor, i);
                    return m_listSpriteObjests[i].m_spriteYellow;
                }
                else if (colorFix == 6 && m_listSpriteObjests[i].m_activeMagenta == true)
                {
                    m_listSpriteObjests[i].m_activeMagenta = false;
                    m_listSpriteObjests[i].lastColor = "Magenta";
                    activeColors(actualColor, i);
                    return m_listSpriteObjests[i].m_spriteMagenta;
                }
                else if (colorFix == 7 && m_listSpriteObjests[i].m_activeBlack == true)
                {
                    m_listSpriteObjests[i].m_activeBlack = false;
                    m_listSpriteObjests[i].lastColor = "Black";
                    activeColors(actualColor, i);
                    return m_listSpriteObjests[i].m_spriteBlack;
                }
            }
        }
        return null;
    }

    private void activeColors(string actualColor, int iter)
    {
        if (actualColor == "Red")
        {
            m_listSpriteObjests[iter].m_activeRed = true;
        }
        else if (actualColor == "Blue")
        {
            m_listSpriteObjests[iter].m_activeBlue = true;
        }
        else if (actualColor == "Green")
        {
            m_listSpriteObjests[iter].m_activeGreen = true;
        }
        else if (actualColor == "White")
        {
            m_listSpriteObjests[iter].m_activeWhite = true;
        }
        else if (actualColor == "Black")
        {
            m_listSpriteObjests[iter].m_activeBlack = true;
        }
        else if (actualColor == "Cyan")
        {
            m_listSpriteObjests[iter].m_activeCyan = true;
        }
        else if (actualColor == "Yellow")
        {
            m_listSpriteObjests[iter].m_activeYellow = true;
        }
        else if (actualColor == "Magenta")
        {
            m_listSpriteObjests[iter].m_activeMagenta = true;
        }
    }
    public string getLastColor(int iter)
    { 
        return m_listSpriteObjests[iter].lastColor; 
    }
    public string getLastColor(string idObject)
    {
        for (int i = 0; i < m_listSpriteObjests.Count; i++)
        {
            if (m_listSpriteObjests[i].m_typeObject == idObject)
            {
                return m_listSpriteObjests[i].lastColor;
            }
        }
        return null;
    }
}
