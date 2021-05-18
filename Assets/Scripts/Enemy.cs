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
  Func<Enemy, Vector3> GetPosition = (Enemy) => Enemy.transform.position;
  Func<Enemy, Quaternion> GetRotation = (Enemy) => Enemy.transform.rotation;
  Func<string, Vector3, Vector3, Quaternion, GameObject> GetPrefab = (PrefabName, offset, thisPostion, thisRotation)
      => Instantiate(Resources.Load<GameObject>("Prefabs/" + PrefabName),
                     thisPostion + offset,
                     thisRotation);

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

    Vector3 toPlayerDir = player.transform.position - transform.position;
    Action<GameObject> BulletFire = (gameobject) => gameobject.GetComponent<Rigidbody2D>().AddForce(toPlayerDir.normalized * 3, ForceMode2D.Impulse);

    if (enemyName == "S")
    {
      BulletFire(GetPrefab("Enemy_Bullet_A", Vector3.zero, GetPosition(this), GetRotation(this)));
    }
    if (enemyName == "L")
    {
      BulletFire(GetPrefab("Enemy_Bullet_B", Vector3.right * 0.35f, GetPosition(this), GetRotation(this)));
      BulletFire(GetPrefab("Enemy_Bullet_B", Vector3.left * 0.35f, GetPosition(this), GetRotation(this)));
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
        GetPrefab("Item Coin", Vector3.zero, GetPosition(this), GetRotation(this)).transform.rotation = LookDown;
      }
      else if (ran < 8) //Power 20%
      {
        GetPrefab("Item Power", Vector3.zero, GetPosition(this), GetRotation(this)).transform.rotation = LookDown;
      }
      else if (ran < 10) //Boom 20%
      {
        GetPrefab("Item Boom", Vector3.zero, GetPosition(this), GetRotation(this)).transform.rotation = LookDown;
      }
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
