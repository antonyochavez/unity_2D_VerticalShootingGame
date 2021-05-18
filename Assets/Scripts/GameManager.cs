using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
  public GameObject[] enemyObjs;
  public Transform[] spawnPoints;
  public float maxSpawnDelay;
  public float curSpawnDelay;

  public GameObject player;
  public Text scoreText;
  public Image[] lifeImage;
  public Image[] boomImage;
  public GameObject gameOverSet;

  private Vector3 RespawnPoistion = new Vector3(0, -4, 0);

  void Update()
  {
    curSpawnDelay += Time.deltaTime;

    if (curSpawnDelay > maxSpawnDelay)
    {
      SpawnEnemy();
      maxSpawnDelay = Random.Range(0.5f, 3f);
      curSpawnDelay = 0;
    }

    //#.UI Score Update
    Player playerLogic = player.GetComponent<Player>();
    scoreText.text = string.Format("{0:n0}", playerLogic.score);
  }

  private void SpawnEnemy()
  {
    int ranEnemy = Random.Range(0, 3);
    int ranPoint = Random.Range(0, 9);
    GameObject enemy = Instantiate(enemyObjs[ranEnemy], spawnPoints[ranPoint].position, spawnPoints[ranPoint].rotation);
    Rigidbody2D rigidbody = enemy.GetComponent<Rigidbody2D>();
    Enemy enemyLogic = enemy.GetComponent<Enemy>();
    enemyLogic.player = player;
    if (ranPoint == 5 || ranPoint == 6)
    {
      enemy.transform.Rotate(Vector3.back * 90);
      rigidbody.velocity = new Vector2(enemyLogic.speed * (-1), -1);
    }
    else if (ranPoint == 7 || ranPoint == 8)
    {
      enemy.transform.Rotate(Vector3.forward * 90);
      rigidbody.velocity = new Vector2(enemyLogic.speed, -1);
    }
    else
    {
      rigidbody.velocity = new Vector2(0, enemyLogic.speed * (-1));
    }
  }

  public void RespawnPlayer()
  {
    Invoke("RespawnPlayerExe", 2f);
  }
  void RespawnPlayerExe()
  {
    player.transform.position = RespawnPoistion;
    player.SetActive(true);

    Player playerLogic = player.GetComponent<Player>();
    playerLogic.isHit = false;
  }

  public void UpdateLifeIcon(int life)
  {
    //#.UI Life Init Disable
    for (int index = 0; index < 3; index++)
    {
      lifeImage[index].color = new Color(1, 1, 1, 0);
    }

    //#.UI Life Active
    for (int index = 0; index < life; index++)
    {
      lifeImage[index].color = new Color(1, 1, 1, 1);
    }
  }
  public void UpdateBoomIcon(int boom)
  {
    //#.UI Boom Init Disable
    for (int index = 0; index < 3; index++)
    {
      boomImage[index].color = new Color(1, 1, 1, 0);
    }

    //#.UI Boom Active
    for (int index = 0; index < boom; index++)
    {
      boomImage[index].color = new Color(1, 1, 1, 1);
    }
  }

  public void GameOver()
  {
    gameOverSet.SetActive(true);
  }

  public void GameRetry() => SceneManager.LoadScene(0);
}
