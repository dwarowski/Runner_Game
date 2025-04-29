using UnityEngine;

public class BuffModuleController : MonoBehaviour
{
    public GameObject leftBuffObject;  // 👈 Один из баффов
    public GameObject rightBuffObject; // 👈 Второй бафф

    private bool hasChosen = false;

    public void ChooseBuff()
    {
        if (hasChosen) return;

        hasChosen = true;

        if (leftBuffObject != null)
            Destroy(leftBuffObject);

        if (rightBuffObject != null)
            Destroy(rightBuffObject);
    }
}
