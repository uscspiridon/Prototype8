using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(DiscreteMovement), typeof(SpriteRenderer))]
public class Player : MonoBehaviour {
    public static Player Instance;

    // component stuff
    private DiscreteMovement discreteMovement;
    private SpriteRenderer sprite;
    private Color originalColor;

    public delegate void OnPlayerMove();
    public OnPlayerMove onPlayerMove;
    
    // public constants
    public KeyCode leftKey;
    public KeyCode rightKey;
    public KeyCode upKey;
    public KeyCode downKey;
    public KeyCode dashKey;
    public int dashDistance = 1;
    public Color dashColor;
    
    // state variables
    private bool dashing;

    public GameObject grids;

    private void Awake() {
        Instance = this;
        discreteMovement = GetComponent<DiscreteMovement>();
        sprite = GetComponent<SpriteRenderer>();
    }

    // Start is called before the first frame update
    void Start() {
        discreteMovement.onCompleteMovement += () => dashing = false;
        originalColor = sprite.color;

        onPlayerMove += () => Debug.Log("(" + discreteMovement.movePoint.position.x + ", " + discreteMovement.movePoint.position.y + ")");
    }

    // Update is called once per frame
    void Update() {
        // set color when dashing
        if (dashing) sprite.color = dashColor;
        else sprite.color = originalColor;
        
        // get player input
        if (Input.GetKeyDown(leftKey) && !discreteMovement.moving)
        {
            if (transform.position.x - 1 >= -0.1) {
                if (Input.GetKey(dashKey)) {
                    dashing = true;
                    discreteMovement.MoveLeft(dashDistance);
                }
                else discreteMovement.MoveLeft();

                onPlayerMove?.Invoke();
            }
        }
        else if (Input.GetKeyDown(rightKey) && !discreteMovement.moving)
        {
            if (transform.position.x +1 <= grids.GetComponent<GridManager>().width - 1)
            {
                if (Input.GetKey(dashKey))
                {
                    dashing = true;
                    discreteMovement.MoveRight(dashDistance);
                }
                else discreteMovement.MoveRight();
                onPlayerMove?.Invoke();
            }
            
        }
        else if (Input.GetKeyDown(upKey) && !discreteMovement.moving)
        {
            if (transform.position.y +1 <= grids.GetComponent<GridManager>().height-1)
            {
                if (Input.GetKey(dashKey))
                {
                    dashing = true;
                    discreteMovement.MoveUp(dashDistance);
                }
                else discreteMovement.MoveUp();
                onPlayerMove?.Invoke();
            }
            
        }
        else if (Input.GetKeyDown(downKey) && !discreteMovement.moving)
        {
            if (transform.position.y -1 >= -0.1)
            {
                if (Input.GetKey(dashKey))
                {
                    dashing = true;
                    discreteMovement.MoveDown(dashDistance);
                }
                else discreteMovement.MoveDown();
                onPlayerMove?.Invoke();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Enemy")) {
            if (dashing) {
                Destroy(other.gameObject);
                // other.gameObject.SetActive(false);
            }
            else {
                Destroy(gameObject);
                // gameObject.SetActive(false);
            }
        }
        if (other.CompareTag("Bullet")) {
            if (dashing) {

                DiscreteMovement bulletMove = other.GetComponent<DiscreteMovement>();
                Vector3 stopPoint = bulletMove.movePoint.position;

                Vector2 moveDirection = discreteMovement.movePoint.position - discreteMovement.startPos;
                Vector2 bulletMoveDirection = bulletMove.movePoint.position - bulletMove.startPos;
                Vector2 wholePart = new Vector2((int)transform.position.x, (int)transform.position.y);

                if (moveDirection.x == bulletMoveDirection.x) {
                    if (bulletMoveDirection.y < 0) {
                        if (transform.position.y < 0) stopPoint.y = wholePart.y - 1;
                        else stopPoint.y = wholePart.y;
                    }
                    else if (bulletMoveDirection.y > 0) {
                        if (transform.position.y > 0) stopPoint.y = wholePart.y + 1;
                        else stopPoint.y = wholePart.y;
                    }
                }
                else if (moveDirection.y == bulletMoveDirection.y) {
                    if (bulletMoveDirection.x < 0) {
                        if (transform.position.x < 0) stopPoint.x = wholePart.x - 1;
                        else stopPoint.x = wholePart.x;
                    }
                    else if (bulletMoveDirection.x > 0) {
                        if (transform.position.x > 0) stopPoint.x = wholePart.x + 1;
                        else stopPoint.x = wholePart.x;
                    }
                }
                else {
                    Debug.Log("MOVING PERPENDICULAR");
                    // find intersection of lines
                    stopPoint = discreteMovement.movePoint.position;
                    if (moveDirection.y < 0) {
                        if (transform.position.y < 0) stopPoint.y = wholePart.y - 1;
                        else stopPoint.y = wholePart.y;
                    }
                    else if (moveDirection.y > 0) {
                        if (transform.position.y > 0) stopPoint.y = wholePart.y + 1;
                        else stopPoint.y = wholePart.y;
                    }
                    if (moveDirection.x < 0) {
                        if (transform.position.x < 0) stopPoint.x = wholePart.x - 1;
                        else stopPoint.x = wholePart.x;
                    }
                    else if (moveDirection.x > 0) {
                        if (transform.position.x > 0) stopPoint.x = wholePart.x + 1;
                        else stopPoint.x = wholePart.x;
                    }
                }
                
                Debug.Log("bullet at (" + other.transform.position.x + ", " + other.transform.position.y + ")");
                Debug.Log("at (" + transform.position.x + ", " + transform.position.y + ") and whole part is (" + wholePart.x + ", " + wholePart.y + ")");
                Debug.Log("moving in direction (" + moveDirection.x + ", " + moveDirection.y + ")");
                Debug.Log("stop point = (" + stopPoint.x + ", " + stopPoint.y + ")");
                discreteMovement.SetNewStopPoint(stopPoint);
                Destroy(other.gameObject);
            }
            else {
                Destroy(gameObject);
            }
        }
    }
}
