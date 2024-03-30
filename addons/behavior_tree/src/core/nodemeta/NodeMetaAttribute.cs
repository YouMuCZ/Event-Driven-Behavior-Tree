using System;
using System.Runtime.InteropServices;
using Godot;

[Tool]
public class NodeMetaAttribute : Attribute
{
    // 要通过GetCustomAttributes去获取属性的特性标记,需要给属性加上{get;set;}
}
