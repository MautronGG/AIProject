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
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Sprite getSprite(string colorFix)
    {
        if (colorFix == "Red")
        {
            return m_spriteRed;
        }
        else if (colorFix == "Blue")
        {
            return m_spriteBlue;
        }
        else if (colorFix == "Green")
        {
            return m_spriteGreen;
        }
        else
        {
            return m_spriteWhite;
        }
    }
}
