using Character;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;


public class PostProcessing : MonoBehaviour
{
    private Volume _volume;
    private VolumeProfile _profile;
    private ChromaticAberration _chromaticAberration;

    void Start()
    {
        _volume = GetComponent<Volume>();
        _profile = _volume.profile;
        _profile.TryGet<ChromaticAberration>(out _chromaticAberration);
    }

    private void Update()
    {
        GlobalVolumeFeedback();
    }

    private void GlobalVolumeFeedback()
    {
        float intensity = Mathf.Clamp( _chromaticAberration.intensity.value, 0.1f,0.4f);
        
        if (PlayerController.instance.isShooting)
        {
            intensity += Time.deltaTime;
        }
        else
        {
            intensity -= Time.deltaTime;
        }
        
        _chromaticAberration.intensity.value = intensity;
    }
}
