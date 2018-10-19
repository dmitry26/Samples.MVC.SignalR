namespace SamplesSignalR.Models
{
    public class ViewModelBase
    {
        protected ViewModelBase(string title = "")
        {
            Title = title;
        }

        public static ViewModelBase Default(string title = "")
        {
            return new ViewModelBase(title);            
        }

        public string Title { get; set; }
    }
}