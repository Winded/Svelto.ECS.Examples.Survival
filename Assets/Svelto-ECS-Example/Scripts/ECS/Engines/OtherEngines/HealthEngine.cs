using Svelto.ECS.Example.Survive.Player;

namespace Svelto.ECS.Example.Survive
{
    public class HealthEngine : IQueryingEntitiesEngine, IStep<DamageInfo>
    {
        public void Ready()
        { }

        public HealthEngine(SingleSequence<DamageInfo> damageSequence)
        {
            _damageSequence = damageSequence;
        }

        public IEntitiesDB entitiesDB { set; private get; }

        public void Step(ref DamageInfo damage, int condition)
        {
            entitiesDB.ExecuteOnEntity(damage.entityDamagedID, ref damage,
                                          (ref HealthEntityStruct healthEntityStruct,
                                           ref DamageInfo         damageInfo) =>
                                          {
                                              healthEntityStruct.currentHealth -=
                                                  damageInfo.damagePerShot;

                                              //the HealthEngine can branch the sequencer flow triggering two different
                                              //conditions
                                              if (healthEntityStruct.currentHealth <= 0)
                                                  _damageSequence.Trigger(ref damageInfo, (int)DamageCondition.Dead);
                                              else
                                                  _damageSequence.Trigger(ref damageInfo, (int)DamageCondition.Damage);
                                          }
                                         );
            
        }

        readonly SingleSequence<DamageInfo> _damageSequence;
    }
}
