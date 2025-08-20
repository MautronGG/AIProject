using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    float m_speed;
    public float m_runSpeed;
    public float m_defaultSpeed;
    public bool m_canMove = true;
    public bool m_autoMove = false;
    public GameObject m_minion;
    public Vector3 m_defaultPosition;
    public EditorBorderManager m_borders;
    float m_xValue = 9.5f;
    float m_yValue = 4.75f;
        
    private void Start()
    {
        m_speed = m_defaultSpeed;
        m_defaultPosition = transform.position;
        m_borders = FindFirstObjectByType<EditorBorderManager>();
        //m_xValue *= 1.2f;
        //m_yValue *= 1.2f;
    }
    // Update is called once per frame
    void Update()
    {
        if (m_canMove)
        {
            if (Input.GetKey(KeyCode.D))
            {
                transform.position += new Vector3(1f, 0f, 0f) * Time.deltaTime * m_speed;
                if (transform.position.x >= m_borders.m_rightBorder.transform.position.x - m_xValue)
                {
                    transform.position = new Vector3(m_borders.m_rightBorder.transform.position.x - m_xValue, transform.position.y, transform.position.z);
                }
            }
            if (Input.GetKey(KeyCode.A))
            {
                transform.position += new Vector3(-1f, 0f, 0f) * Time.deltaTime * m_speed;
                if (transform.position.x <= m_borders.m_leftBorder.transform.position.x + m_xValue)
                {
                    transform.position = new Vector3(m_borders.m_leftBorder.transform.position.x + m_xValue, transform.position.y, transform.position.z);
                }
            }
            if (Input.GetKey(KeyCode.W))
            {
                transform.position += new Vector3(0f, 1f, 0f) * Time.deltaTime * m_speed;
                if (transform.position.y >= m_borders.m_topBorder.transform.position.y - m_yValue)
                {
                    transform.position = new Vector3(transform.position.x, m_borders.m_topBorder.transform.position.y - m_yValue, transform.position.z);
                }
            }   
            if (Input.GetKey(KeyCode.S))    
            {
                transform.position += new Vector3(0f, -1f, 0f) * Time.deltaTime * m_speed;
                if (transform.position.y <= m_borders.m_bottomBorder.transform.position.y + m_yValue)
                {
                    transform.position = new Vector3(transform.position.x, m_borders.m_bottomBorder.transform.position.y + m_yValue, transform.position.z);
                }
            }
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                m_speed = m_runSpeed;
            }
            if (Input.GetKeyUp(KeyCode.LeftShift))
            {
                m_speed = m_defaultSpeed;
            }
        }
        if (m_autoMove)
        {
            transform.position = new Vector3(m_minion.transform.position.x + 3, m_minion.transform.position.y + 3.240495f, transform.position.z);
            if (transform.position.x >= m_borders.m_rightBorder.transform.position.x - m_xValue)
            {
                transform.position = new Vector3(m_borders.m_rightBorder.transform.position.x - m_xValue, transform.position.y, transform.position.z);
            }
            if (transform.position.x <= m_borders.m_leftBorder.transform.position.x + m_xValue)
            {
                transform.position = new Vector3(m_borders.m_leftBorder.transform.position.x + m_xValue, transform.position.y, transform.position.z);
            }
            if (transform.position.y >= m_borders.m_topBorder.transform.position.y - m_yValue)
            {
                transform.position = new Vector3(transform.position.x, m_borders.m_topBorder.transform.position.y - m_yValue, transform.position.z);
            }
            if (transform.position.y <= m_borders.m_bottomBorder.transform.position.y + m_yValue)
            {
                transform.position = new Vector3(transform.position.x, m_borders.m_bottomBorder.transform.position.y + m_yValue, transform.position.z);
            }

        }
    }
    public void AutomaticMovement(bool Bool)
    {
        //m_canMove = false;
        m_autoMove = Bool;
    }
    public void ChangeMovement(bool newMove)
    {
        m_canMove = newMove;
    }
    public void ResetTransform()
    {
        transform.position = m_defaultPosition;
    }
}
