namespace VibeNet.Core.ViewModels
{
    public class ProfilePictureViewModel
    {
        public int Id { get; set; }

        public string? Name { get; set; }

        public string? ContentType { get; set; }

        public byte[]? Data { get; set; }

        public bool? IsSelected { get; set; }
    }
}
