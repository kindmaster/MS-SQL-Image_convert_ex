using System;
using System.IO;
using System.Runtime.InteropServices;

namespace MS_SQL_Image_convert
{
    /// <summary>
    /// PowerBuilder에서 사용하기 위한 래퍼 클래스
    /// 간단한 메서드명과 명확한 반환값을 제공
    /// </summary>
    [ComVisible(true)]
    [Guid("87654321-4321-4321-4321-210987654321")]
    [ClassInterface(ClassInterfaceType.AutoDual)]
    [ProgId("MS_SQL_Image_convert.PowerBuilderWrapper")]
    public class PowerBuilderWrapper
    {
        /// <summary>
        /// 이미지 파일 암호화 (간단한 형태)
        /// </summary>
        /// <param name="sourcePath">원본 파일 경로</param>
        /// <param name="targetPath">암호화된 파일 저장 경로</param>
        /// <param name="password">비밀번호</param>
        /// <returns>성공시 1, 실패시 0</returns>
        [ComVisible(true)]
        public int EncryptFile(string sourcePath, string targetPath, string password)
        {
            try
            {
                bool result = LocalImageEncryption.EncryptImageFile(sourcePath, targetPath, password);
                return result ? 1 : 0;
            }
            catch
            {
                return 0;
            }
        }

        /// <summary>
        /// 이미지 파일 복호화 (간단한 형태)
        /// </summary>
        /// <param name="sourcePath">암호화된 파일 경로</param>
        /// <param name="targetPath">복호화된 파일 저장 경로</param>
        /// <param name="password">비밀번호</param>
        /// <returns>성공시 1, 실패시 0</returns>
        [ComVisible(true)]
        public int DecryptFile(string sourcePath, string targetPath, string password)
        {
            try
            {
                bool result = LocalImageEncryption.DecryptImageFile(sourcePath, targetPath, password);
                return result ? 1 : 0;
            }
            catch
            {
                return 0;
            }
        }

        /// <summary>
        /// 파일을 같은 위치에 .encrypted 확장자로 암호화
        /// </summary>
        /// <param name="filePath">암호화할 파일 경로</param>
        /// <param name="password">비밀번호</param>
        /// <returns>암호화된 파일 경로 (실패시 빈 문자열)</returns>
        [ComVisible(true)]
        public string EncryptFileInPlace(string filePath, string password)
        {
            try
            {
                return LocalImageEncryption.EncryptImageFileInPlace(filePath, password);
            }
            catch
            {
                return "";
            }
        }

        /// <summary>
        /// .encrypted 확장자 파일을 원본으로 복호화
        /// </summary>
        /// <param name="encryptedFilePath">암호화된 파일 경로</param>
        /// <param name="password">비밀번호</param>
        /// <returns>복호화된 파일 경로 (실패시 빈 문자열)</returns>
        [ComVisible(true)]
        public string DecryptFileInPlace(string encryptedFilePath, string password)
        {
            try
            {
                return LocalImageEncryption.DecryptImageFileInPlace(encryptedFilePath, password);
            }
            catch
            {
                return "";
            }
        }

        /// <summary>
        /// 디렉토리 내 모든 이미지 파일 암호화
        /// </summary>
        /// <param name="sourceDir">소스 디렉토리</param>
        /// <param name="targetDir">대상 디렉토리</param>
        /// <param name="password">비밀번호</param>
        /// <returns>암호화된 파일 개수</returns>
        [ComVisible(true)]
        public int EncryptDirectory(string sourceDir, string targetDir, string password)
        {
            try
            {
                return LocalImageEncryption.EncryptImageDirectory(sourceDir, targetDir, password);
            }
            catch
            {
                return 0;
            }
        }

        /// <summary>
        /// 디렉토리 내 모든 암호화된 파일 복호화
        /// </summary>
        /// <param name="sourceDir">암호화된 파일 디렉토리</param>
        /// <param name="targetDir">복호화된 파일 저장 디렉토리</param>
        /// <param name="password">비밀번호</param>
        /// <returns>복호화된 파일 개수</returns>
        [ComVisible(true)]
        public int DecryptDirectory(string sourceDir, string targetDir, string password)
        {
            try
            {
                return LocalImageEncryption.DecryptImageDirectory(sourceDir, targetDir, password);
            }
            catch
            {
                return 0;
            }
        }

        /// <summary>
        /// 이미지 파일 정보 가져오기
        /// </summary>
        /// <param name="filePath">파일 경로</param>
        /// <returns>이미지 정보 (실패시 빈 문자열)</returns>
        [ComVisible(true)]
        public string GetImageInfo(string filePath)
        {
            try
            {
                return LocalImageEncryption.GetImageFileInfo(filePath);
            }
            catch
            {
                return "";
            }
        }

        /// <summary>
        /// 파일 존재 여부 확인
        /// </summary>
        /// <param name="filePath">파일 경로</param>
        /// <returns>존재시 1, 없으면 0</returns>
        [ComVisible(true)]
        public int FileExists(string filePath)
        {
            try
            {
                return LocalImageEncryption.FileExists(filePath) ? 1 : 0;
            }
            catch
            {
                return 0;
            }
        }

        /// <summary>
        /// 디렉토리 존재 여부 확인
        /// </summary>
        /// <param name="dirPath">디렉토리 경로</param>
        /// <returns>존재시 1, 없으면 0</returns>
        [ComVisible(true)]
        public int DirectoryExists(string dirPath)
        {
            try
            {
                return LocalImageEncryption.DirectoryExists(dirPath) ? 1 : 0;
            }
            catch
            {
                return 0;
            }
        }

        /// <summary>
        /// 디렉토리 생성
        /// </summary>
        /// <param name="dirPath">생성할 디렉토리 경로</param>
        /// <returns>성공시 1, 실패시 0</returns>
        [ComVisible(true)]
        public int CreateDirectory(string dirPath)
        {
            try
            {
                return LocalImageEncryption.CreateDirectory(dirPath) ? 1 : 0;
            }
            catch
            {
                return 0;
            }
        }

        /// <summary>
        /// 파일 크기 가져오기
        /// </summary>
        /// <param name="filePath">파일 경로</param>
        /// <returns>파일 크기 (바이트), 실패시 -1</returns>
        [ComVisible(true)]
        public long GetFileSize(string filePath)
        {
            try
            {
                return LocalImageEncryption.GetFileSize(filePath);
            }
            catch
            {
                return -1;
            }
        }

        /// <summary>
        /// 마지막 오류 메시지 (PowerBuilder에서 사용)
        /// </summary>
        [ComVisible(true)]
        public string LastErrorMessage { get; set; } = "";

        /// <summary>
        /// 오류 발생 여부 확인
        /// </summary>
        /// <returns>오류 발생시 1, 없으면 0</returns>
        [ComVisible(true)]
        public int HasError()
        {
            return string.IsNullOrEmpty(LastErrorMessage) ? 0 : 1;
        }

        /// <summary>
        /// 오류 메시지 초기화
        /// </summary>
        [ComVisible(true)]
        public void ClearError()
        {
            LastErrorMessage = "";
        }

        /// <summary>
        /// 라이브러리 버전 정보
        /// </summary>
        /// <returns>버전 문자열</returns>
        [ComVisible(true)]
        public string GetVersion()
        {
            return "1.0.0";
        }

        /// <summary>
        /// 지원하는 이미지 확장자 목록
        /// </summary>
        /// <returns>확장자 목록 문자열</returns>
        [ComVisible(true)]
        public string GetSupportedExtensions()
        {
            return "*.jpg, *.jpeg, *.png, *.bmp, *.gif, *.tiff";
        }
    }
} 