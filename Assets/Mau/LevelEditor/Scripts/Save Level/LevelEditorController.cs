using UnityEngine;
using TMPro;
using UnityEditor;

public class LevelEditorController : MonoBehaviour
{
    [Header("Save UI")]
    [SerializeField] private GameObject saveCanvas;
    [SerializeField] private TMP_InputField levelNameInput;

    private bool isWaitingForName;

    EditorManager m_editorManager;

    private void Start()
    {
        m_editorManager = EditorManager.Instance;
        levelNameInput.text = "NewLevel";
        levelNameInput.MoveTextEnd(false);
    }

    private void Update()
    {
        // Step 1: Press L → open input
        if (Input.GetKeyDown(KeyCode.L) && !isWaitingForName)
        {
            OpenSaveInput();
            return;
        }

        // Step 5: Press K → save
        if (isWaitingForName && Input.GetKeyDown(KeyCode.Return))
        {
            ConfirmSave();
        }

        if (isWaitingForName && Input.GetKeyDown(KeyCode.Escape))
        {
            CloseSaveInput();
        }

        //if (Input.GetKeyDown(KeyCode.L))
        //{
        //    SaveCurrentLevel("Kappa");
        //}
    }

    public void SaveCurrentLevel(string filename)
    {
        var level = m_editorManager.currentLevel;
        level.bridges = m_editorManager.m_editorBridgeCounter.m_numBridges;
        level.levelName = filename;
        SaveLoadManager.SaveLevel(level, filename);
    }

    public void OpenSaveInput()
    {
        Camera.main.GetComponent<CameraMovement>().m_canMove = false;
        m_editorManager.currentMode = EditorMode.Publishing;

        isWaitingForName = true;

        saveCanvas.SetActive(true);

        // Focus input field
        levelNameInput.ActivateInputField();
        levelNameInput.Select();

        // Optional: pause editor interactions
        m_editorManager.m_isEditing = true;

        m_editorManager.m_HUDCanvas.SetActive(false);
        m_editorManager.m_globalVerification.SetActive(false); 
    }

    public void ConfirmSave()
    {
        string filename = levelNameInput.text.Trim();

        if (string.IsNullOrEmpty(filename))
        {
            Debug.LogWarning("Level name is empty");
            return;
        }

        SaveCurrentLevel(filename);

        CloseSaveInput(); 
    }

    public void CloseSaveInput()
    {
        isWaitingForName = false;

        saveCanvas.SetActive(false);
        levelNameInput.DeactivateInputField();

        m_editorManager.m_isEditing = false;

        m_editorManager.m_HUDCanvas.SetActive(true);
        m_editorManager.m_globalVerification.SetActive(true);

        levelNameInput.text = "NewLevel";
        levelNameInput.MoveTextEnd(false);
    }
}
