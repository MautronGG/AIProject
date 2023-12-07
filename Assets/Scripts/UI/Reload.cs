using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Reload : MonoBehaviour
{
    public void Scene(int scene)
  {
    SceneManager.LoadScene(scene);
  }
  public void Restart()
  {
    var thisscene = SceneManager.GetActiveScene();
    SceneManager.LoadScene(thisscene.name);
  }
  public void Quit()
  {
    Application.Quit();
  }
}
