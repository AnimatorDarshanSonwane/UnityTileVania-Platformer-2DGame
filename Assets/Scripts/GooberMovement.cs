using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GooberMovement : MonoBehaviour
{

    [SerializeField] float moveSpeed = 1f;

    Rigidbody2D myRigidbody2D;
    // Start is called before the first frame update
    void Start()
    {
        myRigidbody2D = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        myRigidbody2D.velocity = new Vector2(moveSpeed, 0f);
    }

    void OnTriggerExit2D(Collider2D other)
    {

        moveSpeed = -moveSpeed;

        FlipEnemyFacing();

    }

    private void FlipEnemyFacing()
    {
        transform.localScale = new Vector2(-(MathF.Sign(myRigidbody2D.velocity.x)), 1f);
    }
}
