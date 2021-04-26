using System.Collections;
using System.Collections.Generic;
using UnityEngine;


using LockingPolicy = Thalmic.Myo.LockingPolicy;
using Pose = Thalmic.Myo.Pose;
using UnlockType = Thalmic.Myo.UnlockType;
using VibrationType = Thalmic.Myo.VibrationType;
public class MyoMovement : MonoBehaviour
{
    public Vector3 rotationPoint;
    private float previousTime;
    public static float fallTime = 0.8f;
    public static int height = 20;
    public static int width = 10;
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

           
            if (thalmicMyo.pose == Pose.WaveOut)
            {
                transform.position += new Vector3(-1, 0, 0);
                if (!ValidMove())
                {
                    transform.position -= new Vector3(-1, 0, 0);
                }
                Debug.Log("WaveIn");

                ExtendUnlockAndNotifyUserAction(thalmicMyo);
            }
            else if (thalmicMyo.pose == Pose.WaveIn)
            {
                transform.position += new Vector3(1, 0, 0);
                if (!ValidMove())
                {
                    transform.position -= new Vector3(1, 0, 0);
                }
                Debug.Log("WaveOut");

                ExtendUnlockAndNotifyUserAction(thalmicMyo);
            }
            else if (thalmicMyo.pose == Pose.DoubleTap)
            {
                transform.RotateAround(transform.TransformPoint(rotationPoint), new Vector3(0, 0, 1), 90);
                if (!ValidMove())
                {
                    transform.RotateAround(transform.TransformPoint(rotationPoint), new Vector3(0, 0, 1), -90);
                }
                Debug.Log("DoubleTap");

                ExtendUnlockAndNotifyUserAction(thalmicMyo);
            }

            // Vibrate the Myo armband when a fist is made.
            else if  (thalmicMyo.pose == Pose.Fist) 
            {
                transform.position += new Vector3(0, -1, 0);
                if (!ValidMove())
                {
                    transform.position -= new Vector3(0, -1, 0);
                    AddToGrid();
                    CheckForLines();
                    this.enabled = false;
                }
                previousTime = Time.time;
                thalmicMyo.Vibrate(VibrationType.Medium);
                Debug.Log("Fist");
                ExtendUnlockAndNotifyUserAction(thalmicMyo);

                // Change material when wave in, wave out or double tap poses are made.
            }
        }
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

    private void CheckForLines()
    {
        for (int i = height - 1; i >= 0; i--)
        {
            if (HasLine(i))
            {
                DeleteLine(i);
                RowDown(i);
            }
        }
    }

    private void RowDown(int i)
    {
        for (int y = i; y < height; y++)
        {
            for (int j = 0; j < width; j++)
            {
                if (grid[j, y] != null)
                {
                    grid[j, y - 1] = grid[j, y];
                    grid[j, y] = null;
                    grid[j, y - 1].transform.position -= new Vector3(0, 1, 0);
                }
            }
        }
    }

    private void DeleteLine(int i)
    {
        for (int j = 0; j < width; j++)
        {
            Destroy(grid[j, i].gameObject);
            grid[j, i] = null;

        }
    }

    private bool HasLine(int i)
    {
        for (int j = 0; j < width; j++)
        {
            if (grid[j, i] == null)
            {
                return false;
            }
        }

        return true;

    }



    void AddToGrid()
    {
        foreach (Transform children in transform)
        {
            int roundedX = Mathf.RoundToInt(children.transform.position.x);
            int roundedY = Mathf.RoundToInt(children.transform.position.y);

            grid[roundedX, roundedY] = children;

            if (grid[roundedX, roundedY] == grid[roundedX, 18])
            {
                FindObjectOfType<GameManager>().GameOver();
            }
        }
    }

    bool ValidMove()
    {
        foreach (Transform children in transform)
        {
            int roundedX = Mathf.RoundToInt(children.transform.position.x);
            int roundedY = Mathf.RoundToInt(children.transform.position.y);

            if (roundedX < 0 || roundedX >= width || roundedY < 0 || roundedY >= height)
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

    public void Method()
    {
        throw new System.NotImplementedException();
    }
}
