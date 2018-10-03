using UnityEngine.UI;
using UnityEngine;

public class PlayerStats : MonoBehaviour {

    public float energy = 50;
    public float health = 100;

    [Header("Energy Regen")]
    public float energyRepeatTime = 15;
    public float energyAmount = 1;

    [Header("Health Regen")]
    public float healthRepeatTime = 15;
    public float healthAmount = 1;

    [Header("UI")]
    public Slider healthSlider;
    public Slider energySlider;
    public float sliderFillTime = 0.1f;

    public bool godMode = false;

    private void Start()
    {
        InvokeRepeating("RegenHealth", healthRepeatTime, healthRepeatTime);
        InvokeRepeating("RegenEnergy", energyRepeatTime, energyRepeatTime);
    }

    private void Update()
    {
        if(!godMode)
        {
            energy = Mathf.Clamp(energy, 0, 100);
            health = Mathf.Clamp(health, 0, 100);
        }

        healthSlider.value = Mathf.Lerp(healthSlider.value, health, sliderFillTime);
        energySlider.value = Mathf.Lerp(energySlider.value, energy, sliderFillTime);

        if (health <= 0)
            _GameManager.instance.gameOver = true;
    }

    private void RegenEnergy ()
    {
        Debug.LogWarning("Regen Energy");
        energy += energyAmount;
    }

    private void RegenHealth ()
    {
        Debug.LogWarning("Regen Health");
        health += healthAmount;
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

            case Pickup.PickupType.Both:
                energy += amount / 2;
                health += amount / 2;
                break;
        }
    }

    public void TakeAmount(float amount)
    {
        Debug.Log("Damage");
        health -= amount;
    }
}
