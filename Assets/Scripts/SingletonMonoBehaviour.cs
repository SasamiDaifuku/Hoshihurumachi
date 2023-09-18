using UnityEngine;

public class SingletonMonoBehaviour<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _instance;
    public static T Instance {
        get {
            if (_instance == null) {
                _instance = (T)FindObjectOfType(typeof(T));
 
                if (_instance == null) {
                    Debug.LogError (typeof(T) + "is nothing");
                }
            }
 
            return _instance;
        }
    }

}