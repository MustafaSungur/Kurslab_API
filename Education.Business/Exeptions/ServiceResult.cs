
namespace Education.Business.Exeptions
{
	public class ServiceResult<T>
	{
		public bool Success { get; set; }
		public T? Data { get; set; }
		public string? ErrorMessage { get; set; }
		public string? SuccessMessage { get; set; }

		public static ServiceResult<T> SuccessResult(T data) => new() { Success = true, Data = data };
		public static ServiceResult<T> SuccessMessageResult(string message) => new() { Success = true, SuccessMessage = message };
		public static ServiceResult<T> FailureResult(string errorMessage) => new() { Success = false, ErrorMessage = errorMessage };
	}
}
