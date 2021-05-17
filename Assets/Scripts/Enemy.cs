using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
  public float speed;
  public int health;
  public Sprite[] sprites;

  SpriteRenderer spriteRenderer;
  Rigidbody2D rigid;

  void Awake()
  {
    spriteRenderer = GetComponent<SpriteRenderer>();
    rigid = GetComponent<Rigidbody2D>();
    rigid.velocity = Vector2.down * speed;
  }

  void OnHit(int dmg)
  {
    health -= dmg;
    spriteRenderer.sprite = sprites[1];
    Invoke("ReturnSprite", 0.1f);
    if (health <= 0)
    {
      Destroy(gameObject);
    }
  }

  void ReturnSprite()
  {
    spriteRenderer.sprite = sprites[0];
  }

  void OnTriggerEnter2D(Collider2D collision)
  {
    var colObj = collision.gameObject;
    if (colObj.tag == "BorderBullet")
    {
      Destroy(gameObject);
    }
    else if (colObj.tag == "PlayerBullet")
    {
      Bullet bullet = colObj.GetComponent<Bullet>();
      OnHit(bullet.damage);

      Destroy(colObj);
    }
  }
}
