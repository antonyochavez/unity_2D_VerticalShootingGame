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
  public ObjectManager objectManager;
  Func<Enemy, Vector3> GetPosition = (Enemy) => Enemy.transform.position;
  Func<Enemy, Quaternion> GetRotation = (Enemy) => Enemy.transform.rotation;


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

  void OnEnable()
  {
    health = enemyName switch
    {
      "S" => 3,
      "M" => 10,
      "L" => 40,
      _ => 0,
    };
  }
  void Fire()
  {

    if (curShotDelay < maxShotDelay)
      return;

    Vector3 toPlayerDir = player.transform.position - transform.position;
    Action<GameObject> BulletFire = (gameobject) => gameobject.GetComponent<Rigidbody2D>().AddForce(toPlayerDir.normalized * 3, ForceMode2D.Impulse);

    if (enemyName == "S")
    {
      GameObject bullet = objectManager.MakeObj("BulletEnemyA");
      bullet.transform.position = transform.position;
      BulletFire(bullet);
    }
    if (enemyName == "L")
    {
      GameObject bulletL = objectManager.MakeObj("BulletEnemyB");
      GameObject bulletR = objectManager.MakeObj("BulletEnemyB");
      bulletL.transform.position = transform.position + Vector3.left * 0.35f;
      bulletR.transform.position = transform.position + Vector3.right * 0.35f;
      BulletFire(bulletL);
      BulletFire(bulletR);
    }
    curShotDelay = 0;
  }
  void Reload()
  {
    curShotDelay += Time.deltaTime;
  }
  public void OnHit(int dmg)
  {
    if (health <= 0)
      return;

    health -= dmg;
    spriteRenderer.sprite = sprites[1];
    Invoke("ReturnSprite", 0.1f);
    if (health <= 0)
    {
      Player playerLogic = player.GetComponent<Player>();
      playerLogic.score += enemyScore;

      Quaternion LookDown = Quaternion.LookRotation(Vector3.forward);
      //#.Random Ratio Item Drop
      int ran = UnityEngine.Random.Range(0, 10);
      if (ran < 3) // Not item 30%
      {

      }
      else if (ran < 6) //Coin 30%
      {
        GameObject item = objectManager.MakeObj("ItemCoin");
        item.transform.position = transform.position;
        item.transform.rotation = LookDown;
      }
      else if (ran < 8) //Power 20%
      {
        GameObject item = objectManager.MakeObj("ItemPower");
        item.transform.position = transform.position;
        item.transform.rotation = LookDown;
      }
      else if (ran < 10) //Boom 20%
      {
        GameObject item = objectManager.MakeObj("ItemBoom");
        item.transform.position = transform.position;
        item.transform.rotation = LookDown;
      }
      gameObject.SetActive(false);
      transform.rotation = Quaternion.identity;
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
      gameObject.SetActive(false);
      transform.rotation = Quaternion.identity;
    }
    else if (colObj.tag == "PlayerBullet")
    {
      Bullet bullet = colObj.GetComponent<Bullet>();
      colObj.SetActive(false);
      OnHit(bullet.damage);
    }
  }
}
