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

    class VertexStateMachine : StateMachine
    {
        public VertexStateMachine() : base ()
        {
            this.currentState = "change";

            State render = new State("change");
            render.AddTransition("set", "no change");
            State noRender = new State("no change");
            noRender.AddTransition("no set", "change");

            AddState(render);
            AddState(noRender);
        }

    }

    class InitStateMachine : StateMachine
    {
        public InitStateMachine() : base()
        {
            this.currentState = "init";

            State initState = new State("init");
            State nonInitState = new State("non init");

            initState.AddTransition("non init", "non init");

            AddState(initState);
            AddState(nonInitState);
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


            AddState(notHover);
            AddState(hovered);
        }
    }

    class ClickStateMachine : StateMachine
    {
        public ClickStateMachine() : base()
        {
            this.currentState = "mouse up";

            State mouseUp = new State("mouse up");
            State mouseDown = new State("mouse down");
            State clicked = new State("clicked");

            mouseUp.AddTransition("click", "mouse down");
            mouseDown.AddTransition("release", "clicked");
            clicked.AddTransition("reset", "mouse up");

            AddState(mouseUp);
            AddState(mouseDown);
            AddState(clicked);
        }
    }

    class GameStateMachine : StateMachine
    {
        public GameStateMachine() : base()
        {
            this.currentState = "main menu";

            State mainMenu = new State("main menu");
            State editor = new State("editor");

            mainMenu.AddTransition("editor", "editor");

            AddState(mainMenu);
            AddState(editor);
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
