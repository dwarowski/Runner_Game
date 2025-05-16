﻿using TMPro; // Обязательно для TextMeshPro
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    public TextMeshProUGUI distanceText; // Ссылка на текстовый элемент для расстояния
    public Image speedometerNeedle; // Ссылка на стрелку
    public Image healthBarFill; // Назначь в инспекторе Image → Fill
    public float minNeedleAngle = -130f;
    public float maxNeedleAngle = 130f;
    public float maxSpeed = 200f; // Максимальная скорость спидометра в км/ч


    void Start()
    {
        if (speedometerNeedle == null || distanceText == null)
        {
            Debug.LogError("Не все ссылки на UI элементы назначены в UIManager!");
            enabled = false;
            return;
        }
    }

    public void UpdateSpeed(float speed, float distance)
    {
        float speedNormalized = Mathf.Clamp01(speed / maxSpeed);
        float angle = Mathf.Lerp(minNeedleAngle, maxNeedleAngle, speedNormalized);
        speedometerNeedle.rectTransform.rotation = Quaternion.Euler(0, 0, -angle); // -angle, чтобы стрелка шла вправо
        distanceText.text = "Distance: " + distance.ToString("F1") + " m";
    }

    public void UpdateHealthBar(float hpPercent)
    {
        if (healthBarFill != null)
        {
            healthBarFill.fillAmount = Mathf.Clamp01(hpPercent); // от 0 до 1
        }
    }
}
