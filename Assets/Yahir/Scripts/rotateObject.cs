using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotateObject : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField]
    private float rotationDegrees = 5.0f;
    [SerializeField]
    private float limitRotationDegrees = 45.0f;
    private float rotationActual = 0.0f;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (rotationActual < limitRotationDegrees)
            {
                transform.Rotate(0, 0, rotationDegrees);
                rotationActual += rotationDegrees;
            }
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (rotationActual > -limitRotationDegrees)
            {
                transform.Rotate(0, 0, -rotationDegrees);
                rotationActual -= rotationDegrees;
            }
        }
        else if (Input.GetKeyDown(KeyCode.R))
        {
            transform.Rotate(0, 0, -rotationActual);
            rotationActual = 0.0f;
        }
    }
}
