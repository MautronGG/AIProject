using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Reload : MonoBehaviour
{
    LevelManager _manager;
    private void Start()
    {
        _manager = GetComponent<LevelManager>();
    }
    public void Scene(int scene)
    {
        SceneManager.LoadScene(scene);
    }
    public void Restart()
    {
        //var thisscene = SceneManager.GetActiveScene();
        //SceneManager.LoadScene(thisscene.name);
        if (_manager)
            _manager.m_restartEvents.Invoke();
    }
    public void NextLevel()
    {
        var nextlevel = SceneManager.GetActiveScene().buildIndex + 1;
        if (nextlevel > SceneManager.sceneCount)
        {
            nextlevel = 0;
        }
        SceneManager.LoadScene(nextlevel);
    }
    public void ReloadLevel()
    {
        var thisscene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(thisscene.name);
    }
    public void Quit()
    {
        Application.Quit();
    }

    public void OnNextLevelButton()
    {
        GetScene.LoadNextLevel();
    }
}
