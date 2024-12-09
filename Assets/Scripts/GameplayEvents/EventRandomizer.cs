using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventRandomizer : MonoBehaviour
{
public List<GameObject> eventsList;

public GameObject GetRandomGameObject()
    {
        if (eventsList == null || eventsList.Count == 0)
        {
            return null;
        }

        int randomIndex = Random.Range(0, eventsList.Count);
        return eventsList[randomIndex];
    }

    public void eventRandomized()
    {
        GameObject chosenObject = GetRandomGameObject();
        if (chosenObject != null)
        {
            chosenObject.SetActive(true);
        }
    }
}
