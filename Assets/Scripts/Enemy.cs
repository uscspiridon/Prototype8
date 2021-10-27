using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(DiscreteMovement))]
public class Enemy : MonoBehaviour {
    private DiscreteMovement discreteMovement;

    public GameObject bulletPrefab;

    private bool shootNextTurn = false;

    private void Awake() {
        discreteMovement = GetComponent<DiscreteMovement>();
    }

    // Start is called before the first frame update
    void Start() {
        Player.Instance.onPlayerMove += MakeNextMove;
    }

    private void MakeNextMove() {
        Vector3 playerPos = Player.Instance.transform.position;

        // find direction to player
        Vector3 left = new Vector3(-1, 0, 0);
        Vector3 toLeft = transform.position + left - playerPos;
        float leftLen = toLeft.magnitude;

        Vector3 right = new Vector3(1, 0, 0);
        Vector3 toRight = transform.position + right - playerPos;
        float rightLen = toRight.magnitude;

        Vector3 up = new Vector3(0, 1, 0);
        Vector3 toUp = transform.position + up - playerPos;
        float upLen = toUp.magnitude;

        Vector3 down = new Vector3(0, -1, 0);
        Vector3 toDown = transform.position + down - playerPos;
        float downLen = toDown.magnitude;

        float[] dists = { leftLen, rightLen, upLen, downLen };
        float mindist = Mathf.Min(dists);

        if (mindist == leftLen)
        {
            if (shootNextTurn) {
                Bullet bullet = Instantiate(bulletPrefab, transform.position + left, Quaternion.identity).GetComponent<Bullet>();
                bullet.direction = new Vector2(-1, 0);
                shootNextTurn = false;
            }
            else {
                discreteMovement.MoveLeft();
                shootNextTurn = true;
            }
        }
        else if (mindist == rightLen)
        {
            if (shootNextTurn) {
                Bullet bullet = Instantiate(bulletPrefab, transform.position + right, Quaternion.identity).GetComponent<Bullet>();
                bullet.direction = new Vector2(1, 0);
                shootNextTurn = false;
            }
            else {
                discreteMovement.MoveRight();
                shootNextTurn = true;
            }
        }
        else if (mindist == downLen)
        {
            if (shootNextTurn) {
                Bullet bullet = Instantiate(bulletPrefab, transform.position + down, Quaternion.identity).GetComponent<Bullet>();
                bullet.direction = new Vector2(0, -1);
                shootNextTurn = false;
            }
            else {
                discreteMovement.MoveDown();
                shootNextTurn = true;
            }
        }
        else if (mindist == upLen)
        {
            if (shootNextTurn) {
                Bullet bullet = Instantiate(bulletPrefab, transform.position + up, Quaternion.identity).GetComponent<Bullet>();
                bullet.direction = new Vector2(0, 1);
                shootNextTurn = false;
            }
            else {
                discreteMovement.MoveUp();
                shootNextTurn = true;
            }
        }
    }

    private void OnDestroy() {
        Player.Instance.onPlayerMove -= MakeNextMove;
    }
}
