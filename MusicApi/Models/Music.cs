using MusicApi.Interfaces;
using System.Collections.Generic;

namespace MusicApi.Models
{
    public class Music : IData
    {
        public int Id { get; set; }
        public string Song { get; set; }
        public string Link { get; set; }
        public string Singer { get; set; }
        public string Author { get; set; }

        public Music(int _id, string _song, string _link, string _singer, string _author)
        {
            Id = _id;
            Song = _song;
            Link = _link;  
            Singer = _singer;
            Author = _author;
        }

        public Music()
        {
        }

        public bool CheckNull()
        {
            if (Song == null || Link == null || Singer == null || Author == null)
                return true;
            return false;
        }

        public Result GetResult()
        {
            Result result = new Result();
            result.Status = 400;
            result.Message = "Hãy nhập đủ các trường";
            List<Error> errors = new List<Error>();
            if (Song == null)
            {
                errors.Add(new Error("song", "Hãy nhập tên bài hát"));
                result.TotalError++;
            }
            if (Link == null)
            {
                errors.Add(new Error("link", "Hãy link tên bài hát."));
                result.TotalError++;
            }
            if(Singer == null)
            {
                errors.Add(new Error("singer", "Hãy tên ca sĩ."));
                result.TotalError++;
            }
            if (Author == null)
            {
                errors.Add(new Error("author", "Hãy tên tác giả."));
                result.TotalError++;
            }
            result.Errors = errors;
            return result;
        }
    }
}
