﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebMvc.Services;
using WebMvc.ViewModels;

namespace WebMvc.Controllers
{
    public class CatalogController : Controller
    {
        private readonly ICatalogService _service;
        public CatalogController(ICatalogService service)
        {
            _service = service;
        }

        public async Task<IActionResult> Index(int? page, int? brandFilterApplied, int? typesFilterApplied)
        {
            var itemsOnPage = 8;

            var catalog = await _service.GetCatalogItemsAsync(page ?? 0, itemsOnPage, brandFilterApplied, typesFilterApplied);

            var vm = new CatalogIndexViewModel
            {
                CatalogItems = catalog.Data,
                Brands = await _service.GetBrandsAsync(),
                Types = await _service.GetTypesAsync(),
                PaginationInfo = new PaginationInfo
                {
                    ActualPage = page ?? 0,
                    //ItemsPerPage = itemsOnPage,
                    ItemsPerPage = catalog.PageSize,
                    TotalItems = catalog.Count,
                    TotalPages = (int)Math.Ceiling((decimal)catalog.Count / itemsOnPage)
                },
                BrandFilterApplied = brandFilterApplied ?? 0,
                TypesFilterApplied = typesFilterApplied ?? 0
            };

            return View(vm);

        }
        [Authorize]
        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";


            return View();
        }
    }
}