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
    public bool isHaveItem = false; //������ ȿ�� Ȱ��ȭ ����


    private void Awake()
    {
        tr = GetComponent<Rigidbody>();
        stageMgr = GameObject.Find("StageManager").GetComponent<StageManager>();
    }

    private void OnEnable()
    {
        state = State.IDLE; //�÷��̾� �ʱ� ���� ���� (������ �ڸ��� �����ֱ�)
    }

    private void Update() 
        //�÷��̾� �̵� ���� ����
        //�ش� �������� �ڵ� �̵��Ǹ�, Ű�� ������ �̵����°� ����Ǿ� �ش� �������� �̵��Ѵ�
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

        if (isHaveItem == true) //������������ ������ �÷��̾ ���� ���� �� �ִ� ���°� �ȴ�
        {
            stageMgr.isPlayerGetItem = true;
            runawayTimer -= Time.deltaTime;

            if (runawayTimer < 0)   //���� �ð��� 15�� (�̵��ӵ� 9f ����)
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
        Move(); //�÷��̾��� �������� ���۽����� �Լ�
    }

    void Move() //�̵� �Լ�
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

    public void PlayerDie()  //�÷��̾� ��� ó��
    {
        //���������Ŵ����� ���� ü�»谨, ���� ü�¿� ���� ������ �Ǵ� ���ӿ��� ǥ��
        stageMgr.PlayerDie();
        Destroy(this.gameObject); //���� ��Ƽ� ��� �� �÷��̾� ������Ʈ ����
    }

    public void GetScore() //������ ȹ�� �Ǵ� �� ���� �� ���� �߰�
    {
        stageMgr.GamePointUp();
    }

    public void PlayerHaveItem() //���������� ȹ�� (�̵��ӵ� 10f ����)
    {
        stageMgr.GamePointUp();
        moveSpeed = 10f;   
        //�̵��ӵ��� 10 �̻����� �����ϸ� �ʿ� ��ġ�� ������ ����ϴ� ������ �߻���
        //�׷��� �̵��ӵ��� 9~10 ���� �����Ͽ���
        isHaveItem = true;
    }
}
