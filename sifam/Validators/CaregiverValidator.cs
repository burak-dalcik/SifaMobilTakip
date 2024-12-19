using sifam.Models;

namespace sifam.Validators
{
    public static class CaregiverValidator
    {
        public static List<string> Validate(Caregiver caregiver)
        {
            var errors = new List<string>();

            if (caregiver == null)
            {
                errors.Add("Caregiver nesnesi null olamaz.");
                return errors; // Diğer kontrolleri yapmaya gerek yok
            }

            if (caregiver.UserId <= 0)
            {
                errors.Add("Geçerli bir UserId giriniz.");
            }

            if (caregiver.AssignedPatientId == null)
            {
                errors.Add("AssignedPatientId boş olamaz.");
            }

            // İlişkili kullanıcı var mı kontrol edilebilir
            // (Bu kontrol veri tabanından sorgulama sırasında yapılırsa buraya gerek kalmaz)

            return errors;
        }
    }
}
