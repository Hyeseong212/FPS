public enum Protocol
{
    ///<summary>
    ///�α��� ���� ���� ��û ��Ŷ
    ///</summary>
    Login = 0x00,
    ///<summary>
    ///ä�� ���� ���� ��û ��Ŷ
    ///</summary>
    Chat = 0x16,
    ///<summary>
    ///��� ���� ���� ��û ��Ŷ
    ///</summary>
    Guild = 0x17,
    ///<summary>
    ///��Ī ���� ���� ��û ��Ŷ
    ///</summary>
    Match = 0x32
}
public enum GuildProtocol
{
    ///<summary>
    ///���� uid�� ��� �����˻� �ϴ� ��������
    ///</summary>
    IsUserGuildEnable = 0x00,

    ///<summary>
    ///����̸����� �˻��ϴ� ��������
    ///</summary>
    SelectGuildName = 0x01,

    ///<summary>
    ///���� ������ ��忡�� ���� ��ȸ�ϴ� ��������
    ///</summary>
    SelectGuildCrew = 0x02,

    ///<summary>
    ///��� ���� ��������
    ///</summary>
    CreateGuild = 0x03
}
public enum ChatStatus
{
    ///<summary>
    ///��ü ä��
    ///</summary>
    ENTIRE = 0x00,

    ///<summary>
    ///�ӼӸ�
    ///</summary>
    WHISPER = 0x01,

    ///<summary>
    ///���
    ///</summary>
    GUILD = 0x02
}
public enum LoginRequestType
{
    ///<summary>
    /// �ű� ȸ������ ��û
    ///</summary>
    SignupRequest = 0x00,

    ///<summary>
    /// ���� ȸ���� �α��� ��û
    ///</summary>
    LoginRequest = 0x01,

    ///<summary>
    /// ���� ȸ���� �α׾ƿ� ��û
    ///</summary>
    LogoutRequest = 0x02,

    ///<summary>
    /// ȸ�� ���� ��û
    ///</summary>
    DeleteRequest = 0x03,

    ///<summary>
    /// ȸ�� ���� ���� ��û
    ///</summary>
    UpdateRequest = 0x04,

}

public enum ResponseType
{
    Success = 0x00,
    Fail = 0x01,
}
