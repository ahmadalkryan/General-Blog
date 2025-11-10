using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Applicarion.Dto.UserDto
{
    public class UpdateUserRoleDto
    {
         public int Id { get; set; }

        [Required(ErrorMessage = "الدور مطلوب")]
        [RegularExpression("^(User|Writer|Admin)$", ErrorMessage = "الدور يجب أن يكون User أو Writer أو Admin")]
        public string Role { get; set; }

        
    }
}
