using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{

    [SerializeField] private float maxHP = 100;
    [field:SerializeField] public int AmmoCapacity { get; private set; } = 15;
    [SerializeField] private float hitstun = 2;
    [SerializeField] private TextMeshProUGUI ammoText;
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private TextMeshProUGUI medkitsText;
    [SerializeField] private AudioClip playerHurt;

    public float Medkits { get; private set; } = 0;
    public int Ammo { get; private set; } = 0;
    public float currentHP { get; private set; }
    private ThirdPersonShooterController tpsController;
    private ThirdPersonController tpController;
    private StarterAssetsInputs inputs;

    private bool healPressed = false;

    private void Awake()
    {
        tpsController = GetComponent<ThirdPersonShooterController>();
        tpController = GetComponent<ThirdPersonController>();
        currentHP = maxHP;
        inputs = GetComponent<StarterAssetsInputs>();
    }

    private void Update()
    {
        
        ammoText.text = $"Ammo: {tpsController.LoadedAmmo} | {Ammo}";
        healthText.text = $"Health: {currentHP}";
        medkitsText.text = $"Medkits: {Medkits}";
        if (inputs.heal)
        {
            if (!healPressed && Medkits > 0) {
                healPressed = true;
                Heal(100);
                Medkits--;
                Debug.Log("Healing");
            }
        } else
        {
            healPressed = false;
        }
    }

    public void Damage(float damage)
    {
        // Hit animation
        tpsController.InterruptAimFocus();
        tpsController.InterruptReload();
        StartCoroutine(Hitstun(hitstun));
        AudioSource.PlayClipAtPoint(playerHurt, transform.position);
        currentHP -= damage;
        if (currentHP <= 0)
        {
            // Death animation
            tpController.SetCanMove(false);
            GameOver();
        }
    }

    private IEnumerator Hitstun(float time)
    {
        tpController.SetCanMove(false);
        tpsController.CanAim = false;
        yield return new WaitForSeconds(time);
        tpController.SetCanMove(true);
        tpsController.CanAim = true;
    }

    public void Heal(float quantity)
    {
        currentHP += quantity;
        if (currentHP > maxHP)
        {
            currentHP = maxHP;
        }
    }

    public void GameOver()
    {

    }

    public void Victory()
    {
        Debug.Log("You Escaped!");
    }

    public int ReloadAmmo(int quantity)
    {
        int usedAmmo = AmmoCapacity - quantity;
        if (Ammo >= usedAmmo)
        {
            Ammo-=usedAmmo;
            return AmmoCapacity;
        } else
        {
            int newClip = Ammo;
            Ammo = 0;
            return newClip;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Ammo"))
        {
            Ammo += other.gameObject.GetComponent<ItemHandler>().Quantity;
            GameObject.Destroy(other.gameObject);
        }
        if (other.gameObject.CompareTag("Medkit"))
        {
            Medkits += other.gameObject.GetComponent<ItemHandler>().Quantity;
            GameObject.Destroy(other.gameObject);
        }
        if (other.gameObject.CompareTag("Goal"))
        {
            tpsController.CanAim = false;
            tpController.SetCanMove(false);
            Victory();
        }
    }
}
