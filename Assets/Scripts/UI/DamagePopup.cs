using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DamagePopup : MonoBehaviour
{
    [SerializeField] private GameObject popUpPrefab;
    [SerializeField] private Canvas canvas;
    public Vector3 testPosition;
    public GameObject testObject;

    private void Start()
    {
        testObject = GameObject.FindGameObjectWithTag("PlayerUIComponent");
        testPosition = testObject.transform.localPosition;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            CreatePopup(testPosition, Random.Range(0, 100).ToString());
        }
        
    }

    public void CreatePopup(Vector3 position, string text)
    {

        var popUp = Instantiate(popUpPrefab, transform.parent);
        popUp.GetComponent<RectTransform>().localPosition = position;
        var popUpText = popUp.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        popUpText.text = text;

        Debug.Log($"Object's position: {position}");

        Destroy(popUp, 10f);
    }
}
