using UnityEngine;

public class GasDetector : MonoBehaviour
{
    public Material redMaterial;
    public Material greenMaterial;
    public Renderer rend;
    public AudioSource alarmSound;

    private KnobAndStove currentStove;
    private bool faulty;
    private bool danger = false;
    


    void Start()
    {
        rend.material = greenMaterial;
        alarmSound.Stop();
    }

    void Update()
    {
        if (currentStove != null)
        {
             
            if (currentStove.isEnabled && (currentStove.StoveFire == null || !currentStove.StoveFire.isPlaying) || faulty == true)
            {
                danger = true;
                Debug.Log("Gas leak detected!");
                if (!alarmSound.isPlaying)
                {
                    alarmSound.Play();
                }
            }
            else
            {
                danger = false;
                if(alarmSound.isPlaying)
                {
                    alarmSound.Stop();
                    alarmSound.time = 0f;
                }
            }
            rend.material = danger ? redMaterial : greenMaterial;
        }
        else
        {
            rend.material = greenMaterial;
            alarmSound.Stop();
            alarmSound.time = 0f;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Stove"))
        {
            Debug.Log("Stove collider hit: " + other.gameObject.name);

            currentStove = other.GetComponent<Stove>().knob;
            faulty = other.GetComponent<Stove>().isFaulty;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Stove"))
        {
            currentStove = null;
            faulty = false;
        }
    }
}
