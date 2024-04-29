using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update

    public static GameManager Inst { get; private set; }
    void Awake() => Inst = this;


    void Start()
    {
        StartGame();
    }

    // Update is called once per frame
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
    }

    public void StartGame()
    {
        StartCoroutine(TurnManager.Inst.StartGameCo());
    }
}
