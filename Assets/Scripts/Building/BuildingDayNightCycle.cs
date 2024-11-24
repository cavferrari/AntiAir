using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingDayNightCycle : MonoBehaviour
{
    private MeshRenderer[] meshRenderers;
    private Material[] materials;

    void Start()
    {
        meshRenderers = this.GetComponentsInChildren<MeshRenderer>();
        materials = new Material[meshRenderers.Length];
        for (int i = 0; i < meshRenderers.Length; i++)
        {
            materials[i] = meshRenderers[i].material;
        }
    }

    void Update()
    {
        if (GameManager.Instance.IsNight())
        {
            for (int i = 0; i < materials.Length; i++)
            {
                float blend = materials[i].GetFloat("_Blend");
                if (blend != 1f)
                {
                    blend = Mathf.Lerp(blend, 1f, 1f);
                    materials[i].SetFloat("_Blend", blend);
                }
                Color emissiveColor = materials[i].GetColor("_EmissiveColor");
                if (emissiveColor != Color.white)
                {
                    emissiveColor = Color.Lerp(emissiveColor, Color.white, 1f);
                    materials[i].SetColor("_EmissiveColor", emissiveColor);
                }
            }
        }
        else
        {
            for (int i = 0; i < materials.Length; i++)
            {
                float blend = materials[i].GetFloat("_Blend");
                if (blend != 0f)
                {
                    blend = Mathf.Lerp(blend, 0f, 1f);
                    materials[i].SetFloat("_Blend", blend);
                }
                Color emissiveColor = materials[i].GetColor("_EmissiveColor");
                if (emissiveColor != Color.black)
                {
                    emissiveColor = Color.Lerp(emissiveColor, Color.black, 1f);
                    materials[i].SetColor("_EmissiveColor", emissiveColor);
                }
            }
        }
    }
}