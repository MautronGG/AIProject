
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using System.Collections.Generic;

public class LevelSceneCreator : MonoBehaviour
{
    [Header("Scene Paths")]
    [Tooltip("Path to the default empty level scene")]
    string defaultScenePath = "Assets/Mau/Scenes/Level_Default.unity";

    [Tooltip("Folder where generated scenes will be saved")]
    string outputFolder = "Assets/Mau/Scenes/GeneratedLevels";

    [Header("Level Source")]
    [Tooltip("JSON filename saved at runtime (without .json)")]
    public string jsonFileNameWithoutExt = "level_saved";

    [Header("Prefabs")]
    [Tooltip("Index = ID used by the editor")]
    //public List<GameObject> levelPrefabList = new List<GameObject>();

    EditorManager m_editorManager;

    GameObject m_levelParent;

    // =============================
    // CONTEXT MENU ENTRY
    // =============================
    [ContextMenu("Generate Scene From JSON Level")]
    public void GenerateSceneFromJson()
    {
        // Validate paths
        if (!File.Exists(defaultScenePath))
        {
            Debug.LogError("Default scene not found: " + defaultScenePath);
            return;
        }

        // Load JSON level
        LevelData levelData = LoadLevelDataFromJson();
        if (levelData == null)
            return;

        // Create output folder if needed
        if (!Directory.Exists(outputFolder))
            Directory.CreateDirectory(outputFolder);

        // Copy default scene
        string sceneName = $"Generated_{levelData.levelName}_{System.DateTime.Now:yyyyMMdd_HHmmss}.unity";
        string sceneCopyPath = Path.Combine(outputFolder, sceneName)
            .Replace(Path.DirectorySeparatorChar, '/');

        File.Copy(defaultScenePath, sceneCopyPath, true);
        AssetDatabase.Refresh();

        // Open copied scene
        Scene scene = EditorSceneManager.OpenScene(sceneCopyPath, OpenSceneMode.Single);
        EditorSceneManager.SetActiveScene(scene);

        // Instantiate level objects
        BuildLevelFromData(scene, levelData);

        // Save final scene
        EditorSceneManager.SaveScene(scene);
        Debug.Log("Scene generated: " + sceneCopyPath);
    }

    // =============================
    // LOAD JSON
    // =============================
    LevelData LoadLevelDataFromJson()
    {
        string path = Path.Combine(Application.persistentDataPath, jsonFileNameWithoutExt + ".json");

        if (!File.Exists(path))
        {
            Debug.LogError("JSON level file not found:\n" + path);
            return null;
        }

        string json = File.ReadAllText(path);
        return JsonUtility.FromJson<LevelData>(json);
    }

    // =============================
    // BUILD LEVEL
    // =============================
    void BuildLevelFromData(Scene scene, LevelData levelData)
    {
        m_levelParent = GameObject.FindGameObjectWithTag("LevelEditorManager");

        var bridgesButton = FindObjectOfType<ButtonScript>();
        if (bridgesButton != null)
        {
            bridgesButton.m_numBridges = levelData.bridges;
            EditorUtility.SetDirty(bridgesButton);
            EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
        }

        var sceneObjects = scene.GetRootGameObjects();
        EditorBorderManager voids = null;
        foreach (var sceneObject in sceneObjects)
        {
            voids = sceneObject.GetComponent<EditorBorderManager>();
            if (voids != null)
            {
                break;
            }
        }

        int voidsIndex = 0;

        foreach (var data in levelData.objects)
        {
            int prefabIndex = 0;
            if (!int.TryParse(data.id, out prefabIndex) ||
                prefabIndex < 0 ||
                prefabIndex >= m_editorManager.prefabLookup.Count)
            {
                Debug.LogWarning($"Invalid prefab ID: {data.id}");
                continue;
            }



            GameObject prefab = m_editorManager.prefabLookup[prefabIndex.ToString()].playablePrefab;
            if (prefab == null)
                continue;

            if (prefab.CompareTag("Void"))
            {
                if (voidsIndex == 0)
                {
                    Transformdata(voids.m_leftBorder, data);
                    voidsIndex++;
                    continue;
                }
                else if (voidsIndex == 1)
                {
                    Transformdata(voids.m_rightBorder, data);
                    voidsIndex++;
                    continue;
                }
                else if (voidsIndex == 2)
                {
                    Transformdata(voids.m_topBorder, data);
                    voidsIndex++;
                    continue;
                }
                else if (voidsIndex == 3)
                {
                    Transformdata(voids.m_bottomBorder, data);
                    continue;
                }
            }


            GameObject instance = (GameObject)PrefabUtility.InstantiatePrefab(prefab, scene);

            if (m_levelParent != null)
                instance.transform.SetParent(m_levelParent.transform, false);

            // Root transform
            Transformdata(instance.transform, data);

            // Paired object support
            LevelPairedObject paired = instance.GetComponent<LevelPairedObject>();
            if (paired != null && data.children != null && data.children.Count == 2)
            {
                ApplyChildTransform(paired.m_childA.transform, data.children[0]);
                ApplyChildTransform(paired.m_childB.transform, data.children[1]);
            }
        }
    }

    void ApplyChildTransform(Transform child, LevelObjectData data)
    {
        child.localPosition = Vector3.zero;
        child.localRotation = Quaternion.identity;
        child.localScale = Vector3.one;

        child.localPosition = data.position.ToVector3();
        child.localRotation = data.rotation.ToQuaternion();
        child.localScale = data.scale.ToVector3();
    }

    void Transformdata(Transform newData, LevelObjectData olddata)
    {
        newData.position = olddata.position.ToVector3();
        newData.rotation = olddata.rotation.ToQuaternion();
        newData.localScale = olddata.scale.ToVector3();
    }
}
#endif
