using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class SprintController : MonoBehaviour
{
    [SerializeField] private float maxStamina = 100;
    [SerializeField] private float staminaLossRate = 15;
    [SerializeField] private float staminaGainRate = 15;
    //[SerializeField] private CanvasGroup staminaGroup;
    //[SerializeField] private Image staminaBar;
    //[SerializeField] private Image staminaBarBack;

    [SerializeField] private float sprintSpeed = 10;

    private float stamina;
    
    public bool CanSprint { get; private set; }
    public bool IsSprinting { get; private set; }
    public float SprintSpeed { get { return sprintSpeed; } }

    private void Start()
    {
        stamina = maxStamina;
        //staminaGroup.alpha = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (IsSprinting && CanSprint)
        {
            //staminaGroup.alpha = Mathf.Lerp(staminaGroup.alpha, 1, .025f);

            stamina = Mathf.Clamp(stamina - staminaLossRate * Time.deltaTime, 0, maxStamina);

            if (stamina <= 1) { CanSprint = false; }
        }
        else
        {
            stamina = Mathf.Clamp(stamina + staminaGainRate * Time.deltaTime, 0, maxStamina);

            //if (stamina > maxStamina * .75) { staminaGroup.alpha = Mathf.Lerp(staminaGroup.alpha, 0, .02f); }
        }

        if (!CanSprint && stamina > maxStamina * .25) {  CanSprint = true; }

        //staminaBarBack.color = Color.Lerp(Color.red, Color.black, staminaBar.fillAmount * 2);
        //staminaBar.fillAmount = stamina / maxStamina;
    }

    public void OnSprint(InputAction.CallbackContext context)
    {
        if (context.ReadValueAsButton() && CanSprint)
        {
            IsSprinting = true;
        }
        else { IsSprinting = false; }
    }
}
