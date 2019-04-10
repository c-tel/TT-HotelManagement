using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TTHohel.Services;
using TTHotel.Contracts.Cleanings;

namespace TTHohel.Models
{
    public class MaidMainModel
    {
        internal void CreateCleaning(CleaningDTO cleaning)
        {
            if(!HotelApiClient.GetInstance().CreateCleaning(cleaning))
                MessageBox.Show("gg");
        }

        internal List<CleaningDTO> GetCleanings()
        {
            return HotelApiClient.GetInstance().GetAllCleanings();
        }
    }
}
