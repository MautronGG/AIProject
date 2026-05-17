//using System.Collections;
//using System.Collections.Generic;
//using System.IO;
//using UnityEditor;
//using UnityEngine;
//using UnityEngine.SceneManagement;

//public class LoadLevelInEditor : MonoBehaviour
//{
//    [Header("Scene Paths")]
//    [Tooltip("Path to the default empty level scene")]
//    string defaultLevelScene = "Assets/Mau/Scenes/Level_Default.unity";
//    string defaultEditorScene = "Assets/Mau/Scenes/Level_Editor.unity";

//    [Header("Level Source")]
//    [Tooltip("JSON filename saved at runtime (without .json)")]
//    public string jsonFileNameWithoutExt = "level_saved";

//    [Header("Prefabs")]
//    [Tooltip("Index = ID used by the editor")]
//    //public List<GameObject> levelPrefabList = new List<GameObject>();

//    [SerializeField] Dictionary<string, ObjectPrefabEntry> prefabLookup;

//    GameObject m_levelParent;
//    GameObject m_editorParent;

//    // =============================
//    // LOAD JSON
//    // =============================
//    LevelData LoadLevelDataFromJson()
//    {
//        string path = Path.Combine(Application.persistentDataPath, jsonFileNameWithoutExt + ".json");

//        if (!File.Exists(path))
//        {
//            Debug.LogError("JSON level file not found:\n" + path);
//            return null;
//        }

//        string json = File.ReadAllText(path);
//        return JsonUtility.FromJson<LevelData>(json);
//    }

//    // =============================
//    // BUILD LEVEL
//    // =============================
//    void BuildPlayableLevel(Scene scene, LevelData levelData)
//    {
//        m_levelParent = GameObject.FindGameObjectWithTag("LevelEditorManager");

//        var bridgesButton = FindObjectOfType<ButtonScript>();
//        if (bridgesButton != null)
//        {
//            bridgesButton.m_numBridges = levelData.bridges;
//            EditorUtility.SetDirty(bridgesButton);
//            EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
//        }

//        var sceneObjects = scene.GetRootGameObjects();
//        EditorBorderManager voids = null;
//        foreach (var sceneObject in sceneObjects)
//        {
//            voids = sceneObject.GetComponent<EditorBorderManager>();
//            if (voids != null)
//            {
//                break;
//            }
//        }

//        int voidsIndex = 0;

//        foreach (var data in levelData.objects)
//        {
//            int prefabIndex = 0;
//            if (!int.TryParse(data.id, out prefabIndex) ||
//                prefabIndex < 0 ||
//                prefabIndex >= prefabLookup.Count)
//            {
//                Debug.LogWarning($"Invalid prefab ID: {data.id}");
//                continue;
//            }

//            GameObject prefab = prefabLookup[prefabIndex.ToString()].playablePrefab;
//            if (prefab == null)
//                continue;

//            if (prefab.CompareTag("Void"))
//            {
//                if (voidsIndex == 0)
//                {
//                    Transformdata(voids.m_leftBorder, data);
//                    voidsIndex++;
//                    continue;
//                }
//                else if (voidsIndex == 1)
//                {
//                    Transformdata(voids.m_rightBorder, data);
//                    voidsIndex++;
//                    continue;
//                }
//                else if (voidsIndex == 2)
//                {
//                    Transformdata(voids.m_topBorder, data);
//                    voidsIndex++;
//                    continue;
//                }
//                else if (voidsIndex == 3)
//                {
//                    Transformdata(voids.m_bottomBorder, data);
//                    continue;
//                }
//            }


//            GameObject instance = (GameObject)PrefabUtility.InstantiatePrefab(prefab, scene);

//            if (m_levelParent != null)
//                instance.transform.SetParent(m_levelParent.transform, false);

//            // Root transform
//            Transformdata(instance.transform, data);

//            // Paired object support
//            LevelPairedObject paired = instance.GetComponent<LevelPairedObject>();
//            if (paired != null && data.children != null && data.children.Count == 2)
//            {
//                ApplyChildTransform(paired.m_childA.transform, data.children[0]);
//                ApplyChildTransform(paired.m_childB.transform, data.children[1]);
//            }
//        }
//    }

//    void BuildEditorLevel(Scene scene, LevelData levelData)
//    {
//        m_levelParent = GameObject.FindGameObjectWithTag("LevelEditorManager");

//        foreach (var data in levelData.objects)
//        {
//            if (!int.TryParse(data.id, out int prefabIndex) ||
//                prefabIndex < 0 ||
//                prefabIndex >= prefabLookup.Count)
//            {
//                Debug.LogWarning($"Invalid prefab ID: {data.id}");
//                continue;
//            }

//            GameObject prefab = prefabLookup[prefabIndex.ToString()].editorPrefab;
//            if (prefab == null)
//                continue;

//            GameObject instance =
//                (GameObject)PrefabUtility.InstantiatePrefab(prefab, scene);

//            if (m_levelParent != null)
//                instance.transform.SetParent(m_levelParent.transform, false);

//            // Root transform
//            Transformdata(instance.transform, data);

//            // Register editor data
//            var editorItem = instance.GetComponent<EditorItem>();
//            if (editorItem != null)
//            {
//                editorItem.id = data.id;
//                editorItem.data = data;
//            }

//            // Paired editor object
//            var paired = instance.GetComponent<EditorPairedObject>();
//            if (paired != null && data.children?.Count == 2)
//            {
//                ApplyChildTransform(paired.m_childA.transform, data.children[0]);
//                ApplyChildTransform(paired.m_childB.transform, data.children[1]);
//            }
//        }
//    }

//    void ApplyChildTransform(Transform child, LevelObjectData data)
//    {
//        child.localPosition = Vector3.zero;
//        child.localRotation = Quaternion.identity;
//        child.localScale = Vector3.one;

//        child.localPosition = data.position.ToVector3();
//        child.localRotation = data.rotation.ToQuaternion();
//        child.localScale = data.scale.ToVector3();
//    }

//    void Transformdata(Transform newData, LevelObjectData olddata)
//    {
//        newData.position = olddata.position.ToVector3();
//        newData.rotation = olddata.rotation.ToQuaternion();
//        newData.localScale = olddata.scale.ToVector3();
//    }
//}

using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadLevelInEditor : MonoBehaviour
{
    public enum LoadTarget
    {
        Editor,
        Playable
    }

    [Header("Scene Names")]
    [SerializeField] string editorSceneName = "Editor_Default";
    [SerializeField] string playableSceneName = "Level_Default";

    [Header("JSON")]
    [SerializeField] public string jsonFileNameWithoutExt = "";

    [Header("Prefabs (ID → Editor + Playable)")]
    [SerializeField] List<ObjectPrefabEntry> prefabEntries;

    Dictionary<string, ObjectPrefabEntry> prefabLookup;

    Transform editorParent;
    Transform levelParent;

    LoadTarget pendingTarget;

    [Header("Button Settings")]
    [SerializeField] LoadTarget defaultButtonTarget = LoadTarget.Editor;

    [Header("Editor Manager")]
    [SerializeField] GameObject editorManagerObj;
    [SerializeField] EditorManager editorManager;
    [SerializeField] EditorBorderManager editorBorderManager;

    void Start()
    {
        editorSceneName = "Editor_Default";

        Button btn = GetComponent<Button>();
        if (btn != null)
        {
            btn.onClick.AddListener(() => LoadLevel(defaultButtonTarget));
        }
    }

    // ─────────────────────────────────────────────
    // PUBLIC ENTRY
    // ─────────────────────────────────────────────
    public void LoadLevel(LoadTarget target)
    {
        pendingTarget = target;
        BuildPrefabLookup();

        string sceneName = target == LoadTarget.Editor
            ? editorSceneName
            : playableSceneName;

        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.LoadScene(sceneName);
    }

    // ─────────────────────────────────────────────
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;

        editorParent = GameObject.FindGameObjectWithTag("EditorParent")?.transform;
        levelParent = GameObject.FindGameObjectWithTag("LevelEditorManager")?.transform;

        editorManagerObj = GameObject.FindGameObjectWithTag("EditorManager");
        if (editorManagerObj != null)
        {
            editorManager = editorManagerObj.GetComponent<EditorManager>();
            editorBorderManager = editorManagerObj.GetComponent<EditorBorderManager>();
            if (editorManager != null && editorManager.objectPrefabs != null)
            {
                prefabEntries = editorManager.objectPrefabs;
                BuildPrefabLookup();
            }
        }

        LevelData levelData = LoadLevelDataFromJson();
        if (levelData == null)
            return;

        if (pendingTarget == LoadTarget.Editor)
        {
            CleanupEditorScene();
            BuildEditorLevel(levelData);
        }
        else
        {
            BuildPlayableLevel(levelData);
        }
    }

    // ─────────────────────────────────────────────
    // JSON
    // ─────────────────────────────────────────────
    LevelData LoadLevelDataFromJson()
    {
        string path = Path.Combine(
            Application.persistentDataPath,
            jsonFileNameWithoutExt + ".json"
        );

        if (!File.Exists(path))
        {
            Debug.LogError("Level JSON not found:\n" + path);
            return null;
        }

        return JsonUtility.FromJson<LevelData>(File.ReadAllText(path));
    }

    // ─────────────────────────────────────────────
    // PREFABS
    // ─────────────────────────────────────────────
    void BuildPrefabLookup()
    {
        prefabLookup = new Dictionary<string, ObjectPrefabEntry>();

        foreach (var entry in prefabEntries)
        {
            if (!string.IsNullOrEmpty(entry.id))
                prefabLookup[entry.id] = entry;
        }
    }

    // ─────────────────────────────────────────────
    // PLAYABLE
    // ─────────────────────────────────────────────
    void BuildPlayableLevel(LevelData levelData)
    {
        foreach (var data in levelData.objects)
        {
            if (!prefabLookup.TryGetValue(data.id, out var entry))
                continue;

            GameObject prefab = entry.playablePrefab;
            if (prefab == null)
                continue;

            GameObject instance = Instantiate(prefab, levelParent);
            ApplyTransform(instance.transform, data);

            LevelPairedObject paired = instance.GetComponent<LevelPairedObject>();
            if (paired != null && data.children?.Count == 2)
            {
                ApplyChildTransform(paired.m_childA.transform, data.children[0]);
                ApplyChildTransform(paired.m_childB.transform, data.children[1]);
            }
        }

        foreach (var data in levelData.voids)
        {
            if (!prefabLookup.TryGetValue(data.id, out var entry))
                continue;

            GameObject prefab = entry.playablePrefab;
            if (prefab == null)
                continue;

            GameObject instance = Instantiate(prefab, levelParent);
            ApplyVoidTransform(instance.transform, data);
        }
    }

    // ─────────────────────────────────────────────
    // EDITOR
    // ─────────────────────────────────────────────
    void BuildEditorLevel(LevelData levelData)
    {
        foreach (var data in levelData.objects)
        {
            if (!prefabLookup.TryGetValue(data.id, out var entry))
                continue;

            GameObject prefab = entry.editorPrefab;
            if (prefab == null)
                continue;

            GameObject instance = Instantiate(prefab, editorParent);
            ApplyTransform(instance.transform, data);

            EditorItem editorItem = instance.GetComponent<EditorItem>();
            if (editorItem != null)
            {
                editorItem.id = data.id;
                editorItem.data = data;
            }

            EditorPairedObject paired = instance.GetComponent<EditorPairedObject>();
            if (paired != null && data.children?.Count == 2)
            {
                ApplyChildTransform(paired.m_childA.transform, data.children[0]);
                ApplyChildTransform(paired.m_childB.transform, data.children[1]);
            }
        }

        foreach (var data in levelData.voids)
        {
            if (!prefabLookup.TryGetValue(data.id, out var entry))
                continue;

            GameObject prefab = entry.editorPrefab;
            if (prefab == null)
                continue;

            GameObject instance = Instantiate(prefab, editorParent);
            ApplyVoidTransform(instance.transform, data);

            EditorBorderDragger dragger = instance.GetComponent<EditorBorderDragger>();
            if (dragger != null)
            {
                dragger.id = data.id;
                dragger.data = data;
                dragger.m_maxCells = data.maxCells;
                dragger.m_cellsRemainingToMax = data.cellsRemainingToMax;
                dragger.m_cellsRemainingToMin = data.cellsRemainingToMin;
                dragger.m_type = data.type;

                if (editorBorderManager != null)
                {
                    if (dragger.m_type == EditorBorderDragger.Type.LeftBorder)
                        editorBorderManager.m_leftBorder = dragger.transform;
                    else if (dragger.m_type == EditorBorderDragger.Type.RightBorder)
                        editorBorderManager.m_rightBorder = dragger.transform;
                    else if (dragger.m_type == EditorBorderDragger.Type.TopBorder)
                        editorBorderManager.m_topBorder = dragger.transform;
                    else if (dragger.m_type == EditorBorderDragger.Type.BottomBorder)
                        editorBorderManager.m_bottomBorder = dragger.transform;
                }
            }
        }
    }

    // ─────────────────────────────────────────────
    // CLEANUP EDITOR SCENE
    // ─────────────────────────────────────────────
    void CleanupEditorScene()
    {
        if (editorParent == null)
            return;

        List<GameObject> toDelete = new List<GameObject>();

        foreach (Transform child in editorParent)
        {
            if (!child.CompareTag("Player") &&
                !child.CompareTag("Destiny"))
            {
                toDelete.Add(child.gameObject);
            }
        }

        foreach (var go in toDelete)
            Destroy(go);
    }

    // ─────────────────────────────────────────────
    // TRANSFORMS
    // ─────────────────────────────────────────────
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
        child.localPosition = data.position.ToVector3();
        child.localRotation = data.rotation.ToQuaternion();
        child.localScale = data.scale.ToVector3();
    }
}

