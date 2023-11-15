using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Reload : MonoBehaviour
{
  [SerializeField] int scene = 0;
    public void Scene()
  {
    SceneManager.LoadScene(scene);
  }
}
