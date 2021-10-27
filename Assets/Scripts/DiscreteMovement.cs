using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiscreteMovement : MonoBehaviour {
    public Transform movePoint;

    // constants
    private const int MoveDistance = 1;
    private const float MoveTime = 0.5f;
    private const float CloseEnoughToMovePoint = 0.05f;

    // lerp variables
    [HideInInspector] public bool moving;
    [HideInInspector] public Vector3 startPos;
    private float elapsedTime;
    
    // callback function
    public delegate void OnCompleteMovement();
    public OnCompleteMovement onCompleteMovement;

    // Start is called before the first frame update
    void Start() {
        movePoint.parent = null;
        startPos = transform.position;
    }

    // Update is called once per frame
    void Update() {
        if (moving) {
            elapsedTime += Time.deltaTime;
            transform.position = Vector3.Lerp(startPos, movePoint.position, elapsedTime / MoveTime);
            if (Vector3.Distance(transform.position, movePoint.position) <= CloseEnoughToMovePoint) {
                moving = false;
                transform.position = movePoint.position;
                onCompleteMovement?.Invoke();
            }
        }
    }

    public void MoveLeft(int multiplier = 1) {
        AdjustMovePoint(-MoveDistance * multiplier, 0);
    }
    public void MoveRight(int multiplier = 1) {
        AdjustMovePoint(MoveDistance * multiplier, 0);
    }
    public void MoveUp(int multiplier = 1) {
        AdjustMovePoint(0, MoveDistance * multiplier);
    }
    public void MoveDown(int multiplier = 1) {
        AdjustMovePoint(0, -MoveDistance * multiplier);
    }
    public void Move(int x, int y, int multiplier = 1) {
        AdjustMovePoint(x * multiplier, y * multiplier);
    }
    public void SetNewStopPoint(Vector3 stopPoint) {
        // // x
        // int signX = (int)(transform.position.x / transform.position.x);
        // int roundedUpRawX = (int) Mathf.Ceil(Mathf.Abs(transform.position.x));
        // int roundedUpX = signX * roundedUpRawX;
        // // y
        // int signY = (int)(transform.position.y / transform.position.y);
        // int roundedUpRawY = (int) Mathf.Ceil(Mathf.Abs(transform.position.y));
        // int roundedUpY = signY * roundedUpRawY;
        // // put them together
        // movePoint.position = new Vector3(roundedUpX, roundedUpY, movePoint.position.z);
        // Debug.Log("pos = (" + transform.position.x + ", " + transform.position.y + ") rounded = (" + roundedUpX + ", " + roundedUpY + ")");
        startPos = transform.position;
        movePoint.position = stopPoint;
    }

    private void AdjustMovePoint(int x, int y) {
        // if still moving, don't move again
        if (moving) return;
        // adjust move point
        movePoint.position += new Vector3(x, y, 0f);
        // reset lerp variables
        moving = true;
        elapsedTime = 0f;
        startPos = transform.position;
    }
}
