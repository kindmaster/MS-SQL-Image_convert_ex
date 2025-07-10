# PowerBuilder에서 MS-SQL-Image_convert DLL 사용법 및 예시

이 문서는 PowerBuilder의 .NET DLL Importer로 MS-SQL-Image_convert.dll을 등록한 후 사용하는 방법과 주요 메서드, 실전 예시를 정리한 문서입니다.

---

## 1. 기본 사용 흐름

1. **.NET DLL Importer로 DLL 등록**
2. PBL(라이브러리)에 생성된 NonVisualObject(`nvo_powerbuilderwrapper` 등) 사용
3. DotNetObject 메서드 호출로 암호화/복호화 등 기능 활용

---

## 2. 인스턴스 생성 및 기본 사용 예시

```powerbuilder
// 1. NonVisualObject 인스턴스 생성
nvo_powerbuilderwrapper lnv_encrypt
lnv_encrypt = CREATE nvo_powerbuilderwrapper

// 2. 파일 암호화
string ls_source, ls_target, ls_password
integer li_result

ls_source = "C:\\Images\\photo.jpg"
ls_target = "C:\\Encrypted\\photo.jpg.encrypted"
ls_password = "MySecretPassword123!"

li_result = lnv_encrypt.of_EncryptFile(ls_source, ls_target, ls_password)

IF li_result = 1 THEN
    MessageBox("성공", "파일이 성공적으로 암호화되었습니다.")
ELSE
    MessageBox("실패", "파일 암호화에 실패했습니다.")
END IF
```

---

## 3. 주요 메서드 요약

| PowerBuilder 메서드명         | 기능 설명                |
|------------------------------|--------------------------|
| of_EncryptFile               | 파일 암호화              |
| of_DecryptFile               | 파일 복호화              |
| of_EncryptFileInPlace        | 같은 위치에 .encrypted   |
| of_DecryptFileInPlace        | .encrypted → 원본 복호화 |
| of_EncryptDirectory          | 디렉토리 일괄 암호화     |
| of_DecryptDirectory          | 디렉토리 일괄 복호화     |
| of_GetImageInfo              | 이미지 정보 조회         |
| of_FileExists                | 파일 존재 확인           |
| of_DirectoryExists           | 디렉토리 존재 확인       |
| of_CreateDirectory           | 디렉토리 생성            |
| of_GetFileSize               | 파일 크기(바이트)        |

---

## 4. 추가 사용 예시

### 1) 파일 복호화
```powerbuilder
string ls_source, ls_target, ls_password
integer li_result

ls_source = "C:\\Encrypted\\photo.jpg.encrypted"
ls_target = "C:\\Decrypted\\photo.jpg"
ls_password = "MySecretPassword123!"

li_result = lnv_encrypt.of_DecryptFile(ls_source, ls_target, ls_password)

IF li_result = 1 THEN
    MessageBox("성공", "파일이 성공적으로 복호화되었습니다.")
ELSE
    MessageBox("실패", "파일 복호화에 실패했습니다.")
END IF
```

### 2) 이미지 정보 조회
```powerbuilder
string ls_info
ls_info = lnv_encrypt.of_GetImageInfo("C:\\Images\\photo.jpg")
MessageBox("이미지 정보", ls_info)
```

### 3) 디렉토리 일괄 암호화
```powerbuilder
string ls_source_dir, ls_target_dir, ls_password
integer li_count

ls_source_dir = "C:\\Images"
ls_target_dir = "C:\\Encrypted"
ls_password = "MySecretPassword123!"

li_count = lnv_encrypt.of_EncryptDirectory(ls_source_dir, ls_target_dir, ls_password)
MessageBox("완료", "총 " + String(li_count) + "개 파일이 암호화되었습니다.")
```

### 4) 파일 존재 확인
```powerbuilder
string ls_file_path
integer li_exists

ls_file_path = "C:\\Images\\photo.jpg"
li_exists = lnv_encrypt.of_FileExists(ls_file_path)

IF li_exists = 1 THEN
    MessageBox("확인", "파일이 존재합니다.")
ELSE
    MessageBox("확인", "파일이 존재하지 않습니다.")
END IF
```

---

## 5. 실전 팁

- 메서드명 앞에 `of_`가 붙는 경우가 많으니, Import된 오브젝트의 메서드 목록을 확인하세요.
- 경로는 항상 **절대경로**로 입력하는 것이 좋습니다.
- 암호화/복호화 실패 시, 반환값이 0이면 예외 메시지를 별도 프로퍼티(예: `of_LastErrorMessage`)로 확인할 수 있습니다.
- PowerBuilder 32비트 환경에서만 정상 동작합니다.

---

## 6. 전체 예제 (윈도우 오픈 이벤트)

```powerbuilder
nvo_powerbuilderwrapper lnv_encrypt
lnv_encrypt = CREATE nvo_powerbuilderwrapper

string ls_source, ls_target, ls_password
integer li_result

ls_source = "C:\\Images\\photo.jpg"
ls_target = "C:\\Encrypted\\photo.jpg.encrypted"
ls_password = "MySecretPassword123!"

li_result = lnv_encrypt.of_EncryptFile(ls_source, ls_target, ls_password)

IF li_result = 1 THEN
    MessageBox("성공", "파일이 성공적으로 암호화되었습니다.")
ELSE
    MessageBox("실패", "파일 암호화에 실패했습니다.")
END IF
```

---

궁금한 점이 있거나, 특정 메서드 사용법이 더 필요하면 언제든 문의해 주세요! 




아래 해당 소스는 아래 git fork 해서 기능을 추가 한 버전입니다.

별도의 함수를 추가 했기 때문에 아래 git의 기능은 그대로 사용 가능합니다.
https://github.com/yuseok-kim-edushare/MS-SQL-Image_convert

아래는 원본 git readme.md 내용임

# MS-SQL-Image_convert
This Repository is for convert image inplace in MS-SQL Server.
convert format or en/decrypt and so on.

## Features

- Convert Image byte stream to specific format byte stream (like jpg, png)

## Requirements

- Windows 10 20H2 or Later (server 2022 or later)
  - .NET Framework 4.8.1
- Not Ensured, but it should work with .NET 4.8 and windows 7 or later
  - MS introduce no comapatibility change from .NET 4.8 to .NET 4.8.1
    - then, it should work with .NET 4.8 and windows 7 or later

## Building the Library

first, you need to install .NET Framework 4.8.1 SDK.
also if you want to use dotnet cli, you need to install .NET 8+ SDK.

1. Open the solution in Visual Studio 2022+
2. Build the solution in Release mode
3. if you want to build with dotnet cli(cause of not having visual studio)
   ```powershell
   dotnet build MS-SQL-Image_convert.csproj --configuration Release
   ```
4. **For Production Use** I recommand to use your own key file. for avoid malicious copy install.
    - but, hopefully, github release artifact hash can easily see on github release page.
        - So, you can download the dll and check the hash.
    - And, you can create key file with `sn -k MS-SQL-Image_convert.snk`
        - `sn` is a tool for creating and managing strong names. provided by .NET Framework SDK.
            - you can find it in `C:\Program Files (x86)\Microsoft SDKs\Windows\v10.0A\bin\NETFX 4.8.1 Tools\`

## Functions Available

### 1. ConvertToJpg
Converts any image format to JPEG with customizable quality.
```sql
dbo.ConvertToJpg(@imageData VARBINARY(MAX), @quality INT = 85) RETURNS VARBINARY(MAX)
```
- `@imageData`: The source image binary data
- `@quality`: JPEG quality (1-100, default: 85)

### 2. ConvertToPng
Converts any image format to PNG.
```sql
dbo.ConvertToPng(@imageData VARBINARY(MAX)) RETURNS VARBINARY(MAX)
```
- `@imageData`: The source image binary data

### 3. ResizeImage
Resizes an image to specified dimensions with optional aspect ratio preservation.
```sql
dbo.ResizeImage(@imageData VARBINARY(MAX), @width INT, @height INT, @maintainAspectRatio BIT = 1) RETURNS VARBINARY(MAX)
```
- `@imageData`: The source image binary data
- `@width`: Target width in pixels
- `@height`: Target height in pixels
- `@maintainAspectRatio`: 1 to maintain aspect ratio, 0 to stretch (default: 1)

### 4. ReduceImageSize
Reduces image file size by applying compression and optionally resizing.
**This Function convert image to jpeg format to reduce size by compression.**
```sql
dbo.ReduceImageSize(@imageData VARBINARY(MAX), @maxSizeKB INT = 100, @jpegQuality INT = 85) RETURNS VARBINARY(MAX)
```
- `@imageData`: The source image binary data
- `@maxSizeKB`: Maximum output size in KB (default: 100)
- `@jpegQuality`: JPEG compression quality (1-100, default: 85)

### 5. EncryptImage
Encrypts image data using AES-256 GCM encryption.
```sql
dbo.EncryptImage(@imageData VARBINARY(MAX), @password NVARCHAR(MAX)) RETURNS VARBINARY(MAX)
```
- `@imageData`: The image to encrypt
- `@password`: Encryption password

### 6. DecryptImage
Decrypts previously encrypted image data.
```sql
dbo.DecryptImage(@encryptedData VARBINARY(MAX), @password NVARCHAR(MAX)) RETURNS VARBINARY(MAX)
```
- `@encryptedData`: The encrypted image data
- `@password`: Decryption password (must match encryption password)

### 7. GetImageInfo
Returns detailed information about an image.
```sql
dbo.GetImageInfo(@imageData VARBINARY(MAX)) RETURNS NVARCHAR(MAX)
```
- `@imageData`: The image to analyze
- Returns: Format, dimensions, size, resolution, and pixel format

## Installation

1. Build the project to generate the DLL
2. Copy the DLL to your SQL Server
3. **Deploy to databases**: See [Deployment Guide](README_deployment.md) for multiple deployment options
4. Ensure CLR is enabled

### Quick Start Deployment

For the easiest deployment experience, use the **Single Database Script**:

1. Use the `deploy_single_db.sql` script
2. Edit the `@target_db` variable for each database you want to deploy to
3. Run the script once per database

📖 **Full deployment documentation**: [README_deployment.md](README_deployment.md)

## Usage Examples

### Convert PNG to JPG
```sql
UPDATE MyImages
SET ImageData = dbo.ConvertToJpg(ImageData, 90)
WHERE ImageFormat = 'PNG';
```

### Create thumbnails
```sql
SELECT 
    ImageId,
    dbo.ResizeImage(FullImage, 150, 150, 1) AS Thumbnail
FROM ProductImages;
```

### Encrypt sensitive images
```sql
UPDATE SensitiveDocuments
SET ImageData = dbo.EncryptImage(ImageData, 'StrongPassword123!');
```

### Reduce storage size
```sql
UPDATE LargeImages
SET ImageData = dbo.ReduceImageSize(ImageData, 500, 80)
WHERE DATALENGTH(ImageData) > 1024 * 1024; -- Images larger than 1MB
```

## Security Considerations

- The assembly requires UNSAFE permission due to System.Drawing usage
- Store encryption passwords securely, not in plain text
- Consider using SQL Server's built-in encryption for password storage
- Test thoroughly in a non-production environment first

## Performance Tips

- Process images in batches during off-peak hours
- Consider creating a separate filegroup for image data
- Monitor tempdb usage during large batch operations
- Use appropriate indexes on tables containing images

## Troubleshooting

- If you get "Assembly not found" errors, ensure the DLL path is correct
- For "Permission denied" errors, check TRUSTWORTHY setting and CLR permissions
- For out of memory errors, process images in smaller batches
- Check SQL Server error log for detailed CLR error messages


