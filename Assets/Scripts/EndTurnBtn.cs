using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
public class EndTurnBtn : MonoBehaviour
{
    [SerializeField]
    Sprite active;

    [SerializeField]
    Sprite inactive;

    [SerializeField]
    Text btnText;


    void Start()
    {
        Setup(false);
        TurnManager.OnTurnStarted += Setup;
    }

    void OnDestroy()
    {
        TurnManager.OnTurnStarted -= Setup;
    }

    void Setup(bool isActive)
    {
        GetComponent<Image>().sprite = isActive ? active : inactive;
        GetComponent<Button>().interactable = isActive;
        btnText.color = isActive ? new Color32(255, 192, 90, 255) : new Color32(55, 55, 55, 255);
    }
}
