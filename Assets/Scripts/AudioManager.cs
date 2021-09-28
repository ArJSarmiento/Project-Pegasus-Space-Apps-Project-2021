using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

[System.Serializable]
public class Sound {

    public string name;
 
    public AudioClip clip;

    [Range(0f, 1f)]
    public float volume = 0.7f;
    [Range(0.5f, 1.5f)]
    public float pitch = 1f;

    [Range(0f, 0.5f)]
    public float randomVolume = 0.1f;
    [Range(0f, 0.5f)]
    public float randomPitch = 0.1f;

    public bool loop = false;
    [HideInInspector]
    public AudioSource source;
    public AudioMixerGroup group;
    

    public void setSource(AudioSource _source) {
        source = _source;
        source.clip = clip;
        source.loop = loop;
        source.outputAudioMixerGroup = group;
    }

    //randomize for variance
    public void play() {
        source.volume = volume * 1 + Random.Range(-randomVolume/2f, randomVolume/2);
        source.pitch = pitch * 1 + Random.Range(-randomPitch / 2f, randomPitch / 2);
        source.Play();
    }

    public void stop() {
        source.Stop();
    }
}

public class AudioManager : MonoBehaviour {

    public static AudioManager instance;
    [SerializeField]
    Sound[] sounds;
    public AudioMixerGroup audioMixer;
    public bool isMainMenu = false;

    void OnEnable() {
        //same as the start method but is called right before
        // goal is to loop onto a list

    
        if (instance != null) 
        {
            if(instance != this) 
            {
                Destroy(this.gameObject);
            }
        } 
        else 
        {
            instance = this;
            
            if (isMainMenu)
            {
                DontDestroyOnLoad(this);
                //isMainMenu = false;
            }            
        }        
        
        foreach (Sound s in sounds)//loop to the list <s> array
        {
            
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.outputAudioMixerGroup =  s.group; 
            //component audiosource should be hosted in a variable so it can be easily called
            //added s.source to point it to the wright source
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            //added this here to map it to have the custom mixer
            
        }
    }

    void Start() {
        for(int i = 0; i < sounds.Length; ++i) {
            GameObject _go = new GameObject("Sound_"+i+"_"+sounds[i].name);
            _go.transform.SetParent(this.transform);
            sounds[i].setSource(_go.AddComponent<AudioSource>());

        }
        playSound("Making Love Out Of Nothing At All");
    }

    public void playSound(string _name) {
        for (int i =0 ; i < sounds.Length; ++i) {
            if(sounds[i].name == _name) {
                //found the sound, now play it!
                sounds[i].play();
                return;
            }
        }

        //no sounds with _name
        Debug.LogWarning("AudioManager: Sound " + _name + " not found in sounds array");
    }

    public void stopSound(string _name) {
        for (int i = 0; i < sounds.Length; ++i) {
            if (sounds[i].name == _name) {
                //found the sound, now play it!
                sounds[i].stop();
                return;
            }
        }
    }

}
