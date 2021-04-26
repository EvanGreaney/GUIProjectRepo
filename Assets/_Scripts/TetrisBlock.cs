using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using LockingPolicy = Thalmic.Myo.LockingPolicy;
using Pose = Thalmic.Myo.Pose;
using UnlockType = Thalmic.Myo.UnlockType;
using VibrationType = Thalmic.Myo.VibrationType;
public class TetrisBlock : MonoBehaviour
{
    public Vector3 rotationPoint;
    private float previousTime;
    public static float fallTime = 0.8f;
    public static int height = 20;
    public static int width = 10;
    private static int score = 0;
    private static Transform[,] grid = new Transform[width, height];

    // Myo game object to connect with.
    // This object must have a ThalmicMyo script attached.
    public GameObject myo = null;
    // The pose from the last update. This is used to determine if the pose has changed
    // so that actions are only performed upon making them rather than every frame during
    // which they are active.
    private Pose _lastPose = Pose.Unknown;

    // Start is called before the first frame update
    void Start()
    {
        myo = GameObject.FindGameObjectWithTag("myo");
    }

    // Update is called once per frame
    void Update()
    {
        ThalmicMyo thalmicMyo = myo.GetComponent<ThalmicMyo>();
        if (thalmicMyo.pose != _lastPose)
        {
            _lastPose = thalmicMyo.pose;
            //Left and Right movement using the arrow keys or the Wave Out or Wave In Gesture
            if (Input.GetKeyDown(KeyCode.LeftArrow) || thalmicMyo.pose == Pose.WaveOut)
            {
                transform.position += new Vector3(-1, 0, 0);
                if (!ValidMove())
                {
                    transform.position -= new Vector3(-1, 0, 0);
                }
                Debug.Log("WaveIn");

                ExtendUnlockAndNotifyUserAction(thalmicMyo);
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow) || thalmicMyo.pose == Pose.WaveIn)
            {
                transform.position += new Vector3(1, 0, 0);
                if (!ValidMove())
                {
                    transform.position -= new Vector3(1, 0, 0);
                }

                Debug.Log("WaveOut");

                ExtendUnlockAndNotifyUserAction(thalmicMyo);
            }
            //Rotate Movment controlled by the up arrow or the DoubleTap Gesture
            else if (Input.GetKeyDown(KeyCode.UpArrow) || thalmicMyo.pose == Pose.DoubleTap)
            {
                transform.RotateAround(transform.TransformPoint(rotationPoint), new Vector3(0, 0, 1), 90);
                if (!ValidMove())
                {
                    transform.RotateAround(transform.TransformPoint(rotationPoint), new Vector3(0, 0, 1), -90);
                }
                Debug.Log("DoubleTap");

                ExtendUnlockAndNotifyUserAction(thalmicMyo);
            }
        }
        else
        {
            //Left and Right movement using the arrow keys
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                transform.position += new Vector3(-1, 0, 0);
                if (!ValidMove())
                {
                    transform.position -= new Vector3(-1, 0, 0);
                }
                Debug.Log("WaveIn");

                ExtendUnlockAndNotifyUserAction(thalmicMyo);
            }

            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                transform.position += new Vector3(1, 0, 0);
                if (!ValidMove())
                {
                    transform.position -= new Vector3(1, 0, 0);
                }

                Debug.Log("WaveOut");

                ExtendUnlockAndNotifyUserAction(thalmicMyo);
            }
            //Rotate Movment controlled by the up arrow 
            else if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                transform.RotateAround(transform.TransformPoint(rotationPoint), new Vector3(0, 0, 1), 90);
                if (!ValidMove())
                {
                    transform.RotateAround(transform.TransformPoint(rotationPoint), new Vector3(0, 0, 1), -90);
                }
                Debug.Log("DoubleTap");

                ExtendUnlockAndNotifyUserAction(thalmicMyo);
            }
        }

            //DropTime
            // This controls the drop of the block , it constantly drops at a steady rate or if the 
            // down arrow key is pressed , drops at an accelarated rate
            if (Time.time - previousTime > (Input.GetKey(KeyCode.DownArrow) ? fallTime / 10 : fallTime))
            {
                transform.position += new Vector3(0, -1, 0);
                if (!ValidMove())
                {
                    transform.position -= new Vector3(0, -1, 0);
                    AddToGrid();
                    CheckForLines();
                    this.enabled = false;
                    FindObjectOfType<SpawnBlocks>().NewTetromino();
                }
                previousTime = Time.time;
            }
        
    }
    //Method that when called, checks to see if lines have been created within the grid 
    // and deletes and moves the row above it down
    private void CheckForLines()
    {
        for (int i = height -1; i >= 0; i--)
        {
            if(HasLine(i))
            {
                DeleteLine(i);
                RowDown(i);
            }
        }
    }
    // method that when called checks for the row above and if the line below it is deleted
    // it moves the top line down
    private void RowDown(int i)
    {
        for (int y = i; y < height; y++)
        {
            for (int j = 0; j < width; j++)
            {
                if(grid[j,y] != null)
                {
                    grid[j, y - 1] = grid[j, y];
                    grid[j, y] = null;
                    grid[j, y - 1].transform.position -= new Vector3(0, 1, 0);
                }
            }
        }
    }

    // if a line has been created in the grid, deletes that line and updates the score
    private void DeleteLine(int i)
    {
        
        Scoring.scoreValue += 200;
        checkLevel(Scoring.scoreValue);
        Debug.Log(Scoring.scoreValue);
        for (int j = 0; j < width; j++)
        {
            Destroy(grid[j, i].gameObject);
            grid[j, i] = null;
            
        }
    }
    //method that checks the level the player is at , once the score reaches a certain level
    // the speed is increased
    private void checkLevel(int score)
    {
        if (score >= 2000)
        {
            fallTime -= 0.2f;
            Debug.Log("Level 2");
        }
        else if (score > 2000 && score <= 6000)
        {
            fallTime += 0.4f;
            Debug.Log("Level 3");
        }
        else if (score > 6000 && score <= 10000)
        {
            fallTime -= 0.4f;
            Debug.Log("Level 4");
        }
        else if (score > 10000)
        {
            fallTime -= 0.2f;
            Debug.Log("Level 5");
        }
    }
    // checks to see if the grid contains a line
    private bool HasLine(int i)
    {
        for (int j = 0; j < width; j++)
        {
           if( grid[j,i] == null)
            {
                return false;
            }
        }

        return true;
        
    }

    
    // once the block reaches the bottom of the grid , it adds it to grid so that blocks can 
    // stack and be deleted when a line is formed 
    void AddToGrid()
    {
        foreach(Transform children in transform)
        {
            int roundedX = Mathf.RoundToInt(children.transform.position.x);
            int roundedY = Mathf.RoundToInt(children.transform.position.y);

            grid[roundedX, roundedY] = children;

            if(grid[roundedX, roundedY] == grid[roundedX, 18])
            {
                FindObjectOfType<GameManager>().GameOver();
            }
        }
    } 
    
    // checks to see if the player can make a valid move, if the player cannot the action is not performed
    // this is checked in the update method when any action is performed
    bool ValidMove()
    {
        foreach(Transform children in transform)
        {
            int roundedX = Mathf.RoundToInt(children.transform.position.x);
            int roundedY = Mathf.RoundToInt(children.transform.position.y);

            if(roundedX < 0 || roundedX >= width || roundedY < 0 || roundedY >= height)
            {
                return false;
            }

            if (grid[roundedX, roundedY] != null)
            {
                return false;
            }
        }
        return true;
    }
    // Extend the unlock if ThalmcHub's locking policy is standard, and notifies the given myo that a user action was
    // recognized.
    void ExtendUnlockAndNotifyUserAction(ThalmicMyo myo)
    {
        ThalmicHub hub = ThalmicHub.instance;

        if (hub.lockingPolicy == LockingPolicy.Standard)
        {
            myo.Unlock(UnlockType.Timed);
        }

        myo.NotifyUserAction();
    }

    public void Method()
    {
        throw new System.NotImplementedException();
    }
}
