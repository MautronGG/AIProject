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
    [SerializeField] float speed = 5;

    [SerializeField]
    private float scaleDegrees = 5.0f;
    [SerializeField]
    private float limitScaleDegreesPos = 1.0f;
    [SerializeField]
    private float limitScaleDegreesNeg = 0.2f;
    [SerializeField] 
    private float scaleActual = 0.0f;

    SpriteFollow m_sprite;

    Vector3 defaultScale;
    void Start()
    {
        defaultScale = transform.localScale;
        scaleActual = defaultScale.x;
        m_sprite = GetComponent<SpriteFollow>();
    }

    // Update is called once per frame
    void Update()
    {
        if(m_sprite.m_follow)
        {
            ChangeRotation();
            ChangeScale();
        }
    }
    public void ChangeRotation()
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
    public void ChangeScale()
    {
        if (Input.GetKey(KeyCode.F))
        {
            if (transform.localScale.x < limitScaleDegreesPos)
            {
                transform.localScale += new Vector3(scaleDegrees, 0,0) * Time.deltaTime * speed;
            }
        }
        if (Input.GetKey(KeyCode.G))
        {
            if (transform.localScale.x > limitScaleDegreesNeg)
            {
                transform.localScale += new Vector3(-scaleDegrees, 0,0) * Time.deltaTime * speed;
            }
        }
        else if (Input.GetKeyDown(KeyCode.R))
        {
            transform.localScale = defaultScale;
        }
    }
}
