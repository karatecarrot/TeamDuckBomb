using UnityEngine.UI;
using UnityEngine;

public class Heart : MonoBehaviour {

    public float health = 1000;

    public GameObject healthBar;
    public Slider healthSlider;

    public Image colourSlider;

    private void Start()
    {
        colourSlider.color = Color.green;
    }

    void Update ()
    {
        healthSlider.value = Mathf.Lerp(healthSlider.value, health, 0.1f);
        healthBar.transform.LookAt(_GameManager.instance.player.transform);

        if (health <= 0)
            _GameManager.instance.gameOver = true;

        if (health <= 700)
            colourSlider.color = Color.Lerp(Color.green, Color.yellow, 1);

        if (health <= 300)
            colourSlider.color = Color.Lerp(Color.yellow, Color.red, 1);
    }

    public void TakeAmount (float amount)
    {
        health -= amount;
    }
}
