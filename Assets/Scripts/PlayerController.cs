using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 10f;
    public float[] lanes = { 0.5f, -2.5f, -4f }; // pastikan sama dengan SpawnManager.laneY
    private int currentLane = 1; // start di lane tengah (index 1)
    [HideInInspector] public bool gameOver = false;

    private Camera mainCam;

    // --- Mode kontrol (Keyboard / Mouse) ---
    public enum ControlMode { Keyboard, Mouse }
    private ControlMode controlMode = ControlMode.Keyboard;

    void Start()
    {
        mainCam = Camera.main;
        SnapToNearestLane();
    }

    void Update()
    {
        if (gameOver) return;

        // jalankan input sesuai mode yang dipilih
        if (controlMode == ControlMode.Keyboard)
            HandleKeyboardInput();
        else if (controlMode == ControlMode.Mouse)
            HandleMouseInput();

        // gerakkan player menuju lane yang dituju
        Vector3 targetPos = new Vector3(transform.position.x, lanes[currentLane], transform.position.z);
        transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * moveSpeed);
    }

    // --- Fungsi untuk memilih mode kontrol dari ControllerSelector ---
    public void SetControlMode(string mode)
    {
        if (mode.ToLower() == "mouse")
            controlMode = ControlMode.Mouse;
        else
            controlMode = ControlMode.Keyboard;

        Debug.Log($"[PlayerController] Control mode set to: {controlMode}");
    }

    // --- Input dengan Keyboard ---
    void HandleKeyboardInput()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow) && currentLane > 0)
        {
            currentLane--;
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow) && currentLane < lanes.Length - 1)
        {
            currentLane++;
        }
    }

    // --- Input dengan Mouse ---
    void HandleMouseInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mouseWorldPos = mainCam.ScreenToWorldPoint(Input.mousePosition);

            // cari lane terdekat dengan posisi klik
            float minDist = Mathf.Infinity;
            int nearestLane = currentLane;

            for (int i = 0; i < lanes.Length; i++)
            {
                float dist = Mathf.Abs(mouseWorldPos.y - lanes[i]);
                if (dist < minDist)
                {
                    minDist = dist;
                    nearestLane = i;
                }
            }

            // ubah lane ke posisi klik
            currentLane = nearestLane;
        }
    }

    // --- Snap player ke lane terdekat di awal permainan ---
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
        transform.position = new Vector3(transform.position.x, lanes[currentLane], transform.position.z);
    }

    // --- Deteksi tabrakan dengan obstacle ---
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Obstacle"))
        {
            Debug.Log("[PlayerController] Game Over - hit obstacle");
            gameOver = true;

            // nonaktifkan obstacle
            other.gameObject.SetActive(false);

            // panggil Game Over
            GameManager.instance.GameOver();
        }
    }
}
