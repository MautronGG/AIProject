using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditorSpriteFollow : MonoBehaviour
{
    EditorManager m_editor;
    public bool m_follow = true;
    [SerializeField] bool m_isParent = false;
    Collider2D m_collider;

    public EditorGridManager m_gridManager;

    public Vector3 m_lastMouseWorld;
    private Vector2 prefabSize;

    private Vector3 m_colliderOffset;

    public Vector3 m_lastValidWorldPosition;
    public Vector3 m_lastValidLocalPosition;
    //public GameObject m_child;

    private void Awake()
    {
        m_collider = GetComponentInChildren<Collider2D>();
        m_editor = GameObject.FindGameObjectWithTag("EditorManager").GetComponent<EditorManager>();
        m_gridManager = FindFirstObjectByType<EditorGridManager>();

        //ChangeDefaults(m_defaultRotation, m_defaultScale, 7);
        prefabSize = CalculatePrefabSize(gameObject);

        if (m_collider != null)
        {
            // Store local-space offset (relative to object pivot)
            m_colliderOffset = m_collider.bounds.center - transform.position;
        }

    }


    void Update()
    {
        if (m_follow)
        {
            Vector3 mouseWorld = GetMouseWorld();

            Vector3 snapped = SnapToGrid(mouseWorld, prefabSize);
            transform.position = snapped;

            // right click to cancel placement
            //if (Input.GetMouseButtonDown(1))
            //{
            //    PlaceDown();
            //    Destroy(gameObject);
            //    m_follow = false;
            //}
            //if (Input.GetMouseButtonDown(0) && m_isParent)
            //{
            //    PlaceDown();
            //}
        }

    }

    //public void Move()
    //{
    //    Vector3 mouse = GetMouseWorld();
    //    Vector3 delta = mouse - m_lastMouseWorld;
    //
    //    float size = m_gridManager.m_cellSize;
    //
    //    int stepx = (int)(delta.y / size); // truncates toward zero, so only moves when >= 1 tile
    //    int stepy = (int)(delta.x / size);
    //    if (stepx != 0)
    //    {
    //        transform.position += new Vector3(0f, stepx * size, 0f);
    //        m_lastMouseWorld += new Vector3(0f, stepx * size, 0f);
    //        m_gridManager.UpdateGrid(); // rebuild grid for new size
    //    }
    //
    //    if (stepy != 0)
    //    {
    //        transform.position += new Vector3(stepy * size, 0f, 0f);
    //        m_lastMouseWorld += new Vector3(stepy * size, 0f, 0f);
    //        m_gridManager.UpdateGrid();
    //    }
    //}

    //public void StartFollow()
    //{
    //    m_lastMouseWorld = GetMouseWorld();
    //    m_follow = true;
    //    m_editor.m_HUDCanvas.SetActive(false);
    //    m_editor.m_isEditing = true;
    //}

    //private void PlaceDown()
    //{
    //    //m_editor.m_audioManager.PlaySFX(m_levelManager.m_audioManager.m_sfx_PlaceBridge);
    //    m_follow = false;
    //    m_editor.m_isEditing = false;
    //    m_editor.m_HUDCanvas.SetActive(true);
    //    //_editor.m_controlCanvas.SetActive(false);
    //}

    private Vector3 GetMouseWorld()
    {
        Vector3 w = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        w.z = 0f;
        return w;
    }
    private Vector3 SnapToGrid(Vector3 worldPos, Vector2 size)
    {
        float cs = m_gridManager.m_cellSize;

        // Snap mouse to grid
        int gx = Mathf.FloorToInt(worldPos.x / cs);
        int gy = Mathf.FloorToInt(worldPos.y / cs);

        // Offset for even-sized prefabs
        float offsetX = (size.x % 2 == 0) ? cs / 2f : 0f;
        float offsetY = (size.y % 2 == 0) ? cs / 2f : 0f;

        // Base snapped position
        float px = gx * cs + offsetX;
        float py = gy * cs + offsetY;

        Vector3 candidate = new Vector3(px + 0.5f, py + 0.5f, 0f) - m_colliderOffset;

        candidate = ClampToGrid(candidate);

        return candidate;
        //Vector3 snapped = new Vector3(px + 0.5f, py + 0.5f, 0f);

        //snapped -= m_colliderOffset;
        //snapped = ClampToGrid(snapped);
        //
        //return snapped;
    }
    private Vector3 ClampToGrid(Vector3 pivotPos)
    {
        if (m_collider == null) return pivotPos;

        // collider half-size (world units) – does not depend on position
        Vector3 half = m_collider.bounds.extents;

        // grid borders (world)
        Vector3 bl = m_gridManager.m_borders.BottomLeft;
        Vector3 tr = m_gridManager.m_borders.TopRight;

        // We clamp the *collider center* to [bl+half, tr-half].
        // colliderCenter = pivotPos + m_colliderOffset
        // => pivotPos must be clamped to those limits minus the offset.
        float minX = bl.x + half.x - m_colliderOffset.x;
        float maxX = tr.x - half.x - m_colliderOffset.x;
        float minY = bl.y + half.y - m_colliderOffset.y;
        float maxY = tr.y - half.y - m_colliderOffset.y;

        // Handle case where object is bigger than grid (avoid NaNs)
        if (minX > maxX) { float midX = (bl.x + tr.x) * 0.5f - m_colliderOffset.x; pivotPos.x = midX; }
        else { pivotPos.x = Mathf.Clamp(pivotPos.x, minX, maxX); }

        if (minY > maxY) { float midY = (bl.y + tr.y) * 0.5f - m_colliderOffset.y; pivotPos.y = midY; }
        else { pivotPos.y = Mathf.Clamp(pivotPos.y, minY, maxY); }

        return pivotPos;

        //if (m_collider == null) return candidatePos;
        //
        //// Temporarily move to candidate position to evaluate bounds
        //Vector3 originalPos = transform.position;
        //transform.position = candidatePos;
        //
        //Bounds b = m_collider.bounds;
        //
        //// Grid borders
        //Vector3 bottomLeft = m_gridManager.m_borders.BottomLeft;
        //Vector3 topRight = m_gridManager.m_borders.TopRight;
        //
        //float halfWidth = b.extents.x;
        //float halfHeight = b.extents.y;
        //
        //float clampedX = Mathf.Clamp(candidatePos.x,
        //    bottomLeft.x + halfWidth,
        //    topRight.x - halfWidth);
        //
        //float clampedY = Mathf.Clamp(candidatePos.y,
        //    bottomLeft.y + halfHeight,
        //    topRight.y - halfHeight);
        //
        //// restore position (in case it's mid-frame check)
        //transform.position = originalPos;
        //
        //return new Vector3(clampedX, clampedY, candidatePos.z);
    }
    //private void ClampToGrid()//Vector3 pos, Vector2 size//)
    //{
    //    //float cs = m_gridManager.m_cellSize;
    //    //
    //    //// Assume grid starts at (0,0) bottom-left and extends width/height in world units
    //    //float gridWidth = m_gridManager.m_gridWidth * cs;
    //    //float gridHeight = m_gridManager.m_gridHeight * cs;
    //    //
    //    //// Half extents in world units
    //    //float halfWidth = (size.x * cs) / 2f;
    //    //float halfHeight = (size.y * cs) / 2f;
    //    //
    //    //// Clamp the object’s center so collider stays inside
    //    //float clampedX = Mathf.Clamp(pos.x, halfWidth, gridWidth - halfWidth);
    //    //float clampedY = Mathf.Clamp(pos.y, halfHeight, gridHeight - halfHeight);
    //    //
    //    //return new Vector3(clampedX, clampedY, pos.z);
    //
    //    // Bounds of the object in world space
    //    Collider2D col = GetComponent<Collider2D>();
    //    Bounds b = col.bounds;
    //
    //    // Grid borders
    //    Vector3 bottomLeft = m_gridManager.m_borders.BottomLeft;
    //    Vector3 topRight = m_gridManager.m_borders.TopRight;
    //
    //    // Clamp the object's position so its bounds stay inside grid
    //    Vector3 position = transform.position;
    //
    //    float halfWidth = b.extents.x;
    //    float halfHeight = b.extents.y;
    //
    //    position.x = Mathf.Clamp(position.x,
    //        bottomLeft.x + halfWidth,
    //        topRight.x - halfWidth);
    //
    //    position.y = Mathf.Clamp(position.y,
    //        bottomLeft.y + halfHeight,
    //        topRight.y - halfHeight);
    //
    //    transform.position = position;
    //}


    private Vector2 CalculatePrefabSize(GameObject prefab)
    {
        
        if (m_collider != null)
        {
            Vector2 sizeWorld = m_collider.bounds.size;
            float cs = m_gridManager.m_cellSize;

            int cellsX = Mathf.Max(1, Mathf.RoundToInt(sizeWorld.x / cs));
            int cellsY = Mathf.Max(1, Mathf.RoundToInt(sizeWorld.y / cs));

            return new Vector2(cellsX, cellsY);
        }

        // fallback if no collider
        return Vector2.one;
    }
}
