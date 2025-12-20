using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public float m_speed;
    public float m_runSpeed;
    public float m_defaultSpeed;
    public bool m_canMove = true;
    public bool m_autoMove = false;
    public bool m_restrictedMove = true;

    public GameObject m_minion;
    public Vector3 m_defaultPosition;
    public EditorBorderManager m_borders;
    float m_xValue = 9.5f;
    float m_yValue = 4.75f;

    Camera m_camera;
    float m_maxSizeValue = 40f;
    float m_minSizeValue = 2.5f;

    public GameObject m_viggenette;
    public bool m_canQEZoom = true;

    private void Start()
    {
        m_speed = m_defaultSpeed;
        m_defaultPosition = transform.position;
        m_borders = FindFirstObjectByType<EditorBorderManager>();
        m_camera = GetComponent<Camera>();
        m_minion = FindWithTagAndLayer("Player", 8);
        //m_xValue *= 1.2f;
        //m_yValue *= 1.2f;
    }
    // Update is called once per frame
    void Update()
    {
        if (m_canMove)
        {
            m_speed = m_defaultSpeed;

            if (Input.GetKey(KeyCode.LeftShift))
            {
                m_speed = m_runSpeed;
            }
            if (Input.GetKeyUp(KeyCode.LeftShift))
            {
                m_speed = m_defaultSpeed;
            }

            m_speed += (m_camera.orthographicSize);

            if (Input.GetKey(KeyCode.D))
            {
                transform.position += new Vector3(1f, 0f, 0f) * Time.deltaTime * m_speed ;
                if (m_restrictedMove && transform.position.x >= m_borders.m_rightBorder.transform.position.x - m_xValue)
                {
                    transform.position = new Vector3(m_borders.m_rightBorder.transform.position.x - m_xValue, transform.position.y, transform.position.z);
                }
            }
            if (Input.GetKey(KeyCode.A))
            {
                transform.position += new Vector3(-1f, 0f, 0f) * Time.deltaTime * m_speed;
                if (m_restrictedMove && transform.position.x <= m_borders.m_leftBorder.transform.position.x + m_xValue)
                {
                    transform.position = new Vector3(m_borders.m_leftBorder.transform.position.x + m_xValue, transform.position.y, transform.position.z);
                }
            }
            if (Input.GetKey(KeyCode.W))
            {
                transform.position += new Vector3(0f, 1f, 0f) * Time.deltaTime * m_speed;
                if (m_restrictedMove && transform.position.y >= m_borders.m_topBorder.transform.position.y - m_yValue)
                {
                    transform.position = new Vector3(transform.position.x, m_borders.m_topBorder.transform.position.y - m_yValue, transform.position.z);
                }
            }   
            if (Input.GetKey(KeyCode.S))    
            {
                transform.position += new Vector3(0f, -1f, 0f) * Time.deltaTime * m_speed;
                if (m_restrictedMove && transform.position.y <= m_borders.m_bottomBorder.transform.position.y + m_yValue)
                {
                    transform.position = new Vector3(transform.position.x, m_borders.m_bottomBorder.transform.position.y + m_yValue, transform.position.z);
                }
            }

            if (m_camera.orthographicSize < m_maxSizeValue)
            {
                if (Input.GetKey(KeyCode.Q) && m_canQEZoom)
                {
                    m_camera.orthographicSize += 1 * Time.deltaTime * m_speed;
                    m_viggenette.transform.localScale += new Vector3(1f, 1f, 0f) * Time.deltaTime * m_speed;
                }
                else if (Input.GetAxis("Mouse ScrollWheel") < 0)
                {
                    m_camera.orthographicSize += 2 * Time.deltaTime * m_speed;
                    m_viggenette.transform.localScale += new Vector3(2f, 2f, 0f) * Time.deltaTime * m_speed;
                }

                if (m_camera.orthographicSize >= m_maxSizeValue)
                {
                    m_camera.orthographicSize = m_maxSizeValue;
                }
            }

            if (m_camera.orthographicSize > m_minSizeValue)
            {
                if (Input.GetKey(KeyCode.E) && m_canQEZoom)
                {
                    m_camera.orthographicSize -= 1 * Time.deltaTime * m_speed;
                    m_viggenette.transform.localScale -= new Vector3(1f, 1f, 0f) * Time.deltaTime * m_speed;
                }
                else if (Input.GetAxis("Mouse ScrollWheel") > 0)
                {
                    m_camera.orthographicSize -= 2 * Time.deltaTime * m_speed;
                    m_viggenette.transform.localScale -= new Vector3(2f, 2f, 0f) * Time.deltaTime * m_speed;
                }

                if (m_camera.orthographicSize <= m_minSizeValue)
                {
                    m_camera.orthographicSize = m_minSizeValue;
                }
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

    GameObject FindWithTagAndLayer(string tag, int layer)
    {
        GameObject[] taggedObjects = GameObject.FindGameObjectsWithTag(tag);

        foreach (var obj in taggedObjects)
        {
            if (obj.layer == layer)
                return obj;
        }

        return null;
    }
}
