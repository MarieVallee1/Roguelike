using Character;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;


public class PostProcessing : MonoBehaviour
{
    private Volume _volume;
    private VolumeProfile _profile;
    private ChromaticAberration _chromaticAberration;
    private LensDistortion _lensDistortion;

    void Start()
    {
        _volume = GetComponent<Volume>();
        _profile = _volume.profile;
        _profile.TryGet<ChromaticAberration>(out _chromaticAberration);
        _profile.TryGet<LensDistortion>(out _lensDistortion);
    }

    private void Update()
    {
        ChromaticAberrationFeedback();
        LensDistortionFeedback();
    }

    private void ChromaticAberrationFeedback()
    {
        float intensity = Mathf.Clamp( _chromaticAberration.intensity.value, 0f,0.10f);
        
        if (PlayerController.Instance.onShoot)
        {
            intensity += Time.deltaTime;
        }
        else
        {
            intensity -= Time.deltaTime;
        }
        
        _chromaticAberration.intensity.value = intensity;
    }

    private void LensDistortionFeedback()
    {
        float intensity = Mathf.Clamp( _lensDistortion.intensity.value, -0.15f,0f);
        
        if (PlayerController.Instance.onParry)
        {
            intensity -= Time.deltaTime;
        }
        else
        {
            intensity += Time.deltaTime;
        }
        
        _lensDistortion.intensity.value = intensity;
    }
}
