﻿using DTOLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAOLibrary.Repository.Interface
{
    public interface ICategoryRepository
    {
        Category GetCategoryById(int id);
        Category GetCategoryByName(string name);
        IEnumerable<Category> GetCategoryList();
        void CreateCategory(Category category);
        void DeleteCategory(int id);
        void UpdateCategory(Category category);

    }
}
