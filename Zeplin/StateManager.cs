using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Zeplin
{
    public class StateManager
    {
        public StateManager(ZeplinGame game) 
        {
            stateQueue = new Queue<State>();
            CurrentState = new State(string.Empty, TimeSpan.Zero);
            game.OnUpdate += this.ProcessState;
        }

        public State CurrentState { get; private set; }
        
        /// <summary>
        /// The state that is being transitioned to. Not defined if Transitioning is false.
        /// </summary>
        public State NextState
        {
            get { return stateQueue.Peek(); }
        }
        
        /// <summary>
        /// Enqueues a state with no transition time.
        /// </summary>
        /// <param name="name"></param>
        public void AddState(String name)
        {
            stateQueue.Enqueue(new State(name, TimeSpan.Zero));
        }

        /// <summary>
        /// Enqueues a state with a defined transition time.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="duration">The amount of time it will take to transition to this state.</param>
        public void AddState(String name, TimeSpan duration)
        {
            stateQueue.Enqueue(new State(String.Format("transition {0} to {1}", CurrentState, name)));
            stateQueue.Enqueue(new State(name, duration));
        }

        internal void ProcessState(GameTime time)
        {
            currentTime = time.TotalRealTime;
            if (Transitioning && (time.TotalRealTime - transitionBeginTimestamp) > NextState.Duration)
            {
                CurrentState = stateQueue.Dequeue();
                transitionBeginTimestamp = time.TotalRealTime;
            }
        }

        //Indicates that there is a transition between states occurring.
        public bool Transitioning
        {
            get
            {
                return stateQueue.Count > 0;
            }
        }

        /// <summary>
        /// Gets a TimeSpan defining the amount of time remaining before the next State is active. 
        /// </summary>
        public TimeSpan TransitionTimeLeft
        {
            get
            {
                return (transitionBeginTimestamp + NextState.Duration) - currentTime;
            }
        }

        /// <summary>
        /// Gets a TimeSpan desining the total duration of the current transition.
        /// </summary>
        public TimeSpan TransitionDuration
        {
            get
            {
                return NextState.Duration;
            }
        }

        /// <summary>
        /// Gets a time stamp marking the beginning of the transition in progress, in terms of GameTime.TotalRealTime.
        /// </summary>
        public TimeSpan TransitionTimeStarted
        {
            get
            {
                return transitionBeginTimestamp;
            }
        }

        /// <summary>
        /// Gets a time stamp marking completion of the transition in progress, in terms of GameTime.TotalRealTime.
        /// </summary>
        public TimeSpan TransitionTimeFinished
        {
            get
            {
                return transitionBeginTimestamp + NextState.Duration;
            }
        }

        /// <summary>
        /// Gets a fractional value between 0 and 1 representing the progress of the current transition.
        /// </summary>
        public float TransitionPercentComplete
        {
            get
            {
                return (float)(currentTime.TotalSeconds / (transitionBeginTimestamp + NextState.Duration).TotalSeconds);
            }
        }



        Queue<State> stateQueue;
        TimeSpan transitionBeginTimestamp;
        TimeSpan currentTime;
    }

    public struct State
    {
        public State(string name, TimeSpan duration) : this()
        {
            Name = name;
            Duration = duration;
        }

        public State(string name) : this(name, TimeSpan.Zero) { }

        public string Name { get; internal set; }
        
        public override string ToString()
        {
            return Name;
        }

        public static bool operator ==(State left, State right)
        {
            return left.Name.Equals(right.Name);
        }
        public static bool operator !=(State left, State right)
        {
            return !(left == right);
        }

        internal TimeSpan Duration;
    }
}
