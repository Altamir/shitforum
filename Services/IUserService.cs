﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Domain;
using Domain.IpHash;
using Optional;

namespace Services
{
    public interface IUserService
    {
        Task BanUser(IIpHash hash, string reason, DateTime expiry);

        Task<Option<IIpHash>> GetHashForPost(Guid postId);

        Task<IReadOnlyList<BannedIp>> GetAllBans();
    }
}
