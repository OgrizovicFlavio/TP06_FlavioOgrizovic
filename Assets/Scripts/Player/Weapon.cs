using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject bulletPrefab;

    private AudioManager audioManager;
    private float bulletScaleMultiplier = 1f;
    private float bulletDamageMultiplier = 1f;

    private void Start()
    {
        audioManager = FindAnyObjectByType<AudioManager>();
    }
    public void Shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Bullet bulletScript = bullet.GetComponent<Bullet>();
        bullet.transform.localScale *= bulletScaleMultiplier;
        bulletScript.SetDamageMultiplier(bulletDamageMultiplier);
    }

    public void SetBulletScaleMultiplier(float multiplier)
    {
        bulletScaleMultiplier = multiplier;
    }

    public void SetBulletDamageMultiplier(float multiplier)
    {
        bulletDamageMultiplier = multiplier;
    }

    public void PlayShootSound()
    {
        audioManager.Play("PlayerShoot");
    }
}
