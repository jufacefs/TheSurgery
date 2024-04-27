using UnityEngine;

public class ValueManager : MonoBehaviour
{
    // Make the currentValue field public or provide a public property to access it
    public int currentValue { get; private set; }

    // Singleton pattern
    private static ValueManager instance;
    public static ValueManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<ValueManager>();
                if (instance == null)
                {
                    GameObject obj = new GameObject();
                    obj.name = typeof(ValueManager).Name;
                    instance = obj.AddComponent<ValueManager>();
                }
            }
            return instance;
        }
    }

    public void ResetValue()
    {
        currentValue = 1;
    }

    public void IncrementValue()
    {
        currentValue++;
    }

    // Add other methods or logic as needed
}
