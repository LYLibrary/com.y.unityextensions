using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
[DefaultExecutionOrder(-1000)]
public class AudioTrackManager : MonoBehaviour
{
    private void Reset()
    {
        gameObject.name = "[音轨管理器]";
        transform.position = Vector3.zero;
    }

    private static AudioTrackManager instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            if (!instance.gameObject.Equals(gameObject))
            {
                gameObject.SetActive(false);
                DestroyImmediate(gameObject);
            }
        }
    }

    [SerializeField, Header("音轨")] private List<AudioTrack> audioTracks = new List<AudioTrack>();

    public static void CreateAudioTrack(string trackName)
    {
        if (string.IsNullOrEmpty(trackName)) { return; }
        AudioTrack findTrack = instance.audioTracks.Find(x => x.Name == trackName);
        if (findTrack != null) { return; }
        GameObject audioTrack = new GameObject(trackName, typeof(AudioSource));
        audioTrack.transform.SetParent(instance.transform);
        audioTrack.transform.position = Vector3.zero;
        audioTrack.transform.eulerAngles = Vector3.zero;
        AudioSource source = audioTrack.GetComponent<AudioSource>();
        source.playOnAwake = false;
        AudioTrack track = new AudioTrack(trackName, source);
        instance.audioTracks.Add(track);
    }

    public static void DeleteAudioTrack(string trackName)
    {
        AudioTrack findTrack = instance.audioTracks.Find(x => x.Name == trackName);
        if (findTrack != null)
        {
            instance.audioTracks.Remove(findTrack);
            findTrack.Delete();
        }
    }

    public static void DeleteAllAudioTrack()
    {
        List<AudioTrack> tempList = new List<AudioTrack>(instance.audioTracks);
        instance.audioTracks.Clear();
        for (int i = 0; i < tempList.Count; i++)
        {
            tempList[i].Delete();
        }
    }

    public static void StopAllAudioTrack()
    {
        for (int i = 0; i < instance.audioTracks.Count; i++)
        {
            instance.audioTracks[i].Stop();
        }
    }

    public static void SetAudioClip(string trackName, AudioClip clip)
    {
        AudioTrack findTrack = instance.audioTracks.Find(x => x.Name == trackName);
        if (findTrack != null)
        {
            findTrack.clip = clip;
        }
    }

    public static AudioTrack GetAudioTrack(string trackName)
    {
        AudioTrack findTrack = instance.audioTracks.Find(x => x.Name == trackName);
        return findTrack;
    }

    [Serializable]
    public class AudioTrack
    {
        [SerializeField] private string name;
        [SerializeField] private AudioSource source;
        private AudioTrackListener trackListener;

        public AudioTrack(string name, AudioSource source)
        {
            this.name = name;
            this.source = source;
            trackListener = this.source.gameObject.AddComponent<AudioTrackListener>();
            trackListener.finishOne = () => { finishOne?.Invoke(); };
        }

        public string Name { get { return name; } }
        public bool Mute { get { return source.mute; } set { source.mute = value; } }
        public bool Loop { get { return source.loop; } set { source.loop = value; } }
        public float Volume { get { return source.volume; } set { source.volume = value; } }
        public float CurrentTime { get { return source.time; } set { source.time = value; } }
        public int timeSamples { get { return source.timeSamples; } set { source.timeSamples = value; } }
        public AudioClip clip { get { return source.clip; } set { source.clip = value; } }
        public Action finishOne { get; set; }

        public void Play()
        {
            source.Play();
            trackListener.StartListener();
        }

        public void Stop()
        {
            source.Stop();
            trackListener.StopListener();
        }

        public void Pause()
        {
            source.Pause();
        }

        public void UnPause()
        {
            source.UnPause();
        }

        public void Delete()
        {
            if (source != null)
            {
                trackListener.StopListener();
                source.gameObject.SetActive(false);
                Destroy(source.gameObject);
            }
        }
    }

    [DisallowMultipleComponent]
    public class AudioTrackListener : MonoBehaviour
    {
        public Action finishOne { get; set; }

        public void StartListener()
        { 
        
        }
        public void StopListener()
        { 
        
        }
    }
}
