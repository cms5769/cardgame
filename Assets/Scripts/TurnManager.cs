using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun.Demo.Cockpit;
using Photon.Realtime;
using UnityEngine;
using Random = UnityEngine.Random;

public class TurnManager : MonoBehaviour
{
    // 싱글톤 패턴을 위한 인스턴스
    public static TurnManager Inst { get; private set; }

    private Player player1;  // 플레이어 1
    private Player player2;  // 플레이어 2

    private void Awake()
    {
        // GameManager 클래스의 Player 인스턴스를 가져옴
        Inst = this;
        player1 = GameManager.Inst.player1;
        player2 = GameManager.Inst.player2;

    }

    [Header("Develop")]
    [SerializeField][Tooltip("시작 턴 모드를 정합닌다")] ETrunMode eTrunMode;  // 시작 턴 모드를 설정

    [SerializeField][Tooltip("카드 배분이 매우 빨라집니다.")] bool fastMode;  // 카드 배분 속도를 설정

    [SerializeField][Tooltip("턴이 시작될 때 플레이어가 가지고 있는 카드의 수")] int startCardCount;  // 턴 시작 시 플레이어가 가지고 있는 카드 수

    [Header("Properties")]
    public bool isLoading; // 게임이 로딩 중인지 여부
    public bool myTurn;  // 내 턴인지 여부

    private int turnCount = 1;  // 현재 턴 수

    enum ETrunMode { Random, My, Other }  // 턴 모드를 나타내는 열거형
    WaitForSeconds delay05 = new WaitForSeconds(0.5f);  // 0.5초 대기
    WaitForSeconds delay07 = new WaitForSeconds(0.7f);  // 0.7초 대기

    public static Action<bool> OnAddCard1;  // 카드 추가 이벤트 1
    public static Action<bool> OnAddCard2;  // 카드 추가 이벤트 2

    public static event Action<bool> OnTurnStarted;  // 턴 시작 이벤트

    // 게임 설정을 하는 메소드
    void GameSetup()
    {
        if (fastMode)
            delay05 = new WaitForSeconds(0.05f);  // 빠른 모드인 경우 대기 시간을 줄임

        // 턴 모드에 따라 내 턴인지 결정
        switch (eTrunMode)
        {
            case ETrunMode.Random:  // 무작위 모드인 경우
                myTurn = Random.Range(0, 2) == 0 ? true : false;  // 0 또는 1을 무작위로 생성하여 내 턴인지 결정
                break;
            case ETrunMode.My:  // 내 턴 모드인 경우
                myTurn = true;  // 내 턴으로 설정
                break;
            case ETrunMode.Other:  // 다른 턴 모드인 경우
                myTurn = false;  // 내 턴이 아님으로 설정
                break;
        }
    }

    // 게임을 시작하는 코루틴
    public IEnumerator StartGameCo()
    {
        GameSetup();  // 게임 설정
        isLoading = true;  // 로딩 중으로 설정

        // 모든 플레이어의 최대 스텔라 코스트를 1로 설정하고, 스텔라 코스트를 최대치로 설정
        player1.IncreaseMaxStelaCost();
        player1.ResetStelaCost();
        player2.IncreaseMaxStelaCost();
        player2.ResetStelaCost();

        // 각 플레이어에게 카드를 배분
        for (int i = 0; i < startCardCount; i++)
        {
            yield return delay05;
            OnAddCard1?.Invoke(true);  // 카드 추가 이벤트 1 호출
            yield return delay05;
            OnAddCard2?.Invoke(true);  // 카드 추가 이벤트 2 호출
        }
    }

    // 턴을 시작하는 코루틴
    IEnumerator StartTurnCo()
    {
        isLoading = true;  // 로딩 중으로 설정)
        yield return delay07;
        if (myTurn)
        {
            OnAddCard1?.Invoke(true);  // 내 턴인 경우 카드 추가 이벤트 1 호출
            if (turnCount >= 2)  // 2번째 턴부터
            {
                player1.IncreaseMaxStelaCost();  // 최대 스텔라 코스트 증가
            }
            player1.ResetStelaCost();  // 스텔라 코스트를 최대치로 설정

            // 스텔라 코스트와 최대 스텔라 코스트 출력
            Debug.Log("Player 1 Stela Cost: " + player1.StelaCost);
            Debug.Log("Player 1 Max Stela Cost: " + player1.MaxStelaCost);
        }
        else
        {
            OnAddCard2?.Invoke(true);  // 내 턴이 아닌 경우 카드 추가 이벤트 2 호출
            if (turnCount >= 2)  // 2번째 턴부터
            {
                player2.IncreaseMaxStelaCost();  // 최대 스텔라 코스트 증가
            }
            player2.ResetStelaCost();  // 스텔라 코스트를 최대치로 설정

            // 스텔라 코스트와 최대 스텔라 코스트 출력
            Debug.Log("Player 2 Stela Cost: " + player2.StelaCost);
            Debug.Log("Player 2 Max Stela Cost: " + player2.MaxStelaCost);
        }
        yield return delay07;
        isLoading = false;  // 로딩 완료
        OnTurnStarted?.Invoke(myTurn);
    }

    public void EndTurn()
    {
        turnCount++;  // 턴 수 증가
        myTurn = !myTurn;  // 턴 변경
        StartCoroutine(StartTurnCo());  // 턴 시작 코루틴 호출
    }
}