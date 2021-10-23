using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(DiscreteMovement))]
public class Enemy : MonoBehaviour {
    private DiscreteMovement discreteMovement;

    public GameObject bulletPrefab;

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
        
    }

    private void MakeNextMove() {
        // discreteMovement.MoveLeft();
        // assume shoot left
        Vector3 spawnOffset = new Vector3(-1, 0, 0);
        Bullet bullet = Instantiate(bulletPrefab, transform.position + spawnOffset, Quaternion.identity).GetComponent<Bullet>();
        bullet.direction = new Vector2(-1, 0);
    }

    private void OnDestroy() {
        Player.Instance.onPlayerMove -= MakeNextMove;
    }
}
