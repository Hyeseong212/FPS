using System;
using System.Collections.Generic;

public enum VIEWTYPE
{
    LOGIN,
    SIGNUP,
    GUILD
}

public enum POPUPTYPE
{
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
    public long guildUid;

    public StandbyInfo()
    {
        Reset();
    }
    public void Reset()
    {
        userUid = long.MinValue;
        guildUid = long.MinValue;
    }
}
public class MessageInfo
{
    public int idx;
    public string message;
}
[Serializable]
public class GuildInfo
{
    public long guildUid;
    public string guildName;
    public List<GuildCrew> guildCrews;
    public GuildInfo()
    {
        guildUid = long.MinValue;
        guildName = string.Empty;
        guildCrews = new List<GuildCrew>();
    }
}
[Serializable]
public class GuildCrew
{
    public long crewUid;
    public string crewName;
    public GuildCrew()
    {
        crewUid = long.MinValue;
        crewName = string.Empty;
    }
}