using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(DiscreteMovement))]
public class Enemy : MonoBehaviour {
    private DiscreteMovement discreteMovement;

    public GameObject bulletPrefab;

    public GameObject player;

    public bool enemyMoved = true;

    private void Awake() {
        discreteMovement = GetComponent<DiscreteMovement>();
    }

    // Start is called before the first frame update
    void Start() {
        Player.Instance.onPlayerMove += MakeNextMove;
    }

    // Update is called once per frame
    void Update()
    {
        bool playerMoved = player.GetComponent<Player>().playerMoved;

        if (!enemyMoved && playerMoved)
        {
            MakeNextMove();
            enemyMoved = true;
            player.GetComponent<Player>().playerMoved = false;
        }
    }

    private void MakeNextMove() {
        // discreteMovement.MoveLeft();
        // assume shoot left
        Vector3 spawnOffset = new Vector3(-1, 0, 0);
        Bullet bullet = Instantiate(bulletPrefab, transform.position + spawnOffset, Quaternion.identity).GetComponent<Bullet>();
        bullet.direction = new Vector2(-1, 0);
        Vector3 toPlayer = player.transform.position - transform.position;
        float toPlayerX = Math.Abs(toPlayer.x);
        float toPlayerY = Math.Abs(toPlayer.y);
        Vector3 moveDir = (toPlayerX < toPlayerY) ? new Vector3(1, 0, 0) : new Vector3(0, 1, 0);
        if (toPlayer.x < 0 || toPlayer.y < 0)
        {
            moveDir *= -1;
        }
        if (moveDir.x <= 0)
        {
            discreteMovement.MoveLeft();
        }
        else if (moveDir.x > 0)
        {
            discreteMovement.MoveLeft();
        }
        else if (moveDir.y <= 0)
        {
            discreteMovement.MoveDown();
        }
        else if (moveDir.y > 0)
        {
            discreteMovement.MoveUp();
        }
    }

    private void OnDestroy() {
        Player.Instance.onPlayerMove -= MakeNextMove;
    }
}
