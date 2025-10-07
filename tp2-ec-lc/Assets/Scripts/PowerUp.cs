using Unity.VisualScripting;
using UnityEngine;

public class PowerUp : MonoBehaviour
{

    PlayerController playerControllerScript;
    public enum PowerUpType
    {

        tailleMasseBoost,
        augmenteForce
    }

    private PowerUpType type;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerControllerScript = GameObject.Find("Player").GetComponent<PlayerController>();

        // Déterminer le type en fonction du tag de l'objet
        switch (gameObject.tag)
        {
            case "eclairPower":
                type = PowerUpType.augmenteForce;
                break;
            case "gemmePower":
                type = PowerUpType.tailleMasseBoost;
                break;
            default:
                Debug.LogWarning("PowerUp non reconnu : tag = " + gameObject.tag);
                break;
        }

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerControllerScript.EnablePowerUp(type);
            Destroy(gameObject);
        }
    }
}
