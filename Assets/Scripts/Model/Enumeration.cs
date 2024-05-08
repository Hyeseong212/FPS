public enum Protocol
{
    //로그인 서버 관련 요청 패킷
    Login = 0x00,
    //채팅 서버 관련 요청 패킷
    Chat = 0x16,
    //매칭 서버 관련 요청 패킷
    Match = 0x32
}
public enum ChatStatus
{
    //전체 채팅
    ENTIRE = 0x00,

    //귓속말
    WHISPER = 0x01,

    //길드
    GUILD = 0x02
}
public enum LoginRequestType
{
    // 신규 회원가입 요청
    SignupRequest = 0x00,

    // 기존 회원의 로그인 요청
    LoginRequest = 0x01,

    // 기존 회원의 로그아웃 요청
    LogoutRequest = 0x02,

    // 회원 삭제 요청
    DeleteRequest = 0x03,

    // 회원 정보 수정 요청
    UpdateRequest = 0x04,

}

public enum ResponseType
{
    Success = 0x00,
    Fail = 0x01,
}
