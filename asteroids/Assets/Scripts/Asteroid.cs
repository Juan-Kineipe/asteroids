using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    private float colliderRadius;

    void Start()
    {
        colliderRadius = GetComponent<CircleCollider2D>().radius;

        const float MinImpulseForce = 2f;
        const float MaxImpulseForce = 4f;
        float angle = Random.Range(0, 2 * Mathf.PI);
        Vector2 direction = new Vector2(
            Mathf.Cos(angle), Mathf.Sin(angle));
        float magnitude = Random.Range(MinImpulseForce, MaxImpulseForce);
        GetComponent<Rigidbody2D>().AddForce(
            direction * magnitude,
            ForceMode2D.Impulse);
    }
    
    void FixedUpdate()
    {
       Vector2 position = transform.position;

        // Check left, right, top, and bottom sides
        if (position.x + colliderRadius < ScreenUtils.instance.screenLeft ||
            position.x - colliderRadius > ScreenUtils.instance.screenRight)
        {
            position.x *= -1;
        }
        if (position.y - colliderRadius > ScreenUtils.instance.screenTop ||
            position.y + colliderRadius < ScreenUtils.instance.screenBottom)
        {
            position.y *= -1;
        }

        // Move ASTEROID to the other side of the screen
        transform.position = position;
    }

}
