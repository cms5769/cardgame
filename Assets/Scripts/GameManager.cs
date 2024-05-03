using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Inst { get; private set; }

    public Player player1;  // 플레이어 1
    public Player player2;  // 플레이어 2

    private void Awake()
    {
        Inst = this;
    }


    void Start()
    {
        StartGame();
    }

    void Update()
    {
        InputCheatKey();
    }

    void InputCheatKey()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            TurnManager.OnAddCard1?.Invoke(true);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            TurnManager.OnAddCard2?.Invoke(true);
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            TurnManager.Inst.EndTurn();
        }
        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            player1.UseStelaCost(1);
        }
        if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            player2.UseStelaCost(1);
        }


    }

    public void StartGame()
    {
        StartCoroutine(TurnManager.Inst.StartGameCo());
    }
}