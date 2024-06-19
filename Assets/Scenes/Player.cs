using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Callbacks;
using UnityEngine;

public class Player : MonoBehaviour
{
    float horizontalInput; // 수평 입력 값을 저장할 변수
    float movespeed = 5f; // 이동 속도를 설정하는 변수
    bool isFacingRight = false; // 플레이어가 오른쪽을 보고 있는지 여부를 나타내는 변수
    float jumpPower = 5f; // 점프 힘을 설정하는 변수
    bool isGrounded = false; // 플레이어가 땅에 닿았는지 여부를 나타내는 변수
    bool isFalling = false; // 플레이어가 떨어지고 있는지 여부를 나타내는 변수
    Rigidbody2D rb; // 플레이어의 Rigidbody2D 컴포넌트를 참조하기 위한 변수
    Animator animator; // 플레이어의 Animator 컴포넌트를 참조하기 위한 변수
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); // Rigidbody2D 컴포넌트를 가져옵니다.
        animator = GetComponent<Animator>(); // Animator 컴포넌트를 가져옵니다.
    }

    // Update is called once per frame
    void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal"); // 수평 입력 값을 업데이트합니다.

        FlipSprite(); // 스프라이트를 뒤집는 함수 호출horizontalInput = Input.GetAxis("Horizontal");


        if(Input.GetButtonDown("Jump") && isGrounded)               // 점프 버튼이 눌리고 플레이어가 땅에 있으면
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpPower);    // y축으로 점프합니다.
            isGrounded = false;                                     // 플레이어가 땅에 닿아있지 않음을 표시합니다.
            animator.SetBool("isJumping", !isGrounded);             // 애니메이터의 점프 상태를 업데이트합니다.
        }

        if (!isGrounded && rb.velocity.y < 0)                       // 플레이어가 땅에 닿지 않고 떨어지고 있으면
        {
            isFalling = true;                                       // 플레이어가 떨어지고 있음을 표시합니다.
            animator.SetBool("isFalling", true);                    // 애니메이터의 낙하 상태를 업데이트합니다.
        }
        else
        {
            isFalling = false;                                      // 플레이어가 떨어지고 있지 않음을 표시합니다.
            animator.SetBool("isFalling", false);                   // 애니메이터의 낙하 상태를 업데이트합니다.
        }
    }
    private void FixedUpdate()
    {
        rb.velocity = new Vector2(horizontalInput * movespeed, rb.velocity.y);      // 수평 이동을 적용합니다.
        animator.SetFloat("xVelocity", Math.Abs(rb.velocity.x));                    // 애니메이터에 x축 속도를 전달합니다.
        animator.SetFloat("yVelocity", rb.velocity.y);                              // 애니메이터에 y축 속도를 전달합니다.
    }

    void FlipSprite()
    {
        if(isFacingRight && horizontalInput < 0f || !isFacingRight && horizontalInput > 0f)         // 현재 방향과 입력 방향이 반대인 경우
        {
            isFacingRight = !isFacingRight;                                                         // 플레이어의 방향을 반전시킵니다.
            Vector3 ls = transform.localScale;                                                      // 현재 스케일을 가져옵니다.
            ls.x *= -1f;                                                                            // x축 스케일을 반전시킵니다.
            transform.localScale = ls;                                                              // 반전된 스케일을 적용합니다.
        }
    }
     // 다른 Collider2D와 충돌할 때 호출되는 함수
    private void OnTriggerEnter2D(Collider2D collision)
    {
        isGrounded = true;                                                             // 플레이어가 땅에 닿았음을 표시합니다.
        animator.SetBool("isJumping", !isGrounded);                                    // 애니메이터의 점프 상태를 업데이트합니다.
    }
}
