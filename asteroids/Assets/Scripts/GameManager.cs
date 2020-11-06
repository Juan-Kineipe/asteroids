using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject asteroid;
    private GameObject player;

    public float spawnInterval = 1f;
    private float time = 0f;
    private float elapsedTime = 0f;
    private int minutes;
    public Text timeText;

    // Maximum asteroids at same time at the begginning
    public int difficulty = 3;

    // Player actual health
    public float healthPoints = 5f;

    // Health image (UI)
    public Image[] healthImages;
    public Sprite fullHp;
    public Sprite emptyHp;

    // Game over screen
    public GameObject gameOver;

    private AudioSource audio;
    public AudioClip DifficultyUp;
    public AudioClip Lose;

    public static GameManager instance;
    

    void Start()
    {
        instance = this;
        player = GameObject.FindGameObjectWithTag("Player");
        audio = GetComponent<AudioSource>();
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

            if(elapsedTime >= 60)
            {
                difficulty += 2;
                minutes += 1;
                elapsedTime = 0;
                audio.PlayOneShot(DifficultyUp);
            }

            timeText.text = minutes.ToString() + ":" + ((int)elapsedTime).ToString();
            
        }
    }

    void SpawnAsteroids() {
        GameObject[] asteroids = GameObject.FindGameObjectsWithTag("Asteroid");
        // Maximum asteroids at same time (this value gets increased when difficulty gets higher)
        if (asteroids.Length < difficulty)
        {
            GameObject Asteroid = Instantiate(asteroid) as GameObject;
            Asteroid.transform.position = RandomPosition();
        }
    }

    Vector2 RandomPosition()
    {
        Vector2[] possiblePositions = {
            new Vector2((ScreenUtils.instance.screenBottom)+4, (ScreenUtils.instance.screenLeft)+4),
            new Vector2((ScreenUtils.instance.screenTop)-4, (ScreenUtils.instance.screenLeft)+4),
            new Vector2((ScreenUtils.instance.screenBottom)+4, (ScreenUtils.instance.screenRight)-4),
            new Vector2((ScreenUtils.instance.screenTop)-4, (ScreenUtils.instance.screenRight)-4)
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
        //audio.PlayOneShot(Lose);
    }

    public void Restart()
    {
        SceneManager.LoadScene("SampleScene");
    }
   
}