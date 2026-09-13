
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
    public string defaultScenePath = "Assets/Mau/Scenes/Final/Level_Default.unity";

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
        string path = Path.Combine(SaveLoadManager.DefaultSavePath, jsonFileNameWithoutExt + ".json");

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

        foreach (var data in levelData.objects)
        {
<<<<<<< HEAD
            int prefabIndex = 0;
            if (!int.TryParse(data.id, out prefabIndex) ||
                prefabIndex < 0 ||
                prefabIndex >= m_editorManager.prefabLookup.Count)
            {
                Debug.LogWarning($"Invalid prefab ID: {data.id}");
=======
            if (!prefabLookup.TryGetValue(data.id, out var entry))
>>>>>>> MauGG
                continue;

<<<<<<< HEAD


            GameObject prefab = m_editorManager.prefabLookup[prefabIndex.ToString()].playablePrefab;
=======
            GameObject prefab = entry.playablePrefab;
>>>>>>> MauGG
            if (prefab == null)
                continue;

            GameObject instance = (GameObject)PrefabUtility.InstantiatePrefab(prefab, scene);
            if (m_levelParent != null)
                instance.transform.SetParent(m_levelParent.transform, false);

            ApplyTransform(instance.transform, data);

            LevelPairedObject paired = instance.GetComponent<LevelPairedObject>();
            if (paired != null && data.children?.Count == 2)
            {
                ApplyChildTransform(paired.m_childA.transform, data.children[0]);
                ApplyChildTransform(paired.m_childB.transform, data.children[1]);
            }
        }

        if (levelData.voids != null)
        {
            int voidIndex = 0;
            EditorBorderManager editorBorderManager = null;
            if (m_levelParent != null)
                editorBorderManager = m_levelParent.GetComponent<EditorBorderManager>();

            if (editorBorderManager == null)
                editorBorderManager = FindObjectOfType<EditorBorderManager>();
            foreach (var data in levelData.voids)
            {
                if (!prefabLookup.TryGetValue(data.id, out var entry))
                    continue;

                GameObject prefab = entry.playablePrefab;
                if (prefab == null)
                    continue;

                GameObject instance = (GameObject)PrefabUtility.InstantiatePrefab(prefab, scene);
                if (m_levelParent != null)
                    instance.transform.SetParent(m_levelParent.transform, false);

                ApplyVoidTransform(instance.transform, data);

                if (editorBorderManager != null)
                {
                    if (voidIndex == 0)
                        editorBorderManager.m_leftBorder = instance.transform;
                    else if (voidIndex == 1)
                        editorBorderManager.m_rightBorder = instance.transform;
                    else if (voidIndex == 2)
                        editorBorderManager.m_topBorder = instance.transform;
                    else if (voidIndex == 3)
                        editorBorderManager.m_bottomBorder = instance.transform;
                }

                voidIndex++;
            }
        }

        void ApplyTransform(Transform target, LevelObjectData data)
        {
            target.position = data.position.ToVector3();
            target.rotation = data.rotation.ToQuaternion();
            target.localScale = data.scale.ToVector3();
        }

        void ApplyVoidTransform(Transform target, LevelVoidData data)
        {
            target.position = data.position.ToVector3();
            target.rotation = data.rotation.ToQuaternion();
            target.localScale = data.scale.ToVector3();
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
    }
}
#endif
