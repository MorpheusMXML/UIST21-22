using UnityEngine;
using System.Collections;
using TUIO;
using System.Collections.Generic;
using Assets;

public partial class TouchReceiver : MonoBehaviour
{

    [SerializeField]
    Mesh debugMesh;
    [SerializeField]
    Material startMat;
    [SerializeField]
    Material currentMat;
    [SerializeField]
    GestureState currentState = GestureState.None;

    public int offSet = 1;

    public TouchPoint FirstTouchPoint { get; set; }
    public TouchPoint SecondTouchPoint { get; set; }

    private Vector3 startVec = new Vector3(0f,0f,0f);
    private bool rotating = false;
    private float oldAngle = 0;
    private float angle = 0;


    void Start()
    {
    }

    void Update()
    {
        DrawDebugMeshes();
        HandleTouchInput();
    }

    void HandleTouchInput()
    {
        //TODO
        //Simple example:
        if (FirstTouchPoint != null)
        {
            this.transform.position = FirstTouchPoint.fromTUIO();

            if (SecondTouchPoint != null)
            {
                //TODO
                //Calling of Scale Methods
                rotateObject(FirstTouchPoint.fromTUIO(), SecondTouchPoint.fromTUIO());
            }
            else
            {
                rotating = false;
            }
           
        }
        
    }

    void rotateObject(Vector3 first, Vector3 second)
    {

        if (!rotating)
        {
            startVec = first - second;
            rotating = true;
            
        }
        else
        {
            Vector3 movedVec = first - second;
            angle = Vector3.SignedAngle(startVec, movedVec, this.transform.position);
            Debug.Log("angle: " + angle);
            

            if (oldAngle != angle)
            {
                this.transform.RotateAround(this.transform.forward, angle/offSet);
                oldAngle = angle;

            }
            else
            {
                rotating = false;
                //angle = 0;
                //oldAngle = 0;
            }

        }
    }

    void scaleObject()
    {

    }

    /// <summary>
    /// Processes Tuio-Events
    /// </summary>
    /// <param name="events"></param>
	void processEvents(ArrayList events)
    {
        foreach (BBCursorEvent cursorEvent in events)
        {
            TuioCursor myCursor = cursorEvent.cursor;
            if (myCursor.getCursorID() == 0)
            {
                FirstTouchPoint = new TouchPoint(myCursor);
            }
            if (myCursor.getCursorID() == 1)
            {
                SecondTouchPoint = new TouchPoint(myCursor);
            }
        }
    }

    void DrawDebugMeshes()
    {
        DrawDebugMeshForTouchPoint(FirstTouchPoint);
        DrawDebugMeshForTouchPoint(SecondTouchPoint);

    }

    private void DrawDebugMeshForTouchPoint(TouchPoint touchPoint)
    {
        if (touchPoint != null && touchPoint.Active)
        {
            DrawDebugMeshFor(touchPoint.fromTUIO());
        }
    }

    private void DrawDebugMeshFor(Vector3 touchPointVector)
    {
        Graphics.DrawMesh(debugMesh,
                            Matrix4x4.TRS(touchPointVector, Quaternion.identity,
                                new Vector3(0.05f, 0.05f, 0.05f)), currentMat, 0);
    }
}
