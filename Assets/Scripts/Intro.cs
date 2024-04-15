using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Intro : MonoBehaviour
{
    // Start is called before the first frame update
    private IEnumerator Start()
    {   
        //4초 대기
        yield return new WaitForSeconds(4);

         // 기다린 후 메소드를 실행
         ExecuteAfterDelay();
    }

    // Update is called once per frame
    private void ExecuteAfterDelay()
    {
        SceneManager.LoadScene("Roby");
    }
}
