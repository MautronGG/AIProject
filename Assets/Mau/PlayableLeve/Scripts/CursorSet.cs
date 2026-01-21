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

    public int color = 8;
    bool isOn = false;
    [SerializeField]float timer;
    float currentTimer = 0f;
    bool startTimer = false;
    // Start is called before the first frame update
    private void Start()
    {        
        ChangeCursor();
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

        if (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Keypad1))
        {
            ChangeColor(0);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.Keypad2))
        {
            ChangeColor(1);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3) || Input.GetKeyDown(KeyCode.Keypad3))
        {
            ChangeColor(2);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4) || Input.GetKeyDown(KeyCode.Keypad4))
        {
            ChangeColor(3);
        }
        if (Input.GetKeyDown(KeyCode.Alpha5) || Input.GetKeyDown(KeyCode.Keypad5))
        {
            ChangeColor(4);
        }
        if (Input.GetKeyDown(KeyCode.Alpha6) || Input.GetKeyDown(KeyCode.Keypad6))
        {
            ChangeColor(5);
        }
        if (Input.GetKeyDown(KeyCode.Alpha7) || Input.GetKeyDown(KeyCode.Keypad7))
        {
            ChangeColor(6);
        }
        if (Input.GetKeyDown(KeyCode.Alpha8) || Input.GetKeyDown(KeyCode.Keypad8))
        {
            ChangeColor(7);
        }
        if (Input.GetKeyDown(KeyCode.Alpha9) || Input.GetKeyDown(KeyCode.Keypad9))
        {
            ChangeColor(8);
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
        ChangeCursor();
    }
}

