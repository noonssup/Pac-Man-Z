using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageManager : MonoBehaviour
{
    public GameObject player;  //�÷��̾� ���ӿ�����Ʈ
    public GameObject curPlayer; //���� ȭ�鿡 �ִ� �÷��̾�
    public int playerHp = 3;  //�÷��̾� ���
    public Text textPlayerHp;
    public bool isPlayerGetItem = false;

    [Header("�� ���� ����")]
    public GameObject[] enemys;  //�� ������Ʈ�� ���� �迭
    public Transform spawnPoint; //���� ������ ��ġ�� Transform
    public int enemyCount = 0; //�� ���� �� �� ������Ʈ�� ���ڸ� ���� ����
    public int maxEnemy = 5;   //�� �ִ� ���� (4��������)
    public float createEnemyTime = 3f; //�� �߰� ���� �� ���� ������ �ð����� ������

    [Header("�������� ����")]
    public int gameScore = 0;
    public int bestScore = 0;
    public float runawayTimer = 15f; //�����ð�
    public Text textGameScore; //���ӽ��ھ� �ؽ�Ʈ
    public Text textBestScore; //����Ʈ���ھ� �ؽ�Ʈ
    public Text textRunawayTimer; //�����ð� �ؽ�Ʈ
    public GameObject textGameOver;  //���ӿ��� �� ȭ�鿡 ǥ���� ���ӿ��� �ؽ�Ʈ
    public int scorePointCount = 6; //���������� �����ϴ� ������������ ���� (����������+�Ϲݾ������� �������� ����Ŭ���� ���� ����)

    private void Awake()
    {
        spawnPoint = GameObject.Find("EnemySpawnPoint").GetComponent<Transform>();
        textGameScore = GameObject.Find("TextGamePoint").GetComponent<Text>();
        textBestScore = GameObject.Find("TextBestScore").GetComponent<Text>();
        textRunawayTimer = GameObject.Find("TextTimer").GetComponent<Text>();
        textPlayerHp = GameObject.Find("TextPlayerHp").GetComponent<Text>();
        textPlayerHp.text = "���� ���: 3��";
        textRunawayTimer.text = "�����ð�: 15��";
        textGameOver = GameObject.Find("TextGameOver");
        textGameOver.SetActive(false);

        LoadGameData(); //����� �����Ͱ� ������ �ҷ��� (���� ����Ʈ���ھ ������)
    }

    void LoadGameData() //����� ������ �ҷ�����
    {
        bestScore = PlayerPrefs.GetInt("BestScore", 0);
        textBestScore.text = "BEST : " + bestScore.ToString("00000");
    }

    public void GameOverText() //���ӿ��� �Ǵ� ����Ŭ���� �� ȭ�鿡 ��Ÿ�� �ؽ�Ʈ
    {
        textGameOver.SetActive(true);
        if (scorePointCount <= 0)
        {
            textGameOver.GetComponent<Text>().text = "GAME CLEAR\n\n��������: "+ gameScore.ToString() +"\n\n������ �ٽ� �����Ͻ÷��� Q Ű�� �����ּ���";
            Time.timeScale = 0;
        }
        else
        {
            textGameOver.GetComponent<Text>().text = "GAME OVER\n\n������ �ٽ� �����Ͻ÷��� Q Ű�� �����ּ���";
            Time.timeScale = 0;
        }

    }

    public void GamePointUp()  //�÷��̾ �������� ȹ���ϸ� ���� �Ǵ� �������¿��� ���� �����ص� ����
    {
        gameScore++;

        textGameScore.text = "SCORE : " + gameScore.ToString("00000");
        if (gameScore >= bestScore) //����Ʈ���ھ� ����
        {
            PlayerPrefs.SetInt("BestScore", gameScore);
            textBestScore.text = "BEST : " + bestScore.ToString("00000");
        }
        if(scorePointCount <= 0)
        {
            GameOver(); //���������� �����ϴ� ��� �������� ������ ����Ŭ����
        }
    }

    private void Start()
    {
        GameManager.instance.isGameOver = false; //������ ���۵� ������ isGameOver �� false �� �ʱ�ȭ
        CreatePlayer(); //�÷��̾� ����
        GameObject[] tempObj = GameObject.FindGameObjectsWithTag("Score");  //ȭ�鿡 �ִ� �������� ������ Ȯ��
        foreach (GameObject ob in tempObj)
        {
            scorePointCount++;  //���� ���������� ������ ������ �������� ������ �����ش�
        }
    }

    void CreatePlayer() //�÷��̾� ����, ������ �ÿ��� ������ �Լ� ���
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
    public void PlayerDie() //�÷��̾� ��� �Լ�
    {
        playerHp--;
        textPlayerHp.text = "���� ���: "+ playerHp +"��";
        if (playerHp > 0)
        {
            Invoke("CreatePlayer", 3f); //�������� �����ϴٸ� 3���� �÷��̾� ������Ʈ ����
        }
        else if (playerHp <= 0)
        {
            Invoke("GameOver", 1.5f); //ü���� ��� �����Ǹ� ���ӿ���ó�� 
        }
    }

    void GameOver() //���ӿ��� ó��
    {
        GameManager.instance.isGameOver = true;
        GameOverText();
    }

    private void Update()
    {
        SpawnEnemy(); //�� ����
        if (isPlayerGetItem == true)  //���������� ȹ�� ���ο� ���� �����ð� ī��Ʈ �ؽ�Ʈ ����
        {
            runawayTimer -= Time.deltaTime;
            textRunawayTimer.text = "�����ð�: " + runawayTimer.ToString("N0") + "��";
        }
        else if(isPlayerGetItem == false)  //���������� ��ȹ�� ��
        {
            runawayTimer = 15f;
            textRunawayTimer.text = "�����ð�: 15��";
        }
    }
    void SpawnEnemy() //�� ���� ����
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
