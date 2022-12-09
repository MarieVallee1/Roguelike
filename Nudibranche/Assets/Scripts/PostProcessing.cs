using System;
using Character;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;


public class PostProcessing : MonoBehaviour
{
    public static PostProcessing Instance;
    private Volume _volume;
    private VolumeProfile _profile;
    public ChromaticAberration _chromaticAberration;
    public LensDistortion _lensDistortion;

    public bool gotHit;
    public bool dashing;

    private void Awake()
    {
        if(Instance!= null && Instance != this)Destroy(this);
        Instance = this;
    }

    void Start()
    {
        _volume = GetComponent<Volume>();
        _profile = _volume.profile;
        _profile.TryGet<ChromaticAberration>(out _chromaticAberration);
        _profile.TryGet<LensDistortion>(out _lensDistortion);
    }

    private void Update()
    {
        PlayerShootFeedback();
        ParryFeedback();
        if(gotHit)PlayerHitFeedback();
        if(dashing)PlayerDashingFeedback();
    }

    private void PlayerShootFeedback()
    {
        if (PlayerController.Instance.onShoot && _chromaticAberration.intensity.value < 0.1f)
        {
            _chromaticAberration.intensity.value += Time.deltaTime;
        }
        else if(!PlayerController.Instance.onShoot && PlayerController.Instance.vulnerable && _chromaticAberration.intensity.value > 0)
        {
            _chromaticAberration.intensity.value -= Time.deltaTime;
        }
    }
    
    public void PlayerHitFeedback()
    {
        _chromaticAberration.intensity.value -= Time.deltaTime;

        if (_chromaticAberration.intensity.value <= 0) gotHit = false;
    }

    private void ParryFeedback()
    {
        if (PlayerController.Instance.onParry &&  _lensDistortion.intensity.value > -0.15f)
        {
            _lensDistortion.intensity.value -= Time.deltaTime;
        }
        else if(_lensDistortion.intensity.value == 0f)
        {
            _lensDistortion.intensity.value += Time.deltaTime;
        }
    }
    
    public void PlayerDashingFeedback()
    {
        _lensDistortion.intensity.value += Time.deltaTime;

        if (_lensDistortion.intensity.value >= 0) dashing = false;
    }
}
