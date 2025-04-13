using UnityEngine;

public class BuffChoose : MonoBehaviour
{
    public float speedModifier = 0f; // Модификатор скорости (положительное или отрицательное значение)
    public float powerModifier = 0f;  // Модификатор мощности (положительное или отрицательное значение)
    public string buffText = "";        // Текст баффа для отображения
    private void OnTriggerEnter(Collider other)
    {
        // Проверяем, что триггер сработал с игроком
        if (other.CompareTag("Player")) // Убедитесь, что у вашего игрока установлен тег "Player"
        {
            // Получаем компонент PlayerController (или как он у тебя называется)
            CarControl carControl = other.GetComponentInParent<CarControl>();

            if (carControl != null)
            {
                // Применяем бафф/дебафф к игроку
                carControl.ApplyBuff( powerModifier, speedModifier, buffText);
            }
            else
            {
                Debug.LogError("Игрок не имеет компонента PlayerController!");
            }
        }
    }
}
