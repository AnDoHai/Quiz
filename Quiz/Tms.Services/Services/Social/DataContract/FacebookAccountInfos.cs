using System.Runtime.Serialization;

namespace Tms.Services.DataContract
{
    [DataContract]
    public class FacebookLocation
    {
        [DataMember(Name = "id")]
        public string Id { get; set; }
        [DataMember(Name = "name")]
        public string Name { get; set; }
    }
    [DataContract]
    public class Picture
    {
        [DataMember(Name = "data")]
        public PicureData Data { get; set; }
    }
    [DataContract]
    public class PicureData
    {
        [DataMember(Name = "url")]
        public string Url { get; set; }
    }
    [DataContract]
    public class FacebookAccountInfos
    {
        [DataMember(Name = "id")]
        public string Id { get; set; }
        [DataMember(Name = "name")]
        public string Name { get; set; }
        [DataMember(Name = "email")]
        public string Email { get; set; }
        [DataMember(Name = "first_name")]
        public string FirstName { get; set; }
        [DataMember(Name = "last_name")]
        public string LastName { get; set; }
        [DataMember(Name = "gender")]
        public string Gender { get; set; }
        [DataMember(Name = "locale")]
        public string Locale { get; set; }
        [DataMember(Name = "link")]
        public string Link { get; set; }
        [DataMember(Name = "timezone")]
        public int Timezone { get; set; }
        [DataMember(Name = "location")]
        public FacebookLocation Location { get; set; }
        [DataMember(Name = "picture")]
        public Picture Picture { get; set; }


    }
}
