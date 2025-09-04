using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement1 : MonoBehaviour
{
    public float speed;
    public float rotationSpeed;
    public Animator playerAnimator;
    private Rigidbody rigid;

    // Start is called before the first frame update
    void Start()
    {
        rigid = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        rigid.AddForce(new Vector3(h, 0, v) * speed, ForceMode.Impulse);
        if (h != 0 || v != 0){
            playerAnimator.SetBool("isMoving",true);
        }else{
            playerAnimator.SetBool("isMoving",false);
        }

        Vector3 direction = new Vector3(h, 0f, v).normalized;

        if (direction != Vector3.zero){
            Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
        }
    }
}

