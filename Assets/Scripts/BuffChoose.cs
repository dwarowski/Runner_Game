using UnityEngine;

public class BuffChoose : MonoBehaviour
{
    public float speedModifier = 0f; // ����������� �������� (������������� ��� ������������� ��������)
    public float powerModifier = 0f;  // ����������� �������� (������������� ��� ������������� ��������)
    public string buffText = "";        // ����� ����� ��� �����������
    private void OnTriggerEnter(Collider other)
    {
        // ���������, ��� ������� �������� � �������
        if (other.CompareTag("Player")) // ���������, ��� � ������ ������ ���������� ��� "Player"
        {
            // �������� ��������� PlayerController (��� ��� �� � ���� ����������)
            CarControl carControl = other.GetComponentInParent<CarControl>();

            if (carControl != null)
            {
                // ��������� ����/������ � ������
                carControl.ApplyBuff( powerModifier, speedModifier, buffText);
            }
            else
            {
                Debug.LogError("����� �� ����� ���������� PlayerController!");
            }
        }
    }
}
