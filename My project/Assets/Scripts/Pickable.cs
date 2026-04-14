using UnityEngine;

public class Pickable : MonoBehaviour
{
    public Rigidbody rb;

    void Awake() 
    {
        rb = GetComponent<Rigidbody>();
    }
}
