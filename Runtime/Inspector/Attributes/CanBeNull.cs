using System;
using UnityEngine;

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Class, AllowMultiple = true)]
public class CanBeNull : PropertyAttribute
{
}