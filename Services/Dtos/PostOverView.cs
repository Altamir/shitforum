﻿using System;
using Domain;
using EnsureThat;
using Optional;

namespace Services.Dtos
{
    public sealed class PostOverView 
    {
        public PostOverView(Guid id, DateTime created, string name, string ipHash, string comment, Option<File> file)
        {
            Id = EnsureArg.IsNotEmpty(id, nameof(id));
            Created = EnsureArg.IsNotDefault(created, nameof(created));
            Name = EnsureArg.IsNotNullOrWhiteSpace(name, nameof(name));
            IpHash = EnsureArg.IsNotNull(ipHash, nameof(ipHash));
            Comment = EnsureArg.IsNotNullOrWhiteSpace(comment, nameof(comment));
            File = file;
        }

        public Guid Id { get; }
        public DateTime Created { get; }

        public string Name { get; }
        public string IpHash { get; }
        public string Comment { get; }
        public Option<File> File { get; }
    }
}
