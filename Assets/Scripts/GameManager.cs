using System.Collections;
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
    private GameObject EndMenu;

    [SerializeField]
    private Button StartButton;

    [SerializeField]
    private Button RestartButton;

    [SerializeField]
    private Vehicle PlayerVehicle;

    [SerializeField]
    private GameObject TrackStart;

    [SerializeField]
    private AnimationCurve SirenAudioCurve;

    [SerializeField]
    private float RelativeTimerBlinkTime;


    [SerializeField]
    private float TrackTotalScore;

    [SerializeField]
    private float CountdownTime = 60f;

    // NEW
    [SerializeField]
    private Animator PoliceAnimator;

    // NEW
    [SerializeField]
    private float PoliceFlyDuration = 0.8f;

    private bool started = false;

    private int TotalScore = 0;

    private int AdditionalScorePoints = 0;

    private float start_time = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Time.timeScale = 0f;
        StartButton.onClick.AddListener(StartGame);
        RestartButton.onClick.AddListener(ReturnToStart);
    }

    void StartGame()
    {
        // NEW
        StartCoroutine(StartGameSequence());
    }

    void ReturnToStart()
    {
        EndMenu.SetActive(false);
        MainMenu.SetActive(true);
        AudioManager.Instance.SetSirenVolume(0);
    }

    // NEW
    private IEnumerator StartGameSequence()
    {

        PoliceAnimator.SetTrigger("FlyTrigger");

        yield return new WaitForSecondsRealtime(PoliceFlyDuration);
        AudioManager.Instance.SetMix(1, 0.25f);

        TotalScore = 0;
        AdditionalScorePoints = 0;
        Time.timeScale = 1f;
        start_time = Time.time;
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
        return Mathf.Clamp(CountdownTime - (Time.time - start_time),0,CountdownTime);
    }

    void StopGame()
    {
        AudioManager.Instance.SetMix(0, 1);
        Time.timeScale = 0f;
        EndMenu.SetActive(true);
        GameHud.UpdateEndGameText();
        started = false;
    }

    public void AddScorePoints(int points)
    {
        AdditionalScorePoints += points;
        GameHud.AddFlavorText(points);
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
        TotalScore = Mathf.Max(TotalScore,Mathf.RoundToInt(track_progress * TrackTotalScore) + AdditionalScorePoints);

        float t = 1 - Mathf.Clamp01(GetRemainingTime() / CountdownTime);
        AudioManager.Instance.SetSirenVolume(SirenAudioCurve.Evaluate(t));

        if(1-(GetRemainingTime()/CountdownTime) > RelativeTimerBlinkTime)
        {
            GameHud.AddBlink();
        }


        return GetRemainingTime() == 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (started && GameStateUpdate())
        {
            GameHud.RemoveBlink();
            StopGame();
            ResetPosition();
        }
    }
}