using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;


namespace Classifieds.Domain.Model
{
    [Table(name: "app_user")]
    public class User
    {
        [Key]
        [Column(name: "pk_user_id")]
        public long ID { set; get; }

        [Column(name: "activated")]
        public int? Activated { set; get; }

        [Column(name: "verified")]
        public int? IsVerified { set; get; }

        [Column(name: "verification_token")]
        public string VerificationToken { set; get; }

        [Column(name: "activation_code")]
        public String ActivationCode { set; get; }

        [Column(name: "email")]
        public String Email { set; get; }

        [Column(name: "password")]
        public String Password { set; get; }

        public int? Notified { set; get; }

        [Column(name: "last_login_dt")]
        public DateTime? LastLogin { set; get; }

        [Column(name: "reg_dt")]
        public DateTime RegDate { set; get; }

        [Column(name: "pass_expiry_dt")]
        public DateTime? PasswordExpiry { set; get; }

        public UserDetail UserDetail { set; get; }

        public IEnumerable<Role> Roles { set; get; }
    }
}
