using System;

public enum POPUPTYPE
{
    LOGIN,
    SIGNUP,
    MESSAGE
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
public class SignUpInfo
{
    public string id;
    public string pw;
    public string name;
    public SignUpInfo()
    {
        id = string.Empty;
        pw = string.Empty;
        name = string.Empty;
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
public class MessageInfo
{
    public int idx;
    public string message;
}