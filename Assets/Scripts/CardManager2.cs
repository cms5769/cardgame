using System.Collections;
using System.Collections.Generic;
using Photon.Pun.Demo.Cockpit;
using UnityEngine;

public class CardManager2 : MonoBehaviour
{
    public static CardManager2 Inst { get; private set; }
    void Awake() => Inst = this;

    [SerializeField] ItemSO itemSO2;
    [SerializeField] GameObject cardPrefab;
    [SerializeField] List<Card> otherCards;
    [SerializeField] Transform cardSpawnPoint;
    [SerializeField] Transform OtherCardLeft;
    [SerializeField] Transform OtherCardRight;

    List<Item> itemBuffer;

    public Item PopItem()
    {
        if (itemBuffer.Count == 0)
            SetupItemBuffer();

        Item item = itemBuffer[0];
        itemBuffer.RemoveAt(0);
        return item;
    }

    void SetupItemBuffer()
    {
        itemBuffer = new List<Item>(100);
        for (int i = 0; i < itemSO2.items.Length; i++)
        {
            Item item = itemSO2.items[i];
            itemBuffer.Add(item);
        }

        for (int i = 0; i < itemBuffer.Count; i++)
        {
            int rand = Random.Range(i, itemBuffer.Count);
            Item temp = itemBuffer[i];
            itemBuffer[i] = itemBuffer[rand];
            itemBuffer[rand] = temp;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        SetupItemBuffer(); //아이템 버퍼를 설정함
        TurnManager.OnAddCard2 += AddCard;
    }

    void OnDestroy()
    {
        TurnManager.OnAddCard2 -= AddCard;
    }

    void Update()
    {

    }

    void AddCard(bool isMine)
    {
        // Check if the total number of cards is 78
        if (otherCards.Count >= 78)
        {
            Debug.Log("All cards have been dealt.");
            return;
        }

        if (otherCards.Count >= 9)
        {
            Debug.Log("Maximum number of cards reached.");
            return;
        }

        var cardObject = Instantiate(cardPrefab, cardSpawnPoint.position, Utils.QI);
        var card = cardObject.GetComponent<Card>();
        card.Setup(PopItem(), isMine);
        otherCards.Add(card);

        SetOriginOrder();
        CardAlignment();

        var spriteRenderer = cardObject.GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            spriteRenderer.sortingLayerName = "Card";
        }
    }
    void RemoveCard()
    {
        if (otherCards.Count > 0)
        {
            Card cardToRemove = otherCards[otherCards.Count - 1];
            otherCards.RemoveAt(otherCards.Count - 1);
            Destroy(cardToRemove.gameObject);
        }
        else
        {
            Debug.Log("No cards to remove.");
        }
    }
    void SetOriginOrder()
    {
        for (int i = 0; i < otherCards.Count; i++)
        {
            var targetCard = otherCards[i];
            targetCard?.GetComponent<Order>().SetOriginOrder(i);
        }
    }

    void CardAlignment()
    {
        List<PRS> originCardPRSs = RoundAlignment(OtherCardLeft, OtherCardRight, otherCards.Count, -0.5f, Vector3.one * 0.8f);

        for (int i = 0; i < otherCards.Count; i++)
        {
            var targetCard = otherCards[i];

            targetCard.originPRS = originCardPRSs[i];
            targetCard.MoveTransform(targetCard.originPRS, true, 0.7f);
        }
    }
    List<PRS> RoundAlignment(Transform leftTr, Transform rightTr, int objCount, float height, Vector3 scale)
    {
        float[] objLerps = new float[objCount];
        List<PRS> results = new List<PRS>(objCount);

        switch (objCount)
        {
            case 1: objLerps = new float[] { 0.5f }; break;
            case 2: objLerps = new float[] { 0.27f, 0.73f }; break;
            case 3: objLerps = new float[] { 0.1f, 0.5f, 0.9f }; break;
            default:
                float interval = 1f / (objCount - 1);
                for (int i = 0; i < objCount; i++)
                    objLerps[i] = interval * i;
                break;
        }

        for (int i = 0; i < objCount; i++)
        {
            var targetPos = Vector3.Lerp(leftTr.position, rightTr.position, objLerps[i]);
            var targetRot = Quaternion.identity;
            if (objCount >= 4)
            {
                float curve = Mathf.Sqrt(Mathf.Pow(height, 2) - Mathf.Pow(objLerps[i] - 0.5f, 2));
                curve = height >= 0 ? curve : -curve;
                targetPos.y += curve;
                targetRot = Quaternion.Slerp(leftTr.rotation, rightTr.rotation, objLerps[i]);
            }

            results.Add(new PRS(targetPos, targetRot, scale));
        }
        return results;
    }
    public Item GetRandomItem() // 아이템을 출력하는 메서드 , 이 메서드는 아이템 버퍼에서 무작위 아이템을 선택하고 반환
    {
        if (itemBuffer.Count > 0)
        {
            int rand = Random.Range(0, itemBuffer.Count);
            return itemBuffer[rand];
        }
        else
        {
            return null;
        }
    }
}