using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatSystem : MonoBehaviour
{
    [SerializeField] private List<AttackSO> playerAttacks;

    public Player_Component player;

    [SerializeField] private Animator anim;

    [SerializeField] private Weapon playerWeapon;
    [SerializeField] private float comboCooldown;
    [SerializeField] private float timeBetweenAttacks;
    private float lastClickTime;
    private float lastComboEnd;
    private int comboCounter;

    private bool isAttacking;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        player.input.OnPlayerAttack += OnPlayerAttack;
        isAttacking = false;
    }

    private void OnPlayerAttack(bool obj)
    {
        isAttacking = true;
    }


     //Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
            Attack();
    
        ExitAttack();
    }

    private void Attack()
    {
        //comboCooldown = 0.2f;
        //timeBetweenAttacks = 0.2f;
        if (Time.time - lastComboEnd > comboCooldown && comboCounter < playerAttacks.Count)
        {
            CancelInvoke(nameof(EndCombo));

            if (Time.time - lastClickTime >= timeBetweenAttacks)
            {
                anim.runtimeAnimatorController = playerAttacks[comboCounter].animatorOV;
                anim.Play("Sword And Shield Slash", 0, 0);

                if (playerWeapon != null)
                    playerWeapon.damage = playerAttacks[comboCounter].damage;
                // Do vfx , sfx ,etc ,here
                comboCounter++;
                lastClickTime = Time.time;

                if (comboCounter > playerAttacks.Count)
                    comboCounter = 0;
            }
        }
    }

    private void ExitAttack()
    {
        if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.9 &&
            anim.GetCurrentAnimatorStateInfo(0).IsTag("Attack"))
        {
            Invoke(nameof(EndCombo), 1);
        }
    }

    private void EndCombo()
    {
        comboCounter = 0;
        lastComboEnd = Time.time;
        isAttacking = false;
        Debug.LogWarning("EndCombo");
        // you can put vfx sfx ext here to make the combo more flashier 
    }
}