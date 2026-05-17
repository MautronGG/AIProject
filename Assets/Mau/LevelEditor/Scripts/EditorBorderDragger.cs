using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class EditorBorderDragger : MonoBehaviour
{
    [Tooltip("Unique ID used to save this object. Must match an entry in PrefabDatabase.")]
    public string id;
    [HideInInspector] public LevelVoidData data;

    public enum Type { LeftBorder, RightBorder, TopBorder, BottomBorder }
    public Type m_type;

    public enum Axis { Horizontal, Vertical }
    public Axis m_movementAxis;

    public enum Restriction { Positive, Negative }
    public Restriction m_movementRestriction;

    public EditorGridManager m_gridManager;
    public EditorManager m_editorManager;

    public int m_maxCells = 25;

    private Vector3 m_lastMouseWorld;
    private bool m_isDragging = false;
    public bool m_buffer = false;
    public bool m_checks = false;

    private Vector3 m_minPosition; // starting position (min limit)
    private Vector3 m_maxPosition; // computed from restriction

    public int m_cellsRemainingToMax = 0;
    public int m_cellsRemainingToMin = 0;


    private void Start()
    {
        m_gridManager = FindFirstObjectByType<EditorGridManager>();
        m_editorManager = FindFirstObjectByType<EditorManager>();

        m_movementAxis = m_type == Type.TopBorder || m_type == Type.BottomBorder ? Axis.Vertical : Axis.Horizontal;
        m_movementRestriction = m_type == Type.LeftBorder || m_type == Type.BottomBorder ? Restriction.Negative : Restriction.Positive;

        float size = m_gridManager.m_cellSize;
        Vector3 direction = Vector3.zero;

        if (m_movementAxis == Axis.Vertical)
        {
            if (m_movementRestriction == Restriction.Positive)
                direction = new Vector3(0f, 1f, 0f);
            else
                direction = new Vector3(0f, -1f, 0f);
        }
        else // Horizontal
        {
            if (m_movementRestriction == Restriction.Positive)
                direction = new Vector3(1f, 0f, 0f);
            else
                direction = new Vector3(-1f, 0f, 0f);
        }

        m_minPosition = transform.position - direction * (m_cellsRemainingToMin * size);
        m_maxPosition = m_minPosition + direction * (m_maxCells * size);

        if (m_cellsRemainingToMax == 0 && m_cellsRemainingToMin == 0)
        {
            m_cellsRemainingToMax = m_maxCells;
            m_cellsRemainingToMin = 0;
        }
    }

    private void OnMouseOver()
    {
        //m_checks = true;
        ////if (m_editorManager.m_optionsCanvas.activeInHierarchy)
        ////{
        ////    m_checks = false;
        ////}
        //////else if (!m_editor.m_colorCanvas.activeInHierarchy)
        ////{
        ////  m_checks = false;
        ////}
        //////else if (!m_editor.m_winCanvas.activeInHierarchy)
        ////{
        ////  m_checks = false;
        ////}
        //////else if (!m_editor.m_gameOverCanvas.activeInHierarchy)
        ////{
        ////  m_checks = false;
        ////}
        //////else if (!m_editor.m_controlCanvas.activeInHierarchy)
        ////{
        ////  m_checks = false;
        ////}
        ////else if (!m_canOpenOptions)
        ////{
        ////    m_checks = false;
        ////}
        //if (m_editorManager.m_isEditing)
        //{
        //    m_checks = false;
        //}
        //else if (m_editorManager.m_playButton.GetComponent<ButtonSelectionTracker>().IsSelected)
        //{
        //    m_checks = false;
        //}
        //////else if (!m_editor.m_bridgeButton.GetComponent<ButtonSelectionTracker>().IsSelected)
        ////{
        ////  m_checks = false;
        ////}
        //else if (m_editorManager.m_pauseCanvas.activeInHierarchy)
        //{
        //    m_checks = false;
        //}
        //if (m_checks)
        //{
        //    foreach (ButtonSelectionTracker bst in m_editorManager.m_buttonSelectionTrackers)
        //    {
        //        if (bst.IsSelected)
        //        {
        //            m_checks = false;
        //            break;
        //        }
        //    }
        //}

        if (Input.GetMouseButtonDown(0) && CanInteract() && !m_isDragging)
        {
            m_lastMouseWorld = GetMouseWorld();

            m_isDragging = true;

            m_editorManager.m_isEditing = true;
        }
    }

    public void ForceSyncData()
    {
        if (data == null) return;

        data.position = SerializableVector3.From(transform.position);
        data.rotation = SerializableQuaternion.From(transform.rotation);
        data.scale = SerializableVector3.From(transform.localScale);

        data.maxCells = m_maxCells;
        data.cellsRemainingToMax = m_cellsRemainingToMax;
        data.cellsRemainingToMin = m_cellsRemainingToMin;
        data.type = m_type;
    }

    bool CanInteract()
    {
        if (m_editorManager.currentMode != EditorMode.Edit) return false;
        if (m_editorManager.m_isEditing) return false;
        //if (m_editor.m_optionsCanvas.activeInHierarchy) return false;
        if (m_editorManager.m_pauseCanvas.activeInHierarchy) return false;

        foreach (var button in m_editorManager.m_buttonSelectionTrackers)
            if (button.IsSelected) return false;

        return true;
    }




    //private void Update()
    //{
    //    if (m_isDragging)
    //    {
    //        Vector3 mouse = GetMouseWorld();
    //        Vector3 delta = mouse - m_lastMouseWorld;
    //
    //        float size = m_gridManager.m_cellSize;
    //
    //        if (m_movementAxis == Axis.Vertical)
    //        {
    //            // how many full tiles did the mouse move vertically?
    //            int step = (int)(delta.y / size); // truncates toward zero, so only moves when >= 1 tile
    //            if (step != 0)
    //            {
    //                Vector3 newPos = transform.position + new Vector3(0f, step * size, 0f);
    //
    //                // Clamp between min and max
    //                newPos.y = Mathf.Clamp(newPos.y, Mathf.Min(m_minPosition.y, m_maxPosition.y), Mathf.Max(m_minPosition.y, m_maxPosition.y));
    //
    //                transform.position = newPos; //+= new Vector3(0f, step * size, 0f);
    //                m_lastMouseWorld += new Vector3(0f, step * size, 0f);
    //                m_gridManager.UpdateGrid(); // rebuild grid for new size
    //            }
    //        }
    //        else // Horizontal
    //        {
    //            int step = (int)(delta.x / size);
    //            if (step != 0)
    //            {
    //                Vector3 newPos = transform.position + new Vector3(step * size, 0f, 0f);
    //
    //                // Clamp between min and max
    //                newPos.x = Mathf.Clamp(newPos.x, Mathf.Min(m_minPosition.x, m_maxPosition.x), Mathf.Max(m_minPosition.x, m_maxPosition.x));
    //
    //                transform.position = newPos;//+= new Vector3(step * size, 0f, 0f);
    //                m_lastMouseWorld += new Vector3(step * size, 0f, 0f);
    //                m_gridManager.UpdateGrid();
    //            }
    //        }
    //
    //        if (Input.GetMouseButtonDown(0) && m_buffer)
    //        {
    //            m_isDragging = false;
    //            m_editorManager.m_isEditing = false;
    //            m_buffer = false;
    //            return;
    //        }
    //        m_buffer = true;
    //    }
    //}

    private void Update()
    {
        if (m_isDragging)
        {
            Vector3 mouse = GetMouseWorld();
            float size = m_gridManager.m_cellSize;

            Vector3 newPos = transform.position;

            if (m_movementAxis == Axis.Vertical)
            {
                // Clamp the mouse to min/max
                float clampedY = Mathf.Clamp(mouse.y, Mathf.Min(m_minPosition.y, m_maxPosition.y), Mathf.Max(m_minPosition.y, m_maxPosition.y));
                // Snap to grid
                clampedY = Mathf.Round(clampedY / size) * size;

                newPos = new Vector3(m_minPosition.x, clampedY, m_minPosition.z);
            }
            else // Horizontal
            {
                float clampedX = Mathf.Clamp(mouse.x, Mathf.Min(m_minPosition.x, m_maxPosition.x), Mathf.Max(m_minPosition.x, m_maxPosition.x));
                clampedX = Mathf.Round(clampedX / size) * size;

                newPos = new Vector3(clampedX, m_minPosition.y, m_minPosition.z);
            }

            if (newPos != transform.position)
            {
                transform.position = newPos;
                m_gridManager.UpdateGrid();

                m_cellsRemainingToMax = Mathf.RoundToInt(Vector3.Distance(transform.position, m_maxPosition) / size);
                m_cellsRemainingToMin = Mathf.RoundToInt(Vector3.Distance(transform.position, m_minPosition) / size);
            }

            // Release on click
            if (Input.GetMouseButtonDown(0) && m_buffer)
            {
                m_isDragging = false;
                m_editorManager.m_isEditing = false;
                m_buffer = false;
                return;
            }
            m_buffer = true;
        }
    }

    private Vector3 GetMouseWorld()
    {
        Vector3 w = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        w.z = 0f;
        return w;
    }

}
