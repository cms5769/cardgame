using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun.Demo.Cockpit;
using UnityEngine;
using Random = UnityEngine.Random;

public class TurnManager : MonoBehaviour
{
    // 게임의 턴 단계를 나타내는 열거형
    public static TurnManager Inst { get; private set; }
    void Awake() => Inst = this;

    [Header("Develop")]
    [SerializeField][Tooltip("시작 턴 모드를 정합닌다")] ETrunMode eTrunMode;
    
    [SerializeField][Tooltip("카드 배분이 매우 빨라집니다.")] bool fastMode;

    [SerializeField][Tooltip("턴이 시작될 때 플레이어가 가지고 있는 카드의 수")] int startCardCount;

    [Header("Properties")]
    public bool isLoading; // 게임이 로딩 중인지 여부
    public bool myTurn;

    enum ETrunMode { Random, My, Other }
    WaitForSeconds delay05 = new WaitForSeconds(0.5f);
    WaitForSeconds delay07 = new WaitForSeconds(0.7f);

    public static Action<bool> OnAddCard1;
    public static Action<bool> OnAddCard2;


    void GameSetup()
    {
        if(fastMode)
            delay05 = new WaitForSeconds(0.05f);

        switch (eTrunMode)
        {
            case ETrunMode.Random:
                myTurn = Random.Range(0, 2) == 0 ? true : false;
                break;
            case ETrunMode.My:
                myTurn = true;
                break;
            case ETrunMode.Other:
                myTurn = false;
                break;
        }
    }

    public IEnumerator StartGameCo()
    {
        GameSetup();
        isLoading = true;
        
        for (int i = 0; i < startCardCount; i++)
        {
            yield return delay05;
            OnAddCard1?.Invoke(true);
            yield return delay05;
            OnAddCard2?.Invoke(true);
        }

    }

    IEnumerator StartTurnCo()
    {
        isLoading = true;

        yield return delay07;
        if(myTurn)
            OnAddCard1?.Invoke(true);
        else
            OnAddCard2?.Invoke(true);
        yield return delay07;
        isLoading = false;
    }
}