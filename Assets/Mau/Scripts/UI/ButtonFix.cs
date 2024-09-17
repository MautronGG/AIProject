using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonFix : MonoBehaviour
{
  bool m_buttonHit;
  Image m_image;
  //m_color;

  // Start is called before the first frame update
  void Start()
  {
    m_image = GetComponent<Image>();
    m_image.alphaHitTestMinimumThreshold = 0.001f;
  }

  //// Update is called once per frame
  //void Update()
  //{
  //  Vector2 screenPosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
  //  Camera.main.ScreenPointToRay(screenPosition);
  //  //float px = Mathf.Clamp01(localPoint.x, m_rect.rect.width);
  //  //float py = Mathf.Clamp01(localPoint, m_rect.rect.height);
  //
  //  //RaycastHit hit;
  //  //int layer = 1 << 10;
  //  //v = Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, 3f, layer);
  //  //if (!m_buttonHit)
  //  //{
  //  //  return;
  //  //}
  //  //var door = hit.collider.gameObject.GetComponent<DoorOpen>();
  //  //if (door)
  //  //{
  //  //  door.OpenDoor();
  //  //  return;
  //  //}
  //  //var reference = hit.collider.gameObject.GetComponent<DoorReference>();
  //  //if (reference)
  //  //{
  //  //  reference.OpenDoor();
  //  //  return;
  //  //}
  //  //Physics.Raycast(Input.mousePosition, );
  //}
  //
  //public void OnPointerDown(PointerEventData eventData)
  //{
  // //if (m_image != null)
  // //{
  // //  if (m_image.alphaHitTestMinimumThreshold == 0)
  // //  {
  // //
  // //  }
  // //
  // //
  // // //RectTransformUtility.ScreenPointToLocalPointInRectangle(m_image.rectTransform, eventData.position, eventData.pressEventCamera, out Vector2 localCursor);
  // // //float px = Mathf.Clamp01((localCursor.x - m_image.rectTransform.rect.x) / m_image.rectTransform.rect.width);
  // // //float py = Mathf.Clamp01((localCursor.y - m_image.rectTransform.rect.y) / m_image.rectTransform.rect.height);
  // // //
  // // //int texturex = (int)(px * m_image.texture.width);
  // // //int texturey = (int)(py * m_image.texture.height);
  // //
  // //  
  // //
  // //  //m_color = m_regions[FindRegionIndex(texturey, texturex)].m_regionCurrentColor;
  // //}
  //}
}
