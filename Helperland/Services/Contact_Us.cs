using Helperland.Models;
using Helperland.ViewModels;
using Microsoft.AspNetCore.Hosting;
using System;
using System.IO;

namespace Helperland.Services
{
    public class Contact_Us
    {
        private readonly HelperlandContext context;
        private readonly IWebHostEnvironment webHostEnvironment;

        public Contact_Us(HelperlandContext context, IWebHostEnvironment webHostEnvironment)
        {
            this.context = context;
            this.webHostEnvironment = webHostEnvironment;
        }
        public void Add(ContactUsViewModel contact)
        {
            ContactUs newcontact = new ContactUs()
            {
                Name = $"{contact.FirstName} {contact.LastName}",
                PhoneNumber = contact.MobileNumber,
                Email = contact.Email,
                Message = contact.Message,
                Subject = contact.Subject,
                CreatedOn = DateTime.Now
            };
            string uniqueFilename;
            if (contact.File != null)
            {
                string uploadsFolder = Path.Combine(webHostEnvironment.WebRootPath, "images", "ContactUsImages");
                uniqueFilename = Guid.NewGuid().ToString() + "_" + contact.File.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFilename);
                newcontact.UploadFileName = uniqueFilename;
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    contact.File.CopyTo(fileStream);
                }
            }
            context.ContactUs.Add(newcontact);
            context.SaveChanges();
        }
    }
}
