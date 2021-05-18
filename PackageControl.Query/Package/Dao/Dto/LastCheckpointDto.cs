using System;
using System.Collections.Generic;
using System.Text;

namespace PackageControl.Query.Package.Dao.Dto
{
    public class LastCheckpointDto
    {
        public string Country { get; set; }
        public string City { get; set; }
        public byte TypeOfControl { get; set; }
        public byte PlaceType { get; set; }
    }
}
