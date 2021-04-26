using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using LockingPolicy = Thalmic.Myo.LockingPolicy;
using Pose = Thalmic.Myo.Pose;
using UnlockType = Thalmic.Myo.UnlockType;
using VibrationType = Thalmic.Myo.VibrationType;
public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;
    public GameObject pauseMenuUI;

    // Myo game object to connect with.
    // This object must have a ThalmicMyo script attached.
    public GameObject myo = null;
    // The pose from the last update. This is used to determine if the pose has changed
    // so that actions are only performed upon making them rather than every frame during
    // which they are active.
    private Pose _lastPose = Pose.Unknown;

   // public event EventHandler Event;

    // Start is called before the first frame update
    void Start()
    {
        myo = GameObject.FindGameObjectWithTag("myo");
        Time.timeScale = 1f;
    }
    // Update is called once per frame
    void Update()
    {
        ThalmicMyo thalmicMyo = myo.GetComponent<ThalmicMyo>();
        // when the escape key is pressed or the Fingers Spread Gesture, depending on the value of GameIsPaused, either pauses or resumes the game
        if (thalmicMyo.pose != _lastPose)
        {
            _lastPose = thalmicMyo.pose;
            if (Input.GetKeyDown(KeyCode.Escape) || thalmicMyo.pose == Pose.FingersSpread)
            {
                if (GameIsPaused)
                {
                    Resume();
                }
                else
                {
                    Pause();
                }
               
            }
        }
        else if(Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }
    //method that resumes the game 
    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
    }
    //method that pauses the game 
    void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
    }

    public void Method()
    {
        throw new System.NotImplementedException();
    }
}
