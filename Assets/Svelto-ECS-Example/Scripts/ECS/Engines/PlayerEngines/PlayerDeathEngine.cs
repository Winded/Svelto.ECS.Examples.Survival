namespace Svelto.ECS.Example.Survive.Player
{
    public class PlayerDeathEngine:IEngine, IStep<DamageInfo>
    {
        public PlayerDeathEngine(IEntityFunctions entityFunctions)
        {
            _entityFunctions = entityFunctions;
        }
        
        public void Step(ref DamageInfo token, int condition)
        {
            _entityFunctions.RemoveEntity(token.entityDamagedID);
        }

        readonly IEntityFunctions _entityFunctions;
    }
}