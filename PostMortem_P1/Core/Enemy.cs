using PostMortem_P1.Core;
using PostMortem_P1.Systems;
using PostMortem_P1.Behaviors;

namespace PostMortem_P1
{
    public class Enemy : Actor
    {
        public int? TurnsAlerted { get; set; }

        public virtual void PerformAction(CommandSystem commandSystem)
        {
            var behavior = new StandardMoveAndAttack();
            behavior.Act(this, commandSystem);
        }
    }
}
