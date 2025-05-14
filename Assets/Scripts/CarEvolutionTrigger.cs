using UnityEngine;

public class CarEvolutionTrigger : MonoBehaviour
{
    public GameObject evolutionVFXPrefab;
    public Transform vfxSpawnPoint;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // ����������� � �������� � ������ �����������
            CarEvolutionHandler evolution = other.GetComponentInParent<CarEvolutionHandler>();
            if (evolution != null)
            {
                evolution.EvolveCar(evolutionVFXPrefab, vfxSpawnPoint.position);
                Destroy(gameObject); // ������� ����� ����� �������������
            }
            else
            {
                Debug.LogWarning("CarEvolutionHandler �� ������ � �������� ������� � ����� Player.");
            }
        }
    }
}
