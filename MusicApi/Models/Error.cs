namespace MusicApi.Models
{
    public class Error
    {
        public string Label { get; set; }
        public string Message { get; set; }

        public Error()
        {

        }

        public Error(string _name, string _message)
        {
            Label = _name;
            Message = _message;
        }
    }
}
