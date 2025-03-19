
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public interface IUseable
{
    public bool InUse { get; set; }
    public bool LockRotation { get; set; }
    public void PrimaryUse();
    public void SecondaryUse();
    public void OnBreak();
}
