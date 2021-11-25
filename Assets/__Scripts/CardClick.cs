using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardClick : MonoBehaviour
{
    public GameObject otherButton;
    public GameObject newPanel;
    public List<CardCat> playerHand;
    public List<Vector3> listOfPos;
    public GameObject CatCardPrefab;
    public GameObject camera;

    // Start is called before the first frame update
    void Start()
    {

    }

    public void FillHand()
    {
        for(int i = 0; i < RatatatCat.CURRENT_PLAYER.hand.Count; i++)
        {
            playerHand.Add(RatatatCat.CURRENT_PLAYER.hand[i]);
        }
    }

    public void Hide()
    {
        RatatatCat.S.View();
    }

    public void ShowCards()
    {
        otherButton.GetComponent<CardClick>().FillHand();
        camera.transform.position = new Vector3(camera.transform.position.x, -100, camera.transform.position.z);
        otherButton.GetComponent<CardClick>().PopHand();
    }

    private void PopHand()
    {
        CardCat Pcard;
        listOfPos = new List<Vector3>();

        for (int i = 0; i<playerHand.Count; i++) {
            //GameObject obj = Instantiate(CatCardPrefab) as GameObject;
            //Pcard = obj.GetComponent<CardCat>();
            //Pcard = playerHand[i];
            //obj.transform.localScale *= 3;
            Pcard = playerHand[i];
            listOfPos.Add(Pcard.transform.position);
            Pcard.transform.position = new Vector3(-12 + (i * 8), -104, 0);
            if(Pcard == playerHand[0] || Pcard == playerHand[3])
            {
                Pcard.faceUp = true;
            }
        }
    }

    public void SendBack()
    {
        CardCat Pcard;
        Debug.Log(playerHand.Count);
        for (int i = 0; i < playerHand.Count; i++)
        {
            Pcard = playerHand[i];
            Pcard.transform.position = listOfPos[i];
            playerHand[i].transform.position = Pcard.transform.position;
            if (Pcard == playerHand[0] || Pcard == playerHand[3])
            {
                Pcard.faceUp = false;
            }
            Debug.Log("I'm alive");
        }
        listOfPos.Clear();
        camera.transform.position = new Vector3(camera.transform.position.x, 0, camera.transform.position.z);
        RatatatCat.S.ViewStart();
    }

    public void ShowConfirm()
    {
        newPanel.SetActive(true);
    }


    public void HideAll()
    {
        otherButton.GetComponent<CardClick>().Hide();
        Hide();
    }
}
