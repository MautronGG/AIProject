using UnityEngine;
using UnityEngine.SceneManagement;

public enum SceneName
{
    MainMenu,
    Level_Editor,
    Level_0_Tutorial,
    Level_1_Portals,
    Level_2_Springs,
    Level_3_Keys,
    Level_4_Monsters,
    Level_5_Lasers,
}

public static class GetScene
{
    public static SceneName GetCurrent()
    {
        if (System.Enum.TryParse(
            SceneManager.GetActiveScene().name,
            out SceneName scene))
        {
            return scene;
        }

        Debug.LogError("Scene not found in SceneId enum!");
        return SceneName.MainMenu;
    }

    public static void Load(SceneName scene)
    {
        SceneManager.LoadScene(scene.ToString());
    }

    public static void LoadNextLevel(bool loop = false)
    {
        SceneName current = GetCurrent();
        int next = (int)current + 1;

        if (next >= System.Enum.GetValues(typeof(SceneName)).Length)
        {
            if (loop)
                next = 0; // or first level
            else
            {
                Debug.Log("No more levels.");
                return;
            }
        }

        Load((SceneName)next);
    }
}
