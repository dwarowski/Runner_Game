using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MusicPlayer : MonoBehaviour
{
    public bool _Play;
    public ButtonNext; 
    public TextMeshProUGUI name;
    public TextMeshProUGUI isp;
    public Slider time;
    private AudioSource m_AudioSource;
    public AudioClip[] audios;
    public int Rand;
    public int Index;
    // Start is called before the first frame update
    void Start()
    {
        if (name == null || isp == null)
        {
            Debug.LogError("Ничего не вижу");
            return;
        }   
    }
    
    void Awake()
    {
        // 1. Находим AudioSource на этом же объекте
        m_AudioSource = GetComponent<AudioSource>();

        // 2. Проверяем, что он есть
        if (m_AudioSource == null)
        {
            Debug.LogError("AudioSource component is missing on " + gameObject.name);
            return;
        }
    }
    public void Prev()
    {
        Index--;
        if (Index < 0)
        {
            Index = audios.Length - 1;
        }
        _AudioSourcePlay();
    }
    public void Next()
    {
        Index++;
        if (Index > audios.Length - 1)
        {
            Index = 0;
        }
        _AudioSourcePlay();
    }
    public void PlayAudio()
    {

        if (m_AudioSource.clip != null && _Play == false)
        {
            m_AudioSource.Play();
            _Play = true;
        }
        else if (_Play == true)
        {
            m_AudioSource.Pause();
            _Play = false;
        }
        else
        {
            Index = 0;
            _AudioSourcePlay();
        }

    }

    private void Update()
    {
     
    }
    void _AudioSourcePlay()
    {
        _Play = true;
        m_AudioSource.clip = audios[Index];
        m_AudioSource.Play();
        string[] s = m_AudioSource.clip.name.Split('-');
        name.text = s[1];
        isp.text = s[0];
        //time.maxValue = m_AudioSource.clip.length;
        //time.value = 0;
    }

}
