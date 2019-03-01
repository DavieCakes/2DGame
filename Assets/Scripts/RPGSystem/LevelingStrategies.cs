using Creatures;

namespace LevelingStrategies {
    public interface LevelingStrategy {
        void CalculateLevelChanges();

        void attatchCreature(Creature c);
    }

    public class MonkLevelStrategy {
        private Creature self;

        public void attatchCreature(Creature c) {
            this.self = c;
        }
        public void CalculateLevelChanges() {
            
        }
    }

}