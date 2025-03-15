using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : Entity
{
    protected override void OnDeath() => StartCoroutine(BreakBoard());

    private IEnumerator BreakBoard()
    {
        yield return new WaitForSeconds(.01f);

        gameObject.SetActive(false);
    }
}
