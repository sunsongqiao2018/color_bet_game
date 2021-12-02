using System;

public class BoolEventArgs : EventArgs
{
    public bool value { get; set; }
    public BoolEventArgs(bool data)
    {
        value = data;
    }
}
public class PlayerInfoEventArgs : EventArgs
{
    public bool pick { get; private set; }
    public int totalChips { get; private set; }
    public int betChips { get; private set; }
    public PlayerInfoEventArgs(bool _pick, int _totalChips, int _betChips)
    {
        pick = _pick;
        totalChips = _totalChips;
        betChips = _betChips;
    }
}