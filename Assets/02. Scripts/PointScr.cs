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
        if (other.CompareTag("Player") && this.gameObject.CompareTag("Score")) //플레이어가 일반 아이템을 먹으면
        {
            stageMgr.scorePointCount--;  //아이템 갯수 삭감 (맵 상의 아이템 수량을 기준으로 게임클리어 여부 판명)
            stageMgr.GamePointUp();      //스코어 추가
            Destroy(this.gameObject);    //아이템 삭제
        }
        else if(other.CompareTag("Player") && this.gameObject.CompareTag("Item")) //플레이어가 무적 아이템을 먹으면
        {
            stageMgr.scorePointCount--;                         //아이템 갯수 삭감 (맵 상의 아이템 수량을 기준으로 게임클리어 여부 판명)
            other.GetComponent<PlayerMove>().PlayerHaveItem();  //스코어 추가, 무적효과 발동
            Destroy(this.gameObject);                           //아이템 삭제
        }
    }
}
