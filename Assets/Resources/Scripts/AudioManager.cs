using UnityEngine;

public class AudioManager
{
    private static AudioSource m_SoundSource = null;
   
    public static void PlaySound(string clipName)
    {
        if(m_SoundSource == null)
        {
            m_SoundSource = new GameObject("Audio").AddComponent<AudioSource>();
            m_SoundSource.loop = false;
            m_SoundSource.playOnAwake = false;
        }
        AudioClip clip = ResManager.Load<AudioClip>(clipName);
        m_SoundSource.clip = clip;
        m_SoundSource.Play();
    }
}