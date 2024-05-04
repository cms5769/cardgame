using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int StelaCost { get; private set; }  // 스텔라 코스트
    public int MaxStelaCost { get; private set; }  // 최대 스텔라 코스트

    public GameObject stelaCostPrefab;  // 스텔라 코스트를 표시하는 Prefab
    public Transform stelaCostContainer;  // 스텔라 코스트 Prefab의 부모 객체

    public GameObject usedStelaCostPrefab;  // 사용된 스텔라 코스트를 표시하는 Prefab

    private List<GameObject> stelaCostObjects = new List<GameObject>();  // 스텔라 코스트 Prefab 리스트

    [SerializeField]
    public bool isPlayer1;  // 플레이어 1인지 여부를 나타내는 필드

    // 스텔라 코스트를 증가시키는 메소드
    public void IncreaseMaxStelaCost()
    {
        if (MaxStelaCost < 10)  // MaxStelaCost가 10 미만인 경우에만 증가
        {
            MaxStelaCost++;
            UpdateStelaCostUI();  // UI 업데이트
        }
    }

    // 스텔라 코스트를 최대치로 설정하는 메소드
    public void ResetStelaCost()
    {
        StelaCost = MaxStelaCost;
        UpdateStelaCostUI();  // UI 업데이트
    }

    public void UseStelaCost(int cost)
    {
        // 스텔라 코스트가 충분한지 확인
        if (StelaCost >= cost)
        {
            // 스텔라 코스트 감소
            StelaCost -= cost;

            // UI 업데이트
            UpdateStelaCostUI();
        }
        else
        {
            Debug.Log("Not enough stela cost");
        }
    }

    // 스텔라 코스트를 표시하는 UI를 업데이트하는 메소드
    private void UpdateStelaCostUI()
    {
        // 기존에 표시되던 스텔라 코스트 Prefab 삭제
        foreach (Transform child in stelaCostContainer)
        {
            Destroy(child.gameObject);
        }

        stelaCostObjects.Clear();  // 리스트 초기화

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

            // 리스트에 추가
            stelaCostObjects.Add(stelaCostObject);

            // 현재 코스트가 최대 코스트보다 작은 경우, 다른 Prefab으로 변경
            if (i >= StelaCost)
            {
                // 마지막 요소 비활성화
                stelaCostObjects[stelaCostObjects.Count - 1].SetActive(false);

                // 새 Prefab 인스턴스화
                GameObject usedStelaCostObject = Instantiate(usedStelaCostPrefab, position, Quaternion.identity, stelaCostContainer);

                // 리스트의 마지막 요소로 설정
                stelaCostObjects[stelaCostObjects.Count - 1] = usedStelaCostObject;
            }
        }
    }
}