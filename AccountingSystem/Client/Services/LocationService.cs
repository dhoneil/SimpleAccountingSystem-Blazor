using AccountingSystem.Shared.Models;
using AccountingSystem.Shared.Utility;
using System.Net.Http.Json;

namespace AccountingSystem.Client.Services
{
    public interface ILocationService
    {
        Task<List<Location>> GetLocations();
        Task<bool> CreateLocation(Location entity);
        Task<Location> GetDetails(int id);
        Task<bool> UpdateLocation(Location entity);
    }

    public class LocationService : ILocationService
    {
        public readonly HttpClient _http;
        public LocationService(HttpClient httpClient)
        {
            _http = httpClient;
        }

        public async Task<List<Location>> GetLocations()
        {
            return await ApiWrapper.Get<List<Location>>($"{_http.BaseAddress.AbsoluteUri}api/location/getlocations");
        }

        public async Task<bool> CreateLocation(Location entity)
        {
            var result = await _http.PostAsJsonAsync("api/location/create", entity);
            return result.IsSuccessStatusCode;
        }

        public async Task<Location> GetDetails(int id)
        {
            var res = await ApiWrapper.Get<Location>($"{_http.BaseAddress.AbsoluteUri}api/location/getdetails/{id}");
            return res;
        }

        public async Task<bool> UpdateLocation(Location entity)
        {
            var result = await ApiWrapper.Put<Location>($"{_http.BaseAddress.AbsoluteUri}api/location/update", entity);
            return result != null;
        }
    }
}
