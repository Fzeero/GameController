using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 10f;
    public float[] lanes = { 0.5f, -2.5f, -4f }; // pastikan sama dengan SpawnManager.laneY
    private int currentLane = 1; // start di lane tengah (index 1)
    [HideInInspector] public bool gameOver = false;

    void Start()
    {
        // snap ke lane terdekat saat mulai (jika kamu taruh manual di editor)
        SnapToNearestLane();
    }

    void Update()
    {
        if (gameOver) return;

        // kontrol: Up = naik (index--), Down = turun (index++)
        if (Input.GetKeyDown(KeyCode.UpArrow) && currentLane > 0)
        {
            currentLane--;
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow) && currentLane < lanes.Length - 1)
        {
            currentLane++;
        }

        Vector3 targetPos = new Vector3(transform.position.x, lanes[currentLane], transform.position.z);
        transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * moveSpeed);
    }

    void SnapToNearestLane()
    {
        float minDist = Mathf.Infinity;
        for (int i = 0; i < lanes.Length; i++)
        {
            float d = Mathf.Abs(transform.position.y - lanes[i]);
            if (d < minDist)
            {
                minDist = d;
                currentLane = i;
            }
        }
        // langsung posisikan player ke lane terdekat
        transform.position = new Vector3(transform.position.x, lanes[currentLane], transform.position.z);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // pastikan obstacle punya tag "Obstacle"
        if (other.CompareTag("Obstacle"))
        {
            Debug.Log("[PlayerController] Game Over - hit obstacle");
            gameOver = true;

            // kembalikan obstacle ke pool (non-aktifkan)
            other.gameObject.SetActive(false);

            GameManager.instance.GameOver();
        }
    }
}
