using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class EditorBorderDragger : MonoBehaviour
{
    public enum Axis { Horizontal, Vertical }
    public Axis m_movementAxis;

    public EditorGridManager m_gridManager;

    private Vector3 m_lastMouseWorld;
    //private Camera m_camera;
    //private Vector3 m_offSet;

    private void Start()
    {
        //m_camera = Camera.main;
        m_gridManager = FindFirstObjectByType<EditorGridManager>();
    }

    private void OnMouseDown()
    {
        //Vector3 mousePos = m_camera.ScreenToWorldPoint(Input.mousePosition);
        //mousePos.z = 0f;
        //m_offSet = transform.position - mousePos;
        m_lastMouseWorld = GetMouseWorld();
    }

    private void OnMouseDrag()
    {
        //Vector3 mousePos = m_camera.ScreenToWorldPoint(Input.mousePosition);
        //mousePos.z = transform.position.z;
        //
        //Vector3 newPos = transform.position;
        //
        //if (m_movementAxis == Axis.Horizontal)
        //{
        //    newPos.x = mousePos.x + m_offSet.x;
        //}
        //else if (m_movementAxis == Axis.Vertical)
        //{
        //    newPos.y = mousePos.y + m_offSet.y;
        //}
        //
        //transform.position = newPos;

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
