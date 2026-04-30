using System.Net.Http.Json;
using ABCShared.Library.Models.Responses.Schools;

namespace ABCApp.Infrastructure.Services.Implementations.Schools;

public class SchoolService(HttpClient httpClient, ApiSettings apiSettings) : ISchoolService
{
    private readonly HttpClient _httpClient = httpClient;
    private readonly ApiSettings _apiSettings = apiSettings;

    public async Task<IResponseWrapper<int>> CreateAsync(CreateSchoolRequest request)
    {
        var response = await _httpClient.PostAsJsonAsync(_apiSettings.SchoolEndpoints.Create, request);
        var result = await response.WrapToResponse<int>() ?? throw new Exception("result is null");
        return result;
    }

    public async Task<IResponseWrapper<int>> DeleteAsync(string schoolId)
    {
        var response = await _httpClient.DeleteAsync(_apiSettings.SchoolEndpoints.DeleteSchool(schoolId));
        var result = await response.WrapToResponse<int>() ?? throw new Exception("result is null");
        return result;
    }

    public async Task<IResponseWrapper<List<SchoolResponse>>> GetAllAsync()
    {
        var response = await _httpClient.GetAsync(_apiSettings.SchoolEndpoints.All);
        var result = await response.WrapToResponse<List<SchoolResponse>>() ?? throw new Exception("result is null");
        return result;
    }

    public async Task<IResponseWrapper<SchoolResponse>> GetByIdAsync(string schoolId)
    {
        var response = await _httpClient.GetAsync(_apiSettings.SchoolEndpoints.GetById(schoolId));
        var result = await response.WrapToResponse<SchoolResponse>();
        return result ?? throw new Exception("result is null");
    }

    public async Task<IResponseWrapper<SchoolResponse>> GetByNameAsync(string schoolName)
    {
        var response = await _httpClient.GetAsync(_apiSettings.SchoolEndpoints.GetByName(schoolName));
        var result = await response.WrapToResponse<SchoolResponse>();
        return result ?? throw new Exception("result is null");
    }

    public async Task<IResponseWrapper<int>> UpdateAsync(UpdateSchoolRequest request)
    {
        var response = await _httpClient.PutAsJsonAsync(_apiSettings.SchoolEndpoints.Update, request);
        var result = await response.WrapToResponse<int>() ?? throw new Exception("result is null");
        return result;
    }
}
