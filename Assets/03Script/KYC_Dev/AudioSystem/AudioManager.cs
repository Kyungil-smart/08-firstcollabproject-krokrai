using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    [Header("Audio Settings")]
    [SerializeField] private AudioSource _bgmSource;
    [SerializeField] private AudioSource _sfxSource;
    [SerializeField] private AudioMixer _mixer;

    /// <summary>
    /// BGM 플레이
    /// </summary>
    /// <param name="clip">BGM 클립</param>
    public void PlayBgm(AudioClip clip)
    {
        if(_bgmSource.clip != null) _bgmSource.Stop();
        _bgmSource.clip = clip;
        _bgmSource.Play();
    }

    /// <summary>
    /// SFX 플레이
    /// </summary>
    /// <param name="clip">SFX 클립</param>
    public void PlaySfx(AudioClip clip)
    {
        _sfxSource.PlayOneShot(clip);
    }
    
    
    /// <summary>
    /// 메인 볼륨 설정
    /// </summary>
    /// <param name="volume">0 ~ 100</param>
    public void SetMainVolume(float volume)
    {
        if(volume > 100) volume = 100;
        else if (volume < 0) volume = 0;
        
        float temp = volume / 100f;
        
        if (temp <= 0.001f)
        {
            _mixer.SetFloat("Master", -80f);
        }
        else
        {
            _mixer.SetFloat("Master", Mathf.Log10(temp) * 20);
        }
    }
    
    /// <summary>
    /// BGM 볼륨 설정
    /// </summary>
    /// <param name="volume">0 ~ 100</param>
    public void SetBgmVolume(float volume)
    {
        if(volume > 100) volume = 100;
        else if (volume < 0) volume = 0;
        
        float temp = volume / 100f;
        
        if (volume <= 0.001f)
        {
            _mixer.SetFloat("BGM", -80f);
        }
        else
        {
            _mixer.SetFloat("BGM", Mathf.Log10(temp) * 20);
        }
    }
    
    /// <summary>
    /// SFX 볼륨 설정
    /// </summary>
    /// <param name="volume">0 ~ 100</param>
    public void SetSfxVolume(float volume)
    {
        if(volume > 100) volume = 100;
        else if (volume < 0) volume = 0;
        
        float temp = volume / 100f;
        
        if (volume <= 0.001f)
        {
            _mixer.SetFloat("SFX", -80f);
        }
        else
        {
            _mixer.SetFloat("SFX", Mathf.Log10(temp) * 20);
        }
    }
    
}
