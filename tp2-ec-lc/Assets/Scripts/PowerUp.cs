using Unity.VisualScripting;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    private EnemyController enemyControllerScript;
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
        //Player controller script
        playerControllerScript = GameObject.Find("Player").GetComponent<PlayerController>();

        // D�terminer le type de powerUp en fonction du tag de l'objet
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
            //En cas de trigger avec le player active le power up en question et détruit le gameObject
            playerControllerScript.EnablePowerUp(type);
            Destroy(gameObject);
        }
    }
}
