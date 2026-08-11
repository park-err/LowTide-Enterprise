using LowTideEnt.Domain.Entities.Staff;
using System;
using System.Collections.Generic;
using System.Text;

namespace LowTideEnt.Application.Services.Home.Mapping
{
    public static class HomeMapping
    {
        public static Announcement ToResponse(this AnnouncementEntity entity) =>
            new Announcement { Title = entity.Title, Body = entity.Body, LinkUrl = entity.LinkUrl, PostedDate = entity.CreatedDate };
        public static ShiningStar ToResponse(this ShiningStarEntity entity) =>
            new ShiningStar { FullName = entity.FullName, Quote = entity.Quote, Value = entity.Value, NominationDate = entity.CreatedDate };
        public static StaffLink ToResponse(this StaffLinkEntity entity) =>
            new StaffLink { Title = entity.Title, LinkUrl = entity.LinkUrl };
    }
}
