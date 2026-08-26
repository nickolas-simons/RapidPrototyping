using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;



public class GameManager : MonoBehaviour
{
    [SerializeField]
    private HUD GameHud;

    [SerializeField]
    private GameObject MainMenu;

    [SerializeField]
    private Button StartButton;

    [SerializeField]
    private Vehicle PlayerVehicle;

    [SerializeField]
    private float CompletionThreshold = 0.95f;

    [SerializeField]
    private GameObject TrackStart;

    private bool started = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Time.timeScale = 0f;
        StartButton.onClick.AddListener(StartGame);
    }

    void StartGame()
    {
        Time.timeScale = 1f;
        GameHud.ResetTimer();
        GameHud.gameObject.SetActive(true);
        MainMenu.SetActive(false);
        started = true;
    }

    void StopGame()
    {
        Time.timeScale = 0f;
        GameHud.gameObject.SetActive(false);
        MainMenu.SetActive(true);
        started = false;
    }

    private void ResetPosition()
    {
        PlayerVehicle.gameObject.transform.position = TrackStart.transform.position;
        PlayerVehicle.gameObject.transform.rotation = TrackStart.transform.rotation;
        PlayerVehicle.ResetSpeeds();
    }

    bool HasCompletedGame()
    {
        return PlayerVehicle.GetTrackProgress() >= CompletionThreshold;
    }

    // Update is called once per frame
    void Update()
    {
        if (started && HasCompletedGame())
        {
            StopGame();
            ResetPosition();
        }
    }
}
