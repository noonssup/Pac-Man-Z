using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointScr : MonoBehaviour
{
    public StageManager stageMgr;

    private void Awake()
    {
        stageMgr = GameObject.Find("StageManager").GetComponent<StageManager>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && this.gameObject.CompareTag("Score")) //�÷��̾ �Ϲ� �������� ������
        {
            stageMgr.scorePointCount--;  //������ ���� �谨 (�� ���� ������ ������ �������� ����Ŭ���� ���� �Ǹ�)
            stageMgr.GamePointUp();      //���ھ� �߰�
            Destroy(this.gameObject);    //������ ����
        }
        else if(other.CompareTag("Player") && this.gameObject.CompareTag("Item")) //�÷��̾ ���� �������� ������
        {
            stageMgr.scorePointCount--;                         //������ ���� �谨 (�� ���� ������ ������ �������� ����Ŭ���� ���� �Ǹ�)
            other.GetComponent<PlayerMove>().PlayerHaveItem();  //���ھ� �߰�, ����ȿ�� �ߵ�
            Destroy(this.gameObject);                           //������ ����
        }
    }
}
