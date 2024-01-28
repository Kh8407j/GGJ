// KHOGDEN 001115381
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Managers
{
    public class MusicPlayer : MonoBehaviour
    {
        public static MusicPlayer instance;

        // KH - Sub-class containing data for a individual music track.
        [System.Serializable]
        public class Music
        {
            public string name;

            public AudioClip intro;
            public AudioClip main;
        }
        public List<Music> music = new List<Music>();

        [Header("Properties")]
        public float volume = 1f;
        public float pitch = 1f;

        private bool stopMain;
        private AudioSource a;

        // Called before 'void Start()'.
        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else if (instance != this)
                Destroy(gameObject);

            a = GetComponent<AudioSource>();
        }

        // Update is called once per frame
        void Update()
        {
            a.volume = volume;
            a.pitch = pitch;
        }

        // KH - Get a music track by it's name and play it.
        public void Play(string musicName)
        {
            StartCoroutine(IPlay(musicName));
        }

        // KH - Stop any music that's playing.
        public void Stop()
        {
            stopMain = true;
            a.Stop();
        }

        // KH - Get a music track by referencing the name of it.
        public Music GetMusic(string name)
        {
            // KH - Go over all the accessible music tracks in the game.
            for (int i = 0; i < music.Count; i++)
            {
                if (music[i].name == name)
                    return music[i]; // KH - If this music track has the same name as the one being looked for then return this one.
            }

            // KH - If the music tracks couldn't be found then leave a warning in the console log and return null.
            Debug.LogWarning("Couldn't find music track with the name: " + name);
            return null;
        }

        IEnumerator IPlay(string musicName)
        {
            Music m = GetMusic(musicName);
            stopMain = false;

            // KH - Before playing the main track, play the music's intro track.
            a.loop = false;
            a.clip = m.intro;
            a.Play();

            // KH - Wait for the intro to finish playing before playing the main music.
            while (a.isPlaying)
                yield return 0;

            // KH - Check that 'Stop()' hasn't already been called before the main track could play.
            if (!stopMain)
            {
                // KH - Play the main music.
                a.loop = true;
                a.clip = m.main;
                a.Play();
            }
        }
    }
}