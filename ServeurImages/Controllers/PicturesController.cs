using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
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

        // GET: api/GetFile/{size}/{id}
        [HttpGet("{size}/{id}")]
        public async Task<ActionResult> GetFile(string size, int id)
        {
            if (_context.Picture == null)
            {
                return NotFound();
            }

            Picture? picture = await _context.Picture.FindAsync(id);
            if (picture == null || picture.FileName == null || picture.MimeType == null)
            {
                return NotFound(new { Message = "Cette image n'existe pas." });
            }
            if (!(Regex.Match(size, "lg|sm").Success))
            {
                return BadRequest(new { Message = "La taille de l'image doit être 'lg' ou 'sm'." });
            }

            byte[] bytes = System.IO.File.ReadAllBytes(Directory.GetCurrentDirectory() + "/images/" + size + "/" + picture.FileName);
            return File(bytes, picture.MimeType);
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

                    _context.Picture.Add(picture);
                    await _context.SaveChangesAsync();
                    return Ok();
                }
                else 
                    return NotFound(new { Message = "Aucune image fournie." });
            }
            catch (Exception)
            {
                throw;
            }

        }

        // DELETE: api/DeletePicture/5
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
                return NotFound( new {Message = "Cette image n'existe pas."});
            }
            
            if (picture.MimeType != null && picture.FileName != null)
            {
                System.IO.File.Delete(Directory.GetCurrentDirectory() + "/images/lg/" + picture.FileName);
                System.IO.File.Delete(Directory.GetCurrentDirectory() + "/images/sm/" + picture.FileName);
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
