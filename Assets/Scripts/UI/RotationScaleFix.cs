using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationScaleFix : MonoBehaviour
{
  private void Update()
  {
    var rec = this.GetComponent<RectTransform>();
    //GetComponent<Canvas>().worldCamera = Camera.main;
    rec.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
    rec.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
    rec.localRotation = Quaternion.Euler(0f, 0f, 0f);
    rec.rotation = Quaternion.Euler(0f, 0f, 0f);
    rec.localScale = new Vector3(1f, 1f, 1f);
    rec.transform.localScale = new Vector3(1f, 1f, 1f);
    transform.rotation = Quaternion.Euler(0f, 0f, 0f);
    transform.localScale = new Vector3(1f, 1f, 1f);
  }
  public void FixTransform()
  {
    transform.rotation = Quaternion.Euler(0f, 0f, 0f);
    transform.localScale = new Vector3(1f, 1f, 1f);
  }
    
}
