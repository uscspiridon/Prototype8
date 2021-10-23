using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(DiscreteMovement))]
public class Bullet : MonoBehaviour {
    private DiscreteMovement discreteMovement;

    [HideInInspector] public Vector2 direction;
    public int moveDistance;

    private void Awake() {
        discreteMovement = GetComponent<DiscreteMovement>();
    }
    
    // Start is called before the first frame update
    void Start()
    {
        Player.Instance.onPlayerMove += Move;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Move() {
        discreteMovement.Move((int)direction.x, (int)direction.y, moveDistance);
    }

    private void OnDestroy() {
        Player.Instance.onPlayerMove -= Move;
    }
}
