using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Serialization;

public class LevelController : MonoBehaviour
{
   
    public static LevelController instance;
    public Camera mainCamera;
    public GameObject enemyPrefab;
    public GameObject[] powerUpPrefabs;// Les deux sortes de power-up
    public Transform player;
    public GameObject island;//L'île sur laquelle se déroule jeu


    public int nombreEnemiAuDebut = 1;        // nombre d’ennemis de la première vague
    public float difficulty = 0.2f;       // commence à 0.2 (facile)
    public float difficultyIncrease = 0.1f; // augmentation par vague
    public bool isGameOver = false;

    private int enemiesRemaining;
    private int currentWave = 0;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        mainCamera = Camera.main;
        player = GameObject.FindWithTag("Player").transform;
        StartNewWave(); // on commence une vague d'Enemis
    }

    // commebcer une nouvelle vague d’ennemis
    void StartNewWave()
    {
     
        
        currentWave++; 

        // on augmente la difficulté à chaque vague
        difficulty += difficultyIncrease;
        difficulty = Mathf.Clamp01(difficulty);

        
        // Nombre d’ennemis basé sur la difficulté
        int enemyCount = Mathf.RoundToInt(nombreEnemiAuDebut + currentWave * difficulty * 3f);
        enemiesRemaining = enemyCount;

        // on genere autant d'ennemi que la difficulté le demande
        for (int i = 0; i < enemyCount; i++) // on 
        {
            Vector3 pos = new Vector3(
                Random.Range(-10f, 10f),
                1f,
                Random.Range(-10f, 10f)
            );

            GameObject enemy = Instantiate(enemyPrefab, pos, Quaternion.identity);
            enemy.GetComponent<Renderer>().material.SetFloat("_palier", currentWave - 1);
            EnemyController enemyController = enemy.GetComponent<EnemyController>();
            if (enemyController != null)
            {
                enemyController.InitializeEnemy(player, difficulty);
            }
        }

        // Faire spawn un nouveau power up
        spawnPowerUp();
    }

    public void spawnPowerUp()
    {
        if (powerUpPrefabs.Length > 0 && island != null)
        {
            // Choisir un power-up aléatoire
            GameObject randomPowerUp = powerUpPrefabs[Random.Range(0, powerUpPrefabs.Length)];

            // Récupérer le collider et son centre
            Collider islandCollider = island.GetComponent<Collider>();
            Bounds islandBounds = islandCollider.bounds;

            // Centre de l'île (en X et Z)
            Vector3 center = islandBounds.center;

            // Rayon max (en prenant la moitié de la plus petite dimension X ou Z pour rester dans l'île)
            float radius = Mathf.Min(islandBounds.extents.x, islandBounds.extents.z) * 0.5f;

            // Générer une position aléatoire dans un cercle (X,Z)
            Vector2 randomPos2D = Random.insideUnitCircle * radius;

            // Construire la position 3D en ajoutant Y (un peu au-dessus du sol)
            Vector3 spawnPos = new Vector3(
                center.x + randomPos2D.x,
                islandBounds.max.y + 1f,
                center.z + randomPos2D.y
            );

            //// Calculer les limites de l'île
            //Bounds islandBounds = island.GetComponent<Collider>().bounds;
            //float marginX = 0.5f;

            ////Position de spawn aléatoire sur l'île (il apparait un peu en dehors)
            //Vector3 spawnPos = new Vector3(
            //    Random.Range(islandBounds.min.x - marginX, islandBounds.max.x - marginX),
            //    islandBounds.max.y + 0.5f, // un peu au-dessus du sol
            //    Random.Range(islandBounds.min.z, islandBounds.max.z)
            //);

            Instantiate(randomPowerUp, spawnPos, Quaternion.identity);

        }
    }

    // quand un enemi sort de l'arene 
    public void EnemyOutOfBound()
    {
        enemiesRemaining--; // bah il est mort alors haha
        if (enemiesRemaining <= 0)
        {
            StartNewWave();
        }
    }

    // quand eric perd la partie
    public void GameOver()
    {
        isGameOver = true;
        
        //Ajouter l'audio de gameOver
        AudioClip newClip = Resources.Load<AudioClip>("Musics/wasted");
        AudioSource audioSource = mainCamera.GetComponent<AudioSource>();
        
        audioSource.Stop();
        audioSource.clip = newClip;
        audioSource.loop = false;
        audioSource.Play();

        //Animation de fin
        InvokeRepeating("StartNewWave", 10, 2f);
    }
    
}
