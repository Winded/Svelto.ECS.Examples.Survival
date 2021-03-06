namespace Svelto.ECS.Example.Survive.Enemies
{
    public class EnemyAnimationEngine : IQueryingEntitiesEngine, IStep<DamageInfo, DamageCondition>
    {
        public IEntitiesDB entitiesDB { set; private get; }

        public void Ready()
        {}

        void EntityDamaged(DamageInfo damageInfo)
        {
            entitiesDB.ExecuteOnEntity(damageInfo.entityDamagedID, ref damageInfo,
                                              (ref EnemyEntityViewStruct entity, ref DamageInfo damage) =>
                                              {
                                                  entity.vfxComponent.position =
                                                      damage.damagePoint;
                                                  entity.vfxComponent.play.value = true;
                                              });
        }

        void TriggerTargetDeathAnimation()
        {
            int count;
            var entity = entitiesDB.QueryEntities<EnemyEntityViewStruct>(out count);

            for (int i = 0; i < count; i++)
                entity[i].animationComponent.playAnimation = "PlayerDead";
        }

        void TriggerDeathAnimation(EGID targetID)
        {
            entitiesDB.ExecuteOnEntity(targetID,
                                              (ref EnemyEntityViewStruct entity) =>
                                              {
                                                  entity.animationComponent.playAnimation = "Dead";
                                              });
            
            
        }

        public void Step(ref DamageInfo token, DamageCondition condition)
        {
            if (token.entityType == EntityDamagedType.Enemy)
            {
                //if enemy dies
                if (condition == DamageCondition.Dead)
                    TriggerDeathAnimation(token.entityDamagedID);
                else
                //if enemy is damaged
                    EntityDamaged(token);
            }
            else
            {
                //is player is dead, the enemy cheers
                if (condition == DamageCondition.Dead)
                    TriggerTargetDeathAnimation();    
            }
        }
    }
}
