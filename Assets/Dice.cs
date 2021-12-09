using UnityEngine.UI;
using UnityEngine;

public class Dice : MonoBehaviour
{
    public Text score;

   public void RollDice()
    {
       int number = Random.Range(1, 7);
        score.text = number.ToString();
    }
       
    
}
