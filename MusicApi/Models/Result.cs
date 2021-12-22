using MusicApi.Interfaces;
using System.Collections.Generic;

namespace MusicApi.Models
{
    public class Result
    {
        public string Message { get; set; }

        public int Status { get; set; }
        public int TotalResult { get; set; }
        public int TotalError { get; set; }

        public IEnumerable<object> Data { get; set; }
        public IEnumerable<object> Errors { get; set; }

        public Result()
        {
            Status = 200;
            TotalResult = 0;
            TotalError = 0;
            Message = "Không có dữ liệu";
            Data = new List<object>();
            Errors = new List<object>();
        }

        public Result(string _message, int _status, int _total, IEnumerable<object> _data)
        {
            Message = _message;
            Status = _status;
            TotalResult = _total;
            Data = _data;
        }
    }
}
