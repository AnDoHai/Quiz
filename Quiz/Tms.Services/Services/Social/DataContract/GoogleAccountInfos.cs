using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Tms.Services.DataContract
{
    [DataContract]
    public class GoogleAccountInfos
    {
        [DataMember(Name = "id")]
        public string ID { get; set; }
        [DataMember(Name = "email")]
        public string Email { get; set; }
        [DataMember(Name = "given_name")]
        public string GivenName { get; set; }
        [DataMember(Name = "family_name")]
        public string FamilyName { get; set; }
        [DataMember(Name = "link")]
        public string Link { get; set; }
        [DataMember(Name = "picture")]
        public string Picture { get; set; }
        [DataMember(Name = "gender")]
        public string Gender { get; set; }
        [DataMember(Name = "locale")]
        public string Locale { get; set; }
        [DataMember(Name = "name")]
        public string Name { get; set; }
    }
}
