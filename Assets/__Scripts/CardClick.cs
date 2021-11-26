using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardClick : MonoBehaviour
{
    public GameObject otherButton;
    public GameObject newPanel;
    public List<CardCat> playerHand;
    public List<Vector3> listOfPos;
    public List<Quaternion> listOfRot;
    public GameObject CatCardPrefab;
    public GameObject camera;
    public GameObject transition;
    public GameObject sure;


    public void FillHand()
    {
        playerHand = new List<CardCat>();
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
        Color tC;
        tC = transition.GetComponent<Image>().color;
        while (tC.a <= 1)
        {
            tC = transition.GetComponent<Image>().color;
            tC.a += 0.05f * Time.deltaTime;
            transition.GetComponent<Image>().color = tC;
        }
        camera.transform.position = new Vector3(camera.transform.position.x, -100, camera.transform.position.z);
        Camera.main.orthographicSize = 5;
        otherButton.GetComponent<CardClick>().PopHand();
        Invoke("TransitionZero", 0.5f);
    }

    public void TransitionZero()
    {
        Color tC;
        tC = transition.GetComponent<Image>().color;
        tC.a = 0;
        transition.GetComponent<Image>().color = tC;
    }

    private void PopHand()
    {
        CardCat Pcard;
        listOfPos = new List<Vector3>();
        listOfRot = new List<Quaternion>();

        for (int i = 0; i<playerHand.Count; i++) 
        {
            Pcard = playerHand[i];
            listOfPos.Add(Pcard.transform.position);
            Pcard.transform.position = new Vector3(-6 + (i * 4), -103, 0);
            listOfRot.Add(Pcard.transform.rotation);
            Pcard.transform.rotation = Quaternion.Euler(0, 0, 0);
            if (Pcard == playerHand[0] || Pcard == playerHand[3])
            {
                Pcard.faceUp = true;
            }
        }
    }

    public void Sure()
    {
        sure.SetActive(true);
    }

    public void No()
    {
        sure.SetActive(false);
    }

    public void SendBack()
    {
        sure.SetActive(false);
        CardCat Pcard;
        Color tC;
        tC = transition.GetComponent<Image>().color;
        while (tC.a <= 1)
        {
            tC = transition.GetComponent<Image>().color;
            tC.a += 0.05f * Time.deltaTime;
            transition.GetComponent<Image>().color = tC;
        }
        for (int i = 0; i < playerHand.Count; i++)
        {
            Pcard = playerHand[i];
            Pcard.transform.position = listOfPos[i];
            Pcard.transform.rotation = listOfRot[i];
            playerHand[i].transform.position = Pcard.transform.position;
            if (Pcard == playerHand[0] || Pcard == playerHand[3])
            {
                Pcard.faceUp = false;
            }
        }
        listOfPos.Clear();
        camera.transform.position = new Vector3(camera.transform.position.x, 0, camera.transform.position.z);
        Invoke("TransitionZero", 0.5f);
        Camera.main.orthographicSize = 10;
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
