using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DamagePopupAnimation : MonoBehaviour
{
    [SerializeField] private AnimationCurve opacityCurve;
    [SerializeField] private AnimationCurve scaleCurve;
    [SerializeField] private AnimationCurve heightCurve;

    private TextMeshProUGUI text;
    private float time;
    private Vector3 initialPosition;

    private void Awake()
    {
        text = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        initialPosition = transform.position;
    }

    private void Update()
    {
        text.color = new Color(1, 1, 1, opacityCurve.Evaluate(time));
        transform.localScale = Vector3.one * scaleCurve.Evaluate(time);
        transform.position = initialPosition + new Vector3(0, 1 + heightCurve.Evaluate(time), 0);
        time += Time.deltaTime;
    }
}
