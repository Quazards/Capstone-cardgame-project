using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectShake : MonoBehaviour
{
    public Transform currentObject;

    public void ShakeObject(float duration, float size)
    {
        StartCoroutine(Shake(duration, size));
    }

    private IEnumerator Shake(float duration, float size)
    {
        float timeElapsed = 0f;
        Vector3 startPosition = currentObject.localPosition;

        while(timeElapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * size;
            float y = Random.Range(-1f, 1f) * size;

            currentObject.localPosition = new Vector3 (x, y, startPosition.z);

            timeElapsed += Time.deltaTime;
            yield return null;
        }

        currentObject.localPosition = startPosition;
    }
}
