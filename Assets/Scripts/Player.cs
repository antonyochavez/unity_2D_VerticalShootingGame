using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Player : MonoBehaviour
{
  public float speed;
  public int power;
  public float maxShotDelay;
  public float curShotDelay;

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

  void OnTriggerEnter2D(Collider2D collision)
  {
    if (collision.gameObject.tag == "Enemy" || collision.gameObject.tag == "EnemyBullet")
    {
      manager.RespawnPlayer();
      gameObject.SetActive(false);
    }
  }
}
