using UnityEngine;
using TMPro; // ����������� ��� TextMeshPro

public class UI : MonoBehaviour
{
    public TextMeshProUGUI speedText;   // ������ �� ��������� ������� ��� ��������
    public TextMeshProUGUI distanceText; // ������ �� ��������� ������� ��� ����������

    void Start()
    {
        if (speedText == null || distanceText == null)
        {
            Debug.LogError("�� ��� ������ �� UI �������� ��������� � UIManager!");
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
