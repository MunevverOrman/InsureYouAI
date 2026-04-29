namespace InsureYouAI.Models
{
    public class AIInsuranceRecommendationViewModel
    {
        // Kullanıcıdan alınacak alanlar
        public int? Age { get; set; }
        public string? Occupation { get; set; }
        public string? City { get; set; }
        public string? MaritalStatus { get; set; }
        public int? ChildrenCount { get; set; }
        public string? TravelFrequency { get; set; }     // "Yılda 1-2", "Yılda 3-5", "Sık seyahat" gibi
        public decimal? MonthlyBudget { get; set; }      // Aylık maksimum bütçe
        public bool HasChronicDisease { get; set; }
        public string? ChronicDiseaseDetails { get; set; }
        public string? CoveragePriority { get; set; }    // "Sağlık", "Seyahat", "Araç", "Genel" vb.

        // AI çıktısı SONRA doldurulacak, şu anlık boş dursun
        public string? RecommendedPackage { get; set; }
        public string? SecondBestPackage { get; set; }
        public string? AnalysisText { get; set; }

        public string? onerilenPaket { get; set; }
        public string? ikinciSecenek { get; set; }
        public string? neden { get; set; }
    }
}
