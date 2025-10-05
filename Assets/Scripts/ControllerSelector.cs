using UnityEngine;
using UnityEngine.UI;

public class ControllerSelector : MonoBehaviour
{
    public GameObject startPanel;          // Panel pemilihan controller
    public PlayerController player;        // Referensi ke player
    public Button keyboardButton;
    public Button mouseButton;

    private void Start()
    {
        // Pastikan panel pemilihan muncul di awal
        if (startPanel != null)
            startPanel.SetActive(true);

        // Hentikan sementara game (freeze)
        Time.timeScale = 0f;

        // Nonaktifkan kontrol dulu
        if (player != null)
            player.enabled = false;

        // Tambahkan event listener ke tombol
        keyboardButton.onClick.AddListener(() => SelectController("keyboard"));
        mouseButton.onClick.AddListener(() => SelectController("mouse"));
    }

    private void SelectController(string mode)
    {
        if (player == null) return;

        // Aktifkan kontrol player dan atur mode yang dipilih
        player.enabled = true;
        player.SetControlMode(mode);

        // Tutup panel UI
        if (startPanel != null)
            startPanel.SetActive(false);

        // Jalankan kembali game
        Time.timeScale = 1f;

        Debug.Log($"[ControllerSelector] Controller selected: {mode}");
    }
}
