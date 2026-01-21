using UnityEngine;

public class LevelEditorController : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            SaveCurrentLevel("Kappa");
        }
    }

    public void SaveCurrentLevel(string filename)
    {
        var level = EditorManager.Instance.currentLevel;
        level.bridges = EditorManager.Instance.m_editorBridgeCounter.m_numBridges;
        level.levelName = filename;
        SaveLoadManager.SaveLevel(level, filename);
    }
}
