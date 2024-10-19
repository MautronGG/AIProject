using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public partial class DebugBounds : MonoBehaviour
{
    Collider2D m_Collider;
    public Vector3 m_Center;
    public Vector3 m_Size, m_Min, m_Max, m_Extents;
    void Start()
    {
        Bounds();
    }
    public void Bounds()
    {
        m_Collider = GetComponent<Collider2D>();
        m_Center = m_Collider.bounds.center;
        m_Size = m_Collider.bounds.size;
        m_Min = m_Collider.bounds.min;
        m_Max = m_Collider.bounds.max;
        m_Extents = m_Collider.bounds.extents;
        
    }
}

[CustomEditor(typeof(DebugBounds))]
public partial class DebugBoundsEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        DebugBounds db = (DebugBounds)target;
        if (GUILayout.Button("Get Bounds"))
        {
            db.Bounds();
        }
    }
}