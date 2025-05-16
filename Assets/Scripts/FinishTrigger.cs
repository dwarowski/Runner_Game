using UnityEngine;

public class FinishLineTrigger : MonoBehaviour
{
    public VictoryMenuHandler victoryMenuHandler;

    private bool triggered = false;

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
                Debug.LogWarning("VictoryMenuHandler не назначен на финишном тайле.");
            }
        }
    }
}

