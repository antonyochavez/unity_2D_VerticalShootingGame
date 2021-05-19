using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
  public string[] enemyObjs;
  public Transform[] spawnPoints;
  public float nextSpawnDelay;
  public float curSpawnDelay;

  public GameObject player;
  public Text scoreText;
  public Image[] lifeImage;
  public Image[] boomImage;
  public GameObject gameOverSet;
  public ObjectManager objectManager;

  private Vector3 RespawnPoistion = new Vector3(0, -4, 0);
  public List<Spawn> spawnList;
  public int spawnIndex;
  public bool spawnEnd;

  void Awake()
  {
    enemyObjs = new string[] { "EnemyS", "EnemyM", "EnemyL" };
    spawnList = new List<Spawn>();
    ReadSpawnFile();
  }

  void ReadSpawnFile()
  {
    //#1. 변수 초기화
    spawnList.Clear();
    spawnIndex = 0;
    spawnEnd = false;

    //#2. 리스폰 파일 읽기
    TextAsset textFile = Resources.Load<TextAsset>("Stages/" + "Stage 0");
    StringReader stringReader = new StringReader(textFile.text);

    while (stringReader != null)
    {
      string line = stringReader.ReadLine();
      if (line == null)
        break;

      //#. 리스폰 데이터 생성
      Spawn spawnData;
      spawnData.delay = float.Parse(line.Split(',')[0]);
      spawnData.type = line.Split(',')[1];
      spawnData.point = int.Parse(line.Split(',')[2]);
      spawnList.Add(spawnData);
    }

    //#3. 텍스트 파일 닫기
    stringReader.Close();

    //#.첫번째 스폰 딜레이 적용
    nextSpawnDelay = spawnList[0].delay;
  }

  void Update()
  {
    curSpawnDelay += Time.deltaTime;

    if (curSpawnDelay > nextSpawnDelay && !spawnEnd)
    {
      SpawnEnemy();
      curSpawnDelay = 0;
    }

    //#.UI Score Update
    Player playerLogic = player.GetComponent<Player>();
    scoreText.text = string.Format("{0:n0}", playerLogic.score);
  }

  private void SpawnEnemy()
  {
    int enemyIndex = 0;
    switch (spawnList[spawnIndex].type)
    {
      case "S":
        enemyIndex = 0;
        break;
      case "M":
        enemyIndex = 1;
        break;
      case "L":
        enemyIndex = 2;
        break;
    }
    int enemyPoint = spawnList[spawnIndex].point;

    GameObject enemy = objectManager.MakeObj(enemyObjs[enemyIndex]);
    enemy.transform.position = spawnPoints[enemyPoint].position;
    Rigidbody2D rigidbody = enemy.GetComponent<Rigidbody2D>();
    Enemy enemyLogic = enemy.GetComponent<Enemy>();
    enemyLogic.player = player;
    enemyLogic.objectManager = objectManager;
    if (enemyPoint == 5 || enemyPoint == 6)
    {
      enemy.transform.Rotate(Vector3.back * 90);
      rigidbody.velocity = new Vector2(enemyLogic.speed * (-1), -1);
    }
    else if (enemyPoint == 7 || enemyPoint == 8)
    {
      enemy.transform.Rotate(Vector3.forward * 90);
      rigidbody.velocity = new Vector2(enemyLogic.speed, -1);
    }
    else
    {
      rigidbody.velocity = new Vector2(0, enemyLogic.speed * (-1));
    }

    //#.리스폰 인덱스 증가
    spawnIndex++;
    if (spawnIndex == spawnList.Count)
    {
      spawnEnd = true;
      return;
    }
    //#.다음 리스폰 딜레이 갱신
    nextSpawnDelay = spawnList[spawnIndex].delay;
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
