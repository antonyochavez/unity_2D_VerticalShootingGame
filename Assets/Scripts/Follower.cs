using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follower : MonoBehaviour
{
  public float maxShotDelay;
  public float curShotDelay;
  public ObjectManager objectManager;

  public Vector3 followPos;
  public int followDelay;
  public Transform parent;
  public Queue<Vector3> parentPos;

  void Awake()
  {
    parentPos = new Queue<Vector3>();
  }
  void Update()
  {
    Watch();
    Follow();
    Fire();
    Reload();
  }

  void Watch()
  {
    //Queue = FIFO (First Input First Out)
    //#.Input Pos
    if (!parentPos.Contains(parent.position))
      parentPos.Enqueue(parent.position);

    //#.Output Pos
    if (parentPos.Count > followDelay)
      followPos = parentPos.Dequeue();
    else if (parentPos.Count < followDelay)
      followPos = parent.position;
  }
  void Follow()
  {
    transform.position = followPos;
  }
  void Fire()
  {
    if (curShotDelay < maxShotDelay)
      return;


    Action<GameObject> BulletFire = (gameobject) => gameobject.GetComponent<Rigidbody2D>().AddForce(Vector2.up * 10, ForceMode2D.Impulse);

    //Power One
    GameObject bullet = objectManager.MakeObj("Follower_Bullet");
    bullet.transform.position = transform.position;
    BulletFire(bullet);
    curShotDelay = 0;
  }
  void Reload()
  {
    curShotDelay = Mathf.Clamp(curShotDelay + Time.deltaTime, 0, 5f);
  }

}
