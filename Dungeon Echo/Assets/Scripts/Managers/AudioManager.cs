using System.Collections;
using EnumNamespace;
using UnityEngine;
using InterfaceNamespace;

public class AudioManager : IAudioManager, ISubscriber
{
    private IObjectStorage _objectStorage;
    private ICoroutiner _coroutiner;
    
    private AudioSource _efxSource;
    private AudioSource _activeMusic;
    private AudioSource _inactiveMusic;

    private float _lowPitchRange;
    private float _highPitchRange;
    private bool _crossFading;
    private float _crossFadeRate;
    private float _musicVolume;
    public AudioManager(IObjectStorage objectStorage, ICoroutiner coroutiner)
    {
        _objectStorage = objectStorage;
        _coroutiner = coroutiner;
        _lowPitchRange = 0.95f;
        _highPitchRange = 1.05f;
        _crossFadeRate = 1.5f;
        _musicVolume = 0.3f;
    }

    public void OnEvent(CustomEventArgs messageData)
    {
        var message = messageData.Message;
        switch (message)
        {
            case GameEventName.GoStageMainMenu:
                PlayMusic("menu");
                break;
            case GameEventName.GoStageStartGame:
                PlayMusic("main");
                break;
        }
    }

    public void SetDependecies(GameObject soundObGameObject)
    {
        var audioSources = soundObGameObject.GetComponents<AudioSource>();
        _efxSource = audioSources[0];
        _activeMusic = audioSources[1];
        _inactiveMusic = audioSources[2];
        _activeMusic.volume = _musicVolume;
        _inactiveMusic.volume = _musicVolume; 
    }

    private void PlayMusic(string nameAudio)
    {
        var clip = _objectStorage.GetAudioByName(nameAudio);
       // _musicSource1.Play ();
       if (_crossFading) { return; }
       _coroutiner.StartCoroutine(CrossFadeMusic(clip)); // При изменении музыкальной композиции вызываем сопрограмму.
    }

    private IEnumerator CrossFadeMusic(AudioClip clip)
    {
        _crossFading = true;
        _inactiveMusic.clip = clip;
        _inactiveMusic.volume = 0;
        _inactiveMusic.Play();
        var scaledRate = _crossFadeRate * _musicVolume;
        while (_activeMusic.volume > 0)
        {
            _activeMusic.volume -= scaledRate * Time.deltaTime;
            _inactiveMusic.volume += scaledRate * Time.deltaTime;
            yield return null; // Эта инструкция yield останавливает операции на один кадр.
        }
        var temp = _activeMusic; //Временная переменная, используемая, когда мы меняем местами переменные _active и _inactive.
        _activeMusic = _inactiveMusic;
        _activeMusic.volume = _musicVolume;
        _inactiveMusic = temp;
        _inactiveMusic.Stop();
        _crossFading = false;
    }
}
/*

public AudioSource music1Source { get; set; }
    public AudioSource music2Source { get; set; }
    public string music1  { get; set; } //имена музыкальных клипов.
    public string music2 { get; set; } //имена музыкальных клипов.
    public ManagerStatus status { get; private set; }

    public AudioSource _activeMusic { get; set; } // Следим за тем, какой из источников активен, а какой нет.
    public AudioSource _inactiveMusic { get; set; }
    public float crossFadeRate = 1.5f;
   
    public float soundVolume
    { //Свойство с функцией чтения и функцией доступа для громкости.
        get { return AudioListener.volume; } // Реализуем функцию чтения/функцию доступа
        set { AudioListener.volume = value; } // с помощью AudioListener.
    }
    private bool _crossFading; // Переключатель, позволяющий избежать ошибок в процессе перехода.
    public void Startup( )
    {
        status = ManagerStatus.Started; 
    }
    public void PlayMusic1()
    { // Загрузка музыки Battle из папки Resources.
        PlayMusic(Resources.Load("Music/" + music1) as AudioClip);
    }
    public void PlayMusic2()
    { // Загрузка музыки Battle из папки Resources.
        PlayMusic(Resources.Load("Music/" + music2) as AudioClip);
    }
    private void PlayMusic(AudioClip clip)
    { // Воспроизведение музыки при помощи параметра AudioSource.clip.
        //music1Source.clip = clip;
        //music1Source.Play();
        if (_crossFading) { return; }
        StartCoroutine(CrossFadeMusic(clip)); // При изменении музыкальной композиции вызываем сопрограмму.
    }

    private IEnumerator CrossFadeMusic(AudioClip clip)
    {
        _crossFading = true;
        _inactiveMusic.clip = clip;
        _inactiveMusic.volume = 0;
        _inactiveMusic.Play();
        float scaledRate = crossFadeRate * _musicVolume;
        while (_activeMusic.volume > 0)
        {
            _activeMusic.volume -= scaledRate * Time.deltaTime;
            _inactiveMusic.volume += scaledRate * Time.deltaTime;
            yield return null; // Эта инструкция yield останавливает операции на один кадр.
        }
        AudioSource temp = _activeMusic; //Временная переменная, используемая, когда мы меняем местами переменные _active и _inactive.
        _activeMusic = _inactiveMusic;
        _activeMusic.volume = _musicVolume;
        _inactiveMusic = temp;
        _inactiveMusic.Stop();
        _crossFading = false;
    }

    public void StopMusic()
    {
        _activeMusic.Stop();
        _inactiveMusic.Stop();
    }
    private float _musicVolume; //Непосредственный доступ к закрытой переменной невозможен, только через функцию задания свойства.
    public float musicVolume
    {
        set
        {
            _musicVolume = value;
            if (music1Source != null && !_crossFading)
            {
                music1Source.volume = _musicVolume;
                music2Source.volume = _musicVolume; // Регулировка громкости обоих источников музыки.
            }
        }
    }
    public bool musicMute
    {
        set
        {
            if (music1Source != null)
            {
                music1Source.mute = value;
                music2Source.mute = value;
            }
        }
    }*/