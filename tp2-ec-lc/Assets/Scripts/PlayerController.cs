using UnityEngine;

public class PlayerController : MonoBehaviour
{

    private Rigidbody rbPlayer;
    private float moveSpeed = 5f;
    
    
    public Camera mainCam;
    //public GameObject player; Demander au prof
    private Material playerMat;

    private bool isBoostMasse = false;
    private bool isBoostForce = false;

    private float powerUpTimer = 0f;
    private float hitEffectTimer = 0f;

    private readonly float hitEffectDuration = 1f;

    float resistance = 0.1f;
    
    


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rbPlayer = GetComponent<Rigidbody>();
        playerMat = GetComponent<Renderer>().material;
        mainCam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {

        //timer pour les power-ups
        if (powerUpTimer > 0f)
        {
            powerUpTimer -= Time.deltaTime;
            if (powerUpTimer <= 0f)
            {
                ResetPowerUps();
            }
        }

        // timer pour le hit effect (effet rouge)
        if (hitEffectTimer > 0f)
        {
            hitEffectTimer -= Time.deltaTime;
            playerMat.SetFloat("_hitEffect", Mathf.Clamp01(hitEffectTimer / hitEffectDuration));
        }

    }
    private void FixedUpdate()
    {
        //Faut-il-d�tecter l'input d'abord ? je ne pense pas ?

        // 1. Direction vers laquelle la cam�ra regarde (pas sa position !)
        Vector3 directionCam = mainCam.transform.forward;

        // 2. Supprimer la composante Y pour rester dans le plan XZ
        directionCam.y = 0;

        // 3. Normaliser pour �viter une vitesse trop grande ou instable
        directionCam = directionCam.normalized;
        
        // 4. Tourner l'objet dans la direction de d�placement (facultatif)
        //player.transform.forward = directionCam;

        // 5. Appliquer une force physique dans cette direction
        rbPlayer.AddForce(directionCam * moveSpeed, ForceMode.Acceleration);
        
        

        // Si tu veux pousser dans le sens oppos�, remplace par :
        // rb.AddForce(-camDir * moveSpeed, ForceMode.Acceleration);

        // Bon pour le d�placement joueur (stable)
        //rb.AddForce(direction * moveSpeed, ForceMode.Acceleration);

        // Plus r�aliste mais influenc� par la masse
        //rb.AddForce(direction * moveSpeed, ForceMode.Force);
        //rbPlayer.AddForce(directionCam * moveSpeed * Time.deltaTime, ForceMode.Force); Plus lent
    }

    public void EnablePowerUp(PowerUp.PowerUpType type)
    {
        //Instancie le temps de powerup
        powerUpTimer = 20f;

        //effet en fonction du power-up
        switch (type)
        {
            case PowerUp.PowerUpType.tailleMasseBoost:
                isBoostMasse = true;

                transform.localScale = Vector3.one * 1.5f;
                rbPlayer.mass *= 1.5f;
                resistance = 0.6f;

                playerMat.SetFloat("_isBoostMasse", 1f); // variable bool shader
                break;

            case PowerUp.PowerUpType.augmenteForce:
                isBoostForce = true;

                playerMat.SetFloat("_isBoostForce", 1f); // variable bool shader
                break;
        }
    }

    private void ResetPowerUps()
    {
        if (isBoostMasse)
        {
            isBoostMasse = false;
            transform.localScale = Vector3.one;
            rbPlayer.mass /= 1.5f;
            resistance = 0.1f;
            playerMat.SetFloat("_isBoostMasse", 0f);
        }

        if (isBoostForce)
        {
            isBoostForce = false;
            playerMat.SetFloat("_isBoostForce", 0f);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            // Appliquer une force physique � l�ennemi
            Rigidbody enemyRb = collision.gameObject.GetComponent<Rigidbody>();

            if (enemyRb != null)
            {
                Vector3 pushDir = collision.transform.position - transform.position;
                //pushDir.y = 0;
                pushDir.Normalize();

                float impactForce = isBoostForce ? 100f : 10f;

                enemyRb.AddForce(pushDir * impactForce * resistance, ForceMode.Impulse);
            }

            // D�clencher l'effet rouge sur le joueur
            hitEffectTimer = hitEffectDuration;
            playerMat.SetFloat("_hitEffect", 1f);
        }
    }
}
