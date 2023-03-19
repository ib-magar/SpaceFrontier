using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class AudioManager : MonoBehaviour
{

    public static AudioManager _instance;
    [Header("Source")]
    [SerializeField] AudioSource _musicSource;
    [SerializeField] AudioSource _fxSource;
    [Header("bgs")]
    [SerializeField] AudioClip _menuBg;
    [SerializeField] AudioClip _deathBg;
    [SerializeField] AudioClip _victoryBg;
    [Header("fx")]
    [SerializeField] AudioClip _RockerReadySound;
    [SerializeField] AudioClip _RockerLaunchSound;
    [SerializeField] AudioClip _containerFallSound;
    [SerializeField] AudioClip _containerGreenHitSound;
    [SerializeField] AudioClip _rocketBoostSound;
    [SerializeField] AudioClip _containerDestroySound;
    [SerializeField] AudioClip _containerAddSound;
    [SerializeField] AudioClip _buttonClipSound;
    private void Awake()
    {
        if (_instance == null)
            _instance = this;
    }
    public void ButtonClick()
    {
        _fxSource.PlayOneShot(_buttonClipSound);
    }
    private void Start()
    {
        _musicSource.clip = _menuBg;
    }
    public void FadeMusicSound()
    {
        
    }
    public void RocketLaunchSound()
    {
        _musicSource.clip = _RockerLaunchSound;
        _musicSource.Play();
    }
    public void DieSoundPlay()
    {
        _musicSource.clip = _deathBg;
        _musicSource.Play();
    } public void VictorySoundPlay()
    {
        //_musicSource.clip = _victoryBg;
        //_musicSource.Play();
        _fxSource.PlayOneShot(_victoryBg);
        StartCoroutine(sadSound());
    }
    IEnumerator sadSound()
    {
        yield return new WaitForSeconds(8f);
        DieSoundPlay();
    }
    public void ContainerBlastSound()
    {
        _fxSource.PlayOneShot(_containerDestroySound);
    }
    public void  _TapSound()
    {
        _fxSource.PlayOneShot(_containerFallSound);
    }
    public void BoostSound()
    {
        _fxSource.PlayOneShot(_rocketBoostSound);
    }
    public void ContainerAddSound()
    {
        _fxSource.PlayOneShot(_containerAddSound);
    }
}
