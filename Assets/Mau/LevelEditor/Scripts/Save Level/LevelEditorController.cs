using TMPro;
using UnityEditor;
using UnityEngine;

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
        UpdateInputText();
    }

    void UpdateInputText()
    {
        if (m_editorManager != null && !string.IsNullOrEmpty(m_editorManager.loadedLevelName))
            levelNameInput.text = m_editorManager.loadedLevelName;
        else
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

    public void SaveCurrentLevel(string filename, bool overwrite = false)
    {
        var level = m_editorManager.currentLevel;
        level.bridges = m_editorManager.m_editorBridgeCounter.m_numBridges;
        level.levelName = filename;

        SaveLoadManager.SaveLevel(level, filename, overwrite);
    }

    public void OpenSaveInput()
    {
        //Camera.main.GetComponent<CameraMovement>().m_canMove = false;
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

    public void ConfirmSave(bool overwrite = false)
    {
        string filename = levelNameInput.text.Trim();

        if (string.IsNullOrEmpty(filename))
        {
            Debug.LogWarning("Level name is empty");
            return;
        }

        SaveCurrentLevel(filename, overwrite);

        m_editorManager.loadedLevelName = filename;

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

        UpdateInputText();
    }
}
