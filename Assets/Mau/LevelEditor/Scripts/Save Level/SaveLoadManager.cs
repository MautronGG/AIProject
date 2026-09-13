using System.IO;
using UnityEngine;

public static class SaveLoadManager
{
    public static string DefaultSavePath => Path.Combine(Application.persistentDataPath, "Levels");

    public static void SaveLevel(LevelData level, string filenameWithoutExt, bool overwrite = false)
    {
        string basePath = DefaultSavePath;

        if (!Directory.Exists(basePath))
            Directory.CreateDirectory(basePath);

        var path = Path.Combine(DefaultSavePath, filenameWithoutExt + ".json");

        if (!overwrite)
        {
            int index = 1;
            while (File.Exists(path))
            {
                path = Path.Combine(DefaultSavePath, $"{filenameWithoutExt}({index}).json");
                index++;
            }
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

    public static string StripPrefix(string filename)
    {
        if (string.IsNullOrEmpty(filename)) return filename;
        int underscoreIndex = filename.IndexOf('_');
        if (underscoreIndex >= 0)
        {
            if (int.TryParse(filename.Substring(0, underscoreIndex), out _))
            {
                return filename.Substring(underscoreIndex + 1);
            }
        }
        return filename;
    }

    public static int GetPrefixNumber(string filename)
    {
        if (string.IsNullOrEmpty(filename)) return int.MaxValue;
        int underscoreIndex = filename.IndexOf('_');
        if (underscoreIndex >= 0)
        {
            if (int.TryParse(filename.Substring(0, underscoreIndex), out int num))
                return num;
        }
        return int.MaxValue; // If no number, put it at the end
    }

    public static int GetNextLevelNumber()
    {
        string saveDirectory = DefaultSavePath;
        if (!Directory.Exists(saveDirectory)) return 1;

        string[] files = Directory.GetFiles(saveDirectory, "*.json");
        return files.Length + 1;
    }
}
