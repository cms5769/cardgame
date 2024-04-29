using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Photon.Pun.Demo.Cockpit;
using UnityEngine;

using Random = UnityEngine.Random;

public class CardManager : MonoBehaviour
{
    public static CardManager Inst { get; private set; }
    void Awake() => Inst = this;

    [SerializeField] ItemSO itemSO;
    [SerializeField] GameObject cardPrefab;
    [SerializeField] List<Card> myCards;
    [SerializeField] Transform cardSpawnPoint;
    [SerializeField] Transform myCardLeft;
    [SerializeField] Transform myCardRight;

    List<Item> itemBuffer;
    Card selectCard;
    bool isMyCardDrag;
    bool onMyCardArea;

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
        for (int i = 0; i < itemSO.items.Length; i++)
        {
            Item item = itemSO.items[i];
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
        DOTween.SetTweensCapacity(1000, 200);
        TurnManager.OnAddCard1 += AddCard;
    }

    void OnDestroy()
    {
        TurnManager.OnAddCard1 -= AddCard;
    }

    void Update()
    {

        if (isMyCardDrag)
            CardDrag();

        DetectCardArea();
    }



    void RemoveCard()
    {
        if (myCards.Count > 0)
        {
            Card cardToRemove = myCards[myCards.Count - 1];
            myCards.RemoveAt(myCards.Count - 1);
            Destroy(cardToRemove.gameObject);
        }
        else
        {
            Debug.Log("No cards to remove.");
        }
    }

    void AddCard(bool isMine)
    {
        // Check if the total number of cards is 78
        if (myCards.Count >= 78)
        {
            Debug.Log("All cards have been dealt.");
            return;
        }

        if (myCards.Count >= 9)
        {
            Debug.Log("Maximum number of cards reached.");
            return;
        }

        if (itemBuffer.Count == 0)
        {
            Debug.Log("No cards left in the deck.");
            return;
        }

        var cardObject = Instantiate(cardPrefab, cardSpawnPoint.position, Utils.QI);
        var card = cardObject.GetComponent<Card>();
        card.Setup(PopItem(), isMine);
        myCards.Add(card);

        SetOriginOrder();
        CardAlignment();

        var spriteRenderer = cardObject.GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            spriteRenderer.sortingLayerName = "Card";
        }
    }

    void SetOriginOrder()
    {
        for (int i = 0; i < myCards.Count; i++)
        {
            var targetCard = myCards[i];
            targetCard?.GetComponent<Order>().SetOriginOrder(i);
        }
    }

    void CardAlignment()
    {
        List<PRS> originCardPRSs = RoundAlignment(myCardLeft, myCardRight, myCards.Count, 0.5f, Vector3.one * 4.0f);

        for (int i = 0; i < myCards.Count; i++)
        {
            var targetCard = myCards[i];

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



    #region MyCard
    public void CardMouseOver(Card card)
    {
        EnlargeCard(true, card);
    }

    public void CardMouseExit(Card card)
    {
        EnlargeCard(false, card);
    }

    public void CardMouseDown(Card card)
    {
        selectCard = card;
        isMyCardDrag = true;
    }

    public void CardMouseUp()
    {
        isMyCardDrag = false;

    }

    void CardDrag()
    {
        if (selectCard != null)
        {
            selectCard.MoveTransform(new PRS(Utils.MousePos, Utils.QI, selectCard.originPRS.scale), false);
        }
    }

    void DetectCardArea()
    {
        RaycastHit2D[] hits = Physics2D.RaycastAll(Utils.MousePos, Vector3.forward);
        int layer = LayerMask.NameToLayer("MyCardArea");
        onMyCardArea = Array.Exists(hits, x => x.collider.gameObject.layer == layer);


    }

    void EnlargeCard(bool isEnlarge, Card card)
    {
        if (isEnlarge)
        {
            Vector3 enlargePos = new Vector3(card.originPRS.pos.x, -4.8f, 1.0f);
            card.MoveTransform(new PRS(enlargePos, Utils.QI, Vector3.one * 8.0f), false);
        }
        else
        {
            card.MoveTransform(card.originPRS, false);
        }

        card.GetComponent<Order>().SetMostFrontOrder(isEnlarge);
    }


    #endregion
}


