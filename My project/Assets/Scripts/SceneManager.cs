using UnityEngine;

public class SceneManager : MonoBehaviour
{
    public Stove[] stoves;
    public GameObject missionCompleteText;

    int checkedCount = 0;

    void Update()
    {
        checkedCount = 0;
        foreach (Stove s in stoves)
        {
            if(s.isChecked)
            {
                checkedCount++;
            }
        }
            

        if (checkedCount == 4)
        {
            missionCompleteText.SetActive(true);
        }
    }
}
