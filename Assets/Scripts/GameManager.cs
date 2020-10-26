using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject asteroid;
    private GameObject player;

    public float screenBottom;
    public float screenTop;
    public float screenLeft;
    public float screenRight;

    public float spawnInterval = 1f;
    private float time = 0f;
    private float elapsedTime = 0f;

    // Player actual health
    public float healthPoints = 3f;

    // Health image (UI)
    public Image[] healthImages;
    public Sprite fullHp;
    public Sprite emptyHp;

    // Game over screen
    public GameObject gameOver;

    public static GameManager instance;

    void Start()
    {
        instance = this;

        player = GameObject.FindGameObjectWithTag("Player");

        this.screenBottom = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, -Camera.main.transform.position.z)).y;
        this.screenTop = Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height, -Camera.main.transform.position.z)).y;
        this.screenLeft = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, -Camera.main.transform.position.z)).x;
        this.screenRight = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 0, -Camera.main.transform.position.z)).x;
    }


    // Update is called once per frame
    void Update()
    {
        CheckHealth();
        // Check if player is alive (the game is not over)
        if (healthPoints > 0) {
            // Counting time to spawn asteroids
            time += Time.deltaTime;
            if (time >= spawnInterval)
            {
                time = time - spawnInterval;
                SpawnAsteroids();
            }

            // Counting elapsed time to increase difficulty over time
            elapsedTime += Time.deltaTime;
        }
    }

    void SpawnAsteroids() {
        GameObject[] asteroids = GameObject.FindGameObjectsWithTag("Asteroid");
        // Maximum of 10 asteroids at same time 
        if (asteroids != null || asteroids.Length < 10)
        {
            GameObject Asteroid = Instantiate(asteroid) as GameObject;
            Asteroid.transform.position = RandomPosition();
        }
    }

    Vector2 RandomPosition()
    {
        Vector2[] possiblePositions = {
            new Vector2(screenBottom, screenLeft),
            new Vector2(screenTop, screenLeft),
            new Vector2(screenBottom, screenRight),
            new Vector2(screenTop, screenRight)
        };

        return possiblePositions[Random.Range(0,possiblePositions.Length)];
    }

    // Check player actual hearth
    void CheckHealth() {
        // Actual health cannot be higher than max hp
        if (healthPoints > healthImages.Length) {
            healthPoints = healthImages.Length;
        }

        for (int i = 0; i < healthImages.Length; i++)
        {
            if (i < healthPoints) {
                healthImages[i].sprite = fullHp;
            } else {
                healthImages[i].sprite = emptyHp;
            }
        }

        if (healthPoints <= 0) GameOver();
    }

    // Lose health points (when collide with asteroids)
    public void Damage(float amount)
    {
        healthPoints -= amount;
    }

    public void GameOver()
    {
        gameOver.SetActive(true);
        Destroy(player);
        //audio.PlayOneShot(gameOverAudio);
    }

    public void Restart()
    {
        SceneManager.LoadScene("SampleScene");
    }
   
}