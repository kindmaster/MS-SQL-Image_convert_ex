# 로컬 이미지 암호화/복호화 도구

이 도구는 로컬 경로에 저장된 이미지 파일을 AES-256 GCM 암호화를 사용하여 암호화하고 복호화할 수 있는 기능을 제공합니다.

## 주요 기능

- **단일 파일 암호화/복호화**: 개별 이미지 파일을 암호화하거나 복호화
- **디렉토리 일괄 처리**: 폴더 내의 모든 이미지 파일을 한 번에 암호화/복호화
- **이미지 정보 조회**: 암호화 전후의 이미지 파일 정보 확인
- **AES-256 GCM 암호화**: 강력한 보안을 위한 최신 암호화 알고리즘 사용

## 지원 이미지 형식

- JPEG (.jpg, .jpeg)
- PNG (.png)
- BMP (.bmp)
- GIF (.gif)
- TIFF (.tiff)
- 기타 System.Drawing에서 지원하는 모든 이미지 형식

## 사용법

### 1. 단일 파일 암호화

```csharp
// 이미지 파일을 암호화하여 지정된 경로에 저장
bool success = LocalImageEncryption.EncryptImageFile(
    @"C:\images\photo.jpg",           // 소스 이미지 경로
    @"C:\encrypted\photo.jpg.encrypted", // 암호화된 파일 저장 경로
    "MySecretPassword123!"            // 암호화 비밀번호
);
```

### 2. 단일 파일 복호화

```csharp
// 암호화된 파일을 복호화하여 지정된 경로에 저장
bool success = LocalImageEncryption.DecryptImageFile(
    @"C:\encrypted\photo.jpg.encrypted", // 암호화된 파일 경로
    @"C:\decrypted\photo.jpg",           // 복호화된 파일 저장 경로
    "MySecretPassword123!"               // 복호화 비밀번호
);
```

### 3. 자동 확장자 처리

```csharp
// 원본 파일에 .encrypted 확장자를 추가하여 암호화
string encryptedPath = LocalImageEncryption.EncryptImageFileInPlace(
    @"C:\images\photo.jpg",    // 원본 파일 경로
    "MySecretPassword123!"     // 암호화 비밀번호
);
// 결과: C:\images\photo.jpg.encrypted

// .encrypted 확장자를 제거하여 복호화
string decryptedPath = LocalImageEncryption.DecryptImageFileInPlace(
    @"C:\images\photo.jpg.encrypted", // 암호화된 파일 경로
    "MySecretPassword123!"            // 복호화 비밀번호
);
// 결과: C:\images\photo.jpg
```

### 4. 디렉토리 일괄 암호화

```csharp
// 디렉토리 내의 모든 이미지 파일을 암호화
int encryptedCount = LocalImageEncryption.EncryptImageDirectory(
    @"C:\images",              // 소스 디렉토리
    @"C:\encrypted",           // 암호화된 파일 저장 디렉토리
    "MySecretPassword123!",    // 암호화 비밀번호
    new string[] { "*.jpg", "*.png", "*.bmp" } // 특정 확장자만 처리 (선택사항)
);
```

### 5. 디렉토리 일괄 복호화

```csharp
// 디렉토리 내의 모든 암호화된 파일을 복호화
int decryptedCount = LocalImageEncryption.DecryptImageDirectory(
    @"C:\encrypted",           // 암호화된 파일 디렉토리
    @"C:\decrypted",           // 복호화된 파일 저장 디렉토리
    "MySecretPassword123!"     // 복호화 비밀번호
);
```

### 6. 이미지 정보 조회

```csharp
// 이미지 파일의 상세 정보 조회
string imageInfo = LocalImageEncryption.GetImageFileInfo(@"C:\images\photo.jpg");
Console.WriteLine(imageInfo);
```

출력 예시:
```
파일 경로: C:\images\photo.jpg
파일 크기: 1,234,567 bytes
이미지 형식: JPEG
너비: 1920px
높이: 1080px
수평 해상도: 96 dpi
수직 해상도: 96 dpi
픽셀 형식: Format24bppRgb
```

## 콘솔 애플리케이션 사용법

### 빌드 및 실행

```bash
# 프로젝트 빌드
dotnet build

# 실행
dotnet run
```

### 대화형 메뉴

프로그램을 실행하면 다음과 같은 메뉴가 표시됩니다:

```
=== 로컬 이미지 암호화/복호화 대화형 테스트 ===
1. 단일 파일 암호화
2. 단일 파일 복호화
3. 디렉토리 일괄 암호화
4. 디렉토리 일괄 복호화
5. 이미지 정보 조회
6. 종료
선택하세요 (1-6):
```

각 메뉴를 선택하면 필요한 정보를 입력받아 해당 작업을 수행합니다.

## 보안 고려사항

### 암호화 알고리즘
- **AES-256 GCM**: 최신 표준 암호화 알고리즘 사용
- **키 유도**: PBKDF2를 사용하여 비밀번호에서 암호화 키 생성
- **Salt**: 각 암호화마다 고유한 salt 사용
- **Nonce**: 각 암호화마다 고유한 nonce 사용
- **인증 태그**: 데이터 무결성 보장

### 비밀번호 관리
- 강력한 비밀번호 사용 권장 (대소문자, 숫자, 특수문자 포함)
- 비밀번호를 안전한 곳에 보관
- 소스 코드에 비밀번호를 하드코딩하지 않음

### 파일 관리
- 암호화된 파일은 `.encrypted` 확장자로 구분
- 원본 파일과 암호화된 파일을 별도 위치에 보관 권장
- 정기적인 백업 수행

## 오류 처리

모든 메서드는 적절한 예외 처리를 포함합니다:

```csharp
try
{
    bool success = LocalImageEncryption.EncryptImageFile(sourcePath, encryptedPath, password);
    if (success)
    {
        Console.WriteLine("암호화가 성공적으로 완료되었습니다.");
    }
}
catch (FileNotFoundException ex)
{
    Console.WriteLine($"파일을 찾을 수 없습니다: {ex.Message}");
}
catch (ArgumentException ex)
{
    Console.WriteLine($"잘못된 인수입니다: {ex.Message}");
}
catch (InvalidOperationException ex)
{
    Console.WriteLine($"암호화 중 오류가 발생했습니다: {ex.Message}");
}
```

## 성능 고려사항

- 대용량 이미지 파일 처리 시 메모리 사용량 주의
- 디렉토리 일괄 처리 시 진행 상황을 콘솔에 출력
- 네트워크 드라이브보다는 로컬 드라이브 사용 권장
- SSD 사용 시 더 빠른 처리 속도

## 예제 시나리오

### 시나리오 1: 개인 사진 보호
```csharp
// 개인 사진 폴더 암호화
LocalImageEncryption.EncryptImageDirectory(
    @"C:\Users\MyUser\Pictures\Private",
    @"C:\Users\MyUser\Pictures\Private\Encrypted",
    "MyStrongPassword123!"
);
```

### 시나리오 2: 업무 문서 보안
```csharp
// 업무 이미지 파일들을 개별 암호화
string[] businessImages = {
    @"C:\Work\Screenshots\confidential1.png",
    @"C:\Work\Screenshots\confidential2.jpg"
};

foreach (string image in businessImages)
{
    LocalImageEncryption.EncryptImageFileInPlace(image, "WorkPassword456!");
}
```

### 시나리오 3: 백업 파일 복호화
```csharp
// 백업에서 복원된 암호화된 파일들을 복호화
LocalImageEncryption.DecryptImageDirectory(
    @"C:\Backup\EncryptedImages",
    @"C:\Restored\Images",
    "BackupPassword789!"
);
```

## 라이선스

이 프로젝트는 기존 MS-SQL-Image_convert 프로젝트의 라이선스를 따릅니다.

## 지원

문제가 발생하거나 개선 사항이 있으면 프로젝트 이슈를 통해 문의해 주세요. 