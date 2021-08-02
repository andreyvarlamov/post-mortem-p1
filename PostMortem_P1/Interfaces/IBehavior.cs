using PostMortem_P1.Systems;
using PostMortem_P1.Core;

namespace PostMortem_P1.Interfaces
{
    public interface IBehavior
    {
        bool Act(NPC npc, CommandSystem commandSystem);
    }
}
