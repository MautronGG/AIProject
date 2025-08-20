using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditorSpriteFollow : MonoBehaviour
{
    EditorManager m_editor;
    public bool m_follow = true;
    [SerializeField] bool m_isParent = false;

    public enum Axis { Horizontal, Vertical }
    public Axis m_movementAxis;

    public EditorGridManager m_gridManager;

    private Vector3 m_lastMouseWorld;
    //public GameObject m_child;

    private void Awake()
    {
        m_editor = GameObject.FindGameObjectWithTag("EditorManager").GetComponent<EditorManager>();
        m_gridManager = FindFirstObjectByType<EditorGridManager>();
        //ChangeDefaults(m_defaultRotation, m_defaultScale, 7);
    }


    void Update()
    {
        if (m_follow)
        {
            Move();
            //Vector2 screenPosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            //Vector2 worldPosition = Camera.main.ScreenToWorldPoint(screenPosition);
            //transform.position = worldPosition;
            //transform.rotation = Quaternion.Euler(m_defaultRotation.x + m_rotationDegree, m_defaultRotation.y, m_defaultRotation.z);
            if (Input.GetMouseButtonDown(0) && m_isParent)
            {
                m_follow = false;
                m_editor.m_HUDCanvas.SetActive(true);
                m_editor.m_isEditing = false;
            }
        }

    }

    public void Move()
    {
        Vector3 mouse = GetMouseWorld();
        Vector3 delta = mouse - m_lastMouseWorld;

        float size = m_gridManager.m_cellSize;

        if (m_movementAxis == Axis.Vertical)
        {
            // how many full tiles did the mouse move vertically?
            int step = (int)(delta.y / size); // truncates toward zero, so only moves when >= 1 tile
            if (step != 0)
            {
                transform.position += new Vector3(0f, step * size, 0f);
                m_lastMouseWorld += new Vector3(0f, step * size, 0f);
                m_gridManager.UpdateGrid(); // rebuild grid for new size
            }
        }
        else // Horizontal
        {
            int step = (int)(delta.x / size);
            if (step != 0)
            {
                transform.position += new Vector3(step * size, 0f, 0f);
                m_lastMouseWorld += new Vector3(step * size, 0f, 0f);
                m_gridManager.UpdateGrid();
            }
        }
    }

    private Vector3 GetMouseWorld()
    {
        Vector3 w = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        w.z = 0f;
        return w;
    }
}
