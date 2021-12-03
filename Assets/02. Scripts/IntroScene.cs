using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroScene : MonoBehaviour
{
    private void Start()
    {
        //게임 재시작 시 씬이동을 위해 만듬.. 의미없음
        SceneManager.LoadScene(1);
    }
}
