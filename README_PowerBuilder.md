# PowerBuilder에서 .NET SDK 사용 가이드

이 문서는 PowerBuilder에서 MS-SQL-Image_convert 라이브러리를 .NET SDK로 사용하는 방법을 설명합니다.

## 개요

MS-SQL-Image_convert 라이브러리는 PowerBuilder에서 COM 인터페이스를 통해 사용할 수 있도록 설계되었습니다. 로컬 경로에 저장된 이미지 파일을 AES-256 GCM 암호화로 암호화하고 복호화할 수 있습니다.

## 설치 및 등록

### 1. 라이브러리 빌드

```bash
# 프로젝트 빌드
dotnet build --configuration Release

# COM 등록 (관리자 권한 필요)
regasm MS-SQL-Image_convert.dll /tlb:MS-SQL-Image_convert.tlb /codebase
```

### 2. GAC 등록 (선택사항)

```bash
# GAC에 등록
gacutil -i MS-SQL-Image_convert.dll
```

## PowerBuilder에서 사용하기

### 1. OLE Object 선언

PowerBuilder에서 다음과 같이 OLE Object를 선언합니다:

```powerbuilder
// 전역 변수로 선언
OLEObject oleImageEncrypt

// 또는 인스턴스 변수로 선언
OLEObject iole_ImageEncrypt
```

### 2. 객체 생성 및 초기화

```powerbuilder
// 객체 생성
oleImageEncrypt = CREATE OLEObject

// COM 객체 연결
IF oleImageEncrypt.ConnectToNewObject("MS_SQL_Image_convert.PowerBuilderWrapper") = 0 THEN
    // 성공적으로 연결됨
    MessageBox("연결 성공", "라이브러리가 성공적으로 로드되었습니다.")
ELSE
    // 연결 실패
    MessageBox("연결 실패", "라이브러리를 로드할 수 없습니다.")
    RETURN
END IF
```

### 3. 기본 사용 예제

#### 단일 파일 암호화

```powerbuilder
string ls_source_path, ls_target_path, ls_password
integer li_result

ls_source_path = "C:\Images\photo.jpg"
ls_target_path = "C:\Encrypted\photo.jpg.encrypted"
ls_password = "MySecretPassword123!"

// 파일 암호화
li_result = oleImageEncrypt.EncryptFile(ls_source_path, ls_target_path, ls_password)

IF li_result = 1 THEN
    MessageBox("성공", "파일이 성공적으로 암호화되었습니다.")
ELSE
    MessageBox("실패", "파일 암호화에 실패했습니다.")
END IF
```

#### 단일 파일 복호화

```powerbuilder
string ls_source_path, ls_target_path, ls_password
integer li_result

ls_source_path = "C:\Encrypted\photo.jpg.encrypted"
ls_target_path = "C:\Decrypted\photo.jpg"
ls_password = "MySecretPassword123!"

// 파일 복호화
li_result = oleImageEncrypt.DecryptFile(ls_source_path, ls_target_path, ls_password)

IF li_result = 1 THEN
    MessageBox("성공", "파일이 성공적으로 복호화되었습니다.")
ELSE
    MessageBox("실패", "파일 복호화에 실패했습니다.")
END IF
```

#### 자동 확장자 처리

```powerbuilder
string ls_file_path, ls_encrypted_path, ls_password

ls_file_path = "C:\Images\photo.jpg"
ls_password = "MySecretPassword123!"

// 같은 위치에 .encrypted 확장자로 암호화
ls_encrypted_path = oleImageEncrypt.EncryptFileInPlace(ls_file_path, ls_password)

IF ls_encrypted_path <> "" THEN
    MessageBox("성공", "암호화된 파일: " + ls_encrypted_path)
ELSE
    MessageBox("실패", "암호화에 실패했습니다.")
END IF
```

#### 디렉토리 일괄 암호화

```powerbuilder
string ls_source_dir, ls_target_dir, ls_password
integer li_count

ls_source_dir = "C:\Images"
ls_target_dir = "C:\Encrypted"
ls_password = "MySecretPassword123!"

// 디렉토리 내 모든 이미지 파일 암호화
li_count = oleImageEncrypt.EncryptDirectory(ls_source_dir, ls_target_dir, ls_password)

MessageBox("완료", "총 " + String(li_count) + "개 파일이 암호화되었습니다.")
```

#### 이미지 정보 조회

```powerbuilder
string ls_file_path, ls_info

ls_file_path = "C:\Images\photo.jpg"

// 이미지 정보 가져오기
ls_info = oleImageEncrypt.GetImageInfo(ls_file_path)

IF ls_info <> "" THEN
    MessageBox("이미지 정보", ls_info)
ELSE
    MessageBox("오류", "이미지 정보를 가져올 수 없습니다.")
END IF
```

### 4. 유틸리티 함수들

#### 파일 존재 확인

```powerbuilder
string ls_file_path
integer li_exists

ls_file_path = "C:\Images\photo.jpg"
li_exists = oleImageEncrypt.FileExists(ls_file_path)

IF li_exists = 1 THEN
    MessageBox("확인", "파일이 존재합니다.")
ELSE
    MessageBox("확인", "파일이 존재하지 않습니다.")
END IF
```

#### 디렉토리 생성

```powerbuilder
string ls_dir_path
integer li_result

ls_dir_path = "C:\NewFolder"
li_result = oleImageEncrypt.CreateDirectory(ls_dir_path)

IF li_result = 1 THEN
    MessageBox("성공", "디렉토리가 생성되었습니다.")
ELSE
    MessageBox("실패", "디렉토리 생성에 실패했습니다.")
END IF
```

#### 파일 크기 확인

```powerbuilder
string ls_file_path
long ll_size

ls_file_path = "C:\Images\photo.jpg"
ll_size = oleImageEncrypt.GetFileSize(ls_file_path)

IF ll_size >= 0 THEN
    MessageBox("파일 크기", "파일 크기: " + String(ll_size) + " bytes")
ELSE
    MessageBox("오류", "파일 크기를 가져올 수 없습니다.")
END IF
```

## 완전한 PowerBuilder 예제

### 윈도우 이벤트 (Window Events)

```powerbuilder
// 윈도우 열기 이벤트
OLEObject oleImageEncrypt

oleImageEncrypt = CREATE OLEObject

IF oleImageEncrypt.ConnectToNewObject("MS_SQL_Image_convert.PowerBuilderWrapper") = 0 THEN
    MessageBox("연결 성공", "이미지 암호화 라이브러리가 로드되었습니다.")
ELSE
    MessageBox("연결 실패", "라이브러리를 로드할 수 없습니다. COM 등록을 확인하세요.")
END IF
```

### 버튼 클릭 이벤트

```powerbuilder
// 암호화 버튼 클릭 이벤트
string ls_source, ls_target, ls_password
integer li_result

ls_source = sle_source.text
ls_target = sle_target.text
ls_password = sle_password.text

IF ls_source = "" OR ls_target = "" OR ls_password = "" THEN
    MessageBox("입력 오류", "모든 필드를 입력해주세요.")
    RETURN
END IF

li_result = oleImageEncrypt.EncryptFile(ls_source, ls_target, ls_password)

IF li_result = 1 THEN
    MessageBox("성공", "파일이 성공적으로 암호화되었습니다.")
    sle_result.text = "암호화 완료: " + ls_target
ELSE
    MessageBox("실패", "파일 암호화에 실패했습니다.")
    sle_result.text = "암호화 실패"
END IF
```

### 윈도우 닫기 이벤트

```powerbuilder
// 윈도우 닫기 이벤트
IF IsValid(oleImageEncrypt) THEN
    oleImageEncrypt.DisconnectObject()
    DESTROY oleImageEncrypt
END IF
```

## 오류 처리

### 기본 오류 처리

```powerbuilder
// 모든 함수 호출 후 결과 확인
integer li_result
string ls_result

li_result = oleImageEncrypt.EncryptFile(ls_source, ls_target, ls_password)

IF li_result = 0 THEN
    // 오류 발생
    IF oleImageEncrypt.HasError() = 1 THEN
        ls_result = oleImageEncrypt.LastErrorMessage
        MessageBox("오류", ls_result)
    ELSE
        MessageBox("오류", "알 수 없는 오류가 발생했습니다.")
    END IF
END IF
```

### 예외 처리

```powerbuilder
TRY
    li_result = oleImageEncrypt.EncryptFile(ls_source, ls_target, ls_password)
    IF li_result = 1 THEN
        MessageBox("성공", "암호화 완료")
    END IF
CATCH (RuntimeError re)
    MessageBox("런타임 오류", re.GetMessage())
CATCH (Exception e)
    MessageBox("일반 오류", e.GetMessage())
END TRY
```

## 성능 최적화

### 1. 객체 재사용

```powerbuilder
// 전역 변수로 선언하여 재사용
OLEObject g_oleImageEncrypt

// 애플리케이션 시작 시 한 번만 생성
IF NOT IsValid(g_oleImageEncrypt) THEN
    g_oleImageEncrypt = CREATE OLEObject
    g_oleImageEncrypt.ConnectToNewObject("MS_SQL_Image_convert.PowerBuilderWrapper")
END IF
```

### 2. 배치 처리

```powerbuilder
// 여러 파일을 한 번에 처리
string ls_source_dir, ls_target_dir
integer li_count

ls_source_dir = "C:\Images"
ls_target_dir = "C:\Encrypted"

// 디렉토리 일괄 처리 (개별 파일 처리보다 효율적)
li_count = g_oleImageEncrypt.EncryptDirectory(ls_source_dir, ls_target_dir, ls_password)
```

## 지원되는 이미지 형식

- JPEG (.jpg, .jpeg)
- PNG (.png)
- BMP (.bmp)
- GIF (.gif)
- TIFF (.tiff)
- 기타 System.Drawing에서 지원하는 모든 이미지 형식

## 보안 고려사항

1. **강력한 비밀번호 사용**: 대소문자, 숫자, 특수문자를 포함한 강력한 비밀번호 사용
2. **비밀번호 보안**: 소스 코드에 비밀번호를 하드코딩하지 않음
3. **파일 권한**: 암호화된 파일에 대한 적절한 파일 시스템 권한 설정
4. **백업**: 원본 파일의 정기적인 백업 수행

## 문제 해결

### COM 등록 오류

```bash
# 관리자 권한으로 명령 프롬프트 실행 후
regasm MS-SQL-Image_convert.dll /tlb:MS-SQL-Image_convert.tlb /codebase
```

### 권한 오류

- PowerBuilder를 관리자 권한으로 실행
- 대상 폴더에 대한 쓰기 권한 확인

### 파일 경로 오류

- 절대 경로 사용 권장
- 경로에 특수문자가 없는지 확인
- 파일명에 유효하지 않은 문자가 없는지 확인

## 라이선스

이 프로젝트는 기존 MS-SQL-Image_convert 프로젝트의 라이선스를 따릅니다. 