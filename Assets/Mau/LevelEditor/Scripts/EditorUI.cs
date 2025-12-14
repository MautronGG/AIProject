//using System.Collections;
//using System.Collections.Generic;
//using System.IO;
//using UnityEngine;

//public class EditorUI : MonoBehaviour
//{
//    public EditorTileBrush m_tileBrush;

//    public void SelectTile(int index)
//    {
//        m_tileBrush.m_selectedTileIndex = index;
//    }

//    public void SaveLevel()
//    {
//        EditorLevelData level = new EditorLevelData();
//        foreach (var tile in FindObjectsOfType<EditorItemButton>())
//        {
//            for (int i = 0; i < m_tileBrush.m_objectsPrefabs.Length; i++)
//            {
//                if (tile.gameObject.name.Contains(m_tileBrush.m_objectsPrefabs[i].name))
//                {
//                    level.m_tiles.Add(new TileData
//                    {
//                        m_prefabIndex = i,
//                        m_position = tile.transform.position
//                    });
//                }
//            }
//        }

//        string json = JsonUtility.ToJson(level, true);
//        File.WriteAllText(Application.persistentDataPath + "/level.json", json);
//        Debug.Log("Level saved to " + Application.persistentDataPath + "/level.json");
//    }

//    public void LoadLevel()
//    {
//        string path = Application.persistentDataPath + "/level.json";

//        if (File.Exists(path))
//        {
//            string json = File.ReadAllText(path);
//            EditorLevelData level = JsonUtility.FromJson<EditorLevelData>(json);

//            foreach (Transform child in GameObject.Find("LevelRoot").transform)
//            {
//                Destroy(child.gameObject);
//            }

//            foreach (var tile in level.m_tiles)
//            {
//                Instantiate(m_tileBrush.m_objectsPrefabs[tile.m_prefabIndex], tile.m_position, Quaternion.identity);
//            }
//            Debug.Log("Level loaded from " + path);
//        }
//        else
//        {
//            Debug.LogError("Level file not found at " + path);
//        }
//    }
//}
