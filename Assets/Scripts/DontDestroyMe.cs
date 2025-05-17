using UnityEngine;

public class DontDestroyMe : MonoBehaviour
{
    // Static reference to the instance to enforce singleton pattern
    private static DontDestroyMe instance;

    void Awake()
    {
        // If an instance already exists and it's not this one
        if (instance != null && instance != this)
        {
            // Destroy this instance to maintain singleton pattern
            Destroy(gameObject);
            return;
        }

        // This is the first instance - make it the singleton
        instance = this;
        
        // This makes the GameObject persist across scene loads
        DontDestroyOnLoad(gameObject);
    }
    
    void OnDestroy()
    {
        if(instance == this)
        {
            instance = null;
        }
    }
}
