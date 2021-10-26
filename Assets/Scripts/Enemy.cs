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
        /*
        Vector3 spawnOffset = new Vector3(-1, 0, 0);
        Bullet bullet = Instantiate(bulletPrefab, transform.position + spawnOffset, Quaternion.identity).GetComponent<Bullet>();
        bullet.direction = new Vector2(-1, 0);
        */
        Vector3 toPlayer = player.transform.position - transform.position;
        

        float dist = toPlayer.magnitude;

        Vector3 left = new Vector3(-1, 0, 0);
        Vector3 toLeft = transform.position + left - player.transform.position;
        float leftLen = toLeft.magnitude;

        Vector3 right = new Vector3(1, 0, 0);
        Vector3 toRight = transform.position + right - player.transform.position;
        float rightLen = toRight.magnitude;

        Vector3 up = new Vector3(0, 1, 0);
        Vector3 toUp = transform.position + up - player.transform.position;
        float upLen = toUp.magnitude;

        Vector3 down = new Vector3(0, -1, 0);
        Vector3 toDown = transform.position + down - player.transform.position;
        float downLen = toDown.magnitude;

        float[] dists = { leftLen, rightLen, upLen, downLen };
        float mindist = Mathf.Min(dists);

        if (mindist == leftLen)
        {
            discreteMovement.MoveLeft();
            Bullet bullet = Instantiate(bulletPrefab, transform.position + left, Quaternion.identity).GetComponent<Bullet>();
            bullet.direction = new Vector2(-1, 0);
        }
        else if (mindist == rightLen)
        {
            discreteMovement.MoveRight();
            Bullet bullet = Instantiate(bulletPrefab, transform.position + right, Quaternion.identity).GetComponent<Bullet>();
            bullet.direction = new Vector2(1, 0);
        }
        else if (mindist == downLen)
        {
            discreteMovement.MoveDown();
            Bullet bullet = Instantiate(bulletPrefab, transform.position + down, Quaternion.identity).GetComponent<Bullet>();
            bullet.direction = new Vector2(0, -1);
        }
        else if (mindist == upLen)
        {
            discreteMovement.MoveUp();
            Bullet bullet = Instantiate(bulletPrefab, transform.position + up, Quaternion.identity).GetComponent<Bullet>();
            bullet.direction = new Vector2(0, 1);
        }
    }

    private void OnDestroy() {
        Player.Instance.onPlayerMove -= MakeNextMove;
    }
}
