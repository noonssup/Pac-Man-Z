using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageManager : MonoBehaviour
{
    public GameObject player;  //플레이어 게임오브젝트
    public GameObject curPlayer; //현재 화면에 있는 플레이어
    public int playerHp = 3;  //플레이어 목숨
    public Text textPlayerHp;
    public bool isPlayerGetItem = false;

    [Header("적 생성 정보")]
    public GameObject[] enemys;  //적 오브젝트를 담을 배열
    public Transform spawnPoint; //적이 생성될 위치의 Transform
    public int enemyCount = 0; //적 생성 시 적 오브젝트의 숫자를 담을 변수
    public int maxEnemy = 5;   //적 최대 숫자 (4마리까지)
    public float createEnemyTime = 3f; //적 추가 생성 시 이전 적생성 시간과의 딜레이

    [Header("스테이지 정보")]
    public int gameScore = 0;
    public int bestScore = 0;
    public float runawayTimer = 15f; //무적시간
    public Text textGameScore; //게임스코어 텍스트
    public Text textBestScore; //베스트스코어 텍스트
    public Text textRunawayTimer; //무적시간 텍스트
    public GameObject textGameOver;  //게임오버 시 화면에 표시할 게임오버 텍스트
    public int scorePointCount = 6; //스테이지에 존재하는 무적아이템의 갯수 (무적아이템+일반아이템의 수량으로 게임클리어 여부 결정)

    private void Awake()
    {
        spawnPoint = GameObject.Find("EnemySpawnPoint").GetComponent<Transform>();
        textGameScore = GameObject.Find("TextGamePoint").GetComponent<Text>();
        textBestScore = GameObject.Find("TextBestScore").GetComponent<Text>();
        textRunawayTimer = GameObject.Find("TextTimer").GetComponent<Text>();
        textPlayerHp = GameObject.Find("TextPlayerHp").GetComponent<Text>();
        textPlayerHp.text = "나의 목숨: 3개";
        textRunawayTimer.text = "무적시간: 15초";
        textGameOver = GameObject.Find("TextGameOver");
        textGameOver.SetActive(false);

        LoadGameData(); //저장된 데이터가 있으면 불러옴 (현재 베스트스코어만 저장함)
    }

    void LoadGameData() //저장된 데이터 불러오기
    {
        bestScore = PlayerPrefs.GetInt("BestScore", 0);
        textBestScore.text = "BEST : " + bestScore.ToString("00000");
    }

    public void GameOverText() //게임오버 또는 게임클리어 시 화면에 나타낼 텍스트
    {
        textGameOver.SetActive(true);
        if (scorePointCount <= 0)
        {
            textGameOver.GetComponent<Text>().text = "GAME CLEAR\n\n최종득점: "+ gameScore.ToString() +"\n\n게임을 다시 시작하시려면 Q 키를 눌러주세요";
            Time.timeScale = 0;
        }
        else
        {
            textGameOver.GetComponent<Text>().text = "GAME OVER\n\n게임을 다시 시작하시려면 Q 키를 눌러주세요";
            Time.timeScale = 0;
        }

    }

    public void GamePointUp()  //플레이어가 아이템을 획득하면 실행 또는 무적상태에서 적을 제거해도 실행
    {
        gameScore++;

        textGameScore.text = "SCORE : " + gameScore.ToString("00000");
        if (gameScore >= bestScore) //베스트스코어 갱신
        {
            PlayerPrefs.SetInt("BestScore", gameScore);
            textBestScore.text = "BEST : " + bestScore.ToString("00000");
        }
        if(scorePointCount <= 0)
        {
            GameOver(); //스테이지에 존재하는 모든 아이템을 먹으면 게임클리어
        }
    }

    private void Start()
    {
        GameManager.instance.isGameOver = false; //게임이 시작될 때에는 isGameOver 를 false 로 초기화
        CreatePlayer(); //플레이어 생성
        GameObject[] tempObj = GameObject.FindGameObjectsWithTag("Score");  //화면에 있는 아이템의 수량을 확인
        foreach (GameObject ob in tempObj)
        {
            scorePointCount++;  //기존 무적아이템 수량에 나머지 아이템의 수량을 더해준다
        }
    }

    void CreatePlayer() //플레이어 생성, 리스폰 시에도 동일한 함수 사용
    {
        if (!GameManager.instance.isGameOver)
        {
            Instantiate(player, new Vector3(0, 5, -65.3f), Quaternion.identity);
            curPlayer = player;
            GameObject[] tempObj = GameObject.FindGameObjectsWithTag("Enemy");
            foreach (GameObject ob in tempObj)
            {
                ob.GetComponent<EnemyMove>().SetPlayerTransform();
            }
        }
    }
    public void PlayerDie() //플레이어 사망 함수
    {
        playerHp--;
        textPlayerHp.text = "나의 목숨: "+ playerHp +"개";
        if (playerHp > 0)
        {
            Invoke("CreatePlayer", 3f); //리스폰이 가능하다면 3초후 플레이어 오브젝트 생성
        }
        else if (playerHp <= 0)
        {
            Invoke("GameOver", 1.5f); //체력이 모두 소진되면 게임오버처리 
        }
    }

    void GameOver() //게임오버 처리
    {
        GameManager.instance.isGameOver = true;
        GameOverText();
    }

    private void Update()
    {
        SpawnEnemy(); //적 생성
        if (isPlayerGetItem == true)  //무적아이템 획득 여부에 따라 무적시간 카운트 텍스트 변경
        {
            runawayTimer -= Time.deltaTime;
            textRunawayTimer.text = "무적시간: " + runawayTimer.ToString("N0") + "초";
        }
        else if(isPlayerGetItem == false)  //무적아이템 미획득 시
        {
            runawayTimer = 15f;
            textRunawayTimer.text = "무적시간: 15초";
        }
    }
    void SpawnEnemy() //적 유닛 스폰
    {
            int enemyCount = GameObject.FindGameObjectsWithTag("Enemy").Length;
            createEnemyTime -= Time.deltaTime;
            if (enemyCount < maxEnemy && createEnemyTime <=0)
            {
                int enemyNum = Random.Range(0, enemys.Length);
                Instantiate(enemys[enemyNum], spawnPoint.position + new Vector3(0, 3, 0), Quaternion.identity);
                createEnemyTime = 3f;
            }
        
    }
}
