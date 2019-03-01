using Creatures;

namespace ActionStrategies {
    public abstract class ActionStrategy {
        // public virtual void Perform() {}
    }
    abstract class TargetingAction : ActionStrategy {
        public virtual void Perform(Creature from, Creature to) {}
    }
    abstract class AreaAction : ActionStrategy {
        public virtual void Perform(Creature from) {}
    }

    class FireBall : AreaAction {
        public override void Perform(Creature from) {  

        }
    }
}

namespace Actions {
    public class Action {
        public object target;
        public object source;
        public Action(object s) {
            this.source = s;
            
        }
        
    }
}