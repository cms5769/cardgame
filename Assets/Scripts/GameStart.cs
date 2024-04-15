using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class GameStart : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //게임화면 시작하는 함수
    public void StartGame()
    {
        //씬스를 변경하는 코드
        SceneManager.LoadScene("InGame");
    }
}
