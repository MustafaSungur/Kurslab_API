namespace Education.WebApi
{
	using Microsoft.OpenApi.Models;
	using Swashbuckle.AspNetCore.SwaggerGen;

	public class SwaggerFileOperationFilter : IOperationFilter
	{
		public void Apply(OpenApiOperation operation, OperationFilterContext context)
		{
			var fileParams = context.MethodInfo.GetParameters().Where(p => p.ParameterType == typeof(IFormFile));

			foreach (var fileParam in fileParams)
			{
				operation.RequestBody = new OpenApiRequestBody
				{
					Content =
				{
					["multipart/form-data"] = new OpenApiMediaType
					{
						Schema = new OpenApiSchema
						{
							Type = "object",
							Properties =
							{
								[fileParam.Name] = new OpenApiSchema
								{
									Type = "string",
									Format = "binary"
								}
							}
						}
					}
				}
				};
			}
		}
	}

}
