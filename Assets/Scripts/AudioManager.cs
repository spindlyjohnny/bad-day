using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour {
    public static AudioManager instance; // singleton
    [SerializeField] AudioSource sfxaudio, musicaudio; // audiosources for music and sfx
    public AudioClip titlemusic,levelmusic;
    // Start is called before the first frame update
    private void Awake() {
        if (instance == null) { // create singleton
            instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
        }
    }
    public void StopMusic() {
        musicaudio.Stop();
    }
    public void StopSFX() {
        sfxaudio.Stop();
    }
    public void PlayMusic(AudioClip clip) {
        if (musicaudio.isPlaying) return;
        musicaudio.clip = clip;
        musicaudio.Play();
    }
    public void PauseMusic() {
        musicaudio.Pause();
        //musicaudio.volume = 0.5f;
    }
    public void ResumeMusic() {
        musicaudio.UnPause();
        //musicaudio.volume = 1f;
    }
    public void PlaySFX(AudioClip clip, bool varypitch = false) {
        if (varypitch) {
            sfxaudio.pitch = Random.Range(1, 3);
        } else {
            sfxaudio.pitch = 1;
        }
        sfxaudio.PlayOneShot(clip);
    }
    public IEnumerator SwitchMusic(AudioClip music) {
        StopMusic();
        yield return new WaitForSeconds(.3f);
        PlayMusic(music);
    }
    // Update is called once per frame
    void Update() {
        if (SceneManager.GetActiveScene().buildIndex < 1) { // play title music on main menu. main menu buildindex is 0.
            PlayMusic(titlemusic);
        }
    }
}
