using System.IO;
using UnityEngine;

public static class SaveLoadManager
{
    public static string DefaultSavePath => Application.persistentDataPath;

    public static void SaveLevel(LevelData level, string filenameWithoutExt)
    {
        string basePath = DefaultSavePath;

        if (!Directory.Exists(basePath))
            Directory.CreateDirectory(basePath);

        var path = Path.Combine(DefaultSavePath, filenameWithoutExt + ".json");

        int index = 1;
        while (File.Exists(path))
        {
            path = Path.Combine(DefaultSavePath, $"{filenameWithoutExt}({index}).json");
            index++;
        }

        var json = JsonUtility.ToJson(level, true);
        File.WriteAllText(path, json);
        Debug.Log($"Level saved to: {path}");
    }

    public static LevelData LoadLevel(string filenameWithoutExt)
    {
        var path = Path.Combine(DefaultSavePath, filenameWithoutExt + ".json");
        if (!File.Exists(path))
            return null;
        var json = File.ReadAllText(path);
        return JsonUtility.FromJson<LevelData>(json);

    }
}
