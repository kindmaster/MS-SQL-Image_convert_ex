using System;
using System.IO;

namespace MS_SQL_Image_convert
{
    /// <summary>
    /// 로컬 이미지 암호화/복호화 기능 테스트 예제
    /// </summary>
    public class LocalImageEncryptionExample
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("=== 로컬 이미지 암호화/복호화 테스트 ===");
            Console.WriteLine();

            try
            {
                // 테스트할 이미지 파일 경로 (실제 경로로 변경하세요)
                string testImagePath = @"C:\temp\test_image.jpg";
                string password = "MySecretPassword123!";

                // 1. 단일 파일 암호화/복호화 테스트
                Console.WriteLine("1. 단일 파일 암호화/복호화 테스트");
                Console.WriteLine("==================================");
                
                if (File.Exists(testImagePath))
                {
                    // 이미지 정보 출력
                    Console.WriteLine("원본 이미지 정보:");
                    Console.WriteLine(LocalImageEncryption.GetImageFileInfo(testImagePath));
                    Console.WriteLine();

                    // 암호화
                    Console.WriteLine("이미지 암호화 중...");
                    string encryptedPath = LocalImageEncryption.EncryptImageFileInPlace(testImagePath, password);
                    Console.WriteLine($"암호화 완료: {encryptedPath}");
                    Console.WriteLine();

                    // 복호화
                    Console.WriteLine("이미지 복호화 중...");
                    string decryptedPath = LocalImageEncryption.DecryptImageFileInPlace(encryptedPath, password);
                    Console.WriteLine($"복호화 완료: {decryptedPath}");
                    Console.WriteLine();

                    // 복호화된 이미지 정보 출력
                    Console.WriteLine("복호화된 이미지 정보:");
                    Console.WriteLine(LocalImageEncryption.GetImageFileInfo(decryptedPath));
                    Console.WriteLine();
                }
                else
                {
                    Console.WriteLine($"테스트 이미지 파일을 찾을 수 없습니다: {testImagePath}");
                    Console.WriteLine("실제 이미지 파일 경로로 변경하여 테스트하세요.");
                    Console.WriteLine();
                }

                // 2. 디렉토리 일괄 암호화/복호화 테스트
                Console.WriteLine("2. 디렉토리 일괄 암호화/복호화 테스트");
                Console.WriteLine("=====================================");
                
                string sourceDirectory = @"C:\temp\images";
                string encryptedDirectory = @"C:\temp\encrypted";
                string decryptedDirectory = @"C:\temp\decrypted";

                if (Directory.Exists(sourceDirectory))
                {
                    // 디렉토리 암호화
                    Console.WriteLine("디렉토리 암호화 중...");
                    int encryptedCount = LocalImageEncryption.EncryptImageDirectory(
                        sourceDirectory, encryptedDirectory, password);
                    Console.WriteLine($"암호화 완료: {encryptedCount}개 파일");
                    Console.WriteLine();

                    // 디렉토리 복호화
                    Console.WriteLine("디렉토리 복호화 중...");
                    int decryptedCount = LocalImageEncryption.DecryptImageDirectory(
                        encryptedDirectory, decryptedDirectory, password);
                    Console.WriteLine($"복호화 완료: {decryptedCount}개 파일");
                    Console.WriteLine();
                }
                else
                {
                    Console.WriteLine($"테스트 디렉토리를 찾을 수 없습니다: {sourceDirectory}");
                    Console.WriteLine("실제 디렉토리 경로로 변경하여 테스트하세요.");
                    Console.WriteLine();
                }

                // 3. 사용법 안내
                Console.WriteLine("3. 사용법 안내");
                Console.WriteLine("==============");
                Console.WriteLine("단일 파일 암호화:");
                Console.WriteLine("LocalImageEncryption.EncryptImageFile(sourcePath, encryptedPath, password);");
                Console.WriteLine();
                Console.WriteLine("단일 파일 복호화:");
                Console.WriteLine("LocalImageEncryption.DecryptImageFile(encryptedPath, decryptedPath, password);");
                Console.WriteLine();
                Console.WriteLine("디렉토리 일괄 암호화:");
                Console.WriteLine("LocalImageEncryption.EncryptImageDirectory(sourceDir, outputDir, password);");
                Console.WriteLine();
                Console.WriteLine("디렉토리 일괄 복호화:");
                Console.WriteLine("LocalImageEncryption.DecryptImageDirectory(sourceDir, outputDir, password);");
                Console.WriteLine();
                Console.WriteLine("이미지 정보 조회:");
                Console.WriteLine("LocalImageEncryption.GetImageFileInfo(imagePath);");
                Console.WriteLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"오류 발생: {ex.Message}");
                Console.WriteLine($"상세 정보: {ex}");
            }

            Console.WriteLine("테스트 완료. 아무 키나 누르면 종료됩니다.");
            Console.ReadKey();
        }

        /// <summary>
        /// 대화형 테스트 메뉴
        /// </summary>
        public static void RunInteractiveTest()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== 로컬 이미지 암호화/복호화 대화형 테스트 ===");
                Console.WriteLine("1. 단일 파일 암호화");
                Console.WriteLine("2. 단일 파일 복호화");
                Console.WriteLine("3. 디렉토리 일괄 암호화");
                Console.WriteLine("4. 디렉토리 일괄 복호화");
                Console.WriteLine("5. 이미지 정보 조회");
                Console.WriteLine("6. 종료");
                Console.Write("선택하세요 (1-6): ");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        TestSingleFileEncryption();
                        break;
                    case "2":
                        TestSingleFileDecryption();
                        break;
                    case "3":
                        TestDirectoryEncryption();
                        break;
                    case "4":
                        TestDirectoryDecryption();
                        break;
                    case "5":
                        TestImageInfo();
                        break;
                    case "6":
                        return;
                    default:
                        Console.WriteLine("잘못된 선택입니다. 다시 시도하세요.");
                        break;
                }

                Console.WriteLine("\n계속하려면 아무 키나 누르세요...");
                Console.ReadKey();
            }
        }

        private static void TestSingleFileEncryption()
        {
            Console.Write("암호화할 이미지 파일 경로: ");
            string sourcePath = Console.ReadLine();
            Console.Write("암호화된 파일 저장 경로: ");
            string encryptedPath = Console.ReadLine();
            Console.Write("비밀번호: ");
            string password = Console.ReadLine();

            try
            {
                bool success = LocalImageEncryption.EncryptImageFile(sourcePath, encryptedPath, password);
                if (success)
                {
                    Console.WriteLine("암호화가 성공적으로 완료되었습니다.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"암호화 실패: {ex.Message}");
            }
        }

        private static void TestSingleFileDecryption()
        {
            Console.Write("복호화할 암호화된 파일 경로: ");
            string encryptedPath = Console.ReadLine();
            Console.Write("복호화된 파일 저장 경로: ");
            string decryptedPath = Console.ReadLine();
            Console.Write("비밀번호: ");
            string password = Console.ReadLine();

            try
            {
                bool success = LocalImageEncryption.DecryptImageFile(encryptedPath, decryptedPath, password);
                if (success)
                {
                    Console.WriteLine("복호화가 성공적으로 완료되었습니다.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"복호화 실패: {ex.Message}");
            }
        }

        private static void TestDirectoryEncryption()
        {
            Console.Write("암호화할 이미지 디렉토리 경로: ");
            string sourceDir = Console.ReadLine();
            Console.Write("암호화된 파일 저장 디렉토리 경로: ");
            string outputDir = Console.ReadLine();
            Console.Write("비밀번호: ");
            string password = Console.ReadLine();

            try
            {
                int count = LocalImageEncryption.EncryptImageDirectory(sourceDir, outputDir, password);
                Console.WriteLine($"{count}개 파일이 성공적으로 암호화되었습니다.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"디렉토리 암호화 실패: {ex.Message}");
            }
        }

        private static void TestDirectoryDecryption()
        {
            Console.Write("복호화할 암호화된 파일 디렉토리 경로: ");
            string sourceDir = Console.ReadLine();
            Console.Write("복호화된 파일 저장 디렉토리 경로: ");
            string outputDir = Console.ReadLine();
            Console.Write("비밀번호: ");
            string password = Console.ReadLine();

            try
            {
                int count = LocalImageEncryption.DecryptImageDirectory(sourceDir, outputDir, password);
                Console.WriteLine($"{count}개 파일이 성공적으로 복호화되었습니다.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"디렉토리 복호화 실패: {ex.Message}");
            }
        }

        private static void TestImageInfo()
        {
            Console.Write("이미지 파일 경로: ");
            string imagePath = Console.ReadLine();

            try
            {
                string info = LocalImageEncryption.GetImageFileInfo(imagePath);
                Console.WriteLine("이미지 정보:");
                Console.WriteLine(info);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"이미지 정보 조회 실패: {ex.Message}");
            }
        }
    }
} 