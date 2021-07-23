using PostMortem_P1.Systems;
using PostMortem_P1.Behaviors;

namespace PostMortem_P1.Core
{
    public class Enemy : Actor
    {
        public Enemy() : base()
        {

        }

        public int? TurnsAlerted { get; set; }

        public virtual void PerformAction(CommandSystem commandSystem)
        {
            var behavior = new StandardMoveAndAttack();
            behavior.Act(this, commandSystem);
        }

        public void Update()
        {

        }
    }
}
