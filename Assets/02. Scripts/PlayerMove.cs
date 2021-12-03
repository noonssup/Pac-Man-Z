using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public enum State
    {
        UP, DOWN, LEFT, RIGHT,IDLE
    }

    public State state;
    Rigidbody tr;
    Vector3 moveDir;
    StageManager stageMgr;

    float moveSpeed = 9f;
    public float runawayTimer = 15f;
    public bool isHaveItem = false; //아이템 효과 활성화 여부


    private void Awake()
    {
        tr = GetComponent<Rigidbody>();
        stageMgr = GameObject.Find("StageManager").GetComponent<StageManager>();
    }

    private void OnEnable()
    {
        state = State.IDLE; //플레이어 초기 상태 지정 (생성된 자리에 멈춰있기)
    }

    private void Update() 
        //플레이어 이동 방향 지정
        //해당 방향으로 자동 이동되며, 키를 누르면 이동상태가 변경되어 해당 방향으로 이동한다
    {
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            state = State.UP;
        }
        else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            state = State.DOWN;
        }
        else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            state = State.LEFT;
        }
        else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            state = State.RIGHT;
        }

        if (isHaveItem == true) //무적아이템을 먹으면 플레이어가 적을 없앨 수 있는 상태가 된다
        {
            stageMgr.isPlayerGetItem = true;
            runawayTimer -= Time.deltaTime;

            if (runawayTimer < 0)   //지속 시간은 15초 (이동속도 9f 변경)
            {
                stageMgr.isPlayerGetItem = false;
                isHaveItem = false;
                moveSpeed = 9f;
                runawayTimer = 15f;
            }
        }

    }
    private void FixedUpdate()
    {
        Move(); //플레이어의 움직임을 동작시켜줄 함수
    }

    void Move() //이동 함수
    {
        switch (state)
        {
            case State.UP:
                moveDir = Vector3.forward;
                tr.transform.Translate(moveDir * moveSpeed * Time.deltaTime);
                break;
            case State.DOWN:
                moveDir = -Vector3.forward;
                tr.transform.Translate(moveDir * moveSpeed * Time.deltaTime);
                break;
            case State.LEFT:
                moveDir = -Vector3.right;
                tr.transform.Translate(moveDir * moveSpeed * Time.deltaTime);
                break;
            case State.RIGHT:
                moveDir = Vector3.right;
                tr.transform.Translate(moveDir * moveSpeed * Time.deltaTime);
                break;

        }
    }

    public void PlayerDie()  //플레이어 사망 처리
    {
        //스테이지매니저를 통해 체력삭감, 남은 체력에 따라 리스폰 또는 게임오버 표시
        stageMgr.PlayerDie();
        Destroy(this.gameObject); //적과 닿아서 사망 시 플레이어 오브젝트 삭제
    }

    public void GetScore() //아이템 획득 또는 적 제거 시 점수 추가
    {
        stageMgr.GamePointUp();
    }

    public void PlayerHaveItem() //무적아이템 획득 (이동속도 10f 변경)
    {
        stageMgr.GamePointUp();
        moveSpeed = 10f;   
        //이동속도는 10 이상으로 설정하면 맵에 배치된 벽들을 통과하는 현상이 발생함
        //그래서 이동속도는 9~10 으로 제한하였음
        isHaveItem = true;
    }
}
