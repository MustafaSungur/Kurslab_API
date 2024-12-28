
using Education.Business.Core.@abstract;
using Xabe.FFmpeg;

public class VideoConversionManager : IVideoConversionService
{
	public VideoConversionManager()
	{
		// FFmpeg yürütülebilir dosyaların yolunu belirtin, yoksa otomatik indirilecektir
		FFmpeg.SetExecutablesPath(@"C:\Users\10\Desktop\Eğitim Videoları Web Sitesi\backend\Education\Education.WebApi\wwwroot\uploads\videos");
	}

	// H.264 formatına dönüştürme metodu
	public async Task<bool> ConvertToH264(string inputFilePath, string outputFilePath)
	{
		try
		{
			// Dönüştürme işlemi
			var conversion = await FFmpeg.Conversions.FromSnippet.ToMp4(inputFilePath, outputFilePath);

			// H.264 codec ve diğer ayarları ekleyin
			conversion.AddParameter("-c:v libx264") // H.264 codec
					  .SetPreset(ConversionPreset.Fast) // Hızlı dönüşüm ayarı
					  .SetOutput(outputFilePath);

			// Dönüştürme işlemini başlatın
			await conversion.Start();
			
			return true;
		}
		catch (Exception ex)
		{
			Console.WriteLine($"Video dönüştürme sırasında hata: {ex.Message}");
			return false;
		}
	}
}
