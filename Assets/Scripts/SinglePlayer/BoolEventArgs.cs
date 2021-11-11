using System;

public class BoolEventArgs : EventArgs
{
    public bool value { get; set; }
    public BoolEventArgs(bool data)
    {
        value = data;
    }
}
