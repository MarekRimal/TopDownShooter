using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This interfeace ensures two methods
// - GetFisrtSpellCD()
// - GetSecondSpellCD()
public interface ICharacter {

    // Returns remaining cooldowns
    float GetFisrtSpellCD();
    float GetSecondSpellCD();

    string GetFisrtSpellName();
    string GetSecondSpellName();
}
