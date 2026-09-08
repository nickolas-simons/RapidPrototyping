using System.Collections;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField]
    private AudioSource AccelerationAudio;

    [SerializeField]
    private AudioSource BrakeAudio;

    [SerializeField]
    private AudioSource MusicAudio;

    [SerializeField]
    private AudioSource SFXAudio;

    [SerializeField]
    private AudioSource SirenAudio;

    [SerializeField]
    private AudioClip AccelerationHead;

    [SerializeField]
    private AudioClip AccelerationLoop;

    [SerializeField]
    private AudioClip BrakeHead;

    [SerializeField]
    private AudioClip BrakeLoop;

    [SerializeField]
    private AudioClip CrashClip;

    [SerializeField]
    private AudioClip SirenClip;

    [SerializeField]
    private AudioClip PedestrianCrashClip;

    [SerializeField]
    private AudioClip BGM;

    [SerializeField]
    private float FadeOutTime = 0.5f;

    [SerializeField]
    private float SFXMix = 1;

    [SerializeField]
    private float MusicMix = 0.25f;

    [SerializeField] 
    private AudioClip StartScreenMusic;
    [SerializeField] 
    private AudioClip EndScreenMusic;

    private AudioState state;

    static public AudioManager Instance {get; private set;}

    enum AudioState
    {
        Acceleration = 0,
        Brake = 1,
        None = 2,
    }


    void Start()
    {
        AccelerationAudio.enabled = true;
        BrakeAudio.enabled = true;

        AccelerationAudio.clip = AccelerationHead;
        BrakeAudio.clip = BrakeHead;

        Instance = this;
        state = AudioState.None;
        AudioManager.Instance.SetMix(0, 0.25f);
        

        MusicAudio.clip = BGM;
        MusicAudio.Play();
        PlayStartMusic();

        SirenAudio.clip = SirenClip;
        SirenAudio.Play();
        SirenAudio.volume = 0;
    }

    AudioSource GetSource(AudioState s)
    {
        return s == AudioState.Acceleration ? AccelerationAudio : BrakeAudio;
    }

    private IEnumerator PlayMix()
    {
        AudioState initial_state = state;
        GetSource(initial_state).volume = 1* SFXMix;
        AudioClip starting_clip = initial_state == AudioState.Acceleration ? AccelerationHead : BrakeHead;

        GetSource(initial_state).PlayOneShot(starting_clip);
        yield return new WaitForSeconds(starting_clip.length);

        AudioClip loop_clip = initial_state == AudioState.Acceleration ? AccelerationLoop : BrakeLoop;
        GetSource(initial_state).clip = loop_clip;
        GetSource(initial_state).Play();

        yield return new WaitUntil(() => state != initial_state);

        float time = Time.time;
        while(Time.time < time + FadeOutTime)
        {
            float t = 1 - (Time.time - time / FadeOutTime);
            GetSource(initial_state).volume = state == initial_state ? 1*SFXMix : t*SFXMix;
            yield return null;
        }

        if (state != initial_state)
            GetSource(initial_state).Stop();

        yield return null;
    }

    public void UpdateAudio(float control)
{
    if (control >= 0)
    {
       
        if (state == AudioState.Brake)
        {
            BrakeAudio.Stop();
            StopAllCoroutines(); 
        }
        SetAudioMix(AudioState.Acceleration);
    }
    else
    {
        if (state != AudioState.Brake)
        {
            StopAllCoroutines();
            state = AudioState.Brake;
            BrakeAudio.clip = BrakeLoop; 
            BrakeAudio.volume = 1 * SFXMix;
            BrakeAudio.Play();
        }
    }
    Debug.Log(state.ToString());
}

    public void PlayCrash()
    {
        SFXAudio.PlayOneShot(CrashClip);
        return;
    }

    public void PlayPedestrianCrash()
    {
        SFXAudio.PlayOneShot(PedestrianCrashClip);
        return;
    }

    public void SetSirenVolume(float volume)
    {
        SirenAudio.volume = volume * SFXMix;
        return;
    }

    private void SetAudioMix(AudioState new_state)
    {
        if (state != new_state)
        {
            state = new_state;
            StartCoroutine(PlayMix());
        }
        else
        {
            state = new_state;
        }
    }

    public void SetMix(float SFX, float Music)
    {
        SFXMix = SFX;
        MusicMix = Music;

        AccelerationAudio.volume = SFXMix;
        BrakeAudio.volume = SFXMix;
        SFXAudio.volume = SFXMix;
        MusicAudio.volume = MusicMix;

    }
    public void PlayStartMusic()
    {
    if (StartScreenMusic != null)
    {
        MusicAudio.clip = StartScreenMusic;
        MusicAudio.Play();
    }
    }

    public void PlayGameplayMusic()
    {
    if (BGM != null)
    {
        MusicAudio.clip = BGM;
        MusicAudio.Play();
    }
    }

    public void PlayEndMusic()
    {
    if (EndScreenMusic != null)
    {
        MusicAudio.clip = EndScreenMusic;
        MusicAudio.Play();
    }
    }

}
