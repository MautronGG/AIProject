using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorSet : MonoBehaviour
{
    public Texture2D[] cursorOff;
    public Texture2D[] cursorOn;
    public CursorMode mode = CursorMode.Auto;
    //public bool autoCenterHS = false;
    public Vector2 customHS = Vector2.zero;
    //Vector2 autoHS;

    int color = 8;
    bool isOn = false;
    [SerializeField]float timer;
    float currentTimer = 0f;
    bool startTimer = false;
    // Start is called before the first frame update
    private void Start()
    {        
        //ChangeCursor();
    }
    private void Update()
    {
        if (isOn)
        {
            currentTimer = 0f;
            startTimer = true;
            ChangeCursor();
        }
        if (startTimer)
        {
            currentTimer += Time.deltaTime;
            if (currentTimer >= timer)
            {
                currentTimer = 0f;
                isOn = false;
                startTimer = false;
                ChangeCursor();
            }
        }
    }
    public void ChangeCursor()
    {
        Texture2D cursor;
        Vector2 hotSpot = customHS;
        //if (autoCenterHS)
        //{
        //    autoHS = new Vector2(cursorOff[].width * .5f, cursorOff[].height * .5f);
        //    hotSpot = autoHS;
        //}
        //else
        //{
        //    hotSpot = customHS;
        //}
        if (isOn)
        {
            cursor = cursorOn[color];
        }
        else
        {
            cursor = cursorOff[color];
        }
        Cursor.SetCursor(cursor, hotSpot, mode);
    }
    public void ChangeColor(int newColor)
    {
        color = newColor;
    }
}

