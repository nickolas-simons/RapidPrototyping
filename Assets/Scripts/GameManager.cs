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
    private GameObject TrackStart;

    [SerializeField]
    private float TrackTotalScore;

    [SerializeField]
    private float CountdownTime = 60f;

    private bool started = false;

    private int TotalScore = 0;

    private int AdditionalScorePoints = 0;

    private float start_time = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Time.timeScale = 0f;
        StartButton.onClick.AddListener(StartGame);
    }

    void StartGame()
    {
        TotalScore = 0;
        AdditionalScorePoints = 0;
        Time.timeScale = 1f;
        start_time = Time.time;
        GameHud.ResetTimer();
        GameHud.gameObject.SetActive(true);
        MainMenu.SetActive(false);
        started = true;
    }

    public float GetScore()
    {
        return TotalScore;
    }

    public float GetRemainingTime()
    {
        return Mathf.Clamp01(CountdownTime - (Time.time - start_time));
    }

    void StopGame()
    {
        Time.timeScale = 0f;
        GameHud.gameObject.SetActive(false);
        MainMenu.SetActive(true);
        started = false;
    }

    public void AddScorePoints(int points)
    {
        AdditionalScorePoints += points;
    }

    private void ResetPosition()
    {
        PlayerVehicle.gameObject.transform.position = TrackStart.transform.position;
        PlayerVehicle.gameObject.transform.rotation = TrackStart.transform.rotation;
        PlayerVehicle.ResetSpeeds();
    }

    // returns true is game is still running
    bool GameStateUpdate()
    {
        float track_progress = PlayerVehicle.GetTrackProgress();
        TotalScore = Mathf.RoundToInt(track_progress * TrackTotalScore) + AdditionalScorePoints;
        return GetRemainingTime() == 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (started && GameStateUpdate())
        {
            StopGame();
            ResetPosition();
        }
    }
}
