using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class EditorBorderDragger : MonoBehaviour
{
    public enum Axis { Horizontal, Vertical }
    public Axis m_movementAxis;

    private Camera m_camera;
    private Vector3 m_offSet;

    private void Start()
    {
        m_camera = Camera.main;
    }

    private void OnMouseDown()
    {
        Vector3 mousePos = m_camera.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = transform.position.z;
        m_offSet = transform.position - mousePos;
    }

    private void OnMouseDrag()
    {
        Vector3 mousePos = m_camera.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = transform.position.z;

        Vector3 newPos = transform.position;

        if (m_movementAxis == Axis.Horizontal)
        {
            newPos.x = mousePos.x + m_offSet.x;
        }
        else if (m_movementAxis == Axis.Vertical)
        {
            newPos.y = mousePos.y + m_offSet.y;
        }

        transform.position = newPos;
    }
}
