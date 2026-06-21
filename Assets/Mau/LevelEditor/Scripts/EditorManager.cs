using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.Analytics;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;

public enum EditorMode
{
    Edit,
    Verify,
    Publishing
}

public interface IEditorAction
{
    void Undo();
    void Redo();
}

public class MoveAction : IEditorAction
{
    private LevelObjectData data;

    Transform targetTransform;

    private Vector3 oldPos;
    private Vector3 newPos;

    bool useLocalSpace;

    public MoveAction(LevelObjectData data, Transform targetTransform, Vector3 oldPos, Vector3 newPos, bool useLocalSpace)
    {
        this.data = data;
        this.targetTransform = targetTransform;
        this.oldPos = oldPos;
        this.newPos = newPos;
        this.useLocalSpace = useLocalSpace;
    }

    public void Undo()
    {
        //data.position = SerializableVector3.From(oldPos);
        //if (data.editorInstance)
        //    data.editorInstance.transform.position = oldPos;

        Apply(oldPos);
    }

    public void Redo()
    {
        //data.position = SerializableVector3.From(newPos);
        //if (data.editorInstance)
        //    data.editorInstance.transform.position = newPos;

        Apply(newPos);
    }

    private void Apply(Vector3 pos)
    {
        data.position = SerializableVector3.From(pos);

        if (targetTransform == null)
            return;

        if (useLocalSpace)
            targetTransform.localPosition = pos;
        else
            targetTransform.position = pos;
    }
}

public class DeleteAction : IEditorAction
{
    private LevelObjectData data;
    EditorItem item;
    EditorManager manager;
    int index;

    public DeleteAction(EditorItem item)
    {
        this.item = item;
        this.data = item.data;
        this.manager = EditorManager.Instance;
    }

    public void Redo()
    {
        index = manager.currentLevel.objects.IndexOf(data);
        manager.currentLevel.objects.Remove(data);
        item.gameObject.SetActive(false);
    }

    public void Undo()
    {
        manager.currentLevel.objects.Insert(index, data);
        item.gameObject.SetActive(true);
    }
}

public class CreateObjectAction : IEditorAction
{
    private LevelObjectData data;
    private EditorManager manager;


    public CreateObjectAction(LevelObjectData data)
    {
        this.data = data;
        this.manager = EditorManager.Instance;
    }

    public void Undo()
    {
        manager.currentLevel.objects.Remove(data);

        if (data.editorInstance != null)
            data.editorInstance.SetActive(false);
    }

    public void Redo()
    {
        if (!manager.currentLevel.objects.Contains(data))
            manager.currentLevel.objects.Add(data);

        if (data.editorInstance != null)
            data.editorInstance.SetActive(true);
    }
}


public class EditorManager : MonoBehaviour
{
    [Header("Canvas")]
    //public GameObject m_optionsCanvas;
    public GameObject m_globalVerification;
    public GameObject m_HUDCanvas;
    public GameObject m_pauseCanvas;
    public Button m_playButton;
    public ButtonScript m_editorBridgeCounter;
    public ButtonScript m_levelBridgeCounter;
    public GameObject m_publishingCanvas;
    //public GameObject m_typeButtons;
    //public GameObject m_ObjectsButtons;

    public List<ButtonSelectionTracker> m_buttonSelectionTrackers;

    public bool m_pause = false;

    public bool m_isEditing = false;

    public EditorGridManager m_grid;
    public EditorItem m_item;
    public EditorSpriteFollow m_spriteFollow;
    public LevelManager m_levelManager;

    //public List<GameObject> m_editorItemPrefabs = new List<GameObject>();
    //public List<GameObject> m_levelItemPrefabs = new List<GameObject>();

    public static EditorManager Instance;

    [Header("Roots")]
    public Transform editorRoot;
    public Transform playableRoot;

    [Header("Prefabs")]
    public List<ObjectPrefabEntry> objectPrefabs;

    public LevelData currentLevel = new();

    public Dictionary<string, ObjectPrefabEntry> prefabLookup;

    private Stack<IEditorAction> undoStack = new();
    private Stack<IEditorAction> redoStack = new();

    public EditorMode currentMode = EditorMode.Edit;

    public AudioManager m_audioManager;

    private void Awake()
    {
        Instance = this;

        prefabLookup = new Dictionary<string, ObjectPrefabEntry>();
        foreach (var entry in objectPrefabs)
            prefabLookup[entry.id] = entry;

        playableRoot.gameObject.SetActive(false);

    }

    private void Start()
    {
        InitializeEditorObjects();
        //var HUD = m_HUDCanvas.GetComponentsInChildren<ButtonSelectionTracker>();
        //foreach (ButtonSelectionTracker button in HUD)
        //    m_buttonSelectionTrackers.Add(button);

        //var global = m_globalVerification.GetComponentsInChildren<ButtonSelectionTracker>();
        //foreach (ButtonSelectionTracker button in global)
        //    m_buttonSelectionTrackers.Add(button);
    }

    public ObjectPrefabEntry GetPrefab(string id) => prefabLookup[id];

    // ---------------- UNDO / REDO ----------------
    public void DoAction(IEditorAction action)
    {
        action.Redo();
        undoStack.Push(action);
        redoStack.Clear();

        PrintUndoStack();
    }

    public void Undo()
    {
        if (undoStack.Count == 0) return;
        var a = undoStack.Pop();
        a.Undo();
        redoStack.Push(a);
    }

    public void Redo()
    {
        if (redoStack.Count == 0) return;
        var a = redoStack.Pop();
        a.Redo();
        undoStack.Push(a);
    }

    public void InitializeEditorObjects()
    {
        currentLevel.objects.Clear();

        var items = editorRoot.GetComponentsInChildren<EditorItem>(true);
        var borders = editorRoot.GetComponentsInChildren<EditorBorderDragger>(true);

        foreach (var border in borders)
        {
            if(border.data == null)
                border.data = new LevelObjectData();

            var data = border.data;

            data.id = border.id;
            data.position = SerializableVector3.From(border.transform.position);
            data.rotation = SerializableQuaternion.From(border.transform.rotation);
            data.scale = SerializableVector3.From(border.transform.localScale);

            data.editorInstance = border.gameObject;

            currentLevel.objects.Add(data);
        }

        foreach (var item in items)
        {
            if (item.data == null)
                item.data = new LevelObjectData();

            var data = item.data;

            data.id = item.id;
            data.position = SerializableVector3.From(item.transform.position);
            data.rotation = SerializableQuaternion.From(item.transform.rotation);
            data.scale = SerializableVector3.From(item.transform.localScale);

            data.editorInstance = item.gameObject;

            currentLevel.objects.Add(data);
        }

    }

    public void Pause()
    {
        Time.timeScale = 0.0f;
        m_pauseCanvas.SetActive(true);
        m_HUDCanvas.SetActive(false);
        m_pause = true;
    }
    public void UnPause()
    {
        Time.timeScale = 1f;
        m_pause = false;
        m_pauseCanvas.SetActive(false);
        m_HUDCanvas.SetActive(true);
    }


    // ---------------- VERIFICATION ----------------
    public void StartVerification()
    {
        foreach (var item in editorRoot.GetComponentsInChildren<EditorItem>())
            item.ForceSyncData();

        foreach (var border in editorRoot.GetComponentsInChildren<EditorBorderDragger>())
            border.ForceSyncData();

        //DebugDumpLevelData();

        foreach (Transform c in playableRoot)
            Destroy(c.gameObject);


        foreach (var data in currentLevel.objects)
        {
            var obj = Instantiate(
                prefabLookup[data.id].playablePrefab,
                data.position.ToVector3(),
                data.rotation.ToQuaternion(),
                playableRoot
            );
            
            obj.transform.localScale = data.scale.ToVector3();

            if (obj.TryGetComponent(out MinionMovement minion))
            {
                minion.CaptureSpawnPosition();
                minion.m_defaultPositionedInitialized = true;
            }

            if (data.children != null && data.children.Count > 0)
            {
                var paired = obj.GetComponent<LevelPairedObject>();

                ApplyChildData(paired.m_childA, data.children[0]);
                ApplyChildData(paired.m_childB, data.children[1]);
            }
        }

        currentMode = EditorMode.Verify;

        editorRoot.gameObject.SetActive(false);
        playableRoot.gameObject.SetActive(true);
        m_levelManager.gameObject.SetActive(true);

        m_levelBridgeCounter.m_numBridges = m_editorBridgeCounter.m_numBridges;
        m_levelBridgeCounter.ApplyText();

        m_levelManager.Initialized();
    }

    public void StopVerification()
    {
        foreach (Transform c in playableRoot)
            Destroy(c.gameObject);

        StopVerificationEvents();

        currentMode = EditorMode.Edit;

        playableRoot.gameObject.SetActive(false);
        m_levelManager.gameObject.SetActive(false);
        editorRoot.gameObject.SetActive(true);
    }

    public void StopVerificationEvents()
    {
        var camera = Camera.main.GetComponent<CameraMovement>();
        camera.ChangeMovement(true);
        camera.ResetTransform();
        camera.AutomaticMovement(false);
        m_globalVerification.SetActive(true);

        foreach (TurnOffGameObject canvas in m_levelManager.m_canvases)
        {
            canvas.ResetState();
        }
        m_levelManager.ResetDefaults();
    }

    void ApplyChildData(GameObject child, LevelObjectData d)
    {
        child.transform.localPosition = d.position.ToVector3();
        child.transform.localRotation = d.rotation.ToQuaternion();
        child.transform.localScale = d.scale.ToVector3();
    }

    public void DebugDumpLevelData()
    {
        Debug.Log("=== LEVEL DATA DUMP ===");

        foreach (var obj in currentLevel.objects)
        {
            Debug.Log($"PARENT {obj.id} pos={obj.position.x},{obj.position.y},{obj.position.z}");

            if (obj.children != null)
            {
                for (int i = 0; i < obj.children.Count; i++)
                {
                    var c = obj.children[i];
                    Debug.Log($"  CHILD[{i}] pos={c.position.x},{c.position.y},{c.position.z}");
                }
            }
        }
    }

    string ActionToString(IEditorAction action)
    {
        if (action is CreateObjectAction) return "CreateObjectAction";
        if (action is MoveAction) return "MoveAction";
        if (action is DeleteAction) return "DeleteAction";

        return action.GetType().Name;
    }

    void PrintUndoStack()
    {
        Debug.Log("=== UNDO STACK (top → bottom) ===");

        if (undoStack.Count == 0)
        {
            Debug.Log("(empty)");
            return;
        }

        int i = 0;
        foreach (var action in undoStack)
        {
            Debug.Log($"[{i}] {ActionToString(action)}");
            i++;
        }
    }

}
