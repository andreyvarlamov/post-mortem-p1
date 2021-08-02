using PostMortem_P1.Systems;
using PostMortem_P1.Behaviors;
using PostMortem_P1.Interfaces;

namespace PostMortem_P1.Core
{
    public class NPC : Actor
    {
        public NPC() : base()
        {

        }

        public int? TurnsAlerted { get; set; }

        public int Disposition { get; protected set; }

        public virtual void PerformAction(CommandSystem commandSystem)
        {
            IBehavior behavior;
            
            if (Disposition < 0)
            {
                if (TurnsAlerted.HasValue)
                {
                    behavior = new StandardMoveAndAttack();

                    TurnsAlerted++;
                    
                    if (TurnsAlerted > 15)
                    {
                        TurnsAlerted = null;
                    }
                }
                else
                {
                    behavior = new IdleAlert();
                }
            }
            else
            {
                behavior = new Idle();
            }

            behavior.Act(this, commandSystem);
        }

        public void Update()
        {

        }
    }
}
