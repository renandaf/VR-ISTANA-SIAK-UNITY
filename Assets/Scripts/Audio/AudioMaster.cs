using UnityEngine;

[RequireComponent(typeof (AudioSource))]
public class AudioMaster : MonoBehaviour
{
    private AudioManager _audioManager;
    private AudioSource _audioSource;
    public static float[] Samples;
    public static float LerpTime;
    public float fromSeconds;
    public float toSeconds;
    public GameObject pause;
    public GameObject play;


    [Header("Visuals")]
    private int audioSamples = 2048;
    [SerializeField, Range(0f, 20f)]private float lerpAmount;
    [Header("Audio")]
    [SerializeField]private AudioClip audioClip;
    
    private void Awake() {
        //setting necessary values
        Samples = new float[audioSamples];
        LerpTime = lerpAmount;
        _audioSource = GetComponent<AudioSource>();   
    }

    private void Start()
    {
        _audioManager = FindAnyObjectByType<AudioManager>();
    }

    private void OnEnable()
    {
        Invoke("PlayAudioClip", 1);
    }

    private void PlayAudioClip()
    {
        _audioSource.clip = audioClip;
        _audioSource.Stop();
        _audioSource.Play();
        _audioSource.time = fromSeconds;
        pause.SetActive(true);
        play.SetActive(false);
    }

    // Update is called once per frame
    private void Update(){
        GetSpectrumData();
        _audioSource.volume = _audioManager.voiceOverVolume;
        if (_audioSource.time >= toSeconds && toSeconds > fromSeconds)
        {
            PlayAudioClip();
            _audioSource.Pause();
            pause.SetActive(false);
            play.SetActive(true);
        }
    }
    
    private void GetSpectrumData(){
        _audioSource.GetSpectrumData(Samples, 0, FFTWindow.Blackman);
    }

    public void PauseOrUnpause()
    {
        if (pause.activeSelf)
        {
            _audioSource.Pause();
            pause.SetActive(false);
            play.SetActive(true);
        }
        else
        {
            _audioSource.UnPause();
            play.SetActive(false);
            pause.SetActive(true);
        }
    }

    public void Restart()
    {
        _audioSource.Stop();
        PlayAudioClip();
    }
}
