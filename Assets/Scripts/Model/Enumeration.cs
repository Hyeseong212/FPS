public enum Protocol
{
    //�α��� ���� ���� ��û ��Ŷ
    Login = 0x00,
    //ä�� ���� ���� ��û ��Ŷ
    Chat = 0x16,
    //��Ī ���� ���� ��û ��Ŷ
    Match = 0x32
}
public enum ChatStatus
{
    //��ü ä��
    ENTIRE = 0x00,

    //�ӼӸ�
    WHISPER = 0x01,

    //���
    GUILD = 0x02
}
public enum LoginRequestType
{
    // �ű� ȸ������ ��û
    SignupRequest = 0x00,

    // ���� ȸ���� �α��� ��û
    LoginRequest = 0x01,

    // ���� ȸ���� �α׾ƿ� ��û
    LogoutRequest = 0x02,

    // ȸ�� ���� ��û
    DeleteRequest = 0x03,

    // ȸ�� ���� ���� ��û
    UpdateRequest = 0x04,

}

public enum ResponseType
{
    Success = 0x00,
    Fail = 0x01,
}
