using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Events;

public class PlayerChargeShot : MonoBehaviour
{
    [Serializable]
    public class LightEffect
    {
        public string name = "Light";

        public void Start()
        {
            if (InitialiseMaxIntensity && light != null)
                maxIntensity = light.intensity;
        }

        public Light light = null;
        public float minIntensity = 0f;
        public float maxIntensity = 0f;
        public AnimationCurve intensityCurve
            = AnimationCurve.Linear(0f, 0f, 1f, 1f);

        public bool InitialiseMaxIntensity = true;

        public float GetIntensity(float percent)
            => Mathf.Lerp(minIntensity, maxIntensity, intensityCurve.Evaluate(percent));
    }

    [Serializable]
    public class ChargeEvent
    {
        public string name = "Event";

        public ChargeEvent()
        {
            history = Trigger.Below;
        }

        public Trigger trigger = Trigger.Above;
        public float Threshold = 0f;
        public enum Trigger
        {
            Above = 1,
            Both = 0,
            Below = -1
        }

        public UnityEvent Event = new();

        [HideInInspector]
        public Trigger history = Trigger.Below;

        public bool UpdateEvent(float chargeProgress)
        {
            // Above
            if ((trigger == Trigger.Above || trigger == Trigger.Both)
                && history == Trigger.Below
                && chargeProgress >= Threshold)
            {
                history = Trigger.Above;  // Update history
                return Invoke();
            }

            // Below
            if ((trigger == Trigger.Below || trigger == Trigger.Both)
                && history == Trigger.Above
                && chargeProgress <= Threshold)
            {
                history = Trigger.Below;  // Update history
                return Invoke();
            }

            // If no event was invoked, update history based on current chargeProgress
            history = chargeProgress > Threshold ? Trigger.Above : Trigger.Below;

            return false;
        }

        private bool Invoke()
        {
            //Debug.Log("Invoked");
            Event.Invoke();
            return true;
        }
    }

    [Header("Config")]
    private Animator animator = null;
    [SerializeField] private SpriteRenderer bodyRenderer = null;
    [SerializeField] private Material openMat = null;
    [SerializeField] private Material closeMat = null;

    [SerializeField] private float chargeTime = 1.5f;
    [SerializeField] private float passiveCooldownTime = .33f;
    [SerializeField] private float activeCooldownTime = 2.5f;

    [SerializeField]
    private AnimationCurve chargeCurve
        = AnimationCurve.Linear(0f, 0f, 1f, 1f);

    [SerializeField] private List<LightEffect> ChargeLights = new();
    [SerializeField] private List<ChargeEvent> ChargeEvents = new();


    [Header("Data")]
    [SerializeField] private bool isCharged = false;
    [SerializeField] private bool isActiveCoolingDown = false;
    [Range(0f, 1f)]
    [SerializeField] private float chargePercent = 0f;

    [Range(0f, 1f)]
    [SerializeField] private float chargeScaledPercent = 0f;

    public UnityEvent Shoot = new();
    public UnityEvent CooledDown = new();


    void Start()
    {
        animator = GetComponentInChildren<Animator>();

        foreach (LightEffect chargeLight in ChargeLights)
            chargeLight.Start();

        foreach (ChargeEvent chargeEvent in ChargeEvents)
            chargeEvent.UpdateEvent(chargeScaledPercent);
    }

    void Update()
    {
        UpdateInput();

        foreach (ChargeEvent chargeEvent in ChargeEvents)
            chargeEvent.UpdateEvent(chargeScaledPercent);
    }

    private void UpdateInput()
    {
        // charge up, if not already active cooling down
        if (!isActiveCoolingDown)
        {
            if (Input.GetButton("Fire1"))
            {
                UpdateCharge(1f, chargeTime);
                if (Mathf.Approximately(chargePercent, 1f))
                {
                    isCharged = true;
                }
            }
            else if (isCharged && Input.GetButtonUp("Fire1"))
            {
                OnShoot();
                isCharged = false;
                isActiveCoolingDown = true;
            }
            else
                UpdateCharge(0f, passiveCooldownTime);
        }
        else
        {
            UpdateCharge(0f, activeCooldownTime);
            if (Mathf.Approximately(chargePercent, 0f))
            {
                OnCooledDown();
                isActiveCoolingDown = false;
            }
        }
    }

    void UpdateCharge(float target, float time)
    {
        chargePercent = Mathf.MoveTowards(chargePercent, target, (1f / time) * Time.deltaTime);
        chargeScaledPercent = chargeCurve.Evaluate(chargePercent);

        UpdateEffects();
    }

    void UpdateEffects()
    {
        // effect set variable
        foreach (LightEffect chargeLight in ChargeLights)
            if (chargeLight.light)
                chargeLight.light.intensity = chargeLight.GetIntensity(chargeScaledPercent);
    }

    void OnShoot()
    {
        // shoot particle
        Shoot.Invoke();
    }

    void OnCooledDown()
    {
        // effect low power
        CooledDown.Invoke();
    }

    public void ToggleOpenState()
    {
        const string isOpenName = "isOpen";
        bool currentState = animator.GetBool(isOpenName);
        SetOpenState(!currentState);
    }

    public void SetOpenState(bool state)
    {
        const string isOpenName = "isOpen";
        bool currentState = animator.GetBool(isOpenName);

        animator.SetBool(isOpenName, state);

        if (currentState != state)
        {
            Material targetMat = state ? openMat : closeMat;
            const string emissionTexName = "_EmissionTex";
            const string emissionColorName = "_EmissionColor";

            bodyRenderer.material.SetTexture(emissionTexName,
                targetMat.GetTexture(emissionTexName));

            bodyRenderer.material.SetColor(emissionColorName,
                targetMat.GetColor(emissionColorName));
        }
    }
}
