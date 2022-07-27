using System;
using System.Collections.Generic;

namespace MvcAuth.Models
{
    public partial class AddressBook
    {
        public int AddBookId { get; set; }
        public string Fname { get; set; } = null!;
        public string Lname { get; set; } = null!;
        public string Address { get; set; } = null!;
        public string? Nickname { get; set; }
        public long? PhoneNo { get; set; }
        public long? FaxNo { get; set; }
        public string? Email { get; set; }
        public string? Notes { get; set; }
        public string Id { get; set; } = null!;
        public byte[]? ProfilePic { get; set; }
        // Navigation Property
        public virtual AspNetUser IdNavigation { get; set; } = null!;
    }
}
