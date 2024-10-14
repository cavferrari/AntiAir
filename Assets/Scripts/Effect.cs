using UnityEngine;

public class Effect<T> : MonoBehaviour
{
    protected T effect;
    protected Transform poolParent;
    protected bool hasLifeTime = false;
    protected float lifeTimer = -1;

    void Awake()
    {
        CustomAwake();
    }

    void Start()
    {
        CustomStart();
    }

    void Update()
    {
        if (CustomDestroyCondition())
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

    public virtual void SetStartSize(float startSizeMin, float startSizeMax)
    {
    }

    public void Play(Transform parent = null)
    {
        if (parent != null)
        {
            this.transform.parent = parent;
        }
        CustomPlay();
    }

    public void Play(float lifeTime)
    {
        hasLifeTime = true;
        lifeTimer = lifeTime;
        Play();
    }

    public void Stop()
    {
        this.transform.parent = poolParent;
        CustomStop();
    }

    public virtual bool CustomDestroyCondition()
    {
        return false;
    }

    public virtual void CustomPlay()
    {
    }

    public virtual void CustomStop()
    {
    }

    public virtual void CustomStart()
    {
    }

    public virtual void CustomAwake()
    {
        effect = this.GetComponent<T>();
        poolParent = this.transform.parent;
    }
}