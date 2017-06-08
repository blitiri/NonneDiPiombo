using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu]
public class HolyShield : Abilities {



    public override void OnAbilityActivation()
    {
        player.transform.FindChild("Cross").gameObject.SetActive(true);
    }
}
