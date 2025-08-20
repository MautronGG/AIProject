using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class EditorGridManager : MonoBehaviour
{
    [Header("Grid")]
    public float m_cellSize = 1f;
    private float m_lastCellSize;
    private Mesh m_gridMesh;
    public Color m_gridColor = Color.gray;

    [Header("Border")]
    public EditorBorderManager m_borders;
    private Vector3 m_lastBottomLeft;
    private Vector3 m_lastTopRight;


    private void Awake()
    {
        //MeshRenderer mr = GetComponent<MeshRenderer>();
        //mr.material = new Material(Shader.Find("Unlit/Color"));
        //mr.material.color = m_gridColor;

        var mr = GetComponent<MeshRenderer>();
        if (mr.sharedMaterial == null)
        {
            mr.material = new Material(Shader.Find("Unlit/Color"));
        }
        mr.material.color = m_gridColor;

        m_gridMesh = new Mesh { name = "GridMesh" };
        GetComponent<MeshFilter>().mesh = m_gridMesh;
    }

    private void Start()
    {
        UpdateGrid();
    }

    private void Update()
    {
        // Only rebuild grid if borders or cell size changed
        if (m_borders.BottomLeft != m_lastBottomLeft ||
            m_borders.TopRight != m_lastTopRight ||
            Mathf.Abs(m_cellSize - m_lastCellSize) > 0.0001f)
        {
            UpdateGrid();
        }
    }

    public void UpdateGrid()
    {
        Vector3 bottomLeft = m_borders.BottomLeft;
        Vector3 topRight = m_borders.TopRight;
   
        //
        //int width = Mathf.RoundToInt((topRight.x - bottomLeft.x) / m_cellSize);
        //int height = Mathf.RoundToInt((topRight.y - bottomLeft.y) / m_cellSize);
        //
        //if (width <= 0 || height <= 0) return;

        float widthWorld = Mathf.Max(0f, topRight.x - bottomLeft.x);
        float heightWorld = Mathf.Max(0f, topRight.y - bottomLeft.y);

        int cols = Mathf.RoundToInt(widthWorld / m_cellSize);
        int rows = Mathf.RoundToInt(heightWorld / m_cellSize);

        if (cols < 0) cols = 0;
        if (rows < 0) rows = 0;
        //

        List<Vector3> vertices = new List<Vector3>();
        List<int> indices = new List<int>();

        // Vertical lines
        for (int x = 0; x <= cols; x++)
        {
            float vx = bottomLeft.x + x * m_cellSize;
            vertices.Add(new Vector3(vx, bottomLeft.y, 0f));
            vertices.Add(new Vector3(vx, bottomLeft.y + rows * m_cellSize, 0f));
            int s = vertices.Count - 2;
            indices.Add(s); indices.Add(s + 1);
        }

        // Horizontal lines
        for (int y = 0; y <= rows; y++)
        {
            float vy = bottomLeft.y + y * m_cellSize;
            vertices.Add(new Vector3(bottomLeft.x, vy, 0f));
            vertices.Add(new Vector3(bottomLeft.x + cols * m_cellSize, vy, 0f));
            int s = vertices.Count - 2;
            indices.Add(s); indices.Add(s + 1);
        }

        m_gridMesh.Clear();
        m_gridMesh.SetVertices(vertices);
        m_gridMesh.SetIndices(indices, MeshTopology.Lines, 0);

        // cache last state
        m_lastBottomLeft = bottomLeft;
        m_lastTopRight = topRight;
        m_lastCellSize = m_cellSize;

        //// Vertical lines
        //for (int x = 0; x <= width; x++)
        //{
        //    vertices.Add(new Vector3(bottomLeft.x + x * m_cellSize, bottomLeft.y, 0));
        //    vertices.Add(new Vector3(bottomLeft.x + x * m_cellSize, topRight.y, 0));
        //    int startIndex = vertices.Count - 2;
        //    indices.Add(startIndex);
        //    indices.Add(startIndex + 1);
        //}
        //
        //// Horizontal lines
        //for (int y = 0; y <= height; y++)
        //{
        //    vertices.Add(new Vector3(bottomLeft.x, bottomLeft.y + y * m_cellSize, 0));
        //    vertices.Add(new Vector3(topRight.x, bottomLeft.y + y * m_cellSize, 0));
        //    int startIndex = vertices.Count - 2;
        //    indices.Add(startIndex);
        //    indices.Add(startIndex + 1);
        //}
        //
        //if (m_gridMesh == null)
        //{
        //    m_gridMesh = new Mesh();
        //    m_gridMesh.name = "GridMesh";
        //    GetComponent<MeshFilter>().mesh = m_gridMesh;
        //}
        //else
        //{
        //    m_gridMesh.Clear();
        //}
        //
        //m_gridMesh.SetVertices(vertices);
        //m_gridMesh.SetIndices(indices.ToArray(), MeshTopology.Lines, 0);
        //
        //m_lastBottomLeft = bottomLeft;
        //m_lastTopRight = topRight;
        //m_lastCellSize = m_cellSize;
    }




   public Vector3 GetWorldPosition(int x, int y)
   {
       return new Vector3(x, y) * m_cellSize;
   }
   
   public Vector2Int GetGridPosition(Vector3 worldPosition)
   {
       int x = Mathf.RoundToInt(worldPosition.x / m_cellSize);
       int y = Mathf.RoundToInt(worldPosition.y / m_cellSize);
       return new Vector2Int(x, y);
   }
}
