using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;



public class EnemyMove : MonoBehaviour
{
    public enum State
    {
        PATROL, TRACE, DIE, RUNAWAY
        //순찰,  추적,  사망,  플레이어의 먹이
    }

    public State state = State.PATROL; //초기 상태 순찰로 지정

    public Transform playerTr;  //플레이어 위치 저장 변수
    Transform enemyTr;   //적 위치 저장 변수

    NavMeshAgent agent;

    public List<Transform> wayPoints;  //적유닛이 이동할 지점
    public int nextIdx; //다음 이동 지점의 배열 인덱스

    float traceDist = 20f; //추적 사거리
    public bool isEnemyDie = false;  //적 사망 여부 판단 변수

    float waitTime = 0;

    Color enemyColor; //플레이어가 무적아이템을 먹으면 적의 색상을 변경 시켜주기 위해 만든 변수


    private void Awake()
    {
        SetPlayerTransform(); //플레이어 Transform 할당
        enemyTr = GetComponent<Transform>();
        agent = GetComponent<NavMeshAgent>();
        agent.Warp(Vector3.zero);
    }

    private void OnEnable()
    {
        enemyColor = this.transform.GetComponent<Renderer>().material.color;
    }

    private void Start()
    {
        var group = GameObject.Find("WayPointGroup"); //순찰 지역 확인
        if (group != null)
        {
            group.GetComponentsInChildren<Transform>(wayPoints);
            wayPoints.RemoveAt(0);
            /*리스트 요소 중 지정된 인덱스의 오브젝트 삭제.
              :: GetComponent's'InChildren은 자식 오브젝트 뿐만 아니라 자기 자신까지 포함시키기 때문에 자기 자신인 0 인덱스 삭제..*/
            nextIdx = Random.Range(0, wayPoints.Count);
        }
    }

    private void FixedUpdate()
    {
        CheckState(); //유닛의 상태확인
        MoveAction(); //행동
    }

    public void SetPlayerTransform() 
        //플레이어 오브젝트가 지워졌다가 다시 생성될 때 플레이어의 Transform 을 재설정하도록 별도 함수로 구현
    {
        var player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTr = player.GetComponent<Transform>();
        }
    }
    void CheckState() //유닛 상태 갱신
    {
        if (playerTr != null && playerTr.GetComponent<PlayerMove>().isHaveItem == true)
        {
            state = State.RUNAWAY;
        }
        else if (playerTr != null && playerTr.GetComponent<PlayerMove>().isHaveItem == false && Vector3.Distance(this.transform.position, playerTr.position) < traceDist)
        {
            state = State.TRACE;
        }
        else if (playerTr != null && playerTr.GetComponent<PlayerMove>().isHaveItem == false)
        {

            waitTime += Time.deltaTime;
            if (waitTime > 5f)
            {
                waitTime = 0;
                nextIdx = Random.Range(0, wayPoints.Count);
            }
            state = State.PATROL;
        }
    }

    void MoveAction() //행동 패턴
    {
        switch (state)
        {
            case State.PATROL: //순찰
                agent.speed = 9f;
                this.transform.GetComponent<Renderer>().material.color = enemyColor;
                if(this.transform.name == "EnemyBlue(Clone)")
                {
                    if (playerTr != null && Vector3.Distance(playerTr.position, enemyTr.position) <= 30.0f)
                    {
                        agent.SetDestination(playerTr.position);
                    }
                    else
                    {
                        agent.SetDestination(wayPoints[nextIdx].position);
                    }
                }
                else if (this.transform.name == "EnemyRed(Clone)")
                {
                    if (playerTr != null)
                    {
                        agent.SetDestination(playerTr.position);
                    }
                    else
                    {
                        agent.SetDestination(wayPoints[nextIdx].position);
                    }

                }
                else if (this.transform.name == "EnemyPink(Clone)")
                {
                    agent.SetDestination(wayPoints[nextIdx].position);
                }
                else if (this.transform.name == "EnemyOrange(Clone)")
                {
                    if (playerTr != null )
                    {
                        agent.SetDestination(playerTr.position);
                    }
                    else
                    {
                        agent.SetDestination(wayPoints[nextIdx].position);
                    }
                }

                    break;
            case State.TRACE: //추적
                this.transform.GetComponent<Renderer>().material.color = enemyColor;
                if (playerTr != null)
                {
                    agent.SetDestination(playerTr.position);
                }
                else
                {
                    agent.SetDestination(wayPoints[nextIdx].position);
                }
                break;
            case State.RUNAWAY: //플레이어의 먹잇감 상태
                agent.speed = 5f; //이동속도 저하
                this.transform.GetComponent<Renderer>().material.color = new Color(0.6f, 0.6f, 1, 1); //색상 변경
                if (playerTr != null)
                {
                    agent.SetDestination(-playerTr.position);
                }
                else
                {
                    agent.SetDestination(wayPoints[nextIdx].position);
                }
                break;
            case State.DIE: //실제 동작 시에는 효과없는 함수.... (DIE 빼고 싶지만, 혹시 모를 확장성을 위해 남겨둠)
                Destroy(this.gameObject);
                    break;
        }
    }

    private void OnTriggerEnter(Collider other) //플레이어와 닿으면
    {
        PlayerMove player = other.GetComponent<PlayerMove>();
        if (other.CompareTag("Player"))
        {

            if(player.isHaveItem == true) //플레이어가 무적상태일때 적유닛 오브젝트삭제, 점수 추가
            {
                player.GetScore();
                Destroy(this.gameObject);
            }
            else if(player.isHaveItem == false) //플레이어가 무적상태가 아닐 때 플레이어사망 함수 호출
            {
                player.PlayerDie();
            }
        }
    }
}
