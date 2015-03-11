﻿using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Http;
//using LASI.Utilities;
using System.Threading.Tasks;
using System;
using Microsoft.AspNet.Identity;
using AspSixApp.Models;
using AspSixApp.CustomIdentity.MongoDb;
using System.Security.Principal;
using LASI.Utilities;
using LASI.Content;
using LASI.Utilities.Specialized.Enhanced.IList.Linq;
using AspSixApp.CustomIdentity;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

//// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace AspSixApp.Controllers
{
    public class DocumentsController : Controller
    {
        private readonly IInputDocumentStore<UserDocument> documentStore;

        public DocumentsController(IInputDocumentStore<UserDocument> documentStore, UserManager<ApplicationUser> userManager, Microsoft.AspNet.Hosting.IHostingEnvironment hostingEnvironment)
        {
            this.documentStore = documentStore;
            this.userManager = userManager;
            this.hostingEnvironment = hostingEnvironment;
        }
        [HttpPost]
        public async Task<HttpResponse> Upload()
        {
            var files = Request.Form.Files;
            if (files.Count == 0)
            {
                throw new ArgumentException("No files were received");
            }
            if (!files.All(IsValidContentType))
            {
                throw new UnsupportedFileTypeAddedException(
                     $@"One or more of your files was in an incorrect format.
                    The accepted formats are {string.Join(", ", FileManager.AcceptedFileFormats)}"
                 );
            }
            else
            {
                var ownerId = Context.User.Identity.GetUserId();
                var user = await userManager.FindByIdAsync(ownerId);

                var uploadedUserDocuments = from file in files
                                            let fileName = ParseFileName(file)
                                            let textContent = new Lazy<string>(() => ProcessFileContents(file).Result)
                                            select new
                                            {
                                                ContentType = $"{file.ContentType}\n{file.ContentDisposition}\n{file.Length}",
                                                Document =
                                              new UserDocument
                                              {
                                                  Name = fileName,
                                                  Content = textContent.Value,
                                                  OwnerId = ownerId,
                                                  DateUploaded = (string)(JToken)DateTime.Now
                                              }
                                            };
                uploadedUserDocuments.ForEach(async file =>
                {
                    this.documentStore.AddUserInputDocument(ownerId, file.Document);
                    await Response.WriteAsync(file.ContentType);

                });
                user.Documents = user.Documents.Concat(uploadedUserDocuments.Select(upload => upload.Document));

                await this.userManager.UpdateAsync(user);
            }
            return Response;
        }

        private async Task<string> ProcessFileContents(IFormFile file)
        {
            var name = ParseFileName(file);
            var fullpath = await SaveFileAsync(file, name);
            var extension = name.Substring(name.LastIndexOf('.'));
            var wrapped = WrapperFactory[extension](fullpath);
            return await wrapped.GetTextAsync();
        }

        ExtensionWrapperMap WrapperFactory { get; } = new ExtensionWrapperMap(ext => { throw new UnsupportedFileTypeAddedException(ext); });

        Microsoft.AspNet.Hosting.IHostingEnvironment hostingEnvironment;
        private readonly UserManager<ApplicationUser> userManager;

        private async Task<string> SaveFileAsync(IFormFile file, string fileName)
        {
            var physicalPath = Path.Combine(Directory.GetParent(hostingEnvironment.WebRoot).FullName, "App_Data", "Temp", file.GetHashCode() + fileName);
            await file.SaveAsAsync(physicalPath);
            return physicalPath;
        }

        private string ParseFileName(IFormFile file)
        {
            var contentDispositonProperties = file.ContentDisposition.SplitRemoveEmpty(';').Select(s => s.Trim());
            return contentDispositonProperties.First(p => p.StartsWith("filename")).Substring(9).Trim('\"');
        }

        private bool IsValidContentType(IFormFile arg)
        {
            return new[]
            {
                "text/plain", // generally corresponds to .txt
                "application/vnd.openxmlformats-officedocument.wordprocessingml.document", // generally corresponds to .docx
                "application/msword", // generally corresponds to .doc
                "application/pdf" // generally corresponds to .pdf
            }.Contains(arg.ContentType, StringComparer.OrdinalIgnoreCase);
        }
    }
}
