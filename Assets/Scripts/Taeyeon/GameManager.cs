
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{   
    public TMP_Text totalScoreText;
    public static int count=0;
    public int stage;

    // Start is called before the first frame update
    void Awake()
    {
        totalScoreText.text = count.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.R)){
            count=0;
            stage=0;
            SceneManager.LoadScene(stage);
        }
        if(Input.GetKeyDown(KeyCode.Space)){
            stage=1;
            SceneManager.LoadScene(stage);
        }
    }

    public void CountUp(int count){
        totalScoreText.text = count.ToString();
    }
}
