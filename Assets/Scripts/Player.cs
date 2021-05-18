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

  public GameObject boomEffect;

  public GameManager manager;


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

  void Fire()
  {
    if (!Input.GetButton("Fire1"))
      return;

    if (curShotDelay < maxShotDelay)
      return;


    Func<string, Vector3, GameObject> GetBullet = (BulletName, offset) =>
    Instantiate(Resources.Load<GameObject>("Prefabs/" + BulletName),
                transform.position + offset,
                transform.rotation);
    Action<GameObject> BulletFire = (gameobject) => gameobject.GetComponent<Rigidbody2D>().AddForce(Vector2.up * 10, ForceMode2D.Impulse);
    switch (power)
    {
      case 1:
        //Power One
        BulletFire(GetBullet("Player_Bullet_A", Vector3.zero));
        break;
      case 2:
        BulletFire(GetBullet("Player_Bullet_A", Vector3.right * 0.1f));
        BulletFire(GetBullet("Player_Bullet_A", Vector3.left * 0.1f));
        break;
      case 3:
        BulletFire(GetBullet("Player_Bullet_A", Vector3.right * 0.35f));
        BulletFire(GetBullet("Player_Bullet_B", Vector3.zero));
        BulletFire(GetBullet("Player_Bullet_A", Vector3.left * 0.35f));
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
    float h = Input.GetAxisRaw("Horizontal");
    float v = Input.GetAxisRaw("Vertical");
    Vector3 nextPos = transform.position + new Vector3(h, v, 0).normalized * speed * Time.deltaTime;
    transform.position = new Vector3(Mathf.Clamp(nextPos.x, -2.2f, 2.2f), Mathf.Clamp(nextPos.y, -4.5f, 4.5f), nextPos.z);

    if (Input.GetButtonDown("Horizontal") || Input.GetButtonUp("Horizontal"))
    {
      anim.SetInteger("Input", (int)h);
    }
  }

  void Boom()
  {
    if (!Input.GetButton("Fire2"))
      return;
    if (isBoomTime)
      return;

    if (boom == 0)
      return;

    boom--;
    isBoomTime = true;
    manager.UpdateBoomIcon(boom);

    //#1. Effect visible
    boomEffect.SetActive(true);
    Invoke("OffBoomEffect", 3f);
    //#2. Remove Enemy
    GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
    foreach (GameObject enemy in enemies)
    {
      Enemy enemyLogic = enemy.GetComponent<Enemy>();
      enemyLogic.OnHit(1000);
    }
    //#. Remove Enemy Bullet
    GameObject[] enemiesbulltets = GameObject.FindGameObjectsWithTag("EnemyBullet");
    Array.ForEach<GameObject>(enemiesbulltets, (bullet) => Destroy(bullet));
  }
  void OnTriggerEnter2D(Collider2D collision)
  {
    if (collision.gameObject.tag == "Enemy" || collision.gameObject.tag == "EnemyBullet")
    {
      if (isHit)
        return;

      isHit = true;
      life--;
      manager.UpdateLifeIcon(life);
      if (life == 0)
      {
        manager.GameOver();
      }
      else
      {
        manager.RespawnPlayer();
      }
      gameObject.SetActive(false);
      Destroy(collision.gameObject);
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
            power++;
          break;
        case "Boom":
          if (boom == maxBoom)
            score += 1000;
          else
            boom++;
          manager.UpdateBoomIcon(boom);
          break;
      }
      Destroy(collision.gameObject);
    }
  }
  void OffBoomEffect()
  {
    boomEffect.SetActive(false);
    isBoomTime = false;
  }
}
