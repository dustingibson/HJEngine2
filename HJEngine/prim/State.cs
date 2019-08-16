using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HJEngine.prim
{
    class StateMachine
    {
        public Dictionary<string, State> states;
        public string currentState;

        public StateMachine()
        {
            states = new Dictionary<string, State>();                
        }

        public void AddState(State state)
        {
            states.Add(state.name, state);
        }

        public void TransitionState(string transitionName)
        {
            State state = states[currentState];
            foreach (Transition transition in state.transitions)
            {
                if (transition.name == transitionName)
                {
                    currentState = transition.toState;
                }
            }
        }

    }

    class RenderStateMachine : StateMachine
    {
        public RenderStateMachine() : base ()
        {
            this.currentState = "render";

            State render = new State("render");
            render.AddTransition("go", "no render");
            State noRender = new State("no render");
            noRender.AddTransition("go", "render");

            AddState(render);
            AddState(noRender);
        }

    }


    class MouseOverStateMachine : StateMachine
    {
        public MouseOverStateMachine() : base()
        {
            this.currentState = "not hover";

            State notHover = new State("not hover");
            notHover.AddTransition("on", "hover");
            State hovered = new State("hover");
            hovered.AddTransition("off", "not hover");
            //State turnBright = new State("turn bright");
            //turnBright.AddTransition("lambda", "stay bright");
            //State stayBright = new State("stay bright");
            //stayBright.AddTransition("lambda", "turn off bright");
            //State turnOffBright = new State("turn off bright");
            //turnOffBright.AddTransition("lambda", "not hover");

            AddState(notHover);
            AddState(hovered);
            //AddState(turnBright);
            //AddState(stayBright);
            //AddState(turnOffBright);
        }
    }

    class Transition
    {
        public string name;
        public string toState;

        public Transition(string name, string toState)
        {
            this.name = name;
            this.toState = toState;
        }
    }

    class State
    {
        public string name;
        public List<Transition> transitions;

        public State(string name, List<Transition> transitions)
        {
            this.name = name;
            this.transitions = transitions;
        }

        public State(string name)
        {
            this.name = name;
            this.transitions = new List<Transition>();
        }

        public void AddTransition(string name, string toState)
        {
            Transition transition = new Transition(name, toState);
            transitions.Add(transition);
        }
    }
}
