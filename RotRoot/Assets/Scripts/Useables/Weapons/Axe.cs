using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class Axe : Weapon
{
    [SerializeField] private GameObject indicator;

    private void Start()
    {
        canUse = true;

        Vector3 position = transform.position;
        position.z = -.01f;

        indicator = Instantiate(
        indicator,
                position + transform.right * (stats.attackRange - indicator.transform.localScale.y / 2),
                transform.rotation * Quaternion.Euler(0, 0, 90), transform);

        indicator.SetActive(false);

        SetStats();

        Debug.Log(Health);
    }

    public override void PrimaryUse()
    {
        if (InUse && !LockRotation && !inAttack)
            StartCoroutine(Attack());
    }

    public override void SecondaryUse() => InUse = true;

    private void CheckForAttackable()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.right, stats.attackRange, hittable);

        if (hit)
        {
            Debug.Log("Hit object: " + hit.collider.name);

            Entity attackable = hit.collider.GetComponent<Entity>();

            attackable?.TakeDamage(stats.attackDamage);

            Health--;

            if (Health < 0)
                OnBreak();
        }
    }

    public override void OnBreak() => canUse = false;

    private IEnumerator Attack()
    {
        inAttack = true;

        indicator.SetActive(true);

        yield return new WaitForSeconds(stats.attackSpeed / 2);
        
        LockRotation = true;

        yield return new WaitForSeconds(stats.attackSpeed / 2 - .05f);

        CheckForAttackable();
        LockRotation = false;
        inAttack = false;

        indicator.GetComponent<SpriteRenderer>().color = Color.red;

        yield return new WaitForSeconds(.05f);

        indicator.GetComponent<SpriteRenderer>().color = Color.white * .5f;

        indicator.SetActive(false);
    }
}
