using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.Analytics;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;


public interface IEditorAction
{
    void Undo();
    void Redo();
}

public class MoveAction : IEditorAction
{
    private GameObject obj;
    private Vector3 oldPos;
    private Vector3 newPos;

    public MoveAction(GameObject obj, Vector3 oldPos, Vector3 newPos)
    {
        this.obj = obj;
        this.oldPos = oldPos;
        this.newPos = newPos;
    }

    public void Undo()
    {
        if (obj != null) obj.transform.position = oldPos;
    }

    public void Redo()
    {
        if (obj != null) obj.transform.position = newPos;
    }
}

public class DeleteAction : IEditorAction
{
    private GameObject obj;
    private Vector3 position;
    private Quaternion rotation;
    private Transform parent;

    public DeleteAction(GameObject obj)
    {
        this.obj = obj;
        this.position = obj.transform.position;
        this.rotation = obj.transform.rotation;
        this.parent = obj.transform.parent;
    }

    public void Undo()
    {
        if (obj != null) obj.SetActive(true);
        obj.transform.position = position;
        obj.transform.rotation = rotation;
        obj.transform.parent = parent;
    }

    public void Redo()
    {
        if (obj != null) obj.SetActive(false);
    }
}

public class CreateObjectAction : IEditorAction
{
    private Vector3 position;
    private Quaternion rotation;
    private GameObject createdInstance;

    public CreateObjectAction(Vector3 position, Quaternion rotation, GameObject instance)
    {
        this.position = position;
        this.rotation = rotation;
        this.createdInstance = instance;
    }

    public void Undo()
    {
        if (createdInstance != null)
        {
            //GameObject.Destroy(createdInstance);
            createdInstance.SetActive(false);
        }
    }

    public void Redo()
    {
        if (!createdInstance.activeInHierarchy) // If deleted in Undo
        {
            createdInstance.SetActive(true);
        }
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
    private Stack<IEditorAction> undoStack = new Stack<IEditorAction>();
    private Stack<IEditorAction> redoStack = new Stack<IEditorAction>();


    private void Awake()
    {
        Time.timeScale = 1.0f;
        m_buttonSelectionTrackers = m_HUDCanvas.GetComponentsInChildren<ButtonSelectionTracker>();  
    }
   //private void Start()
   //{
   //    //if (m_typeButtons)
   //    //{
   //    //    m_typeButtons.SetActive(false);
   //    //}
   //    //if (m_ObjectsButtons)
   //    //{
   //    //    m_ObjectsButtons.SetActive(false);
   //    //}
   //}
    private void Update()
    {
        ///To Pause Game
        if (Input.GetKeyDown(KeyCode.Escape) && !m_pause && !m_isEditing)
        {
            Time.timeScale = 0.0f;
            m_pauseCanvas.SetActive(true);
            m_pause = true;
            m_HUDCanvas.SetActive(false);
        }
        ///To move Sprite when placing in Level
        Vector2 screenPosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        Vector2 worldPosition = Camera.main.ScreenToWorldPoint(screenPosition);

        if (Input.GetKeyDown(KeyCode.Z)) Undo();
        if (Input.GetKeyDown(KeyCode.Y)) Redo();
    }

    public void UnPause()
    {
        Time.timeScale = 1f;
        m_pause = false;
        m_HUDCanvas.SetActive(true);
        m_pauseCanvas.SetActive(false);
    }

    public void DoAction(IEditorAction action)
    {
        action.Redo();
        undoStack.Push(action);
        redoStack.Clear();
    }

    public void Undo()
    {
        if (undoStack.Count > 0)
        {
            IEditorAction action = undoStack.Pop();
            action.Undo();
            redoStack.Push(action);
        }
    }

    public void Redo()
    {
        if (redoStack.Count > 0)
        {
            IEditorAction action = redoStack.Pop();
            action.Redo();
            undoStack.Push(action);
        }
    }

    //public void SaveLevel()
    //{
    //    SaveLoadManager.SaveLevel(levelData, "Level01");
    //}
}
