using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerConfig", menuName = "Config/Player")]
public class PlayerConfig : ScriptableObject
{
    public float hp;
    public float damage;
    public float moveSpeedFactor;
    public float rotationSpeedFactor;
}
