using Godot;
using System;


public class Enums
{
    /// <summary> 节点运行状态 </summary>
    public enum Status
    {
        Free,
        Failure,
        Success,
        Running,
    }
    
    public enum Monitor
    {
        None,
        Both,
        Self,
        LowerPriority,
    }
    
    /// <summary> Utility Compound Mode, 实用程序组合方式 </summary>
    public enum U14M
    {
        Mul,
        Avg,
        Min,
        Max,
    }

}
