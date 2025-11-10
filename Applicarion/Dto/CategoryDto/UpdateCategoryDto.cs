using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Applicarion.Dto.CategoryDto
{
    public class UpdateCategoryDto
    {
        public int ID { get; set; }
        public string CategoryName { get; set; }

        public string Description { get; set; }
    }
}
