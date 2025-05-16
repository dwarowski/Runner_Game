using UnityEngine;

public class FinishLineTrigger : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameHandler.Instance.ShowVictory();
        }
    }
}
