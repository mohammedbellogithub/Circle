using Circle.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Circle.Shared.Models
{
    public interface IApiResponse<T>
    {
        ApiResponseCodes Code { get; set; }
        string? Description { get; set; }
        T Payload { get; set; }
        int TotalCount { get; set; }
        List<string> Errors { get; set; }
        bool HasErrors { get; }
    }

    public class ApiResponse
    {
        public ApiResponseCodes Code { get; set; }
        public int TotalCount { get; set; }
        public string? Description { get; set; }
        public bool Success { get; set; }
        public dynamic? Payload { get; set; }
    }

    public class ApiResponse<T> : ApiResponse, IApiResponse<T>
    {
        //public int TotalCount { get; set; }
        public string? ResponseCode { get; set; }
        public List<string> Errors { get; set; } = new List<string>();
        public bool HasErrors => Errors.Any();

        T IApiResponse<T>.Payload { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    }
}