﻿using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;

namespace ShitForum.ApiControllers
{
    [Route("api/[controller]")]
    public class ImagesController : Controller
    {
        private readonly IFileService fileRepository;

        public ImagesController(IFileService postRepository)
        {
            this.fileRepository = postRepository;
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<ActionResult<FileResult>> GetPostThumbnail(Guid id, CancellationToken cancellationToken)
        {
            var post = await this.fileRepository.GetPostFile(id, cancellationToken).ConfigureAwait(false);
            return post.Match(some => File(some.ThumbNailJpeg, "image/jpeg"), () => this.NotFound().ToART<FileResult>());
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<ActionResult<FileResult>> GetPostImage(Guid id, CancellationToken cancellationToken)
        {
            var post = await this.fileRepository.GetPostFile(id, cancellationToken).ConfigureAwait(false);
            return post.Match(some => File(some.Data, some.MimeType), () => this.NotFound().ToART<FileResult>());
        }
    }
}
