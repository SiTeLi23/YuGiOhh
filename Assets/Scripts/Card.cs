using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    public CardScriptableObject cardSO;

    public bool isPlayer;

    public int currentHealth;
    public int attackPower, manaCost;

    public TMP_Text healthText, attackText, costText, nameText, actionDescriptionText, loreText;

    public Image characterArt, bgArt;

    private Vector3 targetPoint;
    private Quaternion targetRot;
    public float moveSpeed = 5f, rotateSpeed = 540f;


    public bool inHand;
    public int handPosition;


    private bool isSelected;
    private Collider theCol;

    public LayerMask whatIsDesktop;
    public LayerMask whatIsPlacement;

    private bool justPressed;

    public CardPlacePoint assignedPlace;

    public Animator anim;


    void Start()
    {
        anim = GetComponentInChildren<Animator>();
        //assign new position
        if(targetPoint == Vector3.zero) 
        {
            targetPoint = transform.position;
            targetRot = transform.rotation;
        }

        SetUpCard();
        theCol = GetComponent<Collider>();
    }

    public void SetUpCard() 
    {
        currentHealth = cardSO.currentHealth;
        attackPower = cardSO.attackPower;
        manaCost = cardSO.manaCost;

        UpdateCardDisplay();

        nameText.text = cardSO.cardName;
        actionDescriptionText.text = cardSO.actionDescription;
        loreText.text = cardSO.cardLore;

        characterArt.sprite = cardSO.characterSprite;
        bgArt.sprite = cardSO.bgSprite;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, targetPoint, moveSpeed * Time.deltaTime);
        transform.rotation = Quaternion.RotateTowards(transform.rotation,targetRot, rotateSpeed * Time.deltaTime);

        if (isSelected && BattleController.instance.battleEnded == false && Time.timeScale != 0f) 
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit hit;

            if(Physics.Raycast(ray,out hit, 100f, whatIsDesktop)) 
            {
                MoveToPoint(hit.point + new Vector3(0f,2f,0f),Quaternion.identity);
            }

            if (Input.GetMouseButtonDown(1) && BattleController.instance.battleEnded == false) 
            {
                ReturnToHand();
            }

            if (Input.GetMouseButtonDown(0) && justPressed == false && inHand && BattleController.instance.battleEnded == false) 
            {
               if(Physics.Raycast(ray, out hit, 100f, whatIsPlacement) && BattleController.instance.currentPhase == BattleController.TurnOrder.PlayerActive) 
                {
                    //if the place is able to place the card
                    CardPlacePoint selectedPoint = hit.collider.GetComponent<CardPlacePoint>();

                    if(selectedPoint.activeCard == null && selectedPoint.isPlayerPoint) 
                    {
                        if (BattleController.instance.playerMana >= manaCost)
                        {
                            selectedPoint.activeCard = this;
                            assignedPlace = selectedPoint;

                            MoveToPoint(selectedPoint.transform.position, Quaternion.identity);

                            inHand = false;

                            isSelected = false;

                            HandController.instance.RemoveCardFromHand(this);

                            BattleController.instance.SpendPlayerMana(manaCost);

                            AudioManager.instance.PlaySFX(4);
                        }
                        else 
                        {
                            UIController.instance.ShowManaWarning();
                            ReturnToHand();
                        }
                    }
                    else 
                    {
                        ReturnToHand();
                    }
                }
                else 
                {
                    //if the place is not available
                    ReturnToHand();
                }
            }
        }

        //set back to false
        justPressed = false;
    }


    public void MoveToPoint(Vector3 pointToMoveTo, Quaternion rotToMatch) 
    {
        targetPoint = pointToMoveTo;
        targetRot = rotToMatch;
       
    }

    //mouse interaction
    private void OnMouseOver()
    {
        if (inHand && isPlayer && BattleController.instance.battleEnded == false) 
        {
            MoveToPoint(HandController.instance.cardPosition[handPosition] + new Vector3(0, 1f, .5f), Quaternion.identity);
        }
    }

    private void OnMouseExit()
    {
        if (inHand && isPlayer && BattleController.instance.battleEnded == false)
        {
            MoveToPoint(HandController.instance.cardPosition[handPosition], HandController.instance.minPos.rotation);
        }
    }

    private void OnMouseDown()
    {
        if (inHand && BattleController.instance.currentPhase == BattleController.TurnOrder.PlayerActive && isPlayer && BattleController.instance.battleEnded == false && Time.timeScale != 0f) 
        {
            isSelected = true;
            theCol.enabled = false;

            justPressed = true;
        }
    }


    public void ReturnToHand() 
    {
        isSelected = false;
        theCol.enabled = true;

        MoveToPoint(HandController.instance.cardPosition[handPosition], HandController.instance.minPos.rotation);
    }

    public void DamageCard(int damageAmount) 
    {
        currentHealth -= damageAmount;
        if (currentHealth <= 0) 
        {
            currentHealth = 0;
            assignedPlace.activeCard = null;

            MoveToPoint(BattleController.instance.discardPoint.position, BattleController.instance.discardPoint.rotation);

            anim.SetTrigger("Jump");

            Destroy(gameObject, 5f);

            AudioManager.instance.PlaySFX(2);
        }
        else 
        {
            AudioManager.instance.PlaySFX(1);
        }

        UpdateCardDisplay();
        anim.SetTrigger("Hurt");
        
    }

    public void UpdateCardDisplay() 
    {
        healthText.text = currentHealth.ToString();
        attackText.text = attackPower.ToString();
        costText.text = manaCost.ToString();
    }

}
