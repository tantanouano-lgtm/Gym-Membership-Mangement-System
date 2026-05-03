using System.Collections.Generic;

namespace Gym_Membership_Mangement_System
{
    public class EquipmentModel
    {
        public int id { get; set; }
        public string equipment_name { get; set; }
        public string description { get; set; }
        public string muscles_used { get; set; }
        public string delivery_date { get; set; }
        public decimal cost { get; set; }
        public int quantity { get; set; }
        public string created_at { get; set; }
    }

    public class EquipmentResponse
    {
        public bool success { get; set; }
        public List<EquipmentModel> data { get; set; }
    }
}