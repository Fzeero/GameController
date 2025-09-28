using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    private PlayerController player;
    private SpawnManager spawner;

    private Vector3 playerStartPos;
    private bool isGameOver = false;

    void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        // cari komponen
        player = FindObjectOfType<PlayerController>();
        spawner = FindObjectOfType<SpawnManager>();

        // simpan posisi awal player
        if (player != null)
            playerStartPos = player.transform.position;
    }

    void Update()
    {
        // tekan R untuk restart kalau sudah game over
        if (isGameOver && Input.GetKeyDown(KeyCode.R))
        {
            RestartGame();
        }
    }

    public void GameOver()
    {
        if (isGameOver) return;
        isGameOver = true;

        Debug.Log("[GameManager] Game Over!");

        // hentikan spawner
        if (spawner != null) spawner.enabled = false;

        // hentikan obstacle yang ada
        ObstacleMovement[] obstacles = FindObjectsOfType<ObstacleMovement>();
        foreach (var o in obstacles)
        {
            o.enabled = false;
        }

        // tampilkan info (optional, bisa pakai UI)
        Debug.Log("Tekan R untuk restart");
    }

    public void RestartGame()
    {
        Debug.Log("[GameManager] Restart game...");

        // reset player
        if (player != null)
        {
            player.transform.position = playerStartPos;
            player.gameOver = false;
        }

        // hapus/nonaktifkan semua obstacle
        ObstacleMovement[] obstacles = FindObjectsOfType<ObstacleMovement>();
        foreach (var o in obstacles)
        {
            o.gameObject.SetActive(false);
            o.enabled = true; // aktifkan kembali movement untuk nanti
        }

        // aktifkan spawner lagi
        if (spawner != null)
        {
            spawner.enabled = true;
        }

        isGameOver = false;
    }
}
