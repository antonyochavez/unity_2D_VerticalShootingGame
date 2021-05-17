using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Player : MonoBehaviour
{
  public float speed;
  public float power;
  public float maxShotDelay;
  public float curShotDelay;


  public GameObject bulletObjA;
  public GameObject bulletObjB;
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



    switch (power)
    {
      case 1:
        //Power One
        GameObject bullet = Instantiate(bulletObjA, transform.position, transform.rotation);
        Rigidbody2D rigidbody = bullet.GetComponent<Rigidbody2D>();
        rigidbody.AddForce(Vector2.up * 10, ForceMode2D.Impulse);
        break;
      case 2:
        GameObject bulletR = Instantiate(bulletObjA, transform.position + Vector3.right * 0.1f, transform.rotation);
        GameObject bulletL = Instantiate(bulletObjA, transform.position + Vector3.left * 0.1f, transform.rotation);
        Rigidbody2D rigidbodyR = bulletR.GetComponent<Rigidbody2D>();
        Rigidbody2D rigidbodyL = bulletL.GetComponent<Rigidbody2D>();
        rigidbodyR.AddForce(Vector2.up * 10, ForceMode2D.Impulse);
        rigidbodyL.AddForce(Vector2.up * 10, ForceMode2D.Impulse);
        break;
      case 3:
        GameObject bulletRR = Instantiate(bulletObjA, transform.position + Vector3.right * 0.35f, transform.rotation);
        GameObject bulletCC = Instantiate(bulletObjB, transform.position, transform.rotation);
        GameObject bulletLL = Instantiate(bulletObjA, transform.position + Vector3.left * 0.35f, transform.rotation);
        Rigidbody2D rigidbodyRR = bulletRR.GetComponent<Rigidbody2D>();
        Rigidbody2D rigidbodyCC = bulletCC.GetComponent<Rigidbody2D>();
        Rigidbody2D rigidbodyLL = bulletLL.GetComponent<Rigidbody2D>();
        rigidbodyRR.AddForce(Vector2.up * 10, ForceMode2D.Impulse);
        rigidbodyCC.AddForce(Vector2.up * 10, ForceMode2D.Impulse);
        rigidbodyLL.AddForce(Vector2.up * 10, ForceMode2D.Impulse);
        break;
    }

    curShotDelay = 0;
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

  void Reload()
  {
    curShotDelay = Mathf.Clamp(curShotDelay + Time.deltaTime, 0, 0.3f);
  }
}
