using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ServeurImages.Data;
using ServeurImages.Models;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace ServeurImages.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class PicturesController : ControllerBase
    {
        private readonly ServeurImagesContext _context;

        public PicturesController(ServeurImagesContext context)
        {
            _context = context;
        }

        // GET: api/Pictures
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Picture>>> GetPicture()
        {
            if (_context.Picture == null)
            {
                return NotFound();
            }
            return await _context.Picture.ToListAsync();
        }

        // GET: api/Pictures/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Picture>> GetPicture(int id)
        {
            if (_context.Picture == null)
            {
                return NotFound();
            }
            var picture = await _context.Picture.FindAsync(id);

            if (picture == null)
            {
                return NotFound();
            }

            return picture;
        }

        // POST: api/Pictures
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [DisableRequestSizeLimit]
        public async Task<ActionResult<Picture>> PostPicture()
        {
            try
            {
                IFormCollection formCollection = await Request.ReadFormAsync();
                IFormFile? file = formCollection.Files.GetFile("birdImg");
                if (file != null)
                {
                    Image image = Image.Load(file.OpenReadStream());

                    Picture picture = new Picture()
                    {
                        Id = 0,
                        FileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName),
                        MimeType = file.ContentType
                    };

                    image.Save(Directory.GetCurrentDirectory() + "/images/lg/" + file.FileName);
                    image.Mutate(i => i.Resize(new ResizeOptions()
                    {
                        Mode = ResizeMode.Min,
                        Size = new Size() { Width = 320 }
                    }));
                    image.Save(Directory.GetCurrentDirectory() + "/images/sm/" + file.FileName);

                    _context.Entry(picture).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                    return Ok();
                }
                else 
                    return NotFound(new { Message = "Aucune image fournie" });
            }
            catch (Exception)
            {
                throw;
            }

        }

        // DELETE: api/Pictures/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePicture(int id)
        {
            if (_context.Picture == null)
            {
                return NotFound();
            }
            var picture = await _context.Picture.FindAsync(id);
            if (picture == null)
            {
                return NotFound();
            }

            _context.Picture.Remove(picture);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PictureExists(int id)
        {
            return (_context.Picture?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
