using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DamagePopup : MonoBehaviour
{
    public ObjectPooler textPool;

    public void CreatePopup(string text)
    {
        GameObject popUp = textPool.GetPooledObject();
        var popUpText = popUp.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        popUpText.text = text;
        popUp.SetActive(true);
    }
}
