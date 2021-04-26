using UnityEngine;
using UnityEngine.UI;

public class Scoring : MonoBehaviour
{
    public Text scoreText;
    public static int scoreValue = 0000000;

    // Update is called once per frame
    void Update()
    {
        //updates the socre value of the UI
        scoreText.text = "Score: " + scoreValue;
    }

    public void Method()
    {
        throw new System.NotImplementedException();
    }
}
