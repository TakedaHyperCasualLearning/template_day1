using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Donuts
{
    [System.Serializable]
    public class TopDownCharacterMovementComponent : IComponent
    {
        public Entity owner { get; set; }
        public Transform graphicRoot;
        public float speed;
        [Header("In Radiian")] public float rotationSpeed = 6.28f;
        [System.NonSerialized] public Vector3 movementInput = Vector3.zero;

        [Header("Detect Wall")]
        public bool moveThroughWall = false;
        public float distanceFromAvoidanceLayer = 1;
        public LayerMask avoidLayerMask;
        public float castExtraRange = 1;
        public int castRayNumbers = 1;
        public float castAngle = 10;
        [System.NonSerialized] public Quaternion[] castRotations;
    }

    public class TopDownCharacterGroup : ComponentGroup
    {
        public TopDownCharacterMovementComponent characterMovement;
        public Transform transform;
        public override ComponentGroup CreateInstance(Entity entity)
        {
            return new TopDownCharacterGroup();
        }

        public override long GetTargetBits()
        {
            return ECSDefinition.GetComponentBit<TopDownCharacterMovementComponent>();
        }

        public override void Init(Entity entity)
        {
            characterMovement = entity.GetComponent<TopDownCharacterMovementComponent>();
            characterMovement.castRotations = new Quaternion[characterMovement.castRayNumbers];
            float startAngles = -((characterMovement.castRayNumbers - 1.0f) / 2) * characterMovement.castAngle;
            for (int i = 0; i < characterMovement.castRayNumbers; i++)
            {
                characterMovement.castRotations[i] = Quaternion.Euler(0, startAngles + i * characterMovement.castAngle, 0);
            }
            transform = entity.transform;
        }
    }

    public class TopdownCharacterMovementSystem : AGameSystem, IUpdateSystem
    {
        
        private RaycastHit hit;
        private Vector3 zeroVector = Vector3.zero;

        public void OnUpdate(float deltaTime)
        {
            masterSystem.entityManager.Foreach<TopDownCharacterGroup>(Iterate, deltaTime);
        }

        private void Iterate(TopDownCharacterGroup group, float deltaTime)
        {

            if (group.characterMovement.movementInput.magnitude <= Definition.cutOffSpeed)
            {
                return;
            }

            float speed = group.characterMovement.speed * deltaTime;
            Vector3 speedVector = group.characterMovement.movementInput * speed;

            if (group.characterMovement.moveThroughWall)
            {
                group.transform.position += speedVector;
                return;
            }



            Vector3 castPos = group.transform.position;
            int numbers = group.characterMovement.castRotations.Length;

            Vector3 targetPos;
            Vector3 targetCastPos;
            Vector3 castVector = group.characterMovement.movementInput * (speed + group.characterMovement.castExtraRange);
            group.characterMovement.graphicRoot.forward = Vector3.RotateTowards(group.characterMovement.graphicRoot.forward, group.characterMovement.movementInput, group.characterMovement.rotationSpeed * deltaTime, 0);

            for (int i = 0; i < numbers; i++)
            {
                targetCastPos = group.characterMovement.castRotations[i] * castVector;
                targetPos = castPos + targetCastPos;

                if (Physics.Linecast(castPos, targetPos, out hit, group.characterMovement.avoidLayerMask))
                {
                    return;
                }
            }

            group.transform.position += speedVector;
        }
    }
}