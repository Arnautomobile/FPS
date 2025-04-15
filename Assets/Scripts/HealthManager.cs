using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class HealthManager : MonoBehaviour, IDamageable
{
    [SerializeField] private TextMeshProUGUI _healthText;
    [SerializeField] private GameObject _bloodImage;
    [SerializeField] private float _fadeDuration;
    [SerializeField] private float _startHealth;
    
    public bool TakeDamage;

    private float _currentHealth;

    void Start()
    {
        _currentHealth = _startHealth;
        _healthText.text = "" + _currentHealth;
    }

    void Update()
    {
        if (TakeDamage) {
            TakeDamage = false;
            Damage(1);
        }
    }


    public void Damage(int amount)
    {
        if (_currentHealth <= 0) return;
        
        _currentHealth -= amount;
        _healthText.text = "" + _currentHealth;

        if (_currentHealth < 0) {
            _currentHealth = 0;
            _healthText.text = "0";
            _bloodImage.SetActive(true);
            Destroy(gameObject);
        }
        else {
            StopCoroutine(FadeOut());
            StartCoroutine(FadeOut());
        }
    }

    public void Heal(int amount)
    {
        _currentHealth += amount;
    }



    private IEnumerator FadeOut()
    {
        if (_bloodImage.activeInHierarchy) {
            _bloodImage.SetActive(true);
        }
        Image fadeImage = _bloodImage.GetComponent<Image>();

        float timer = 0f;
        Color startColor = fadeImage.color;
        Color endColor = new Color(0f, 0f, 0f, 1f);
 
        while (timer < _fadeDuration)
        {
            fadeImage.color = Color.Lerp(startColor, endColor, timer / _fadeDuration);
            timer += Time.deltaTime;
            yield return null;
        }
        
        fadeImage.color = startColor;
        if (_bloodImage.activeInHierarchy) {
            _bloodImage.SetActive(false);
        }
    }
}
