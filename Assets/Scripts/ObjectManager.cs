using System;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManager : MonoBehaviour
{
  GameObject[] Enemy_Small;
  GameObject[] Enemy_Medium;
  GameObject[] Enemy_Large;
  GameObject[] Enemy_Boss;

  GameObject[] Item_Coin;
  GameObject[] Item_Power;
  GameObject[] Item_Boom;

  GameObject[] Player_Bullet_A;
  GameObject[] Player_Bullet_B;
  GameObject[] Follower_Bullet;
  GameObject[] Enemy_Bullet_A;
  GameObject[] Enemy_Bullet_B;
  GameObject[] Enemy_Bullet_C;
  GameObject[] Enemy_Bullet_D;
  GameObject[] Explosion;

  GameObject[] targetPool;

  Func<string, GameObject> GetPrefab = (PrefabName) => Instantiate(Resources.Load<GameObject>("Prefabs/" + PrefabName));

  void Awake()
  {
    Enemy_Boss = new GameObject[1];
    Enemy_Large = new GameObject[20];
    Enemy_Medium = new GameObject[20];
    Enemy_Small = new GameObject[100];

    Item_Coin = new GameObject[50];
    Item_Power = new GameObject[50];
    Item_Boom = new GameObject[50];

    Player_Bullet_A = new GameObject[100];
    Player_Bullet_B = new GameObject[100];
    Follower_Bullet = new GameObject[30];
    Enemy_Bullet_A = new GameObject[400];
    Enemy_Bullet_B = new GameObject[100];
    Enemy_Bullet_C = new GameObject[500];
    Enemy_Bullet_D = new GameObject[200];
    Explosion = new GameObject[100];

    Generate();
  }

  void Generate()
  {
    //#1. Enemy
    Enemy_Boss[0] = GetPrefab("Enemy_B");
    Enemy_Boss[0].SetActive(false);
    for (int i = 0; i < Enemy_Large.Length; i++)
    {
      Enemy_Large[i] = GetPrefab("Enemy_Large");
      Enemy_Large[i].SetActive(false);
    }
    for (int i = 0; i < Enemy_Medium.Length; i++)
    {
      Enemy_Medium[i] = GetPrefab("Enemy_Medium");
      Enemy_Medium[i].SetActive(false);

    }
    for (int i = 0; i < Enemy_Small.Length; i++)
    {
      Enemy_Small[i] = GetPrefab("Enemy_Small");
      Enemy_Small[i].SetActive(false);
    }

    //#2. Item
    for (int i = 0; i < Item_Coin.Length; i++)
    {
      Item_Coin[i] = GetPrefab("Item_Coin");
      Item_Coin[i].SetActive(false);
    }
    for (int i = 0; i < Item_Power.Length; i++)
    {
      Item_Power[i] = GetPrefab("Item_Power");
      Item_Power[i].SetActive(false);
    }
    for (int i = 0; i < Item_Boom.Length; i++)
    {
      Item_Boom[i] = GetPrefab("Item_Boom");
      Item_Boom[i].SetActive(false);
    }

    //3. Bullet
    for (int i = 0; i < Player_Bullet_A.Length; i++)
    {
      Player_Bullet_A[i] = GetPrefab("Player_Bullet_A");
      Player_Bullet_A[i].SetActive(false);
    }
    for (int i = 0; i < Player_Bullet_B.Length; i++)
    {
      Player_Bullet_B[i] = GetPrefab("Player_Bullet_B");
      Player_Bullet_B[i].SetActive(false);
    }
    for (int i = 0; i < Follower_Bullet.Length; i++)
    {
      Follower_Bullet[i] = GetPrefab("Follower_Bullet");
      Follower_Bullet[i].SetActive(false);
    }
    for (int i = 0; i < Enemy_Bullet_A.Length; i++)
    {
      Enemy_Bullet_A[i] = GetPrefab("Enemy_Bullet_A");
      Enemy_Bullet_A[i].SetActive(false);
    }
    for (int i = 0; i < Enemy_Bullet_B.Length; i++)
    {
      Enemy_Bullet_B[i] = GetPrefab("Enemy_Bullet_B");
      Enemy_Bullet_B[i].SetActive(false);
    }
    for (int i = 0; i < Enemy_Bullet_C.Length; i++)
    {
      Enemy_Bullet_C[i] = GetPrefab("Enemy_Bullet_C");
      Enemy_Bullet_C[i].SetActive(false);
    }
    for (int i = 0; i < Enemy_Bullet_D.Length; i++)
    {
      Enemy_Bullet_D[i] = GetPrefab("Enemy_Bullet_D");
      Enemy_Bullet_D[i].SetActive(false);
    }

    //4. Effect
    for (int i = 0; i < Explosion.Length; i++)
    {
      Explosion[i] = GetPrefab("Explosion");
      Explosion[i].SetActive(false);
    }


  }

  public GameObject MakeObj(string type)
  {
    targetPool = type switch
    {
      "EnemyL" => Enemy_Large,
      "EnemyM" => Enemy_Medium,
      "EnemyS" => Enemy_Small,
      "EnemyB" => Enemy_Boss,
      "ItemCoin" => Item_Coin,
      "ItemPower" => Item_Power,
      "ItemBoom" => Item_Boom,
      "BulletPlayerA" => Player_Bullet_A,
      "BulletPlayerB" => Player_Bullet_B,
      "Follower_Bullet" => Follower_Bullet,
      "BulletEnemyA" => Enemy_Bullet_A,
      "BulletEnemyB" => Enemy_Bullet_B,
      "BulletEnemyC" => Enemy_Bullet_C,
      "BulletEnemyD" => Enemy_Bullet_D,
      "Explosion" => Explosion,
      _ => throw new ArgumentNullException(),
    };

    for (int i = 0; i < targetPool.Length; i++)
    {
      if (!targetPool[i].activeSelf)
      {
        targetPool[i].SetActive(true);
        return targetPool[i];
      }
    }
    return null;
  }

  public GameObject[] GetPool(string type)
  {
    targetPool = type switch
    {
      "EnemyL" => Enemy_Large,
      "EnemyM" => Enemy_Medium,
      "EnemyS" => Enemy_Small,
      "EnemyB" => Enemy_Boss,
      "ItemCoin" => Item_Coin,
      "ItemPower" => Item_Power,
      "ItemBoom" => Item_Boom,
      "BulletPlayerA" => Player_Bullet_A,
      "BulletPlayerB" => Player_Bullet_B,
      "Follower_Bullet" => Follower_Bullet,
      "BulletEnemyA" => Enemy_Bullet_A,
      "BulletEnemyB" => Enemy_Bullet_B,
      "BulletEnemyC" => Enemy_Bullet_C,
      "BulletEnemyD" => Enemy_Bullet_D,
      "Explosion" => Explosion,
      _ => throw new ArgumentNullException(),
    };
    return targetPool;
  }
}
