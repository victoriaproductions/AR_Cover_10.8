using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

public class TrackableHandler : DefaultObserverEventHandler
{
    public InitializeManager manager;
    public LoadingController controller;

    public GameObject cautionCell;
    public AudioSource src;

    private string targetName;
    private GameObject childObj;


    protected override void Start()
    {
        base.Start();

        targetName = gameObject.name.Replace("_00", string.Empty);
    }

    private GameObject LoadFile()
    {
        GameObject outObj = Instantiate(controller.bundle.LoadAsset<GameObject>(targetName));
        outObj.name = targetName;
        outObj.transform.SetParent(transform, false);
        outObj.transform.localScale = Vector3.one;

        Animator[] anims = outObj.GetComponentsInChildren<Animator>();

        for (int i = 0; i < anims.Length; i++)
        {
            anims[i].applyRootMotion = false;
        }

        ParticleSystemRenderer[] par = outObj.GetComponentsInChildren<ParticleSystemRenderer>();

        if (par.Length > 0)
        {
            if (manager.pars.Find(find => find.name == targetName) != null)
            {
                int ind = manager.pars.FindIndex(find => find.name == targetName);
                if (manager.pars[ind].objectName.Length > 0)
                {
                    for (int i = 0; i < manager.pars[ind].objectName.Length; i++)
                    {
                        for (int j = 0; j < par.Length; j++)
                        {
                            if (par[j].name == manager.pars[ind].objectName[i])
                            {
                                par[j].material.shader = Shader.Find("Legacy Shaders/Particles/Additive (Soft)");
                            }
                            else
                            {
                                par[j].material.shader = Shader.Find("Legacy Shaders/Transparent/Cutout/Soft Edge Unlit");
                            }
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < par.Length; i++)
                    {
                        par[i].material.shader = Shader.Find("Legacy Shaders/Transparent/Cutout/Soft Edge Unlit");
                    }
                }
            }
            else
            {
                for (int i = 0; i < par.Length; i++)
                {
                    par[i].material.shader = Shader.Find("Legacy Shaders/Transparent/Cutout/Soft Edge Unlit");
                }
            }
        }

        for (int i = 0; i < par.Length; i++)
        {

            if (targetName == "TT12" || targetName == "TT13" || targetName == "TT20" || targetName == "TT24" || targetName == "TT29" || targetName == "TT37")
            {

            }
            else
            {

            }
        }

        return outObj;
    }

    protected override void OnTrackingFound()
    {
        if (!cautionCell.activeSelf)
        {
            if (childObj != null)
            {
                childObj.SetActive(true);
                if (!src.isPlaying)
                {
                    src.Play();
                }
            }
            else
            {
                childObj = LoadFile();
                if (!src.isPlaying)
                {
                    src.Play();
                }
            }
        }
    }

    protected override void OnTrackingLost()
    {
        if (childObj != null)
        {
            childObj.SetActive(false);
            if (src.isPlaying)
            {
                src.Stop();
            }
        }
    }
}