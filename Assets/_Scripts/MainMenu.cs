using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using System.Text;  // for stringbuilder
using UnityEngine.Windows.Speech;   // grammar recogniser

public class MainMenu : MonoBehaviour
{
    public GameObject optionsMenuUI;
    public GameObject mainMenuUI;
    public static bool OptionsIsActive = false;
    public static bool MenuIsActive = false;

    private string phrase = "";
    private GrammarRecognizer gr;

    //method that when called loads the game scene 
    public void PlayGame()
    {
        SceneManager.LoadScene("Game");
    }
    //method that ends the game when the game is built
    public void QuitGame()
    {
        Application.Quit();
    }

    //method that when called loads the options panel
    public void Options()
    {
        optionsMenuUI.SetActive(true);
        mainMenuUI.SetActive(false);
        OptionsIsActive = true;

    }
    //method that when called loads the Main Menu panel
    public void BackToMainMenu()
    {
        optionsMenuUI.SetActive(false);
        mainMenuUI.SetActive(true);
        OptionsIsActive = false;
    }

    private void Update()
    {
        switch (phrase)
        {
            case "new":
                PlayGame();
                phrase = "";
                break;
            case "quit":
                QuitGame();
                phrase = "";
                break;
            case "options":
                Options();
                phrase = "";
                break;
            case "mainMenu":
                BackToMainMenu();
                phrase = "";
                break;

        }
    }

    private void Awake()
    {
        // starts the recogizer to listen out to rules from the MainMenuGrammar XML file
        gr = new GrammarRecognizer(Path.Combine(Application.streamingAssetsPath,
                                                "MainMenuGrammar.xml"),
                                    ConfidenceLevel.Low);
        Debug.Log("Grammar loaded!");
        gr.OnPhraseRecognized += GR_OnPhraseRecognized;
        gr.Start();
        if (gr.IsRunning) Debug.Log("Recogniser running");

        phrase = "";
    }

    // if the rule is then recognised, this method then processes the phrase that was listened to .
    private void GR_OnPhraseRecognized(PhraseRecognizedEventArgs args)
    {
        StringBuilder message = new StringBuilder();
        Debug.Log("Recognised a phrase");
        // read the semantic meanings from the args passed in.
        SemanticMeaning[] meanings = args.semanticMeanings;
        // semantic meanings are returned as key/value pairs
        foreach (SemanticMeaning meaning in meanings)
        {
            string keyString = meaning.key.Trim();
            string valueString = meaning.values[0].Trim();
            message.Append("Key: " + keyString + ", Value: " + valueString + " ");

            phrase = valueString;
        }
        Debug.Log(message);
    }
    // when the Apllication Ends, the Grammar recognizer is then ended
    private void OnApplicationQuit()
    {
        if (gr != null && gr.IsRunning)
        {
            gr.OnPhraseRecognized -= GR_OnPhraseRecognized;
            gr.Stop();
        }
    }

    public void Method()
    {
        throw new System.NotImplementedException();
    }
}
