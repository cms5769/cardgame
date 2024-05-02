using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int StelaCost { get; private set; }  // 스텔라 코스트
    public int MaxStelaCost { get; private set; }  // 최대 스텔라 코스트

    public GameObject stelaCostPrefab;  // 스텔라 코스트를 표시하는 Prefab
    public Transform stelaCostContainer;  // 스텔라 코스트 Prefab의 부모 객체

    [SerializeField]
    public bool isPlayer1;  // 플레이어 1인지 여부를 나타내는 필드

    // 스텔라 코스트를 증가시키는 메소드
    public void IncreaseMaxStelaCost()
    {
        MaxStelaCost++;
        UpdateStelaCostUI();  // UI 업데이트
    }

    // 스텔라 코스트를 최대치로 설정하는 메소드
    public void ResetStelaCost()
    {
        StelaCost = MaxStelaCost;
        UpdateStelaCostUI();  // UI 업데이트
    }

    // 스텔라 코스트를 표시하는 UI를 업데이트하는 메소드
   private void UpdateStelaCostUI()
{
    // 기존에 표시되던 스텔라 코스트 Prefab 삭제
    foreach (Transform child in stelaCostContainer)
    {
        Destroy(child.gameObject);
    }

    // 스텔라 코스트 만큼 Prefab 인스턴스화
    for (int i = 0; i < MaxStelaCost; i++)
    {
        // Prefab의 위치 계산
        Vector3 position;
        if (isPlayer1)
        {
            position = new Vector3(25, -12 + i * 1.5f, 0);
        }
        else
        {
            position = new Vector3(-25, 12 - i * 1.5f, 0);  // 위치 변경
        }

        // Prefab 인스턴스화
        GameObject stelaCostObject = Instantiate(stelaCostPrefab, position, Quaternion.identity, stelaCostContainer);

        // 현재 코스트가 최대 코스트보다 작은 경우, Prefab을 비활성화
        if (i >= StelaCost)
        {
            stelaCostObject.SetActive(false);
        }
    }
}
}