using System.Text.Json;
using System.Text.Json.Serialization;

namespace ABCApp.Infrastructure.Extensions;

public static class ResponseWrapperExtensions
{
    private static readonly JsonSerializerOptions options = new()
    {
        PropertyNameCaseInsensitive = true,
        ReferenceHandler = ReferenceHandler.Preserve
    };

    public static async Task<IResponseWrapper<T>?> WrapToResponse<T>(this HttpResponseMessage responseMessage)
    {
        var responseAsString = await responseMessage.Content.ReadAsStringAsync();
        var responseObject = JsonSerializer.Deserialize<ResponseWrapper<T>>(responseAsString, options);
        return responseObject;
    }

    public static async Task<IResponseWrapper?> WrapToResponse(this HttpResponseMessage responseMessage)
    {
        var responseAsString = await responseMessage.Content.ReadAsStringAsync();
        var responseObject = JsonSerializer.Deserialize<ResponseWrapper>(responseAsString, options);
        return responseObject;
    }
}
