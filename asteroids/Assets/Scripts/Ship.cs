﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ship : MonoBehaviour
{
    public float maxSpeed = 4f;
    public float ThrustForce = 2f;
    public float RotateDegreesPerSecond = 200f;
    public GameObject laser;
    public float laserForce = 400f;
    public float cooldown = 0.3f;
    private float time = 0f;

    private Rigidbody2D rb;
    private float colliderRadius;

    Vector2 thrustDirection = new Vector2(1, 0);

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        colliderRadius = GetComponent<CircleCollider2D>().radius;
    }

    // Update is called once per frame
    void Update()
    {
        // Check for rotation input
        float rotationInput = Input.GetAxis("Rotate");
        if (rotationInput != 0)
        {
            // Apply rotation
            float rotationAmount = RotateDegreesPerSecond * Time.deltaTime;
            if (rotationInput < 0)
            {
                rotationAmount *= -1;
            }
            transform.Rotate(Vector3.forward, rotationAmount);

            // Change thrust direction to match ship rotation
            float zRotation = transform.eulerAngles.z * Mathf.Deg2Rad;
            thrustDirection.x = Mathf.Cos(zRotation);
            thrustDirection.y = Mathf.Sin(zRotation);
        }

        // Check cooldown for laser
        if (time > 0f)
        {
            time -= Time.deltaTime;
        }
        // Check for laser input
        else if (Input.GetButtonDown("Fire1"))
        {
            GameObject newLaser = Instantiate(laser, transform.position, transform.rotation);
            newLaser.GetComponent<Rigidbody2D>().AddRelativeForce(Vector2.right * laserForce);
            time = cooldown;
        }
    }

    void FixedUpdate()
    {
        // Check for thrust input
        if (Input.GetAxis("Thrust") != 0)
        {
            rb.AddForce(ThrustForce * thrustDirection,
                ForceMode2D.Force);
        }

        if(rb.velocity.magnitude > maxSpeed)
        {
            rb.velocity = rb.velocity.normalized * maxSpeed;
        }
    }

    void OnBecameInvisible()
    {
        Vector2 position = transform.position;

        // check left, right, top, and bottom sides
        if (position.x + colliderRadius < GameManager.instance.screenLeft ||
            position.x - colliderRadius > GameManager.instance.screenRight)
        {
            position.x *= -1;
        }
        if (position.y - colliderRadius > GameManager.instance.screenTop ||
            position.y + colliderRadius < GameManager.instance.screenBottom)
        {
            position.y *= -1;
        }

        // move ship to the other side of the screen
        transform.position = position;
    }

    // Lose health points when collide with asteroids
    void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.tag == "Asteroid") {
            GameManager.instance.Damage(1f);
        }
    }
}