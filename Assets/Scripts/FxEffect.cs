using UnityEngine;

public class FxEffect : MonoBehaviour
{
    private ParticleSystem effect;
    private ParticleSystem.MainModule mainModule;
    private Transform poolParent;
    private bool hasLifeTime = false;
    private float lifeTimer = -1;

    void Awake()
    {
        effect = this.GetComponent<ParticleSystem>();
        mainModule = effect.main;
        poolParent = this.transform.parent;
    }

    void Update()
    {
        if (!effect.IsAlive())
        {
            ObjectPooling.Instance.ReturnObject(this.gameObject);
        }
        if (hasLifeTime)
        {
            if (lifeTimer <= 0)
            {
                Stop();
                hasLifeTime = false;
            }
            if (lifeTimer > 0f)
            {
                lifeTimer -= Time.deltaTime;
            }
        }
    }

    public void SetStartSize(float startSizeMin, float startSizeMax)
    {
        mainModule.startSize = new ParticleSystem.MinMaxCurve(startSizeMin, startSizeMax);
    }

    public void Play(Transform parent = null)
    {
        if (parent != null)
        {
            this.transform.parent = parent;
        }
        effect.Play();
    }

    public void Play(float lifeTime)
    {
        hasLifeTime = true;
        lifeTimer = lifeTime;
        effect.Play();
    }

    public void Stop()
    {
        this.transform.parent = poolParent;
        effect.Stop();
    }
}
