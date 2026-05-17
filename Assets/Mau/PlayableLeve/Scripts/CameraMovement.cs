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

    [SerializeField] bool m_restrictedRight = false;
    [SerializeField] bool m_restrictedLeft = false;
    [SerializeField] bool m_restrictedTop = false;
    [SerializeField] bool m_restrictedBottom = false;

    //[SerializeField] bool m_overBorderRight = false;
    //[SerializeField] bool m_overBorderLeft = false;
    //[SerializeField] bool m_overBorderTop = false;
    //[SerializeField] bool m_overBorderBottom = false;

    [SerializeField] float m_panSpeed = 0.5f;
    Vector3 m_panOrigin;

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

    Vector3 desiredPosition;


    private void Start()
    {
        m_camera = GetComponent<Camera>();
        m_borders = FindFirstObjectByType<EditorBorderManager>();
        m_panSpeed = 0.5f;
        m_speed = m_defaultSpeed;
        ChangeMinion();
        if (m_minion)
            desiredPosition = new Vector3(m_minion.transform.position.x + 3, m_minion.transform.position.y + 3.240495f, -104.5f);
        else
            desiredPosition = new Vector3(13.1f, 6.13f, -104.5f);

        transform.position = desiredPosition;
        m_defaultPosition = transform.position;
    }
    // Update is called once per frame
    void Update()
    {
        desiredPosition = transform.position;

        if (m_canMove && !m_autoMove)
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

            if (Input.GetKey(KeyCode.D) && !m_restrictedRight)
            {
                transform.position += new Vector3(1f, 0f, 0f) * Time.deltaTime * m_speed;

            }
            if (Input.GetKey(KeyCode.A) && !m_restrictedLeft)
            {
                transform.position += new Vector3(-1f, 0f, 0f) * Time.deltaTime * m_speed;

            }
            if (Input.GetKey(KeyCode.W) && !m_restrictedTop)
            {
                transform.position += new Vector3(0f, 1f, 0f) * Time.deltaTime * m_speed;

            }
            if (Input.GetKey(KeyCode.S) && !m_restrictedBottom)
            {
                transform.position += new Vector3(0f, -1f, 0f) * Time.deltaTime * m_speed;
            }

            if (Input.GetMouseButtonDown(2))
            {
                m_panOrigin = Input.mousePosition;
            }
            if (Input.GetMouseButton(2))
            {
                Vector3 delta = Input.mousePosition - m_panOrigin;

                // Move camera opposite to mouse movement
                Vector3 move = new Vector3(-delta.x, -delta.y, 0) * m_panSpeed * Time.deltaTime;
                transform.Translate(move, Space.Self);

                m_panOrigin = Input.mousePosition;
            }

            if (m_camera.orthographicSize < m_maxSizeValue)
            {
                if (Input.GetKey(KeyCode.Q) && m_canQEZoom)
                {
                    m_camera.orthographicSize += 1 * Time.deltaTime * m_speed;
                    m_panSpeed += .05f * Time.deltaTime * m_speed;
                    //m_viggenette.transform.localScale += new Vector3(1f, 1f, 0f) * Time.deltaTime * m_speed;
                }
                else if (Input.GetAxis("Mouse ScrollWheel") < 0)
                {
                    m_camera.orthographicSize += 2 * Time.deltaTime * m_speed;
                    m_panSpeed += .1f * Time.deltaTime * m_speed;
                    //m_viggenette.transform.localScale += new Vector3(2f, 2f, 0f) * Time.deltaTime * m_speed;
                }

                if (m_camera.orthographicSize >= m_maxSizeValue)
                {
                    m_camera.orthographicSize = m_maxSizeValue;
                    m_panSpeed = 2f;
                }
            }

            if (m_camera.orthographicSize > m_minSizeValue)
            {
                if (Input.GetKey(KeyCode.E) && m_canQEZoom)
                {
                    m_camera.orthographicSize -= 1 * Time.deltaTime * m_speed;
                    m_panSpeed -= .05f * Time.deltaTime * m_speed;
                    //m_viggenette.transform.localScale -= new Vector3(1f, 1f, 0f) * Time.deltaTime * m_speed;
                }
                else if (Input.GetAxis("Mouse ScrollWheel") > 0)
                {
                    m_camera.orthographicSize -= 2 * Time.deltaTime * m_speed;
                    m_panSpeed -= 0.1f * Time.deltaTime * m_speed;
                    //m_viggenette.transform.localScale -= new Vector3(2f, 2f, 0f) * Time.deltaTime * m_speed;
                }

                if (m_camera.orthographicSize <= m_minSizeValue)
                {
                    m_camera.orthographicSize = m_minSizeValue;
                    m_panSpeed = 0.1f;

                }
            }
        }

        if (m_autoMove)
        {
            transform.position = new Vector3(m_minion.transform.position.x + 3, m_minion.transform.position.y + 3.240495f, transform.position.z);
            //if (transform.position.x >= m_borders.m_rightBorder.transform.position.x - m_xValue)
            //{
            //    transform.position = new Vector3(m_borders.m_rightBorder.transform.position.x - m_xValue, transform.position.y, transform.position.z);
            //}
            //if (transform.position.x <= m_borders.m_leftBorder.transform.position.x + m_xValue)
            //{
            //    transform.position = new Vector3(m_borders.m_leftBorder.transform.position.x + m_xValue, transform.position.y, transform.position.z);
            //}
            //if (transform.position.y >= m_borders.m_topBorder.transform.position.y - m_yValue)
            //{
            //    transform.position = new Vector3(transform.position.x, m_borders.m_topBorder.transform.position.y - m_yValue, transform.position.z);
            //}
            //if (transform.position.y <= m_borders.m_bottomBorder.transform.position.y + m_yValue)
            //{
            //    transform.position = new Vector3(transform.position.x, m_borders.m_bottomBorder.transform.position.y + m_yValue, transform.position.z);
            //}
        }

        ClampPosition();
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
    public void ClampPosition()
    {
        if (!m_restrictedMove) return;

        if (transform.position.x >= m_borders.m_rightBorder.transform.position.x - m_xValue)
        {
            //transform.position = new Vector3(m_borders.m_rightBorder.transform.position.x - m_xValue, transform.position.y, transform.position.z);
            m_restrictedRight = true;
            //m_overBorderRight = true;
        }
        else
        {
            m_restrictedRight = false;
            //m_overBorderRight = false;
        }

        if (transform.position.x <= m_borders.m_leftBorder.transform.position.x + m_xValue)
        {
            //transform.position = new Vector3(m_borders.m_leftBorder.transform.position.x + m_xValue, transform.position.y, transform.position.z);
            m_restrictedLeft = true;
            //m_overBorderLeft = true;
        }
        else
        {
            m_restrictedLeft = false;
            //m_overBorderLeft = false;
        }

        if (transform.position.y >= m_borders.m_topBorder.transform.position.y - m_yValue)
        {
            //transform.position = new Vector3(transform.position.x, m_borders.m_topBorder.transform.position.y - m_yValue, transform.position.z);
            m_restrictedTop = true;
            //m_overBorderTop = true;
        }
        else
        {
            m_restrictedTop = false;
            //m_overBorderTop = false;
        }

        if (transform.position.y <= m_borders.m_bottomBorder.transform.position.y + m_yValue)
        {
            //transform.position = new Vector3(transform.position.x, m_borders.m_bottomBorder.transform.position.y + m_yValue, transform.position.z);
            m_restrictedBottom = true;
            //m_overBorderBottom = true;
        }
        else
        {
            m_restrictedBottom = false;
            //m_overBorderBottom = false;
        }
    }

    public void ChangeMinion()
    {
        m_minion = FindWithTagAndLayer("Player", 8);
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
