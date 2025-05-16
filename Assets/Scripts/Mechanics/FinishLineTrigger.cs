using UnityEngine;

public class FinishLineTrigger : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            CarControl carControl = other.GetComponentInParent<CarControl>();
            if (carControl.GetLife() == false)
            {
                GameHandler.Instance.ShowVictory();
            }
            
        }
    }
}
