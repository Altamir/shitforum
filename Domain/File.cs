﻿using EnsureThat;
using System;

namespace Domain
{
    public sealed class File : DomainBase
    {
        public File(Guid id, string fileName, byte[] thumbNailJpeg, byte[] data, string mimeType) : base(id)
        {
            //the id is the postid
            FileName = EnsureArg.IsNotNullOrWhiteSpace(fileName, nameof(fileName));
            ThumbNailJpeg = EnsureArg.IsNotNull(thumbNailJpeg, nameof(thumbNailJpeg));
            Data = EnsureArg.IsNotNull(data, nameof(data));
            MimeType = EnsureArg.IsNotNullOrWhiteSpace(mimeType, nameof(mimeType));
        }
        
        public Post Post { get; private set; }

        public string FileName { get; private set; }

        public byte[] ThumbNailJpeg { get; private set; }

        public byte[] Data { get; private set; }

        public string MimeType { get; private set; }
    }
}
