using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashLightController : MonoBehaviour
{
    public float FlashLightDecay;

    Light flashLight;
    float maxRange;

    float tierRange; // Number of values in one range
    int CurrentTier => Mathf.CeilToInt(flashLight.range / tierRange); //Get which tier the flashlight is in
    int currentTier;

    private void Start()
    {
        flashLight = GetComponent<Light>();
        maxRange = flashLight.range;

        tierRange = maxRange / 4; //4 is the number of battery bar
        currentTier = CurrentTier;
    }

    //Increase range of flashligth
    public void ChargeFlashLight(float amount)
    {
        //Keep value between [0,maxRange]
        flashLight.range = Mathf.Clamp(flashLight.range + amount, 0, maxRange);
        flashLight.range = maxRange;
    }

    private void Update()
    {
        //Keep value between [0,maxRange]
        flashLight.range = Mathf.Clamp(flashLight.range - (FlashLightDecay * Time.deltaTime), 0, maxRange);

        //Update current tier and UI once the tier changed
        if(currentTier != CurrentTier)
        {
            currentTier = CurrentTier;
            UIManager.Instance.UpdateBatteryBar(currentTier);
        }
    }

#if UNITY_EDITOR
    
    private void OnValidate()
    {
        FlashLightDecay = FlashLightDecay <= 0 ? 0 : FlashLightDecay;
    }

#endif
}