using System.Collections;
using UnityEngine;

public class RangedAttackGadjet : Gadjet
{
    public override IEnumerator Ability()
    {
        Debug.Log("Attack!!");
        yield return null;
    }
}