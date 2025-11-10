//using Domain.Entities;
//using Microsoft.EntityFrameworkCore;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Infrastructure.Seeds
//{
//    public class Seed
//    {
//        private  readonly BlogDbContext _context;

//        public Seed(BlogDbContext blogDbContext)
//        {
//            _context = blogDbContext;
//        }
//        public  void SeedData()
//        {
//            var shouldSave = false;





//            //if (!_context._articles.Any())
//            //{
//            //    _context.AddRange(
//            //      new Article
//            //      {
//            //          categoryId = 1,
//            //          Title = "Introduction to AI",
//            //          Content = "Artificial Intelligence is transforming industries...",

//            //          userID = 1,

//            //          ImageUrl = "C:\\Users\\AL HAYTHAM\\source\\repos\\Blogs Applications\\Blogs Applications\\wwwroot\\Images\\Articles\\37f5c260-a495-4122-b9aa-cfc9b74ead97.jpg",
//            //          CreatedAt = DateTime.Now,




//            //      }






//            //     );

//            //    shouldSave = true;
//            //}


//            if (!_context._articles.Any())
//            {
//                _context.AddRange(
//                  new Article
//                  {
//                      categoryId = 1,
//                      Title = "Introduction to AI",
//                      Content = "Artificial Intelligence is transforming industries...",

//                      userID = 1,

//                      ImageUrl = "C:\\Users\\AL HAYTHAM\\source\\repos\\Blogs Applications\\Blogs Applications\\wwwroot\\Images\\Articles\\37f5c260-a495-4122-b9aa-cfc9b74ead97.jpg",
//                      CreatedAt = DateTime.Now,
//                      IsPublished = true,
//                      UpdatedAt = DateTime.Now,



//                  },
//                   new Article
//                   {
//                       categoryId = 3,
//                       Title = "Quick Dinner Recipes",
//                       Content = "5 easy recipes for busy weeknights...",

//                       userID = 2,

//                       ImageUrl = "C:\\Users\\AL HAYTHAM\\source\\repos\\Blogs Applications\\Blogs Applications\\wwwroot\\Images\\Articles\\20ca033a-3f44-446e-b88c-18b268d7df64.jpg",
//                       CreatedAt = DateTime.Now,
//                       IsPublished = true,
//                       UpdatedAt = DateTime.Now,



//                   }



//                 );

//                shouldSave = true;
//            }



//            if (shouldSave)
//            {
//                shouldSave = false;
//                _context.SaveChanges();
//            }









//        }
//    }
//}