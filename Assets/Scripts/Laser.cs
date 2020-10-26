using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.tag == "Asteroid") {
            Destroy(gameObject);
            Destroy(other.gameObject);
        }
    }

    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
