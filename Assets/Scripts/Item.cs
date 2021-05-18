using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
  public string type;
  Rigidbody2D rigid;

  void Awake()
  {
    rigid = GetComponent<Rigidbody2D>();
  }
  void OnEnable()
  {
    rigid.velocity = Vector2.down * 0.5f;
  }
  void OnTriggerEnter2D(Collider2D collision)
  {
    var colObj = collision.gameObject;
    if (colObj.tag == "BorderBullet")
    {
      gameObject.SetActive(false);
    }
  }
}
