using System.Collections;

namespace Svelto.ECS.Example.Survive.Enemies
{
    public class EnemyMovementEngine : IQueryingEntitiesEngine, IStep<DamageInfo, DamageCondition>
    {
        public IEntitiesDB entitiesDB { set; private get; }

        public void Ready()
        {
            Tick().Run();
        }

        IEnumerator Tick()
        {
            while (true)
            {
                int count;
                var enemyTargetEntityViews = entitiesDB.QueryEntities<EnemyTargetEntityViewStruct>(out count);

                if (count > 0)
                {
                    var targetEntityView = enemyTargetEntityViews[0];

                    var enemies = entitiesDB.QueryEntities<EnemyEntityViewStruct>(out count);

                    for (var i = 0; i < count; i++)
                    {
                        enemies[i].movementComponent.navMeshDestination = targetEntityView.targetPositionComponent.position;
                    }
                }

                yield return null;
            }
        }

        void StopEnemyOnDeath(EGID targetID)
        {
            entitiesDB.ExecuteOnEntity(targetID, (ref EnemyEntityViewStruct entityView) =>
            {
                entityView.movementComponent.navMeshEnabled      = false;
                entityView.movementComponent.setCapsuleAsTrigger = true;
            });
        }

        public void Step(ref DamageInfo token, DamageCondition condition)
        {
            StopEnemyOnDeath(token.entityDamagedID);
        }
    }
}
