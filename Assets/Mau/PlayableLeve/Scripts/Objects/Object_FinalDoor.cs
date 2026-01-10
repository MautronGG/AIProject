using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Object_FinalDoor : MonoBehaviour
{
    [SerializeField] Sprite m_normal;
    [SerializeField] Sprite m_locked;
    public bool m_canEnter;

    public void ChangeSprite(bool state)
    {
        if(state)
        {
            GetComponent<SpriteRenderer>().sprite = m_normal;
        }
        else
        {
            GetComponent<SpriteRenderer>().sprite = m_locked;
        }
    }

}
