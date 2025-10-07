using UnityEngine;

public class OutOfBoundsTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        
        if (other.CompareTag("Enemy")) // Est-ce un enemy ?
        {
            Destroy(other.gameObject);  // Détruit l'ennemi
            LevelController.instance.EnemyOutOfBound();
        }

        
        else if (other.CompareTag("Player")) // Est-ce un joueur ?
        {
            LevelController.instance.GameOver();  // fin de jeu
        }
    }
}