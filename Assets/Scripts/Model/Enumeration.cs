using System;
using System.Collections.Generic;

public enum VIEWTYPE
{
    LOGIN,
    SIGNUP,
    GUILD,
    GAMESTART
}

public enum POPUPTYPE
{
    MESSAGE,
    OKCANCEL
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
    public UserEntity userEntity;
    public GuildInfo guildInfo;

    public GameType gameType;

    public bool isMatchingNow; 

    public StandbyInfo()
    {
        Reset();
    }
    public void Reset()
    {
        userEntity = new UserEntity();
        guildInfo = new GuildInfo();
        isMatchingNow = false;
        gameType = GameType.Default;
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
    public long guildLeader;
    public List<UserEntity> guildRequest;
    public GuildInfo()
    {
        guildUid = long.MinValue;
        guildName = string.Empty;
        guildCrews = new List<GuildCrew>();
        guildLeader = long.MinValue;
        guildRequest = new List<UserEntity>();
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
[Serializable]
public class UserEntity
{
    public long UserUID;
    public string UserName;
    public string Userid;
    public string UserPW;
    public long guildUID;
    public UserEntity()
    {
        UserUID = long.MinValue;
        UserName = string.Empty;
        Userid = string.Empty;
        UserPW = string.Empty;
        guildUID = 0;
    }
}