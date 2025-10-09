using UnityEngine;

public class RotateCamera : MonoBehaviour
{

    public float rotationSpeed = 100f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");  // -1 pour gauche, 1 pour droite

        //transform.Rotate(0, horizontalInput * rotationSpeed * Time.deltaTime, 0);//Mouv autour de y

        transform.Rotate(Vector3.up, horizontalInput * rotationSpeed * Time.deltaTime);//Mouv autour de y
    }
}
