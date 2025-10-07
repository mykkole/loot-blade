using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class PlayerVisual : MonoBehaviour
{
    private Animator animator;

    private const string IS_RUNNING = "IsRunning";
    private const string IS_DIE = "IsDie";

    private SpriteRenderer spriteRenderer;
    private FlashBlink flashBlink;

    private Rigidbody2D player;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        flashBlink = GetComponent<FlashBlink>();
    }

    private void Start()
    {
        Player.Instance.OnPlayerDeath += Instance_OnPlayerDeath;
    }

    private void Instance_OnPlayerDeath(object sender, System.EventArgs e)
    {
        animator.SetBool(IS_DIE, true);
        flashBlink.StopBlinking();
    }

    private void Update()
    {
        animator.SetBool(IS_RUNNING, Player.Instance.IsRunning());
        
        if (Player.Instance.IsAlive())
        {
            AdjustPlayerFacingDirection();
        }
    }

    private void OnDestroy()
    {
        Player.Instance.OnPlayerDeath -= Instance_OnPlayerDeath;
    }

    private void AdjustPlayerFacingDirection()
    {
        float horizontalInput = Input.GetAxis("Horizontal");

        if (Mathf.Abs(horizontalInput) > 0.1f)
        {
            Player.Instance.IsFacingRight = horizontalInput > 0;
            spriteRenderer.flipX = horizontalInput < 0;
        }

        //Vector3 mousePos = GameInput.Instance.GetMousePosition();
        //Vector3 playerPos = Player.Instance.GetPlayerScreenPosition();

        //if (mousePos.x < playerPos.x)
        //{
        //    spriteRenderer.flipX = true;
        //}
        //else
        //{
        //    spriteRenderer.flipX = false;
        //}
    }
}
