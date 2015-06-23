﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using LASI.WebApp.Persistence;
using LASI.WebApp.Models;
using LASI.WebApp.Models.User;
using Microsoft.AspNet.Mvc;
using Newtonsoft.Json.Linq;

namespace LASI.WebApp.Controllers.Documents
{
    [Route("api/UserDocuments/[controller]")]
    public class ListController : Controller
    {
        public ListController(IDocumentAccessor<UserDocument> documentStore)
        {
            this.documentStore = documentStore;
        }
        [HttpGet]
        public IEnumerable<dynamic> Get() => from document in documentStore.GetAllForUser(Context.User.GetUserId())
                                             let activeDocument = ActiveUserDocument.FromUserDocument(document)
                                             let dateUploaded = (DateTime)(JToken)(document.DateUploaded)
                                             orderby dateUploaded descending
                                             select new
                                             {
                                                 Id = activeDocument._id.ToString(),
                                                 Name = activeDocument.Name,
                                                 Progress = activeDocument.Progress
                                             };

        [HttpGet("{limit}")]
        public IEnumerable<dynamic> Get(int limit) => this.Get().Take(limit);

        private readonly IDocumentAccessor<UserDocument> documentStore;
    }
}