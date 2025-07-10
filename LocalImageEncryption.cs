using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Runtime.InteropServices;

namespace MS_SQL_Image_convert
{
    /// <summary>
    /// 로컬 경로에 저장된 이미지 파일을 암호화/복호화하는 클래스
    /// PowerBuilder에서 .NET SDK로 사용할 수 있도록 설계됨
    /// </summary>
    [ComVisible(true)]
    [Guid("12345678-1234-1234-1234-123456789012")]
    [ClassInterface(ClassInterfaceType.AutoDual)]
    public class LocalImageEncryption
    {
        /// <summary>
        /// 로컬 경로의 이미지 파일을 암호화하여 지정된 경로에 저장
        /// </summary>
        /// <param name="sourceImagePath">암호화할 이미지 파일 경로</param>
        /// <param name="encryptedImagePath">암호화된 파일을 저장할 경로</param>
        /// <param name="password">암호화 비밀번호</param>
        /// <returns>암호화 성공 여부</returns>
        [ComVisible(true)]
        public static bool EncryptImageFile(string sourceImagePath, string encryptedImagePath, string password)
        {
            try
            {
                // 입력 파일 존재 확인
                if (!File.Exists(sourceImagePath))
                {
                    throw new FileNotFoundException($"소스 이미지 파일을 찾을 수 없습니다: {sourceImagePath}");
                }

                // 비밀번호 유효성 검사
                if (string.IsNullOrEmpty(password))
                {
                    throw new ArgumentException("비밀번호는 null이거나 비어있을 수 없습니다.");
                }

                // 소스 파일 읽기
                byte[] imageData = File.ReadAllBytes(sourceImagePath);

                // 이미지 데이터 암호화
                byte[] encryptedData = BcryptInterop.EncryptAesGcmBytes(imageData, password);

                // 암호화된 데이터를 파일로 저장
                File.WriteAllBytes(encryptedImagePath, encryptedData);

                return true;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"이미지 파일 암호화 중 오류 발생: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// 암호화된 이미지 파일을 복호화하여 지정된 경로에 저장
        /// </summary>
        /// <param name="encryptedImagePath">복호화할 암호화된 이미지 파일 경로</param>
        /// <param name="decryptedImagePath">복호화된 파일을 저장할 경로</param>
        /// <param name="password">복호화 비밀번호</param>
        /// <returns>복호화 성공 여부</returns>
        [ComVisible(true)]
        public static bool DecryptImageFile(string encryptedImagePath, string decryptedImagePath, string password)
        {
            try
            {
                // 입력 파일 존재 확인
                if (!File.Exists(encryptedImagePath))
                {
                    throw new FileNotFoundException($"암호화된 이미지 파일을 찾을 수 없습니다: {encryptedImagePath}");
                }

                // 비밀번호 유효성 검사
                if (string.IsNullOrEmpty(password))
                {
                    throw new ArgumentException("비밀번호는 null이거나 비어있을 수 없습니다.");
                }

                // 암호화된 파일 읽기
                byte[] encryptedData = File.ReadAllBytes(encryptedImagePath);

                // 암호화된 데이터 복호화
                byte[] decryptedData = BcryptInterop.DecryptAesGcmBytes(encryptedData, password);

                // 복호화된 데이터를 파일로 저장
                File.WriteAllBytes(decryptedImagePath, decryptedData);

                return true;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"이미지 파일 복호화 중 오류 발생: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// 로컬 경로의 이미지 파일을 암호화하여 같은 경로에 .encrypted 확장자로 저장
        /// </summary>
        /// <param name="imagePath">암호화할 이미지 파일 경로</param>
        /// <param name="password">암호화 비밀번호</param>
        /// <returns>암호화된 파일 경로</returns>
        [ComVisible(true)]
        public static string EncryptImageFileInPlace(string imagePath, string password)
        {
            try
            {
                string encryptedPath = imagePath + ".encrypted";
                bool success = EncryptImageFile(imagePath, encryptedPath, password);
                
                if (success)
                {
                    return encryptedPath;
                }
                else
                {
                    throw new InvalidOperationException("이미지 파일 암호화에 실패했습니다.");
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"이미지 파일 암호화 중 오류 발생: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// 암호화된 이미지 파일을 복호화하여 원본 확장자로 저장
        /// </summary>
        /// <param name="encryptedImagePath">복호화할 암호화된 이미지 파일 경로</param>
        /// <param name="password">복호화 비밀번호</param>
        /// <returns>복호화된 파일 경로</returns>
        [ComVisible(true)]
        public static string DecryptImageFileInPlace(string encryptedImagePath, string password)
        {
            try
            {
                // .encrypted 확장자 제거
                string decryptedPath = encryptedImagePath.Replace(".encrypted", "");
                
                bool success = DecryptImageFile(encryptedImagePath, decryptedPath, password);
                
                if (success)
                {
                    return decryptedPath;
                }
                else
                {
                    throw new InvalidOperationException("이미지 파일 복호화에 실패했습니다.");
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"이미지 파일 복호화 중 오류 발생: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// 디렉토리 내의 모든 이미지 파일을 일괄 암호화
        /// </summary>
        /// <param name="sourceDirectory">소스 디렉토리 경로</param>
        /// <param name="outputDirectory">출력 디렉토리 경로 (null이면 소스 디렉토리에 저장)</param>
        /// <param name="password">암호화 비밀번호</param>
        /// <param name="imageExtensions">이미지 확장자 배열 (기본값: jpg, jpeg, png, bmp, gif)</param>
        /// <returns>암호화된 파일 개수</returns>
        [ComVisible(true)]
        public static int EncryptImageDirectory(string sourceDirectory, string outputDirectory, string password, string[] imageExtensions = null)
        {
            try
            {
                if (!Directory.Exists(sourceDirectory))
                {
                    throw new DirectoryNotFoundException($"소스 디렉토리를 찾을 수 없습니다: {sourceDirectory}");
                }

                if (string.IsNullOrEmpty(password))
                {
                    throw new ArgumentException("비밀번호는 null이거나 비어있을 수 없습니다.");
                }

                // 기본 이미지 확장자 설정
                if (imageExtensions == null)
                {
                    imageExtensions = new string[] { "*.jpg", "*.jpeg", "*.png", "*.bmp", "*.gif" };
                }

                // 출력 디렉토리 생성
                if (!string.IsNullOrEmpty(outputDirectory) && !Directory.Exists(outputDirectory))
                {
                    Directory.CreateDirectory(outputDirectory);
                }

                int encryptedCount = 0;

                foreach (string extension in imageExtensions)
                {
                    string[] files = Directory.GetFiles(sourceDirectory, extension, SearchOption.TopDirectoryOnly);
                    
                    foreach (string file in files)
                    {
                        try
                        {
                            string fileName = Path.GetFileName(file);
                            string outputPath;

                            if (!string.IsNullOrEmpty(outputDirectory))
                            {
                                outputPath = Path.Combine(outputDirectory, fileName + ".encrypted");
                            }
                            else
                            {
                                outputPath = file + ".encrypted";
                            }

                            bool success = EncryptImageFile(file, outputPath, password);
                            if (success)
                            {
                                encryptedCount++;
                            }
                        }
                        catch (Exception ex)
                        {
                            // 개별 파일 오류는 로그만 남기고 계속 진행
                            System.Diagnostics.Debug.WriteLine($"파일 암호화 실패 {Path.GetFileName(file)}: {ex.Message}");
                        }
                    }
                }

                return encryptedCount;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"디렉토리 암호화 중 오류 발생: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// 디렉토리 내의 모든 암호화된 이미지 파일을 일괄 복호화
        /// </summary>
        /// <param name="sourceDirectory">소스 디렉토리 경로</param>
        /// <param name="outputDirectory">출력 디렉토리 경로 (null이면 소스 디렉토리에 저장)</param>
        /// <param name="password">복호화 비밀번호</param>
        /// <returns>복호화된 파일 개수</returns>
        [ComVisible(true)]
        public static int DecryptImageDirectory(string sourceDirectory, string outputDirectory, string password)
        {
            try
            {
                if (!Directory.Exists(sourceDirectory))
                {
                    throw new DirectoryNotFoundException($"소스 디렉토리를 찾을 수 없습니다: {sourceDirectory}");
                }

                if (string.IsNullOrEmpty(password))
                {
                    throw new ArgumentException("비밀번호는 null이거나 비어있을 수 없습니다.");
                }

                // 출력 디렉토리 생성
                if (!string.IsNullOrEmpty(outputDirectory) && !Directory.Exists(outputDirectory))
                {
                    Directory.CreateDirectory(outputDirectory);
                }

                string[] encryptedFiles = Directory.GetFiles(sourceDirectory, "*.encrypted", SearchOption.TopDirectoryOnly);
                int decryptedCount = 0;

                foreach (string file in encryptedFiles)
                {
                    try
                    {
                        string fileName = Path.GetFileName(file);
                        string originalFileName = fileName.Replace(".encrypted", "");
                        string outputPath;

                        if (!string.IsNullOrEmpty(outputDirectory))
                        {
                            outputPath = Path.Combine(outputDirectory, originalFileName);
                        }
                        else
                        {
                            outputPath = file.Replace(".encrypted", "");
                        }

                        bool success = DecryptImageFile(file, outputPath, password);
                        if (success)
                        {
                            decryptedCount++;
                        }
                    }
                    catch (Exception ex)
                    {
                        // 개별 파일 오류는 로그만 남기고 계속 진행
                        System.Diagnostics.Debug.WriteLine($"파일 복호화 실패 {Path.GetFileName(file)}: {ex.Message}");
                    }
                }

                return decryptedCount;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"디렉토리 복호화 중 오류 발생: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// 이미지 파일 정보를 가져옴
        /// </summary>
        /// <param name="imagePath">이미지 파일 경로</param>
        /// <returns>이미지 정보 문자열</returns>
        [ComVisible(true)]
        public static string GetImageFileInfo(string imagePath)
        {
            try
            {
                if (!File.Exists(imagePath))
                {
                    throw new FileNotFoundException($"이미지 파일을 찾을 수 없습니다: {imagePath}");
                }

                byte[] imageData = File.ReadAllBytes(imagePath);
                
                using (MemoryStream stream = new MemoryStream(imageData))
                using (System.Drawing.Image image = System.Drawing.Image.FromStream(stream))
                {
                    StringBuilder info = new StringBuilder();
                    info.AppendLine($"파일 경로: {imagePath}");
                    info.AppendLine($"파일 크기: {imageData.Length:N0} bytes");
                    info.AppendLine($"이미지 형식: {GetImageFormatName(image.RawFormat)}");
                    info.AppendLine($"너비: {image.Width}px");
                    info.AppendLine($"높이: {image.Height}px");
                    info.AppendLine($"수평 해상도: {image.HorizontalResolution} dpi");
                    info.AppendLine($"수직 해상도: {image.VerticalResolution} dpi");
                    info.AppendLine($"픽셀 형식: {image.PixelFormat}");
                    
                    return info.ToString();
                }
            }
            catch (Exception ex)
            {
                return $"이미지 파일 정보 읽기 오류: {ex.Message}";
            }
        }

        /// <summary>
        /// 파일이 존재하는지 확인
        /// </summary>
        /// <param name="filePath">확인할 파일 경로</param>
        /// <returns>파일 존재 여부</returns>
        [ComVisible(true)]
        public static bool FileExists(string filePath)
        {
            return File.Exists(filePath);
        }

        /// <summary>
        /// 디렉토리가 존재하는지 확인
        /// </summary>
        /// <param name="directoryPath">확인할 디렉토리 경로</param>
        /// <returns>디렉토리 존재 여부</returns>
        [ComVisible(true)]
        public static bool DirectoryExists(string directoryPath)
        {
            return Directory.Exists(directoryPath);
        }

        /// <summary>
        /// 디렉토리 생성
        /// </summary>
        /// <param name="directoryPath">생성할 디렉토리 경로</param>
        /// <returns>생성 성공 여부</returns>
        [ComVisible(true)]
        public static bool CreateDirectory(string directoryPath)
        {
            try
            {
                if (!Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 파일 크기 가져오기
        /// </summary>
        /// <param name="filePath">파일 경로</param>
        /// <returns>파일 크기 (바이트)</returns>
        [ComVisible(true)]
        public static long GetFileSize(string filePath)
        {
            try
            {
                if (File.Exists(filePath))
                {
                    FileInfo fileInfo = new FileInfo(filePath);
                    return fileInfo.Length;
                }
                return -1;
            }
            catch
            {
                return -1;
            }
        }

        /// <summary>
        /// 이미지 형식 이름을 반환
        /// </summary>
        /// <param name="format">이미지 형식</param>
        /// <returns>이미지 형식 이름</returns>
        private static string GetImageFormatName(System.Drawing.Imaging.ImageFormat format)
        {
            if (format.Equals(System.Drawing.Imaging.ImageFormat.Bmp)) return "BMP";
            if (format.Equals(System.Drawing.Imaging.ImageFormat.Emf)) return "EMF";
            if (format.Equals(System.Drawing.Imaging.ImageFormat.Exif)) return "EXIF";
            if (format.Equals(System.Drawing.Imaging.ImageFormat.Gif)) return "GIF";
            if (format.Equals(System.Drawing.Imaging.ImageFormat.Icon)) return "ICO";
            if (format.Equals(System.Drawing.Imaging.ImageFormat.Jpeg)) return "JPEG";
            if (format.Equals(System.Drawing.Imaging.ImageFormat.MemoryBmp)) return "MemoryBMP";
            if (format.Equals(System.Drawing.Imaging.ImageFormat.Png)) return "PNG";
            if (format.Equals(System.Drawing.Imaging.ImageFormat.Tiff)) return "TIFF";
            if (format.Equals(System.Drawing.Imaging.ImageFormat.Wmf)) return "WMF";
            return "Unknown";
        }
    }
} 