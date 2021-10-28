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

    public TouchPoint FirstTouchPoint { get; set; }
    public TouchPoint SecondTouchPoint { get; set; }

    private Vector3 previousFrameT1Pos = new Vector3(0f, 0f, 0f), previousFrameT2Pos = new Vector3(0f, 0f, 0f);
    private bool previousFrameTwoTouches = false;
    private Vector3 currentT1Pos, currentT2Pos;
    public int offSet = 1;

    void Update()
    {
        DrawDebugMeshes();
        HandleTouchInput();
    }

    // Rotate, scale and translate an object, handling up to two touch points
    void HandleTouchInput()
    {
        //TODO
        if (FirstTouchPoint != null)
        {
            this.transform.position = FirstTouchPoint.fromTUIO();
            currentT1Pos = FirstTouchPoint.fromTUIO();

            if (SecondTouchPoint != null && SecondTouchPoint.Active)
            {
                currentT2Pos = SecondTouchPoint.fromTUIO();
                if (previousFrameTwoTouches)
                {
                    Vector3 previousDifferenceT1T2 = previousFrameT2Pos - previousFrameT1Pos; // vector between Touch1 and Touch2 in previous frame (= d)
                    Vector3 currentDifferenceT1T2 = currentT2Pos - currentT1Pos; // vector between Touch1 and Touch2 in current frame (= d')
                    Vector3 movementT1 = currentT1Pos - previousFrameT1Pos; // finger1 movement from previous to current frame
                    Vector3 movementT2 = currentT2Pos - previousFrameT2Pos; // finger2 movement from previous to current frame
                    float previousDistanceT1T2 = previousDifferenceT1T2.magnitude; // |d|
                    float currentDistanceT1T2 = currentDifferenceT1T2.magnitude;  // |d'|
                    float rotationAngle = Vector3.SignedAngle(previousDifferenceT1T2, currentDifferenceT1T2, this.transform.position); // angle between d and d'
                    float scaleFactor = currentDistanceT1T2 / previousDistanceT1T2; // |d'| / |d|
                    Vector3 translation = (movementT1 + movementT2) * 0.5f; // translation vector (= t)

                    transform.RotateAround(this.transform.forward, rotationAngle/ offSet); // rotation
                    transform.localScale *= scaleFactor; // scaling
                    transform.position += translation; // translation
                }
                previousFrameTwoTouches = true;
                previousFrameT1Pos = FirstTouchPoint.fromTUIO();
                previousFrameT2Pos = SecondTouchPoint.fromTUIO();
            }
            else
            {
                previousFrameTwoTouches = false;
                SecondTouchPoint = null;
            }
        }
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


