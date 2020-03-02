﻿// Copyright (c) 2018 Jon P Smith, GitHub: JonPSmith, web: http://www.thereformedprogrammer.net/
// Licensed under MIT licence. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using DataLayer.EfClasses;
using GenericServices;
using Microsoft.EntityFrameworkCore;

namespace ServiceLayer.HomeController.Dtos
{
    public class CreateBookDto : ILinkToEntity<Book>
    {
        //This will be populated with the primary key of the created book
        public int BookId { get; set; }

        //I would normally have the Required attribute to catch this at the front end
        //But to show how the static create method catches that error I have commented it out
        //[Required(AllowEmptyStrings = false)]
        public string Title { get; set; }
        public string Description { get; set; }
        [DataType(DataType.Date)]
        public DateTime PublishedOn { get; set; }
        public string Publisher { get; set; }
        [Range(0,1000)]
        public decimal Price { get; set; }
        public string ImageUrl { get; set; }

        public ICollection<Author> Authors { get; set; }

        public CreateBookDto()
        {
            PublishedOn = DateTime.Today;
        }

        //---------------------------------------------------------
        //Now the data for the front end

        public struct KeyName
        {
            public int AuthorId { get; set; }
            public string Name { get; set; }
        }

        public List<KeyName> AllPossibleAuthors { get; private set; }

        public void BeforeDisplay(DbContext context)
        {
            //AllPossibleAuthors = context.Set<Author>().Select(x => new KeyName(x.AuthorId, x.Name)).ToList();            
            AllPossibleAuthors = context.Set<Author>().Select(x => new KeyName() { AuthorId = x.AuthorId, Name = x.Name } )
               .OrderBy(x => x.Name).ToList();
            // //AllPossibleAuthors = new List<KeyName>() { new KeyName(1, "John"), new KeyName(2, "Paul"), new KeyName(3, "Ringo"), new KeyName(4, "George") };
        }

        public List<int> BookAuthorIds { get; set; } = new List<int>();

        public void BeforeSave(DbContext context)
        {
            Authors = BookAuthorIds.Select(x => context.Find<Author>(x)).Where(x => x != null).ToList();
        }
    }
}