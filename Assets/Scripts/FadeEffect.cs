using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


public enum FadeState{ FadeIn=0, FadeOut, FadeInOut, FadeLoop}

public class FadeEffect : MonoBehaviour
{

    [SerializeField]
    [Range(0.0f, 10f)]
    private float fadeTime; //페이드 효과가 진행되는 시간(값이 클수록 빠름)
    [SerializeField]
    private AnimationCurve fadeCurve; //페이드 효과의 곡선
    private Image image;  //페이드 효과에 사용되는 검은 바탕 이미지
    [SerializeField]
    private FadeState fadeState; //페이드 상태

    private void Awake()
    {
        image = GetComponent<Image>();

        // FadeIn 함수를 호출하여 알파값을 1에서 0으로 변경 (화면이 밝아짐)
        OnFade(fadeState);
    }

    public void OnFade(FadeState state)
    {
        fadeState = state;

        switch (fadeState)
        {
            case FadeState.FadeIn:
                Debug.Log("FadeIn");
                StartCoroutine(Fade(1.0f, 0.0f)); //FadeIn 함수를 호출하여 알파값을 1에서 0으로 변경 (화면이 밝아짐)
                break;
            case FadeState.FadeOut:
                Debug.Log("FadeOut");
                StartCoroutine(Fade(0.0f, 1.0f)); //FadeOut 함수를 호출하여 알파값을 0에서 1로 변경 (화면이 어두워짐)
                break;
            case FadeState.FadeInOut: 
            case FadeState.FadeLoop:
                Debug.Log("FadeInOut");
                StartCoroutine(FadeInOut());
                break;
        }
    }

    private  IEnumerator FadeInOut()
    {
        while (true)
        {
            //코루틴 내부에서 코루틴 함수를 호출하면 해당 코루틴 함수가 종료되어야 다음 문장 실행
            yield return StartCoroutine(Fade(1.0f, 0.0f)); //FadeIn 함수를 호출하여 알파값을 1에서 0으로 변경 (화면이 밝아짐)
            transform.SetAsLastSibling();
            yield return StartCoroutine(Fade(0.0f, 1.0f)); //FadeOut 함수를 호출하여 알파값을 0에서 1로 변경 (화면이 어두워짐)

            //1회만 재생하는 상태일때 break;
            if(fadeState == FadeState.FadeInOut)
            {
                break;
            }
        }
    }

    private IEnumerator Fade(float start, float end)
    {
        float currentTime = 0.0f;
        float percent = 0.0f;

        while (percent < 1)
        {

            //fadeTime동안 while문을 돌면서 percent값을 0에서 1까지 증가시킴
            currentTime += Time.deltaTime;
            percent = currentTime / fadeTime;

            //알파값을 start에서 end까지 fadeTime동안 변경
            Color color = image.color;
            color.a = Mathf.Lerp(start, end, fadeCurve.Evaluate(percent));
            image.color = color;

            yield return null;


        }
        // 페이드 이펙트가 끝나면 Image를 Canvas의 제일 뒤로 보냄
        transform.SetAsFirstSibling();
    }

}   
