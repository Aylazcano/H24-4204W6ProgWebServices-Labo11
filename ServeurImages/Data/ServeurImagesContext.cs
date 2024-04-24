using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ServeurImages.Models;

namespace ServeurImages.Data
{
    public class ServeurImagesContext : DbContext
    {
        public ServeurImagesContext (DbContextOptions<ServeurImagesContext> options)
            : base(options)
        {
        }

        public DbSet<ServeurImages.Models.Picture> Picture { get; set; } = default!;
    }
}
