using UnityEngine;

public class VFXEffect : MonoBehaviour
{
    public Effect[] effects;

    void Start()
    {
        for (int i = 0; i < effects.Length; i++)
        {
            GameObject newEffectInstance = Instantiate(effects[i].effectPrefab, this.transform);
            if (effects[i].audioPrefab)
            {
                GameObject newAudioInstance = Instantiate(effects[i].audioPrefab, this.transform);
                effects[i].Initialize(newEffectInstance, newAudioInstance);
            }
            else
            {
                effects[i].Initialize(newEffectInstance);
            }
        }
    }

    public void Play()
    {
        for (int i = 0; i < effects.Length; i++)
        {
            if (effects[i].alwaysCreate || (Random.Range(0, 2) == 1))
            {
                Play(i);
            }
        }
    }

    public void Play(int index)
    {
        effects[index].PlayEffect();
        effects[index].PlayAudio(this.transform.position);
    }

    public void Stop()
    {
        for (int i = 0; i < effects.Length; i++)
        {
            Stop(i);
        }
    }

    public void Stop(int index)
    {
        effects[index].StopEffect();
        effects[index].StopAudio();
    }

    public bool IsActive()
    {
        for (int i = 0; i < effects.Length; i++)
        {
            if (effects[i].IsActive())
            {
                return true;
            }
        }
        return false;
    }

    [System.Serializable]
    public class Effect
    {
        public GameObject effectPrefab;
        public GameObject audioPrefab;
        public float startSizeMin = 1f;
        public float startSizeMax = 1f;
        public bool alwaysCreate = true;

        private GameObject effectInstance;
        private GameObject audioInstance;
        private ParticleSystem particleSystem;
        private ParticleSystem.MainModule mainModule;
        private AudioSource audioSource;
        private bool hasAudio = false;

        public void Initialize(GameObject _effectInstance, GameObject _audioInstance = null)
        {
            hasAudio = _audioInstance != null;
            effectInstance = _effectInstance;
            particleSystem = effectInstance.GetComponent<ParticleSystem>();
            mainModule = particleSystem.main;
            mainModule.startSize = new ParticleSystem.MinMaxCurve(startSizeMin, startSizeMax);
            if (hasAudio)
            {
                audioInstance = _audioInstance;
                audioSource = audioInstance.GetComponent<AudioSource>();
            }
        }

        public void PlayEffect()
        {
            particleSystem.Play();
        }

        public void PlayAudio(Vector3 position)
        {
            if (hasAudio)
            {
                float distance = Mathf.Clamp(Vector3.Distance(Vector3.zero, position), 0, GameManager.Instance.HorizontalBorderRight());
                float percentage = Mathf.Abs(GameManager.Instance.HorizontalBorderRight() - distance) / GameManager.Instance.HorizontalBorderRight();
                audioSource.volume *= percentage;
                audioSource.Play();
            }
        }

        public void StopEffect()
        {
            particleSystem.Stop();
        }

        public void StopAudio()
        {
            if (hasAudio)
            {
                audioSource.Stop();
            }
        }

        public bool IsActive()
        {
            if (hasAudio)
            {
                return particleSystem.isEmitting && audioSource.isPlaying;
            }
            else
            {
                return particleSystem.isEmitting;
            }
        }
    }
}
