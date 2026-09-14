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
            levelNameInput.text = SaveLoadManager.StripPrefix(m_editorManager.loadedLevelName);
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

    public void SaveCurrentLevel(string filename, bool overwrite = false, string internalLevelName = "")
    {
        var level = m_editorManager.currentLevel;
        level.bridges = m_editorManager.m_editorBridgeCounter.m_numBridges;
        level.levelName = string.IsNullOrEmpty(internalLevelName) ? filename : internalLevelName;

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
        string inputName = levelNameInput.text.Trim();

        if (string.IsNullOrEmpty(inputName))
        {
            Debug.LogWarning("Level name is empty");
            return;
        }

        string loadedName = m_editorManager.loadedLevelName;
        string strippedLoaded = string.IsNullOrEmpty(loadedName) ? "" : SaveLoadManager.StripPrefix(loadedName);

        string finalFilename;
        bool doOverwrite = false;

        if (!string.IsNullOrEmpty(loadedName) && inputName == strippedLoaded)
        {
            // They didn't change the name, so overwrite the original file
            finalFilename = loadedName;
            doOverwrite = true;
        }
        else
        {
            // They typed a new name or there's no loaded level. Treat as a new level.
            int nextNumber = SaveLoadManager.GetNextLevelNumber();
            finalFilename = $"{nextNumber}_{inputName}";
            doOverwrite = false;
        }

        SaveCurrentLevel(finalFilename, doOverwrite, inputName);

        m_editorManager.loadedLevelName = finalFilename;

        foreach (var loader in FindObjectsByType<LoadLevelInEditor>(FindObjectsSortMode.None))
        {
            loader.jsonFileNameWithoutExt = finalFilename;
        }

        #if UNITY_EDITOR
        var sceneCreator = FindFirstObjectByType<LevelSceneCreator>();
        if (sceneCreator != null)
        {
            sceneCreator.jsonFileNameWithoutExt = finalFilename;
        }
        #endif

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
