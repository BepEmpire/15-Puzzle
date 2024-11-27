using UnityEngine;

public class DontDestroyManagers : MonoBehaviour
{
    private static DontDestroyManagers _instance;
    
    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
        }
            
        DontDestroyOnLoad(gameObject);
    }
}