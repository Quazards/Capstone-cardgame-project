using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueController : MonoBehaviour
{
    [SerializeField] private int indexNumber;
    private bool isOnCooldown = false;
    public float cooldownTime = 2f;

    private void OnMouseEnter()
    {
        if (!isOnCooldown && (indexNumber == 0 || indexNumber == 1))
        {
            OHMYGODPLEASEWORK();
            StartCoroutine(CooldownRoutine());
        }
    }

    public void OHMYGODPLEASEWORK()
    {
        Dialogue.instance.index = indexNumber;
        // Dialogue.instance.textComponent.text = string.Empty;
        Dialogue.instance.StartCoroutine(Dialogue.instance.TypeLine());
    }

    private IEnumerator CooldownRoutine()
    {
        isOnCooldown = true;
        yield return new WaitForSeconds(cooldownTime);
        isOnCooldown = false;
    }
}
