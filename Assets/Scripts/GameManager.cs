using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    private PlayerController player;
    private SpawnManager spawner;

    private Vector3 playerStartPos;
    private bool isGameOver = false;

    [Header("UI")]
    public GameObject gameOverPanel; // drag & drop panel UI di Inspector

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

        // pastikan panel game over awalnya tidak aktif
        if (gameOverPanel != null) 
            gameOverPanel.SetActive(false);
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

        // tampilkan panel UI Game Over
        if (gameOverPanel != null) 
            gameOverPanel.SetActive(true);

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

    // nonaktifkan semua obstacle, jangan Destroy
    ObstacleMovement[] obstacles = FindObjectsOfType<ObstacleMovement>();
    foreach (var o in obstacles)
    {
        o.gameObject.SetActive(false);  // cukup disable
        o.enabled = true;               // aktifkan script lagi untuk nanti
    }

    // aktifkan spawner lagi
    if (spawner != null)
    {
        spawner.enabled = true;
    }

    // sembunyikan panel UI Game Over
    if (gameOverPanel != null)
        gameOverPanel.SetActive(false);

    isGameOver = false;
}

}
