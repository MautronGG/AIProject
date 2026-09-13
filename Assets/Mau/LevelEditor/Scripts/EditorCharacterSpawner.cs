using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditorCharacterSpawner : MonoBehaviour
{
    public enum CharaColor { Red, Green, Blue };
    public enum CharaType { Player, Door }
    public CharaColor m_color;
    public CharaType m_type;


    //private void Awake()
    //{
    //    if (m_type == CharaType.Player)
    //    {
    //        switch (m_color)
    //        {
    //            case CharaColor.Red:
    //                transform.GetChild(0).GetComponent<SpriteRenderer>().color = Color.red;
    //                break;
    //            case CharaColor.Green:
    //                transform.GetChild(0).GetComponent<SpriteRenderer>().color = Color.green;
    //                break;
    //            case CharaColor.Blue:
    //                transform.GetChild(0).GetComponent<SpriteRenderer>().color = Color.blue;
    //                break;
    //        }
    //    }
    //    else
    //    {
    //        switch (m_color)
    //        {
    //            case CharaColor.Red:
    //                transform.GetChild(0).GetComponent<SpriteRenderer>().color = Color.red;
    //                break;
    //            case CharaColor.Green:
    //                transform.GetChild(0).GetComponent<SpriteRenderer>().color = Color.green;
    //                break;
    //            case CharaColor.Blue:
    //                transform.GetChild(0).GetComponent<SpriteRenderer>().color = Color.blue;
    //                break;
    //        }
    //    }
    //}
}
