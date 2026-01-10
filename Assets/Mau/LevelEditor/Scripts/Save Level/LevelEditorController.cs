using UnityEngine;

public class LevelEditorController : MonoBehaviour
{
    public void SaveCurrentLevel(string filename)
    {
        var level = EditorManager.Instance.currentLevel;
        level.levelName = filename;
        SaveLoadManager.SaveLevel(level, filename);
    }
}
