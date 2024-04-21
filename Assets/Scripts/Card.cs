using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class Card : MonoBehaviour
{
    [SerializeField] SpriteRenderer card; 
    [SerializeField] SpriteRenderer character; 
    [SerializeField] TMP_Text nameTMP; 
    [SerializeField] TMP_Text attackTMP; 
    [SerializeField] TMP_Text defenseTMP; 
    [SerializeField] TMP_Text costTMP;
    [SerializeField] Sprite cardFront; 
    [SerializeField] Sprite cardBack; 


    public Item item; 
    bool isFront; 
    public PRS originPRS;

    public void Setup(Item item, bool isFront)
    {
        this.item = item;
        this.isFront = isFront;

        if (this.isFront)
        {
            character.sprite = this.item.sprite;
            nameTMP.text = this.item.name;
            attackTMP.text = this.item.attack.ToString();
            defenseTMP.text = this.item.defense.ToString();
            costTMP.text = this.item.cost.ToString();
        }
        else
        {
            card.sprite = cardBack;
            nameTMP.text = "";
            attackTMP.text = "";
            defenseTMP.text = "";
            costTMP.text = "";
        }
    }

    void OnMouseOver()
    {
        if(isFront)
          CardManager.Inst.CardMouseOver(this);
    }

    void OnMouseExit()
    {
        if(isFront)
          CardManager.Inst.CardMouseExit(this);
    }

    void OnMouseDown()
    {
        if(isFront)
          CardManager.Inst.CardMouseDown(this);
    }

    void OnMouseUp()
    {
        if(isFront)
          CardManager.Inst.CardMouseUp();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void MoveTransform(PRS prs, bool useDotween, float dotweenTime = 0)
    {
        
        if (useDotween)
        {
            transform.DOMove(prs.pos, dotweenTime);
            transform.DORotateQuaternion(prs.rot, dotweenTime);
            transform.DOScale(prs.scale, dotweenTime);
        }


        else
        {
            transform.position = prs.pos;
            transform.rotation = prs.rot;
            transform.localScale = prs.scale;
        }
    }

}
