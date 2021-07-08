using PostMortem_P1.Systems;

namespace PostMortem_P1.Interfaces
{
    public interface IBehavior
    {
        bool Act(Enemy enemy, CommandSystem commandSystem);
    }
}
