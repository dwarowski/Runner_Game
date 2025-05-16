using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MusicPlayer : MonoBehaviour
{
    public bool Play;

    public Transform audioPlayer;

    private TextMeshProUGUI title;
    private TextMeshProUGUI singer;

    public int Rand;
    public int Index;

    public AudioClip[] audios;

    private AudioSource m_AudioSource;

    private Button playButton;
    private Button previousButton;
    private Button nextButton;


    // Start is called before the first frame update

    void Start()
    {
        // 2. Проверяем, что он есть
        if (!TryGetComponent(out m_AudioSource))
        {
            Debug.LogError("AudioSource component is missing on " + m_AudioSource);
            return;
        }
        InitComponents();
    }

    public void Prev()
    {
        Index--;
        if (Index < 0)
        {
            Index = audios.Length - 1;
        }
        AudioSourcePlay();
    }
    public void Next()
    {
        Index++;
        if (Index > audios.Length - 1)
        {
            Index = 0;
        }
        AudioSourcePlay();
    }
    public void PlayAudio()
    {
        if (m_AudioSource.clip != null && Play == false)
        {

            Debug.Log("if" + Play);
            m_AudioSource.Play();
            Play = true;
        }
        else if (Play == true)
        {
            Debug.Log("elseif" + Play);
            m_AudioSource.Pause();
            Play = false;
        }
        else
        {
            Debug.Log("else" + Play);
            Index = 0;
            AudioSourcePlay();
        }
    }

    void AudioSourcePlay()
    {
        Play = true;
        m_AudioSource.clip = audios[Index];
        m_AudioSource.Play();
        string[] s = m_AudioSource.clip.name.Split('-');
        title.text = s[1];
        singer.text = s[0];
    }

    public void InitComponents()
    {
        
        audioPlayer.Find("Title").TryGetComponent(out title);
        audioPlayer.Find("Singer").TryGetComponent(out singer);

        Transform navigationTransform = audioPlayer.Find("Navigation");
        if (navigationTransform == null)
        {
            Debug.LogError("Navigation GameObject not found on the AudioPlayerCanvas. Make sure it exists and is named correctly.");
            return; // Stop if Navigation is missing
        }
        navigationTransform.Find("Previous Button").TryGetComponent(out nextButton);
        navigationTransform.Find("Next Button").TryGetComponent(out previousButton);

        audioPlayer.Find("Play Button").TryGetComponent(out playButton);

        playButton.onClick.RemoveAllListeners();
        nextButton.onClick.RemoveAllListeners();
        previousButton.onClick.RemoveAllListeners();

        playButton.onClick.AddListener(PlayAudio);
        nextButton.onClick.AddListener(Next);
        previousButton.onClick.AddListener(Prev);

        if (audios != null && audios.Length > 0)
        {
            if (Index < 0 || Index >= audios.Length)
                Index = 0;

            var clip = audios[Index];
            if (clip != null)
            {
                m_AudioSource.clip = clip;

                string[] s = clip.name.Split('-');
                title.text = s.Length > 1 ? s[1] : clip.name;
                singer.text = s.Length > 0 ? s[0] : "";
            }
        }

        void OnDisable()
        {
            PlayerPrefs.SetInt("MusicIndex", Index);
            PlayerPrefs.SetFloat("MusicTime", m_AudioSource.time);
        }

    }
}
