using System;

public enum POPUPTYPE
{
    LOGIN
}
[Serializable]
public class LoginInfo
{
    public string ID;
    public string PW;
    public LoginInfo()
    {
        ID = string.Empty;
        PW = string.Empty;
    }
}
[Serializable]
public class StandbyInfo
{
    public long userUid;
    public StandbyInfo()
    {
        userUid = long.MinValue;
    }
}