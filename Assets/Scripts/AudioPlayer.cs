using UnityEngine;

public class AudioPlayerController : MonoBehaviour
{
    public AudioClip[] audioClips;  // Array to hold your audio clips
    private AudioSource audioSource; // AudioSource component
    private int currentClipIndex = -1; // Index to keep track of the current audio clip

    public GameObject player; // Reference to the player GameObject
    public float maxDistance = 10f; // Maximum distance at which the sound is audible
    public float minVolume = 0.1f; // Minimum volume level when far away
    public float maxVolume = 1f; // Maximum volume level when close to the source

    void Start()
    {
        // Get the AudioSource component attached to this GameObject
        audioSource = GetComponent<AudioSource>();
        audioSource.Stop();

        if (audioClips.Length == 0)
        {
            Debug.LogWarning("No audio clips assigned to the array.");
        }
    }

    void Update()
    {
        if (player != null)
        {
            // Calculate the distance between the player and the audio source
            float distance = Vector3.Distance(player.transform.position, transform.position);

            // Calculate the volume based on distance
            float volume = Mathf.Clamp01(1 - (distance / maxDistance));
            audioSource.volume = Mathf.Lerp(minVolume, maxVolume, volume);
        }
    }

    // Method to play a random song
    public void PlayRandomSong()
    {
        if (audioClips.Length > 0)
        {
            currentClipIndex = Random.Range(0, audioClips.Length);
            audioSource.clip = audioClips[currentClipIndex];
            audioSource.Play();
        }
        else
        {
            Debug.LogWarning("No audio clips available to play.");
        }
    }

    // Method to stop the current song
    public void StopAudio()
    {
        if (audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }

    // Method to play the next song in the array
    public void PlayNextSong()
    {
        if (audioClips.Length > 0)
        {
            currentClipIndex = (currentClipIndex + 1) % audioClips.Length;
            audioSource.clip = audioClips[currentClipIndex];
            audioSource.Play();
        }
        else
        {
            Debug.LogWarning("No audio clips available to play.");
        }
    }
    
    public void PlayOrStop()
    {
        if (audioSource.isPlaying)
        {
            PlayNextSong();
        }
        else 
        {
            StopAudio();
        }
    }
}