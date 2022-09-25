using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Animator animator; //ĳ���Ͱ� �װų� �̵����� �ִϸ�����
    public AudioSource[] sound;
    public GameManager gameManager;
    public bool isleft = true; //ĳ���Ͱ� �ٶ󺸴� ������ ���� bool��
    public bool isDie = false; //ĳ���Ͱ� �׾����� üũ�ϴ� bool��
    public int characterIndex; //ĳ���� �߰��� ���� ���� ĳ���Ͱ� ���õǾ����� üũ�� ���� ����
    public int stairIndex; //���
    public int money; //����

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
