using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Applicarion.Dto.UserDto
{
    public class RegisterDto
    {
        [Required(ErrorMessage = "اسم المستخدم مطلوب")]
        [StringLength(100, ErrorMessage = "اسم المستخدم لا يمكن أن يزيد عن 100 حرف")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "البريد الإلكتروني مطلوب")]
        [EmailAddress(ErrorMessage = "صيغة البريد الإلكتروني غير صحيحة")]
        public string Email { get; set; }

        [Required(ErrorMessage = "كلمة المرور مطلوبة")]
        [MinLength(6, ErrorMessage = "كلمة المرور يجب أن تكون 6 أحرف على الأقل")]
        public string Password { get; set; }

        [Compare("Password", ErrorMessage = "كلمات المرور غير متطابقة")]
        public string ConfirmPassword { get; set; }
    }
}
