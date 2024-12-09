using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Dialogue : MonoBehaviour
{
    public static Dialogue instance;
    public TextMeshProUGUI textComponent;
    private bool isTyping = false;
    public string[] lines;
    public float textSpeed;

    public int index = 1;
    // private int lastIndex = -1;

    public void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }
    }

    public void DisplayText()
    {
        textComponent.text = string.Empty;
        StartDialogue();
    }

    // void Update()
    // {
    //     if (Input.GetMouseButtonDown(0))
    //     {
    //         if (textComponent.text == lines[index])
    //         {
    //             NextLine();
    //         }
    //         else
    //         {
    //             StopAllCoroutines();
    //             textComponent.text = lines[index];
    //         }
    //     }
    // }

    void StartDialogue()
    {
        // index = GetRandomLineIndex();
        StartCoroutine(TypeLine());
    }

    public IEnumerator TypeLine()
    {
        if (isTyping)
        {
            yield break;
        }
        isTyping = true;
        textComponent.text = string.Empty;
        foreach (char c in lines[index].ToCharArray())
        {
            textComponent.text += c;
            yield return new WaitForSeconds(textSpeed);
        }
        isTyping = false;
    }

    // public void NextLine()
    // {
    //     if (textComponent.text == lines[index])
    //     {
    //        index = GetRandomLineIndex();

    //         textComponent.text = string.Empty;
    //         StartCoroutine(TypeLine());
    //     }
    //     else
    //     {
    //         StopAllCoroutines();
    //         textComponent.text = lines[index];
    //     }
    // }

    // int GetRandomLineIndex()
    // {
    //     int randomIndex;

    //     do
    //     {
    //         randomIndex = Random.Range(0, lines.Length);
    //     } while (randomIndex == lastIndex);

    //     lastIndex = randomIndex;
    //     return randomIndex;
    // }
}
