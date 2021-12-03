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
        if (instance == null)//싱글턴이 존재하지 않을 경우
        {
            instance = this;//해당 오브젝트를 싱글톤 오브젝트로 설정
        }

        else if (instance != this)//instance에 할당된 클래스의 인스턴스가 다를 경우
        {
            Destroy(this.gameObject);//중복 방지를 위해 이 오브젝트를 삭제
        }

        DontDestroyOnLoad(gameObject);//씬이 변경되더라도 삭제하지 않고 유지
        
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q)) //게임 재시작 (게임플레이 중 언제든 실행 가능)
        {
            Time.timeScale = 1; //게임오버, 게임클리어 시에는 타임스케일이 0 으로 바뀜, 재시작 시에는 1로 변경
            SceneManager.LoadScene(0);
        }
    }
}
