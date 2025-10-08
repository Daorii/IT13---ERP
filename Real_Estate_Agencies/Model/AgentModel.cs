using System.Windows.Media.Imaging;
using System.IO;

namespace Real_Estate_Agencies.Model
{
    public class Agent
    {
        public int No { get; set; }              // UI row number
        public int AgentId { get; set; }         // DB primary key (IDENTITY)
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string HireDate { get; set; } = string.Empty;    // yyyy-MM-dd
        public string ContactInfo { get; set; } = string.Empty;
     //    public string ImagePath { get; set; } = ""; // default empty


        public byte[] ProfileImage { get; set; } = null;

        public BitmapImage ProfileBitmap
        {
            get
            {
                if (ProfileImage == null || ProfileImage.Length == 0)
                    return null;

                BitmapImage bitmap = new BitmapImage();
                using (MemoryStream ms = new MemoryStream(ProfileImage))
                {
                    bitmap.BeginInit();
                    bitmap.CacheOption = BitmapCacheOption.OnLoad;
                    bitmap.StreamSource = ms;
                    bitmap.EndInit();
                }
                return bitmap;
            }
        }
    }
}