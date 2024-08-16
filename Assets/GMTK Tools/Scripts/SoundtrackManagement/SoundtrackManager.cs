using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundtrackManager : MonoBehaviour
{
    #region Singleton

    public static SoundtrackManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    #endregion

    [Header("Audiosources")]
    [SerializeField] AudioSource[] soundtrackSources;
    private AudioSource ActiveSource => firstActive ? soundtrackSources[0] : soundtrackSources[1];
    private AudioSource FadingSource => firstActive ? soundtrackSources[1] : soundtrackSources[0];

    private bool firstActive = true;
    private AudioClip lastSoundtrack;

    [Header("Sound Settings")]
    [SerializeField] private float maxVolume = 0.75f;
    [SerializeField] private float maxFadeTime = 2f;

    private IEnumerator soundtrackTransition;

    private void Start()
    {
        ActiveSource.volume = maxVolume;
        FadingSource.volume = 0;

        if (ActiveSource.clip != null)
            lastSoundtrack = ActiveSource.clip;
    }

    public void PlaySoundtrack(AudioClip newSoundtrack, bool crossfade = true, bool keepPlaybackTime = false)
    {
        // Return if it's the same soundtrack that's already playing
        if (newSoundtrack == lastSoundtrack)
            return;

        // If the coroutine was started before the other one has finished, cancel it
        if (soundtrackTransition != null)
        {
            StopCoroutine(soundtrackTransition);
        }

        if (crossfade)
        {
            // Save the IEnumerator for later and run, fading the new audio
            soundtrackTransition = CrossfadeSoundtracks(newSoundtrack, keepPlaybackTime);
            StartCoroutine(soundtrackTransition);
        }
        else
        {
            // If no fading, simply set the desired values
            FadingSource.clip = newSoundtrack;
            FadingSource.volume = maxVolume;
            FadingSource.Play();

            ActiveSource.volume = 0;
            ActiveSource.Stop();

            firstActive = !firstActive;
        }

        // Save reference to check against next time
        lastSoundtrack = newSoundtrack;
    }

    private IEnumerator CrossfadeSoundtracks(AudioClip newAudio, bool keepPlaybackTime)
    {
        // Wait until the the current scene is finished loading to prevent deltaTime being massive
        yield return new WaitUntil(() => !SceneLoader.LoadingScene);

        // Start the audio before fading
        FadingSource.clip = newAudio;
        FadingSource.Play();

        // If the new soundtrack should start where the other one ended
        if (keepPlaybackTime)
            FadingSource.timeSamples = ActiveSource.timeSamples;

        // Crossfade between the old and new audio source over the specified fade time
        float fadeTime = 0;
        while (FadingSource.volume < maxVolume)
        {
            FadingSource.volume = Mathf.Lerp(0, maxVolume, fadeTime);
            ActiveSource.volume = Mathf.Lerp(maxVolume, 0, fadeTime);
            fadeTime += Time.unscaledDeltaTime / maxFadeTime;

            yield return new WaitForEndOfFrame();
        }

        // Stop the old audio source and swap the active sources
        ActiveSource.Stop();
        firstActive = !firstActive;

        soundtrackTransition = null;
    }
}
