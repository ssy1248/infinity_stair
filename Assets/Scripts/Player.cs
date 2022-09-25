using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Animator animator; //캐릭터가 죽거나 이동관련 애니메이터
    public AudioSource[] sound;
    public GameManager gameManager;
    public bool isleft = true; //캐릭터가 바라보는 방향을 위한 bool값
    public bool isDie = false; //캐릭터가 죽었는지 체크하는 bool값
    public int characterIndex; //캐릭터 추가를 위해 무슨 캐릭터가 선택되었는지 체크를 위한 변수
    public int stairIndex; //계단
    public int money; //코인

    private void Awake()
    {
        animator = gameObject.GetComponent<Animator>();
    }

    public void Climb(bool isChange)
    {
        if (isChange)
            isleft = !isleft;
        gameManager.StairMove(stairIndex, isChange, isleft);

        if ((++stairIndex).Equals(20))
            stairIndex = 0;

        MoveAnimation();
    }

    public void MoveAnimation()
    {
        if (!isleft)
            transform.rotation = Quaternion.Euler(0, -100, 0);
        else
            transform.rotation = Quaternion.Euler(0, 0, 0);

        if (isDie)
            return;

        animator.SetBool("Move", true);
        //gameManager.PlaySound(1);
        Invoke("IdleAnimation", 0.05f);
    }

    public void IdleAnimation()
    {
        animator.SetBool("Move", false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Coin")
        {
            collision.gameObject.SetActive(false);
            //gameManager.PlaySound(0);
            money += 2;
            //dslManager.LoadMoney(money);
        }
    }
}
