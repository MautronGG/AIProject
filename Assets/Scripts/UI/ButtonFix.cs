using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonFix : MonoBehaviour
{
  bool m_buttonHit;
  // Start is called before the first frame update
  void Start()
  {

  }

  // Update is called once per frame
  //void Update()
  //{
  //  RaycastHit hit;
  //  int layer = 1 << 10;
  //  v = Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, 3f, layer);
  //  if (!m_buttonHit)
  //  {
  //    return;
  //  }
  //  var door = hit.collider.gameObject.GetComponent<DoorOpen>();
  //  if (door)
  //  {
  //    door.OpenDoor();
  //    return;
  //  }
  //  var reference = hit.collider.gameObject.GetComponent<DoorReference>();
  //  if (reference)
  //  {
  //    reference.OpenDoor();
  //    return;
  //  }
  //  Physics.Raycast(Input.mousePosition, );
  //}
}
