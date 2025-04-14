using UnityEngine;
using TMPro; // Обязательно для TextMeshPro

public class UI : MonoBehaviour
{
    public TextMeshProUGUI speedText;   // Ссылка на текстовый элемент для скорости
    public TextMeshProUGUI distanceText; // Ссылка на текстовый элемент для расстояния

    void Start()
    {
        if (speedText == null || distanceText == null)
        {
            Debug.LogError("Не все ссылки на UI элементы назначены в UIManager!");
            enabled = false;
            return;
        }
    }

    public void UpdateUI(float speed, float distance)
    {
        speedText.text = "Speed: " + speed.ToString("F1") + " km/h";
        distanceText.text = "Distance: " + distance.ToString("F1") + " m";
    }
}
