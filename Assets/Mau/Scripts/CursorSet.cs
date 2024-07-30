using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorSet : MonoBehaviour
{
    public Texture2D cursor;
    public CursorMode mode = CursorMode.Auto;
    public bool autoCenterHS = false;
    public Vector2 customHS = Vector2.zero;
    Vector2 autoHS;

    public bool isOn;
    // Start is called before the first frame update
    public void ChangeCursor()
    {
        Vector2 hotSpot;
        if (autoCenterHS)
        {
            autoHS = new Vector2(cursor.width * .5f, cursor.height * .5f);
            hotSpot = autoHS;
        }
        else
        {
            hotSpot = customHS;
        }

        Cursor.SetCursor(cursor, hotSpot, CursorMode.ForceSoftware);
    }
}

