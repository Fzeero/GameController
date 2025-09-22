using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleMovement : MonoBehaviour
{
    public float speed = 10f;
    public float offscreenX = -15f; // jika melewati nilai ini, dikembalikan ke pool (SetActive(false))

    private PlayerController playerController; // optional reference untuk cek gameOver

    void OnEnable()
    {
        // ambil reference ke player (pakai tag agar lebih aman)
        if (playerController == null)
        {
            GameObject p = GameObject.FindWithTag("Player");
            if (p != null) playerController = p.GetComponent<PlayerController>();
        }
    }

    void Update()
    {
        // cek null dan gameOver
        if (playerController != null && playerController.gameOver) return;

        transform.Translate(Vector3.left * speed * Time.deltaTime);

        // jika keluar layar, non-aktifkan (kembalikan ke pool)
        if (transform.position.x < offscreenX)
        {
            gameObject.SetActive(false);
        }
    }
}

