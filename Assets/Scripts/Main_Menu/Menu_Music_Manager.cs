using UnityEngine;

/// <summary>
/// Class for The Management of The Music in the Main Menu
/// </summary>
public class Menu_Music_Manager : MonoBehaviour
{
    [SerializeField] private AudioClip musicClip;

    private void Awake()
    {
        SoundManager.Instance.StopMusic();
        SoundManager.Instance.PlayMusic(musicClip);
    }
}
