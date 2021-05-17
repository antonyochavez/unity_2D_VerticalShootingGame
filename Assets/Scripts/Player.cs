using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
  public float speed;
  void Update()
  {
    float h = Input.GetAxisRaw("Horizontal");
    float v = Input.GetAxisRaw("Vertical");
    Vector3 nextPos = transform.position + new Vector3(h, v, 0).normalized * speed * Time.deltaTime;
    transform.position = new Vector3(Mathf.Clamp(nextPos.x, -2.2f, 2.2f), Mathf.Clamp(nextPos.y, -4.5f, 4.5f), nextPos.z);
  }
}
