using UnityEngine;

public class Stove : MonoBehaviour
{
    public KnobAndStove knob;
    public bool isFaulty = false;
    public bool isChecked = false;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Match"))
        {
            knob.SetMatchInside(true);
        }
        if(other.CompareTag("GasDetector"))
        {
            isChecked = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Match"))
        {
            knob.SetMatchInside(false);
        }
    }

    /*public bool isEnabled()
    {
        return knob.isEnabled;
    }
    */
}
