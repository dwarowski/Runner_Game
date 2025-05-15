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
            // Поднимаемся к родителю с нужным компонентом
            CarEvolutionHandler evolution = other.GetComponentInParent<CarEvolutionHandler>();
            vfxParent = gameObject.GetComponentInParent<Transform>();
            Debug.Log(vfxParent);
            if (evolution != null)
            {
                evolution.EvolveCar(evolutionVFXPrefab, vfxSpawnPoint.position, vfxParent);
                Destroy(gameObject); // удаляем гараж после использования
            }
            else
            {
                Debug.LogWarning("CarEvolutionHandler не найден у родителя объекта с тегом Player.");
            }
        }
    }
}
