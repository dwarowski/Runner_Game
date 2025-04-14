using UnityEngine;
using TMPro; // ����������� ��� TextMeshPro
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    public TextMeshProUGUI distanceText; // ������ �� ��������� ������� ��� ����������
    public Image speedometerNeedle; // ������ �� �������
    public float minNeedleAngle = -130f;
    public float maxNeedleAngle = 130f;
    public float maxSpeed = 200f; // ������������ �������� ���������� � ��/�

    void Start()
    {
        if (speedometerNeedle == null || distanceText == null)
        {
            Debug.LogError("�� ��� ������ �� UI �������� ��������� � UIManager!");
            enabled = false;
            return;
        }
    }

    public void UpdateUI(float speed, float distance)
    {
        float speedNormalized = Mathf.Clamp01(speed / maxSpeed);
        float angle = Mathf.Lerp(minNeedleAngle, maxNeedleAngle, speedNormalized);
        speedometerNeedle.rectTransform.rotation = Quaternion.Euler(0, 0, -angle); // -angle, ����� ������� ��� ������
        distanceText.text = "Distance: " + distance.ToString("F1") + " m";
    }
}
