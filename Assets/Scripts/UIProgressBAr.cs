using System;
using UnityEngine;
using UnityEngine.UI;

public class UIProgressBAr : MonoBehaviour
{
    [SerializeField] float fillSpeed = 0.4f;
    [SerializeField] ParticleSystem fillParticles;

    private Slider slider;
    private float targetProgress = 0;

    void Awake()
    {
        slider = gameObject.GetComponent<Slider>();
    }
    // Update is called once per frame
    void Update()
    {
        if(slider.value < targetProgress)
        {
            slider.value += fillSpeed * Time.deltaTime;
            if(!fillParticles.isPlaying)
            {
                fillParticles.Play();
            }
        }else
        {
            fillParticles.Stop();
        }
    }

    public void IncrementProgress(float newProgress)
    {
        targetProgress = slider.value + newProgress;
    }
}
