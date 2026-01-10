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
    Verify
}

public interface IEditorAction
{
    void Undo();
    void Redo();
}

public class MoveAction : IEditorAction
{
    private LevelObjectData data;
    private Vector3 oldPos;
    private Vector3 newPos;

    public MoveAction(LevelObjectData data, Vector3 oldPos, Vector3 newPos)
    {
        this.data = data;
        this.oldPos = oldPos;
        this.newPos = newPos;
    }

    public void Undo()
    {
        data.position = SerializableVector3.From(oldPos);
        if (data.editorInstance)
            data.editorInstance.transform.position = oldPos;
    }

    public void Redo()
    {
        data.position = SerializableVector3.From(newPos);
        if (data.editorInstance)
            data.editorInstance.transform.position = newPos;
    }
}

public class DeleteAction : IEditorAction
{
    private LevelObjectData data;

    public DeleteAction(LevelObjectData data)
    {
        this.data = data;
    }

    public void Undo()
    {
        if (data.editorInstance)
            data.editorInstance.SetActive(true);
    }

    public void Redo()
    {
        if (data.editorInstance)
            data.editorInstance.SetActive(false);
    }
}

public class CreateObjectAction : IEditorAction
{
    private LevelObjectData data;

    public CreateObjectAction(LevelObjectData data)
    {
        this.data = data;
    }

    public void Undo()
    {
        if (data.editorInstance != null)
            data.editorInstance.SetActive(false);
    }

    public void Redo()
    {
        if (data.editorInstance != null)
            data.editorInstance.SetActive(true);
    }
}

public class EditorManager : MonoBehaviour
{
    [Header("Canvas")]
    public GameObject m_optionsCanvas;
    public GameObject m_HUDCanvas;
    public GameObject m_pauseCanvas;
    public Button m_playButton;
    //public GameObject m_typeButtons;
    //public GameObject m_ObjectsButtons;

    public ButtonSelectionTracker[] m_buttonSelectionTrackers;

    public bool m_pause = false;

    public bool m_isEditing = false;

    public EditorGridManager m_grid;
    public EditorItem m_item;
    public EditorSpriteFollow m_spriteFollow;

    public List<GameObject> m_editorItemPrefabs = new List<GameObject>();
    public List<GameObject> m_levelItemPrefabs = new List<GameObject>();

    public static EditorManager Instance;

    [Header("Roots")]
    public Transform editorRoot;
    public Transform playableRoot;

    [Header("Prefabs")]
    public List<ObjectPrefabEntry> objectPrefabs;

    public LevelData currentLevel = new();

    private Dictionary<string, ObjectPrefabEntry> prefabLookup;

    private Stack<IEditorAction> undoStack = new();
    private Stack<IEditorAction> redoStack = new();

    public EditorMode currentMode = EditorMode.Edit;

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
    }

    public ObjectPrefabEntry GetPrefab(string id)
        => prefabLookup[id];

    // ---------------- UNDO / REDO ----------------
    public void DoAction(IEditorAction action)
    {
        action.Redo();
        undoStack.Push(action);
        redoStack.Clear();
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

        foreach (var item in items)
        {
            var data = item.data;

            data.id = item.id;
            data.position = SerializableVector3.From(item.transform.position);
            data.rotation = SerializableQuaternion.From(item.transform.rotation);
            data.scale = SerializableVector3.From(item.transform.localScale);

            currentLevel.objects.Add(data);
        }
    }


    // ---------------- VERIFICATION ----------------
    public void StartVerification()
    {
        editorRoot.gameObject.SetActive(false);
        playableRoot.gameObject.SetActive(true);

        foreach (Transform c in playableRoot)
            Destroy(c.gameObject);

        foreach (var data in currentLevel.objects)
        {
            Instantiate(
                prefabLookup[data.id].playablePrefab,
                data.position.ToVector3(),
                data.rotation.ToQuaternion(),
                playableRoot
            );
        }
    }

    public void StopVerification()
    {
        foreach (Transform c in playableRoot)
            Destroy(c.gameObject);

        playableRoot.gameObject.SetActive(false);
        editorRoot.gameObject.SetActive(true);
    }
}
