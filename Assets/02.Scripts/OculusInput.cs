using UnityEngine;
using UnityEngine.UI;

public class OculusInput : MonoBehaviour
{
    public int speedForward = 12;   //���� �ӵ�
    public int speedSide = 6;   //������ �ӵ�

    public Animator anim;

    float dirX = 0;
    float dirZ = 0;

    bool isGrounded = false;

    private void Awake()
    {
        anim = GetComponentInChildren<Animator>();

    }

    void Update()
    {
        MovePlayer();
        CheckGround();
    }


    void MovePlayer()
    {
        dirX = 0;   //�¿� �̵�����, ���� -1 ������ 1
        dirZ = 0;   //�յ� �̵�����, �� 1, �� -1

        if (OVRInput.Get(OVRInput.Touch.PrimaryThumbstick))
        {
            Vector2 coord = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick);


            anim.SetBool("isMoveAt", true);


            //Right
            if (coord.x > 0)
            {
                dirX = 1;
                anim.SetFloat("MoveAt", 1.0f);
            }
            //Left
            else if (coord.x < 0)
            {
                dirX = -1;
                anim.SetFloat("MoveAt", 0.0f);
            }
            //Forward
            if (coord.y > 0)
            {
                dirZ = 1;
                anim.SetFloat("MoveAt", 2.0f);
            }
            //BackWard
            else if (coord.y < 0)
            {
                dirZ = -1;
                anim.SetFloat("MoveAt", 3.0f);
            }

            Vector3 moveDir = new Vector3(dirX*speedSide, 0, dirZ*speedForward);
            transform.Translate(moveDir * Time.smoothDeltaTime);

        }
        else
        {
            anim.SetBool("isMoveAt", false);

        }


        //Jump
        //X��ư
        if (OVRInput.GetDown(OVRInput.Button.Three) && isGrounded)
        {
            this.GetComponent<Rigidbody>().AddForce(0f, 10f, 0f, ForceMode.Impulse);
            isGrounded = false;
            anim.SetBool("isJump", true);

        }
        if (OVRInput.GetUp(OVRInput.Button.Three))
        {
            anim.SetBool("isJump", false);
        }

    }

    private void CheckGround()
    {
        Vector3 origin = new Vector3(transform.position.x, transform.position.y - (transform.localScale.y * .5f), transform.position.z);
        Vector3 direction = transform.TransformDirection(Vector3.down);
        float distance = .8f;

        if (Physics.Raycast(origin, direction, out RaycastHit hit, distance))
        {
            Debug.DrawRay(origin, direction * distance, Color.red);
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }
    }
}