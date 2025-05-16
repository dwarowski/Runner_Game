using UnityEngine;

public class FinishLineTrigger : MonoBehaviour
{
    private bool triggered = false;
    private VictoryMenuHandler victoryMenuHandler;

    private void Start()
    {
        victoryMenuHandler = FindFirstObjectByType<VictoryMenuHandler>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (triggered) return;

        if (other.CompareTag("Player"))
        {
            triggered = true;
            if (victoryMenuHandler != null)
            {
                victoryMenuHandler.ShowVictory();
            }
            else
            {
                Debug.LogWarning("VictoryMenuHandler не найден на сцене.");
            }
        }
    }
}
