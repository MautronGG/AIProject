using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PleaseAssignColor : MonoBehaviour
{
  public float m_time = 0f;
  public TMP_Text m_text;
  Coroutine m_fadeout;
  private void OnEnable()
  {
    m_time = 0;
    m_text.alpha = 1;
  }
  // Update is called once per frame
  void Update()
  {
    m_time += Time.deltaTime;
    if (m_time >= 1f && m_fadeout == null)
    {
      m_fadeout =  StartCoroutine(FadeOut());
    }
  }
  IEnumerator FadeOut()
  {
    Color c = m_text.color;
    for (float alpha = 1f; alpha >= 0; alpha -= 0.1f)
    {
      c.a = alpha;
      m_text.color = c;
      yield return new WaitForSeconds(.1f);
    }
    this.gameObject.SetActive(false);
    m_fadeout = null;
    yield return null;
  }
}
