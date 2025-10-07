using UnityEngine;

public class PlayerController : MonoBehaviour
{

    private Rigidbody rbPlayer;
    private float moveSpeed = 1f;

    public GameObject player;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rbPlayer = player.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void FixedUpdate()
    {
        //Faut-il-d�tecter l'input d'abord ? je ne pense pas ?

        // 1. Direction vers laquelle la cam�ra regarde (pas sa position !)
        Vector3 directionCam = transform.forward;

        // 2. Supprimer la composante Y pour rester dans le plan XZ
        directionCam.y = 0;

        // 3. Normaliser pour �viter une vitesse trop grande ou instable
        directionCam = directionCam.normalized;

        // 4. Tourner l'objet dans la direction de d�placement (facultatif)
        //player.transform.forward = directionCam;

        // 5. Appliquer une force physique dans cette direction
        rbPlayer.AddForce(directionCam * moveSpeed, ForceMode.Force);


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
        if (type == PowerUp.PowerUpType.tailleMasseBoost)
        {

        }

        if (type == PowerUp.PowerUpType.augmenteForce)
        {

        }

        //Exemple
        //case PowerUp.PowerUpType.tailleMasseBoost:
        //    Debug.Log("Power-Up : Taille/Masse Boost !");
        //    transform.localScale *= 2f;
        //    GetComponent<Rigidbody>().mass *= 2f;
        //    break;

        //case PowerUp.PowerUpType.augmenteForce:
        //    Debug.Log("Power-Up : Force augment�e !");
        //    moveSpeed *= 2f;
        //    break;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("Collision avec un ennemi !");
            // Tu peux ici appliquer des d�g�ts, red�marrer le niveau, etc.
        }
    }
}
