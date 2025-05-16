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
            CarControl carControl = other.GetComponentInParent<CarControl>();
            vfxParent = transform.parent;
            if (carControl.GetLife() == false)
            {
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
}
