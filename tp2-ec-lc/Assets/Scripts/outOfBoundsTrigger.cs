using UnityEngine;

public class OutOfBoundsTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        
        if (other.CompareTag("Enemy")) // Est-ce un ennemi ?
        {
            Destroy(other.gameObject);  // Détruit l'ennemi
            LevelController.instance.EnemyOutOfBound(); // Méthode de gestion lorsqu'un ennemi tombe
        }

        
        else if (other.CompareTag("Player")) // Est-ce un joueur ?
        {
            LevelController.instance.GameOver();  // fin de jeu
        }
    }
}