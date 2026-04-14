using UnityEngine;

public class KnobAndStove : MonoBehaviour
{
    [Header("Knob Rotation")]
    public bool isEnabled = false;
    public float enabledAngle = -90f;
    public float speed = 5f;

    private Quaternion offRot;
    private Quaternion onRot;

    [Header("Effects")]
    public ParticleSystem StoveFire;
    public Light ovenLight;
    public bool controlsOvenLight = false;

    private bool matchInside = false;

    void Start()
    {
        offRot = transform.rotation;
        onRot = Quaternion.Euler(transform.eulerAngles + new Vector3(0, 0, enabledAngle));
        if (StoveFire != null)
        {
            StoveFire.Stop();
        }
    }

    public void ToggleKnob()
    {
        isEnabled = !isEnabled;
    }

    void Update()
    {
        Quaternion target = isEnabled ? onRot : offRot;
        transform.rotation = Quaternion.Slerp(transform.rotation, target, Time.deltaTime * speed);

        if (isEnabled == false && StoveFire != null)
        {
            StoveFire.Stop();
        }

        if (controlsOvenLight && ovenLight != null)
        {
            ovenLight.enabled = isEnabled;
        }

        if (isEnabled && StoveFire != null && !StoveFire.isPlaying && matchInside)
        {
            StoveFire.Play();
        }
    }
    public void SetMatchInside(bool value)
    {
        matchInside = value;
    }

}
