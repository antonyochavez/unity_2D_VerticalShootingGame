using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Enemy : MonoBehaviour
{
  public string enemyName;
  public int enemyScore;
  public float speed;
  public int health;
  public Sprite[] sprites;
  public float maxShotDelay;
  public float curShotDelay;
  public GameObject player;

  SpriteRenderer spriteRenderer;

  void Awake()
  {
    spriteRenderer = GetComponent<SpriteRenderer>();
  }
  void Update()
  {
    Reload();
    Fire();
  }
  void Fire()
  {

    if (curShotDelay < maxShotDelay)
      return;


    Func<string, Vector3, GameObject> GetBullet = (BulletName, offset) =>
    Instantiate(Resources.Load<GameObject>("Prefabs/" + BulletName),
                transform.position + offset,
                transform.rotation);
    Vector3 toPlayerDir = player.transform.position - transform.position;
    Action<GameObject> BulletFire = (gameobject) => gameobject.GetComponent<Rigidbody2D>().AddForce(toPlayerDir.normalized * 3, ForceMode2D.Impulse);

    if (enemyName == "S")
    {
      BulletFire(GetBullet("Enemy_Bullet_A", Vector3.zero));
    }
    if (enemyName == "L")
    {
      BulletFire(GetBullet("Enemy_Bullet_B", Vector3.right * 0.35f));
      BulletFire(GetBullet("Enemy_Bullet_B", Vector3.left * 0.35f));
    }
    curShotDelay = 0;
  }
  void Reload()
  {
    curShotDelay += Time.deltaTime;
  }
  public void OnHit(int dmg)
  {
    health -= dmg;
    spriteRenderer.sprite = sprites[1];
    Invoke("ReturnSprite", 0.1f);
    if (health <= 0)
    {
      Player playerLogic = player.GetComponent<Player>();
      playerLogic.score += enemyScore;
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
