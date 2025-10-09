using UnityEngine;

public class EnemyController : MonoBehaviour
{
    
    private Rigidbody rb;
    private Material enemyMat;
        
    private Transform playerPos;
    public float speed;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerPos = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    private void FixedUpdate()
    {
        if (playerPos is null) return;
        
        // Déplacement continu vers le joueur
        Vector3 direction = (playerPos.position - transform.position).normalized;
        rb.AddForce(direction * speed * Time.fixedDeltaTime, ForceMode.Acceleration);
    }
    
    
    public void InitializeEnemy(Transform player, float difficulty)
    {
        
        rb = GetComponent<Rigidbody>();

        // Exemple de caractéristiques dépendantes de la difficulté
        float scale = Mathf.Lerp(0.8f, 2.0f, difficulty);   // taille
        float mass = Mathf.Lerp(1f, 5f, difficulty);        // masse
        speed = Mathf.Lerp(2f, 6f, difficulty);             // vitesse

        transform.localScale = Vector3.one * scale;
        rb.mass = mass;

        // Appliquer un Physics Material rebondissant
        Collider col = GetComponent<Collider>();
        if (col != null)
        {
            PhysicsMaterial mat = new PhysicsMaterial();
            mat.bounciness = 0.5f;
            mat.bounceCombine = PhysicsMaterialCombine.Maximum;
            col.material = mat;
        }
        enemyMat  = GetComponent<Renderer>().material;
        enemyMat.SetFloat("_difficulty", difficulty); 
        
        // Force initiale vers le joueur
        Vector3 direction = (player.position - transform.position).normalized;
        rb.AddForce(direction * speed, ForceMode.Impulse);
        
        
    }
    

}
