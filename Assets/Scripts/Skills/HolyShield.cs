using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu]
public class HolyShield : Abilities {



    public override void OnAbilityActivation()
    {
        player.transform.GetChild(1).gameObject.SetActive(true);
        player.GetComponent<PlayerControl>().isImmortal = true;
    }
}