using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixColorManager : MonoBehaviour
{
    //[SerializeField]
    //List<Sprite> m_sprite = new List<Sprite>();

    [Tooltip("Sprite Red")]
    [SerializeField]
    private Sprite m_spriteRed;
    [Tooltip("Sprite Blue")]
    [SerializeField]
    private Sprite m_spriteBlue;
    [Tooltip("Sprite Green")]
    [SerializeField]
    private Sprite m_spriteGreen;
    [Tooltip("Sprite White")]
    [SerializeField]
    private Sprite m_spriteWhite;

    private string lastColor = "Black";

    private bool m_activeRed = true, m_activeBlue = true, m_activeGreen = true, m_activeWhite = true;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Sprite getSprite(string colorFix, string actualColor)
    {
        if (colorFix == "Red" && m_activeRed == true)
        {
            m_activeRed = false;
            lastColor = "Red";
            activeColors(actualColor);
            return m_spriteRed;
        }
        else if (colorFix == "Blue" && m_activeBlue == true)
        {
            m_activeBlue = false;
            lastColor = "Blue";
            activeColors(actualColor);
            return m_spriteBlue;
        }
        else if (colorFix == "Green" && m_activeGreen == true)
        {
            m_activeGreen = false;
            lastColor = "Green";
            activeColors(actualColor);
            return m_spriteGreen;
        }
        else if (colorFix == "White" && m_activeWhite == true)
        {
            m_activeWhite = false;
            lastColor = "White";
            activeColors(actualColor);
            return m_spriteWhite;
        }
        else
        {
            return null;
        }
    }

    private void activeColors(string actualColor)
    {
        if (actualColor == "Red")
        {
            m_activeRed = true;
        }
        else if (actualColor == "Blue")
        {
            m_activeBlue = true;
        }
        else if (actualColor == "Green")
        {
            m_activeGreen = true;
        }
        else if (actualColor == "White")
        {
            m_activeWhite = true;
        }
    }
    public string getLastColor()
    { 
        return lastColor; 
    }
}
