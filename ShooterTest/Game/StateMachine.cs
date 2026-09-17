using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class StateMachine
{
    Dictionary<int, System.Action> enterStateFuncLookup = new Dictionary<int, System.Action>();
    Dictionary<int, System.Action> updateStateFuncLookup = new Dictionary<int, System.Action>();
    Dictionary<int, System.Action> exitStateFuncLookup = new Dictionary<int, System.Action>();

    public int CurrentState => currentState;
    int currentState, nextState, numOfStates;

    public StateMachine() {
        currentState = -1;
        nextState = -1;
    }
    public StateMachine(int numOfState) {
        this.numOfStates = numOfState;
        currentState = -1;
        nextState = -1;
    }

    //sete the three functions for each state
    public void SetStateFunctions(int state, System.Action enterStateFunc, System.Action updateStateFunc, System.Action exitStateFunc)
    {
        if (ValidState(state)) 
        {
            enterStateFuncLookup[state] = enterStateFunc;
            updateStateFuncLookup[state] = updateStateFunc;
            exitStateFuncLookup[state] = exitStateFunc;
        }
    }

    //total number of states in statemachine
    public void SetNumOfStates(int numOfStates) {
        this.numOfStates = numOfStates;
    }

    public void SetEnterStateFunc(int state, System.Action func) {
        if (ValidState(state)) {
            enterStateFuncLookup[state] = func;
            //runs the enter state func 
        }
    }
    public void SetUpdateStateFunc(int state, System.Action func) {
        if (ValidState(state)) {
            updateStateFuncLookup[state] = func;
        }
        //think i need to modify the update function for gameTime
    }
    public void SetExitStataeFunc(int state, System.Action func) {
        if (ValidState(state)) {
            exitStateFuncLookup[state] = func;
        }
    }
    public void SetState(int nextState) {
        if (ValidState(nextState)) {
            this.nextState = nextState;
        }
   
    }

    public void UpdateState() {
        if (currentState != nextState) {
            if (currentState >= 0) {
                exitStateFuncLookup[currentState]();
            }
            enterStateFuncLookup[nextState]();
            currentState = nextState;
        }
        if (currentState >= 0) {
            updateStateFuncLookup[currentState]();
            //updates current stage;
        }
    }

    private bool ValidState(int state) {
        if (state < numOfStates && state >= 0)
        {
            return true;
            //is valid state
        }
        else {
            throw new ArgumentOutOfRangeException(nameof(state), state,
            "Not a valid state.");
        }
    }
}
