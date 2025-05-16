using UnityEngine;

public class CarEvolutionTrigger : MonoBehaviour
{
    public GameObject evolutionVFXPrefab;
    public Transform vfxSpawnPoint;
    private Transform vfxParent;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // ����������� � �������� � ������ �����������
            CarEvolutionHandler evolution = other.GetComponentInParent<CarEvolutionHandler>();
            CarControl carControl = other.GetComponentInParent<CarControl>();
            vfxParent = transform.parent;
            if (carControl.GetLife() == false)
            {
                if (evolution != null)
                {
                    evolution.EvolveCar(evolutionVFXPrefab, vfxSpawnPoint.position, vfxParent);
                    Destroy(gameObject); // ������� ����� ����� �������������
                }
                else
                {
                    Debug.LogWarning("CarEvolutionHandler �� ������ � �������� ������� � ����� Player.");
                }

            }
        }
    }
}
