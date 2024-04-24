namespace ServeurImages.Models
{
    public class Picture
    {
        // Étape 1: Ajouter les champs nécessaires.
        public int Id { get; set; }
        public string? FileName { get; set; }
        public string? MimeType { get; set; }
    }
}
