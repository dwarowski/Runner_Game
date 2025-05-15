using TMPro;
using UnityEngine;

public class BuffChoose : MonoBehaviour
{
    [System.Serializable]
    public class BuffOption
    {
        public float speedModifier;
        public float powerModifier;
        public string buffText;
    }

    public BuffOption[] possibleBuffs;
    public TextMeshPro upgradeText; 
    private float speedModifier = 0f;
    private float powerModifier = 0f;

    private bool initialized = false;
    private bool alreadyTriggered = false;

    private BuffModuleController parentModule;

    void Start()
    {
        parentModule = GetComponentInParent<BuffModuleController>();

        if (possibleBuffs != null && possibleBuffs.Length > 0)
        {
            int index = Random.Range(0, possibleBuffs.Length);
            BuffOption chosen = possibleBuffs[index];

            speedModifier = chosen.speedModifier;
            powerModifier = chosen.powerModifier;
            upgradeText.text = chosen.buffText;

            initialized = true;
        }
        else
        {
            Debug.LogWarning("Нет доступных баффов в BuffChoose!");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!initialized || alreadyTriggered) return;

        if (other.CompareTag("Player"))
        {
            CarControl carControl = other.GetComponentInParent<CarControl>();

            if (carControl != null)
            {
                alreadyTriggered = true;

                carControl.ApplyBuff(powerModifier, speedModifier);

                if (parentModule != null)
                {
                    parentModule.ChooseBuff(); // удалит весь модуль
                }
                else
                {
                    Destroy(gameObject); // fallback
                }
            }
            else
            {
                Debug.LogError("Игрок не имеет компонента CarControl!");
            }
        }
    }
}
