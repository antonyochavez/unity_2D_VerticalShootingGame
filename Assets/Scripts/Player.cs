using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Player : MonoBehaviour
{
  public int life;
  public int score;
  public float speed;
  public int power;
  public int boom;
  public int maxPower;
  public int maxBoom;
  public float maxShotDelay;
  public float curShotDelay;
  public bool isHit;
  public bool isBoomTime;

  public bool[] joyControl;
  public bool isControl;
  public bool isButtonA;
  public bool isButtonB;

  public GameObject[] followers;

  public GameObject boomEffect;

  public GameManager gameManager;
  public ObjectManager objectManager;


  Animator anim;

  void Awake()
  {
    anim = GetComponent<Animator>();
  }
  void Update()
  {
    Move();
    Fire();
    Boom();
    Reload();
  }

  public void JoyPanel(int type)
  {
    for (int i = 0; i < 9; i++)
    {
      joyControl[i] = i == type;
    }
  }

  public void JoyDown() => isControl = true;
  public void JoyUp() => isControl = false;

  void Fire()
  {
    // if (!Input.GetButton("Fire1"))
    //   return;
    if (!isButtonA)
      return;

    if (curShotDelay < maxShotDelay)
      return;


    Action<GameObject> BulletFire = (gameobject) => gameobject.GetComponent<Rigidbody2D>().AddForce(Vector2.up * 10, ForceMode2D.Impulse);
    switch (power)
    {
      case 1:
        //Power One
        GameObject bullet = objectManager.MakeObj("BulletPlayerA");
        bullet.transform.position = transform.position;
        BulletFire(bullet);
        break;
      case 2:
        GameObject bulletR = objectManager.MakeObj("BulletPlayerA");
        GameObject bulletL = objectManager.MakeObj("BulletPlayerA");
        bulletR.transform.position = transform.position + Vector3.right * 0.1f;
        bulletL.transform.position = transform.position + Vector3.left * 0.1f;
        BulletFire(bulletR);
        BulletFire(bulletL);
        break;
      default:
        GameObject bulletRR = objectManager.MakeObj("BulletPlayerA");
        GameObject bulletC = objectManager.MakeObj("BulletPlayerB");
        GameObject bulletLL = objectManager.MakeObj("BulletPlayerA");
        bulletRR.transform.position = transform.position + Vector3.right * 0.35f;
        bulletC.transform.position = transform.position;
        bulletLL.transform.position = transform.position + Vector3.left * 0.35f;
        BulletFire(bulletRR);
        BulletFire(bulletC);
        BulletFire(bulletLL);
        break;
    }
    curShotDelay = 0;
  }
  void Reload()
  {
    curShotDelay = Mathf.Clamp(curShotDelay + Time.deltaTime, 0, 0.3f);
  }
  void Move()
  {


    //#.Keyboard Control Value
    float h = Input.GetAxisRaw("Horizontal");
    float v = Input.GetAxisRaw("Vertical");

    //#.Joy Control Value
    if (joyControl[0]) { h = -1; v = 1; }
    if (joyControl[1]) { h = 0; v = 1; }
    if (joyControl[2]) { h = 1; v = 1; }
    if (joyControl[3]) { h = -1; v = 0; }
    if (joyControl[4]) { h = 0; v = 0; }
    if (joyControl[5]) { h = 1; v = 0; }
    if (joyControl[6]) { h = -1; v = -1; }
    if (joyControl[7]) { h = 0; v = -1; }
    if (joyControl[8]) { h = 1; v = -1; }

    if (!isControl)
      return;

    Vector3 nextPos = transform.position + new Vector3(h, v, 0).normalized * speed * Time.deltaTime;
    transform.position = new Vector3(Mathf.Clamp(nextPos.x, -2.2f, 2.2f), Mathf.Clamp(nextPos.y, -4.5f, 4.5f), nextPos.z);

    if (Input.GetButtonDown("Horizontal") || Input.GetButtonUp("Horizontal"))
    {
      anim.SetInteger("Input", (int)h);
    }
  }

  public void ButtonADown() => isButtonA = true;
  public void ButtonAUp() => isButtonA = false;
  public void ButtonBDown() => isButtonB = true;

  void Boom()
  {
    // if (!Input.GetButton("Fire2"))
    //   return;
    if (!isButtonB)
      return;
    if (isBoomTime)
      return;

    if (boom == 0)
      return;

    boom--;
    isBoomTime = true;
    gameManager.UpdateBoomIcon(boom);

    //#1. Effect visible
    boomEffect.SetActive(true);
    Invoke("OffBoomEffect", 3f);


    //#2. Remove Enemy
    GameObject[] enemiesL = objectManager.GetPool("EnemyL");
    GameObject[] enemiesM = objectManager.GetPool("EnemyM");
    GameObject[] enemiesS = objectManager.GetPool("EnemyS");
    GameObject[] enemiesBoss = objectManager.GetPool("EnemyB");

    foreach (GameObject enemy in enemiesL)
    {
      if (!enemy.activeSelf) continue;
      Enemy enemyLogic = enemy.GetComponent<Enemy>();
      enemyLogic.OnHit(1000);
    }
    foreach (GameObject enemy in enemiesM)
    {
      if (!enemy.activeSelf) continue;
      Enemy enemyLogic = enemy.GetComponent<Enemy>();
      enemyLogic.OnHit(1000);
    }
    foreach (GameObject enemy in enemiesS)
    {
      if (!enemy.activeSelf) continue;
      Enemy enemyLogic = enemy.GetComponent<Enemy>();
      enemyLogic.OnHit(1000);
    }
    foreach (GameObject enemy in enemiesS)
    {
      if (!enemy.activeSelf) continue;
      Enemy enemyLogic = enemy.GetComponent<Enemy>();
      enemyLogic.OnHit(200);
    }


    //#. Remove Enemy Bullet
    GameObject[] enemiesbulltetsA = objectManager.GetPool("BulletEnemyA");
    GameObject[] enemiesbulltetsB = objectManager.GetPool("BulletEnemyB");
    GameObject[] enemiesbulltetsC = objectManager.GetPool("BulletEnemyC");
    GameObject[] enemiesbulltetsD = objectManager.GetPool("BulletEnemyD");
    for (int i = 0; i < enemiesbulltetsA.Length; i++)
    {
      if (!enemiesbulltetsA[i].activeSelf) continue;
      enemiesbulltetsA[i].SetActive(false);
    }
    for (int i = 0; i < enemiesbulltetsB.Length; i++)
    {
      if (!enemiesbulltetsB[i].activeSelf) continue;
      enemiesbulltetsB[i].SetActive(false);
    }
    for (int i = 0; i < enemiesbulltetsC.Length; i++)
    {
      if (!enemiesbulltetsC[i].activeSelf) continue;
      enemiesbulltetsC[i].SetActive(false);
    }
    for (int i = 0; i < enemiesbulltetsD.Length; i++)
    {
      if (!enemiesbulltetsD[i].activeSelf) continue;
      enemiesbulltetsD[i].SetActive(false);
    }

    isButtonB = false;
  }
  void OnTriggerEnter2D(Collider2D collision)
  {
    if (collision.gameObject.tag == "Enemy" || collision.gameObject.tag == "EnemyBullet")
    {
      if (isHit)
        return;

      isHit = true;
      life--;
      gameManager.UpdateLifeIcon(life);
      gameManager.CallExplosion(transform.position, "P");
      if (life == 0)
      {
        gameManager.GameOver();
      }
      else
      {
        gameManager.RespawnPlayer();
      }
      gameObject.SetActive(false);

      if (collision.gameObject.tag == "Enemy")
      {
        if (collision.gameObject.GetComponent<Enemy>().enemyName == "B")
          return;
      }
      collision.gameObject.SetActive(false);
    }
    else if (collision.gameObject.tag == "Item")
    {
      Item item = collision.gameObject.GetComponent<Item>();
      switch (item.type)
      {
        case "Coin":
          score += 1000;
          break;
        case "Power":
          if (power == maxPower)
            score += 500;
          else
          {
            power++;
            AddFollower();
          }
          break;
        case "Boom":
          if (boom == maxBoom)
            score += 1000;
          else
            boom++;
          gameManager.UpdateBoomIcon(boom);
          break;
      }
      collision.gameObject.SetActive(false);
    }
  }
  void AddFollower()
  {
    if (power == 4)
      followers[0].SetActive(true);
    else if (power == 5)
      followers[1].SetActive(true);
    else if (power == 6)
      followers[2].SetActive(true);
  }
  void OffBoomEffect()
  {
    boomEffect.SetActive(false);
    isBoomTime = false;
  }
}
