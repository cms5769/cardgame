using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class SceneLoader : MonoBehaviour
{
    public GameObject loadingScreen;  // 로딩 화면 UI를 담을 필드
    public Slider progressBar;  // 진행률 바를 담을 필드

    public void LoadScene(string sceneName)
    {
        StartCoroutine(LoadSceneAsync(sceneName));
    }

    IEnumerator LoadSceneAsync(string sceneName)
    {
        // 로딩 화면 활성화
        loadingScreen.SetActive(true);

        // 씬 비동기 로드 시작
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);

        // 씬이 로드되는 동안 로딩 진행률 업데이트
        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);
            progressBar.value = progress;
            yield return null;
        }
        
        // 씬 로드가 완료되면 로딩 화면 비활성화
        loadingScreen.SetActive(false);
    }
}