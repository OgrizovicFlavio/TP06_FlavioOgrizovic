using UnityEngine;

[CreateAssetMenu(fileName = "BossSO", menuName = "Platformer/BossSO", order = 4)]
public class BossSO : ScriptableObject
{
    [Header("Boss Stats")]
    [SerializeField] public float movementSpeed = 5f;
    [SerializeField] public float maxHealth = 200f;
    [SerializeField] public float attackDamage = 25f;
    [SerializeField] public float meleeRange = 1f;
    [SerializeField] public float sightRange = 10f;

    [Header("Boss Collisions")]
    [SerializeField] public LayerMask playerLayerMask;
}
