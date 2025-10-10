using UnityEngine;

public class PlayerController : MonoBehaviour
{

    private Rigidbody rbPlayer;

    private float moveSpeed = 5f;
    
    
    public Camera mainCam;

    //public GameObject player; Demander au prof

    private Material playerMat;

    // bools pour l'activation des powers ups
    private bool isBoostMasse = false;
    private bool isBoostForce = false;

    private float timerBoostMasseTaille = 0f;// Timer du power up de boost de taille et de masse 

    private float timerBoostForce = 0f; //Timer du power up de boost de force

    private float hitEffectTimer = 0f;// Timer du hit effect couleur rouge

    private readonly float hitEffectDuration = 1f; //Durée du hit effect

    float resistance = 1f; //Resistance du jouer à la force exercé sur lui (1 = aucune resistence)

    float timerBasePowerUp = 20f;
    
    


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rbPlayer = GetComponent<Rigidbody>();//Rigid body du player
        playerMat = GetComponent<Renderer>().material;//Material du joueur
        mainCam = Camera.main;// Caméra principal du joueur
    }

    // Update is called once per frame
    void Update()
    {

        //Gestion de la durée du power up de taille et de masse 
        //Désactive le power up une fois le timer à 0
        if (timerBoostMasseTaille > 0f)
        {
            timerBoostMasseTaille -= Time.deltaTime;
            if (timerBoostMasseTaille <= 0f)
            {
                diasblePowerUpsTailleMasse();
            }
        }

        //Gestion de la durée du power up de force
        //Désactive le power up une fois le timer à 0
        if (timerBoostForce > 0f)
        {
            timerBoostForce -= Time.deltaTime;
            if (timerBoostForce <= 0f)
            {
                diasblePowerUpsForce();
            }
        }

        // gestion de la durée du hit effect on modifie la variable du shader impliqué dans le lerp
        // (permet de faire une transition fluide entre la texture de base et la couleur rouge)
        if (hitEffectTimer > 0f)
        {
            hitEffectTimer -= Time.deltaTime;
            playerMat.SetFloat("_hitEffect", Mathf.Clamp01(hitEffectTimer / hitEffectDuration));
        }

    }
    private void FixedUpdate()
    {
        

        // Direction vers laquelle la cam�ra regarde (pas sa position !)
        Vector3 directionCam = mainCam.transform.forward;

        // Supprimer la composante Y pour rester dans le plan XZ
        directionCam.y = 0;

        // Normaliser pour �viter une vitesse trop grande ou instable
        directionCam = directionCam.normalized;
        
        // 4. Tourner l'objet dans la direction de d�placement (facultatif)
        //player.transform.forward = directionCam;

        // 5. Appliquer une force physique dans cette direction
        rbPlayer.AddForce(directionCam * moveSpeed * resistance, ForceMode.Acceleration);
        
    }

    public void EnablePowerUp(PowerUp.PowerUpType type)
    {
        
        //effets en fonction du type de power-up
        switch (type)
        {
            case PowerUp.PowerUpType.tailleMasseBoost:
                //Augmente le timer si déjà en boost de masse
                if (isBoostMasse)
                {
                    timerBoostMasseTaille += 10;
                    return;
                }
                //Timer à 20f
                timerBoostMasseTaille = timerBasePowerUp;
                isBoostMasse = true;

                //Épaisseur, masse et resistance à la force (exercée sur lui) augmentée de moitié
                transform.localScale = Vector3.one * 1.5f;
                rbPlayer.mass *= 1.5f;
                resistance = 0.5f;

                playerMat.SetFloat("_isBoostMasse", 1f);//Activation d'un nouveau rendu pour le materiel
                break;

            case PowerUp.PowerUpType.augmenteForce:

                //Augmente le timer si déjà en boost de force
                if (isBoostForce) { 
                    timerBoostForce += 10;
                    return;
                }
                //Timer à 20f
                timerBoostForce = timerBasePowerUp;
                isBoostForce = true;

                playerMat.SetFloat("_isBoostForce", 1f);//ACtivation d'un nouveau rendu pour le materiel
                break;
        }
    }

    //Désactivation du powerUp de taille et masse
    private void diasblePowerUpsTailleMasse()
    {
        if (isBoostMasse)
        {
            isBoostMasse = false;
            transform.localScale = Vector3.one;
            rbPlayer.mass /= 1.5f;
            resistance = 1f;
            playerMat.SetFloat("_isBoostMasse", 0f);
        }

      
    }

    //Désactivation du power Up de force
    private void diasblePowerUpsForce()
    {
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
            // rigid body de l'ennemi collisionner
            Rigidbody enemyRb = collision.gameObject.GetComponent<Rigidbody>();

            if (enemyRb != null)
            {
                //Direction de poussée de l'ennemi
                Vector3 pushDir = collision.transform.position - transform.position;
                //pushDir.y = 0;
                pushDir.Normalize();

                //Force de poussée en fonction du boost de force
                float impactForce = isBoostForce ? 100f : 20f;

                //Application de la force 
                enemyRb.AddForce(pushDir * impactForce, ForceMode.Impulse);
            }

            // Déclencher l'effet rouge sur le joueur (1f de durée revient fluidement vers la texture de base)
            hitEffectTimer = hitEffectDuration;
            playerMat.SetFloat("_hitEffect", 1f);
        }
    }
}
