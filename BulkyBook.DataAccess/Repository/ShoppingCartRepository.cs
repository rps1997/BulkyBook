﻿using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulkyBook.DataAccess.Repository
{
    public class ShoppingCartRepository : Repository<ShoppingCart>, IShoppingCartRepository
    {
        private ApplicationDbContext _db;
        public ShoppingCartRepository(ApplicationDbContext db):base(db)
        {
            _db = db;
        }

        public int Decrement(ShoppingCart shoppingCart, int count)
        {
            shoppingCart.Count=shoppingCart.Count - count ;
            return shoppingCart.Count;
        }

        public int Increment(ShoppingCart shoppingCart, int count)
        {
            shoppingCart.Count = shoppingCart.Count + count;
            return shoppingCart.Count;
        }

        //public void Update(Category obj)
        //{
        //    _db.Categories.Update(obj);
        //}
    }
}
