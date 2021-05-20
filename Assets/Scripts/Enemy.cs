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
  public GameManager gameManager;
  public ObjectManager objectManager;
  Func<Enemy, Vector3> GetPosition = (Enemy) => Enemy.transform.position;
  Func<Enemy, Quaternion> GetRotation = (Enemy) => Enemy.transform.rotation;
  Action<Vector3, float, GameObject> BulletFire = (targetVector3, power, gameobject) => gameobject.GetComponent<Rigidbody2D>().AddForce(targetVector3.normalized * power, ForceMode2D.Impulse);

  public int patternIndex;
  public int curPatternCount;
  public int[] maxPatternCount;
  SpriteRenderer spriteRenderer;
  Animator anim;
  void Awake()
  {
    spriteRenderer = GetComponent<SpriteRenderer>();

    if (enemyName == "B")
      anim = GetComponent<Animator>();
  }
  void Update()
  {
    if (enemyName == "B")
      return;
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
      "B" => 2500,
      _ => 0,
    };
    if (enemyName == "B")
      Invoke("Stop", 2);
  }

  void Stop()
  {
    if (!gameObject.activeSelf)
      return;

    Rigidbody2D rigid = GetComponent<Rigidbody2D>();
    rigid.velocity = Vector2.zero;

    Invoke("Think", 2);
  }

  void Think()
  {
    patternIndex = patternIndex == 3 ? 0 : patternIndex + 1;
    curPatternCount = 0;
    switch (patternIndex)
    {
      case 0:
        FireFoward();
        break;
      case 1:
        FireShot();
        break;
      case 2:
        FireArc();
        break;
      case 3:
        FireAround();
        break;
    }
  }

  void FireFoward()
  {

    //#. Fire 4 Bullet Foward
    GameObject[] bullets = new GameObject[4];
    for (int i = 0; i < bullets.Length; i++)
    {
      bullets[i] = objectManager.MakeObj("BulletEnemyD");
    }
    bullets[0].transform.position = transform.position + Vector3.left * 0.6f;
    bullets[1].transform.position = transform.position + Vector3.left * 0.85f;
    bullets[2].transform.position = transform.position + Vector3.right * 0.6f;
    bullets[3].transform.position = transform.position + Vector3.right * 0.85f;

    foreach (GameObject bullet in bullets)
      BulletFire(Vector2.down, 8, bullet);


    //#.Pattern Counting
    curPatternCount++;
    if (curPatternCount < maxPatternCount[patternIndex])
      Invoke("FireFoward", 2);
    else
      Invoke("Think", 3);
  }
  void FireShot()
  {
    GameObject[] bullets = new GameObject[8];
    for (int i = 0; i < bullets.Length; i++)
    {
      bullets[i] = objectManager.MakeObj("BulletEnemyB");
      bullets[i].transform.position = transform.position;
      Vector2 toPlayerDir = player.transform.position - transform.position;
      Vector2 ranVec = new Vector2(UnityEngine.Random.Range(-2f, 2f), UnityEngine.Random.Range(0f, 2f));
      toPlayerDir += ranVec;
      BulletFire(toPlayerDir, 7, bullets[i]);
    }

    //#.Pattern Counting
    curPatternCount++;
    if (curPatternCount < maxPatternCount[patternIndex])
      Invoke("FireShot", 1.5f);
    else
      Invoke("Think", 3);
  }
  void FireArc()
  {
    //#.Fire Arc Continue Fire
    GameObject bullet = objectManager.MakeObj("BulletEnemyA");
    bullet.transform.position = transform.position;
    bullet.transform.rotation = Quaternion.identity;

    Vector2 dirVec = new Vector2(Mathf.Cos(Mathf.PI * 8 * curPatternCount / maxPatternCount[patternIndex]), -1);
    BulletFire(dirVec, 5, bullet);


    curPatternCount++;
    if (curPatternCount < maxPatternCount[patternIndex])
      Invoke("FireArc", 0.1f);
    else
      Invoke("Think", 3);
  }


  void FireAround()
  {
    //#. Fire Around
    GameObject[] bullets = curPatternCount % 2 == 0 ? new GameObject[50] : new GameObject[60];

    for (int i = 0; i < bullets.Length; i++)
    {
      bullets[i] = objectManager.MakeObj("BulletEnemyC");
      bullets[i].transform.position = transform.position;
      bullets[i].transform.rotation = Quaternion.identity;
      Vector2 dirVec = new Vector2(Mathf.Cos(Mathf.PI * 2 * i / bullets.Length)
                                  , Mathf.Sin(Mathf.PI * 2 * i / bullets.Length));
      BulletFire(dirVec, 2, bullets[i]);

      Vector3 rotVec = Vector3.forward * 360 * i / bullets.Length + Vector3.forward * 90;
      bullets[i].transform.Rotate(rotVec);
    }




    curPatternCount++;
    if (curPatternCount < maxPatternCount[patternIndex])
      Invoke("FireAround", 0.7f);
    else
      Invoke("Think", 3);
  }




  void Fire()
  {

    if (curShotDelay < maxShotDelay)
      return;

    Vector3 toPlayerDir = player.transform.position - transform.position;


    if (enemyName == "S")
    {
      GameObject bullet = objectManager.MakeObj("BulletEnemyA");
      bullet.transform.position = transform.position;
      BulletFire(toPlayerDir, 1.5f, bullet);
    }
    if (enemyName == "L")
    {
      GameObject bulletL = objectManager.MakeObj("BulletEnemyB");
      GameObject bulletR = objectManager.MakeObj("BulletEnemyB");
      bulletL.transform.position = transform.position + Vector3.left * 0.35f;
      bulletR.transform.position = transform.position + Vector3.right * 0.35f;
      BulletFire(toPlayerDir, 3, bulletL);
      BulletFire(toPlayerDir, 3, bulletR);
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
    if (enemyName == "B")
    {
      anim.SetTrigger("OnHit");
    }
    else
    {
      spriteRenderer.sprite = sprites[1];
      Invoke("ReturnSprite", 0.1f);

    }
    if (health <= 0)
    {
      Player playerLogic = player.GetComponent<Player>();
      playerLogic.score += enemyScore;

      Quaternion LookDown = Quaternion.LookRotation(Vector3.forward);
      //#.Random Ratio Item Drop
      int ran = enemyName == "B" ? 0 : UnityEngine.Random.Range(0, 10);
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
      gameManager.CallExplosion(transform.position, enemyName);

      //#.Boss Kill
      if (enemyName == "B")
        gameManager.StageEnd();
    }
  }

  void ReturnSprite()
  {
    spriteRenderer.sprite = sprites[0];
  }

  void OnTriggerEnter2D(Collider2D collision)
  {
    var colObj = collision.gameObject;
    if (colObj.tag == "BorderBullet" && enemyName != "B")
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
