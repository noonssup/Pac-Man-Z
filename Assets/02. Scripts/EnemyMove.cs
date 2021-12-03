using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;



public class EnemyMove : MonoBehaviour
{
    public enum State
    {
        PATROL, TRACE, DIE, RUNAWAY
        //����,  ����,  ���,  �÷��̾��� ����
    }

    public State state = State.PATROL; //�ʱ� ���� ������ ����

    public Transform playerTr;  //�÷��̾� ��ġ ���� ����
    Transform enemyTr;   //�� ��ġ ���� ����

    NavMeshAgent agent;

    public List<Transform> wayPoints;  //�������� �̵��� ����
    public int nextIdx; //���� �̵� ������ �迭 �ε���

    float traceDist = 20f; //���� ��Ÿ�
    public bool isEnemyDie = false;  //�� ��� ���� �Ǵ� ����

    float waitTime = 0;

    Color enemyColor; //�÷��̾ ������������ ������ ���� ������ ���� �����ֱ� ���� ���� ����


    private void Awake()
    {
        SetPlayerTransform(); //�÷��̾� Transform �Ҵ�
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
        var group = GameObject.Find("WayPointGroup"); //���� ���� Ȯ��
        if (group != null)
        {
            group.GetComponentsInChildren<Transform>(wayPoints);
            wayPoints.RemoveAt(0);
            /*����Ʈ ��� �� ������ �ε����� ������Ʈ ����.
              :: GetComponent's'InChildren�� �ڽ� ������Ʈ �Ӹ� �ƴ϶� �ڱ� �ڽű��� ���Խ�Ű�� ������ �ڱ� �ڽ��� 0 �ε��� ����..*/
            nextIdx = Random.Range(0, wayPoints.Count);
        }
    }

    private void FixedUpdate()
    {
        CheckState(); //������ ����Ȯ��
        MoveAction(); //�ൿ
    }

    public void SetPlayerTransform() 
        //�÷��̾� ������Ʈ�� �������ٰ� �ٽ� ������ �� �÷��̾��� Transform �� �缳���ϵ��� ���� �Լ��� ����
    {
        var player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTr = player.GetComponent<Transform>();
        }
    }
    void CheckState() //���� ���� ����
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

    void MoveAction() //�ൿ ����
    {
        switch (state)
        {
            case State.PATROL: //����
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
            case State.TRACE: //����
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
            case State.RUNAWAY: //�÷��̾��� ���հ� ����
                agent.speed = 5f; //�̵��ӵ� ����
                this.transform.GetComponent<Renderer>().material.color = new Color(0.6f, 0.6f, 1, 1); //���� ����
                if (playerTr != null)
                {
                    agent.SetDestination(-playerTr.position);
                }
                else
                {
                    agent.SetDestination(wayPoints[nextIdx].position);
                }
                break;
            case State.DIE: //���� ���� �ÿ��� ȿ������ �Լ�.... (DIE ���� ������, Ȥ�� �� Ȯ�强�� ���� ���ܵ�)
                Destroy(this.gameObject);
                    break;
        }
    }

    private void OnTriggerEnter(Collider other) //�÷��̾�� ������
    {
        PlayerMove player = other.GetComponent<PlayerMove>();
        if (other.CompareTag("Player"))
        {

            if(player.isHaveItem == true) //�÷��̾ ���������϶� ������ ������Ʈ����, ���� �߰�
            {
                player.GetScore();
                Destroy(this.gameObject);
            }
            else if(player.isHaveItem == false) //�÷��̾ �������°� �ƴ� �� �÷��̾��� �Լ� ȣ��
            {
                player.PlayerDie();
            }
        }
    }
}
