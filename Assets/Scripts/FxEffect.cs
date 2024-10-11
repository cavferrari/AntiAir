using UnityEngine;

public class FxEffect : MonoBehaviour
{
    private ParticleSystem effect;
    private ParticleSystem.MainModule mainModule;
    private Transform poolParent;

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

    public void Stop()
    {
        this.transform.parent = poolParent;
        effect.Stop();
    }
}
