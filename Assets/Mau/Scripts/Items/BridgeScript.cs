using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BridgeScript : ItemManager
{
    public static bool BridgeActivate;
  private void Start()
  {
        BridgeActivate = true;
  }
    private void Update()
    {
        if (BridgeActivate)
        {
            Vector3 mousePosition = Input.mousePosition;

            mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);

            mousePosition.z = transform.position.z;

            transform.position = mousePosition;
        }
    }
}
