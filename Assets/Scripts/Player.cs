using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(DiscreteMovement), typeof(SpriteRenderer))]
public class Player : MonoBehaviour {
    public static Player Instance;

    // component stuff
    private DiscreteMovement discreteMovement;
    private SpriteRenderer sprite;
    private Color originalColor;
    public TextMeshProUGUI dashCooldownText;

    public delegate void OnPlayerMove();
    public OnPlayerMove onPlayerMove;
    
    // public constants
    public KeyCode leftKey;
    public KeyCode rightKey;
    public KeyCode upKey;
    public KeyCode downKey;
    public KeyCode dashKey;
    public int dashDistance = 1;
    public int dashCooldown = 1;
    public Color dashColor;
    
    // state variables
    private bool dashing;
    private int dashTimer;

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

        int width = grids.GetComponent<GridManager>().width;
        int height = grids.GetComponent<GridManager>().height;
        transform.position = new Vector3((float)width / 2, (float)height / 2, 0f);

        onPlayerMove += () => Debug.Log("(" + discreteMovement.movePoint.position.x + ", " + discreteMovement.movePoint.position.y + ")");
    }

    // Update is called once per frame
    void Update() {
        // set color when dashing
        if (dashing) sprite.color = dashColor;
        else sprite.color = originalColor;
        
        // keep dash cooldown text updated
        if (dashTimer == 0) dashCooldownText.text = "";
        else dashCooldownText.text = dashTimer.ToString();

        // get player input
        if (Input.GetKeyDown(leftKey) && !discreteMovement.moving)
        {
            if (transform.position.x - 1 >= -0.1) {
                if (Input.GetKey(dashKey) && dashTimer == 0) {
                    dashing = true;
                    discreteMovement.MoveLeft(dashDistance);
                    dashTimer = dashCooldown;

                    SoundManager.PlaySound("ShootSound");
                }
                else {
                    discreteMovement.MoveLeft();
                    dashTimer--;
                    if (dashTimer < 0) dashTimer = 0;
                }

                onPlayerMove?.Invoke();
            }
        }
        else if (Input.GetKeyDown(rightKey) && !discreteMovement.moving)
        {
            if (transform.position.x +1 <= grids.GetComponent<GridManager>().width - 1)
            {
                if (Input.GetKey(dashKey) && dashTimer == 0)
                {
                    dashing = true;
                    discreteMovement.MoveRight(dashDistance);
                    dashTimer = dashCooldown;

                    SoundManager.PlaySound("ShootSound");
                }
                else {
                    discreteMovement.MoveRight();
                    dashTimer--;
                    if (dashTimer < 0) dashTimer = 0;
                }
                onPlayerMove?.Invoke();
            }
            
        }
        else if (Input.GetKeyDown(upKey) && !discreteMovement.moving)
        {
            if (transform.position.y +1 <= grids.GetComponent<GridManager>().height-1)
            {
                if (Input.GetKey(dashKey) && dashTimer == 0)
                {
                    dashing = true;
                    discreteMovement.MoveUp(dashDistance);
                    dashTimer = dashCooldown;

                    SoundManager.PlaySound("ShootSound");
                }
                else {
                    discreteMovement.MoveUp();
                    dashTimer--;
                    if (dashTimer < 0) dashTimer = 0;
                }
                onPlayerMove?.Invoke();
            }
            
        }
        else if (Input.GetKeyDown(downKey) && !discreteMovement.moving)
        {
            if (transform.position.y -1 >= -0.1)
            {
                if (Input.GetKey(dashKey) && dashTimer == 0)
                {
                    dashing = true;
                    discreteMovement.MoveDown(dashDistance);
                    dashTimer = dashCooldown;

                    SoundManager.PlaySound("ShootSound");
                }
                else {
                    discreteMovement.MoveDown();
                    dashTimer--;
                    if (dashTimer < 0) dashTimer = 0;
                }
                onPlayerMove?.Invoke();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Enemy")) {
            if (dashing) {
                Destroy(other.gameObject);
                SoundManager.PlaySound("KillSound");
            }
            else {
                Destroy(gameObject);
                SoundManager.PlaySound("DieSound");
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
                SoundManager.PlaySound("KillSound");
            }
            else {
                Destroy(gameObject);
                SoundManager.PlaySound("DieSound");
            }
        }
    }
}
