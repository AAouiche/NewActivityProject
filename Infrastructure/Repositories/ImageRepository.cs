using Domain.Interfaces;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class ImageRepository:IImageRepository
    {
        private readonly AppDbContext _context;

        public ImageRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task CreateAsync(Image image)
        {
            // Try to find an existing image by the given ApplicationUserId
            var existingImage = await _context.Images.FirstOrDefaultAsync(i => i.ApplicationUserId == image.ApplicationUserId);

            if (existingImage != null)
            {
                // Update properties of the existing image
                existingImage.Url = image.Url;
                // Add any other properties you want to update here
            }
            else
            {
                await _context.Images.AddAsync(image);
            }

            await _context.SaveChangesAsync();
        }
    }
}
