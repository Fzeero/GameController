using UnityEngine;

public class ObstacleMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float speed = 5f;        // kecepatan obstacle
    public float despawnX = -15f;   // posisi X batas despawn

    void Update()
    {
        // Gerakkan obstacle ke kiri
        transform.Translate(Vector3.left * speed * Time.deltaTime);

        // Jika sudah melewati batas, nonaktifkan agar kembali ke pool
        if (transform.position.x < despawnX)
        {
            gameObject.SetActive(false);
        }
    }
}
