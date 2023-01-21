#if  UNITY_EDITOR
using UnityEngine;
[ExecuteInEditMode]
public class ShowPlayerPrefs : MonoBehaviour
{
    public bool display;
    public bool reset;
    public int[] scoreList = new int[5];
    
    void Update()
    {
        if (!display)
        {
            return;
        }

        if (reset)
        {
            PlayerPrefs.SetInt("score 0", 0);
            PlayerPrefs.SetInt("score 1", 0);
            PlayerPrefs.SetInt("score 2", 0);
            PlayerPrefs.SetInt("score 3", 0);
            PlayerPrefs.SetInt("score 4", 0);
            
            reset = false;
        }

        display = false;

        scoreList[0] = PlayerPrefs.GetInt("score 0", 0);
        scoreList[1] = PlayerPrefs.GetInt("score 1", 0);
        scoreList[4] = PlayerPrefs.GetInt("score 2", 0);
        scoreList[2] = PlayerPrefs.GetInt("score 3", 0);
        scoreList[3] = PlayerPrefs.GetInt("score 4", 0);
    }
}
#endif
