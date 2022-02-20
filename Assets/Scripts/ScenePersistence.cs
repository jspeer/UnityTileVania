using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScenePersistence : MonoBehaviour
{
    void Awake()
    {
        // We only ever want one scene persistence
        int numOfScenePersistences = FindObjectsOfType<ScenePersistence>().Length;
        if (numOfScenePersistences > 1) {
            Destroy(gameObject);
        } else {
            DontDestroyOnLoad(gameObject);
        }
    }

    public void ResetScenePersistence()
    {
        Destroy(gameObject);
    }
}
