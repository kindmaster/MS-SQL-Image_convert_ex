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