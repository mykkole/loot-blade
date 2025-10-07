using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class FlashBlink : MonoBehaviour
{
    [SerializeField] private MonoBehaviour damageObject;
    [SerializeField] private Material blinkMaterial;
    [SerializeField] private float blinkDuration = 0.2f;

    private float blinkTimer;
    private Material defaultMaterial;
    private SpriteRenderer spriteRender;
    private bool isBlinking;

    private void Awake()
    {
        spriteRender = GetComponent<SpriteRenderer>();
        defaultMaterial = spriteRender.material;

        isBlinking = true;
    }

    private void Start()
    {
        if (damageObject is Player)
        {
            (damageObject as Player).OnFlashBlink += FlashBlink_OnFlashBlink;
        }
    }

    private void FlashBlink_OnFlashBlink(object sender, EventArgs e)
    {
        SetBlinkingMaterial();
    }

    void Update()
    {
        if (isBlinking)
        {
            blinkTimer -= Time.deltaTime;
            if (blinkTimer < 0)
            {
                SetDefaultMaterial();
            }
        }
    }

    private void SetBlinkingMaterial()
    {
        blinkTimer = blinkDuration;
        spriteRender.material = blinkMaterial;
    }

    private void SetDefaultMaterial()
    {
        spriteRender.material = defaultMaterial;
    }

    public void StopBlinking()
    {
        SetDefaultMaterial();
        isBlinking = false;
    }

    private void OnDestroy()
    {
        if (damageObject is Player)
        {
            (damageObject as Player).OnFlashBlink -= FlashBlink_OnFlashBlink;
        }
    }
}
