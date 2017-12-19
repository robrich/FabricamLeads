using System.Collections.Generic;

namespace Fabricam.Web.Core.Models
{
    public class RandomUserData
    {
        public List<RandomUserResult> results { get; set; }
        public RandomUserInfo info { get; set; }
    }

    public class RandomUserInfo
    {
        public string seed { get; set; }
        public int results { get; set; }
        public int page { get; set; }
        public string version { get; set; }
    }

    public class RandomUserResult
    {
        public string gender { get; set; }
        public RandomUserName name { get; set; }
        public RandomUserLocation location { get; set; }
        public string email { get; set; }
        //public string dob { get; set; }
        //public string registered { get; set; }
        public string phone { get; set; }
        public string cell { get; set; }
        public string nat { get; set; }
    }

    public class RandomUserName
    {
        public string title { get; set; }
        public string first { get; set; }
        public string last { get; set; }
    }

    public class RandomUserLocation
    {
        public string street { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string postcode { get; set; }
    }

}
