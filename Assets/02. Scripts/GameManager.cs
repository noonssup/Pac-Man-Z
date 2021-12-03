using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;

    public StageManager stageMgr;
    public bool isGameOver = false;

    private void Awake()
    {
        if (instance == null)//�̱����� �������� ���� ���
        {
            instance = this;//�ش� ������Ʈ�� �̱��� ������Ʈ�� ����
        }

        else if (instance != this)//instance�� �Ҵ�� Ŭ������ �ν��Ͻ��� �ٸ� ���
        {
            Destroy(this.gameObject);//�ߺ� ������ ���� �� ������Ʈ�� ����
        }

        DontDestroyOnLoad(gameObject);//���� ����Ǵ��� �������� �ʰ� ����
        
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q)) //���� ����� (�����÷��� �� ������ ���� ����)
        {
            Time.timeScale = 1; //���ӿ���, ����Ŭ���� �ÿ��� Ÿ�ӽ������� 0 ���� �ٲ�, ����� �ÿ��� 1�� ����
            SceneManager.LoadScene(0);
        }
    }
}
