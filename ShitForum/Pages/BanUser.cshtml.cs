﻿using System;
using System.Threading.Tasks;
using Domain;
using EnsureThat;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Services;
using Services.Dtos;
using ShitForum.Attributes;

namespace ShitForum.Pages
{
    [CookieAuth]
    public class BanUserModel : PageModel
    {
        private readonly IUserService userService;
        private readonly IPostService postService;

        public BanUserModel(IUserService userService, IPostService postService)
        {
            this.userService = userService;
            this.postService = postService;
        }

        [BindProperty]
        public string Reason { get; set; }

        [BindProperty]
        public DateTime Expiry { get; set; }

        public PostDetailView Post { get; private set; }

        public async Task<IActionResult> OnGet(Guid id)
        {
            EnsureArg.IsNotEmpty(id, nameof(id));
            var hash = await this.userService.GetHashForPost(id);
            return await hash.Match(async some =>
            {
                var p = await this.postService.GetById(id);
                return p.Match(post =>
                { 
                    this.Post = post;
                    this.Expiry = DateTime.UtcNow.AddDays(7);
                    return Page().ToIAR();
                }, new NotFoundResult().ToIAR);
            }, () => new NotFoundResult().ToIART());
        }

        public async Task<IActionResult> OnPostAsync(Guid id)
        {
            EnsureArg.IsNotEmpty(id, nameof(id));
            var hash = await this.userService.GetHashForPost(id);
            return await hash.Match(async some =>
            {
                await userService.BanUser(some, Reason, Expiry);
                return RedirectToPage("Index").ToIAR();
            }, () => new NotFoundResult().ToIART());
        }
    }
}