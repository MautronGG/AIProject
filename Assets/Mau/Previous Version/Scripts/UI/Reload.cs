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
    _manager.m_restartEvents.Invoke();
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
}
