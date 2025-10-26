
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{   
    public UnityEngine.UI.Image heart3;
    public UnityEngine.UI.Image heart2;
    public UnityEngine.UI.Image heart1;
    public Sprite binHeart;
    public float speed;
    public Rigidbody rigid;
    public int heart;
    public int count;
    public GameManager GM;


    float x;
    float z;

    void Start()
    {
         rigid = GetComponent<Rigidbody>();
         count = 0;
    }

    // Update is called once per frame
    void Update()
    {
        

        x = Random.Range(-8.5f,8.5f);
        z = Random.Range(-8.5f,8.5f);
    }

    

    private void OnTriggerEnter(Collider other){
        if(other.tag=="coin"){
            FindObjectOfType<GameManager>().CountUp(1);
            count++;
            other.gameObject.transform.position=new Vector3(x,-1.5f,z);
            GM.CountUp(count);
        }
        // if(other.tag=="Bullet"){
        //     other.gameObject.SetActive(false);
        //     if(heart==3){
        //         heart3.sprite= binHeart;
        //     }
        //     else if(heart==2){
        //         heart2.sprite= binHeart;
        //     }
        //     else if(heart==1){
        //         heart1.sprite= binHeart;
        //     }
        //     heart--;
        // }
    }

    public int getCount(){
        return count;
    }

}
