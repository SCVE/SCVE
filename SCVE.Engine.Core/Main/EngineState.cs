﻿namespace SCVE.Engine.Core.Main
{
    public enum EngineState
    {
        Uninitialized,
        Starting,
        Ready,
        Running,
        TerminationRequested,
        Terminating,
        Terminated
    }
}