﻿using ClothesShoppingWebApp.Models;
using DAOLibrary.Repository.Interface;
using DAOLibrary.Repository.Object;
using DTOLibrary;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.IO;
using Microsoft.AspNetCore.Authorization;

namespace ClothesShoppingWebApp.Controllers.Guest
{
    public class CartController : Controller
    {
        IProductRepository productRepository = null;
        public CartController()
        {
            productRepository = new ProductRepository();
        }

        //Show Cart Table
        [Authorize(Policy = "UserPolicy")]
        public IActionResult Index()
        { 
            
            var cart = HttpContext.Session.GetComplexData<List<CartItem>>("CART");
            List<CartItem> list = new List<CartItem>();
            if (cart != null)
            {
                list = (List<CartItem>)cart;
            }
            return View(list);
        }
        [Authorize(Roles = "User")]
        public IActionResult Checkout()
        {

            var cart = HttpContext.Session.GetComplexData<List<CartItem>>("CART");
            List<CartItem> list = new List<CartItem>();
            if (cart != null)
            {
                list = (List<CartItem>)cart;
            }
            return View(list);
        }

        //POST - ADD
        [Authorize(Policy = "UserPolicy")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Add(int ProductId, int Quantity)
        {
            var product = productRepository.GetProductById(ProductId);

            var cart = HttpContext.Session.GetComplexData<List<CartItem>>("CART");

            if (cart != null)
            {
                
                var list = (List<CartItem>)cart;

                if (list.Exists(i => i.ProductId == product.ProductId))
                {
                    foreach (var item in list)
                    {
                        if (item.ProductId == product.ProductId)
                        {
                            item.Quantity += Quantity;
                        }
                    }
                    HttpContext.Session.SetComplexData("CART", list);
                }
                
                else //if list does not contain that item yet
                {
                    //New item
                    var item = new CartItem();
                    item.ProductId = product.ProductId;
                    item.ProductName = product.ProductName;
                    item.Price = product.Price;
                    item.Image = product.Image;
                    item.Quantity = Quantity;
                    list.Add(item);
                    HttpContext.Session.SetComplexData("CART", list);
                }
            }
            else //if cart is null
            {
                //New item
                var item = new CartItem();
                item.ProductId = product.ProductId;
                item.ProductName = product.ProductName;
                item.Price = product.Price;
                item.Image = product.Image;
                item.Quantity = Quantity;
                var list = new List<CartItem>();
                list.Add(item);

                //Assign to cart
                HttpContext.Session.SetComplexData("CART", list);
            }
            return Redirect("/Guest");
        }

        //GET - REMOVE
        [Authorize(Policy = "UserPolicy")]
        public IActionResult Remove(int? ProductId)
        {
            if(ProductId == null)
            {
                return NotFound();
            }
            var product = productRepository.GetProductById((int)ProductId);
            if(product == null)
            {
                return NotFound();
            }
            var cart = HttpContext.Session.GetComplexData<List<CartItem>>("CART");
            List<CartItem> list = new List<CartItem>();
            if(cart != null)
            {
                list = (List<CartItem>)cart;
                if(list.Exists(x=> x.ProductId == ProductId))
                {
                    foreach (var item in list)
                    {
                        if (item.ProductId == ProductId)
                        {
                            list.Remove(item);
                            break;
                        }
                    }
                }
                HttpContext.Session.SetComplexData("CART", list);
            }
            return RedirectToAction("Index");
        }
    }
}

#region ExtendSession
public static class SessionExtensions
{
    public static T GetComplexData<T>(this ISession session, string key)
    {
        var data = session.GetString(key);
        if (data == null)
        {
            return default(T);
        }
        return JsonConvert.DeserializeObject<T>(data);
    }

    public static void SetComplexData(this ISession session, string key, object value)
    {
        session.SetString(key, JsonConvert.SerializeObject(value));
    }
}

#endregion