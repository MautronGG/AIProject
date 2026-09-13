using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderKeywordFilter;
using UnityEngine;

public class EditorTileBrush : MonoBehaviour
{
    public EditorGridManager m_grid;
    public GameObject[] m_objectsPrefabs;
    public int m_selectedTileIndex = 0;
    
    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            PlaceTile();
        }
        if (Input.GetMouseButton(1))
        {
            RemoveTile();
        }
    }
    
    void PlaceTile()
    {
        Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorld.z = 0;
        Vector2Int gridPos = m_grid.GetGridPosition(mouseWorld);
    
        Collider2D hit = Physics2D.OverlapPoint(m_grid.GetWorldPosition(gridPos.x, gridPos.y));
        if (hit == null)
        {
            Instantiate(m_objectsPrefabs[m_selectedTileIndex], m_grid.GetWorldPosition(gridPos.x, gridPos.y), Quaternion.identity);
        }
    }
    
    void RemoveTile()
    {
        Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorld.z = 0;
        Collider2D hit = Physics2D.OverlapPoint(mouseWorld);
        if (hit != null)
            Destroy(hit.gameObject);
    }
}
