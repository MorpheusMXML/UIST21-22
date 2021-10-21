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
