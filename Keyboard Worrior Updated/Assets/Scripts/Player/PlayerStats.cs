using UnityEngine.UI;
using UnityEngine;

public class PlayerStats : MonoBehaviour {

    public float energy = 50;
    public float health = 100;

    [Header("UI")]
    public Slider healthSlider;
    public Slider energySlider;
    public float sliderFillTime = 0.1f;

    private void Update()
    {
        energy = Mathf.Clamp(energy, 0, 100);
        health = Mathf.Clamp(health, 0, 100);

        healthSlider.value = Mathf.Lerp(healthSlider.value, health, sliderFillTime);
        energySlider.value = Mathf.Lerp(energySlider.value, energy, sliderFillTime);
    }

    public void AddAmount (float amount, Pickup.PickupType pickupType)
    {
        switch (pickupType)
        {
            case Pickup.PickupType.health:
                health += amount;
                break;

            case Pickup.PickupType.Battery:
                energy += amount;
                break;
        }
    }
}
